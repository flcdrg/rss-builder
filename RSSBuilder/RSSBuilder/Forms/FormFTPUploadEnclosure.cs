using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using EnterpriseDT.Net.Ftp;
using BSoft.Xml;
using System.Collections.Specialized;

namespace RSSBuilder
{
   public partial class FormFTPUploadEnclosure : Form
   {
      private RSSFeed _rssFeed;
      private RSSItem _rssItem;
      private XmlConfig _ftpConfig = null;
      private long _totalBytes = 0;

      private FTPSessionParameters _session = new FTPSessionParameters();
      public string RemotePath 
      {
         get { return _session.RemotePath; }
      }

      public string FileName
      {
         get { return _session.LocalFileName; }
      }

      public FormFTPUploadEnclosure(RSSFeed rssFeed, RSSItem rssItem)
		{        
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

         _rssFeed = rssFeed;
         _rssItem = rssItem;

         this.Text = this.Text + " '" +  _rssItem.Title + "'";

         readFTPConfiguration();

         int selIdx = comboSites.Items.IndexOf(rssFeed.FtpSite);
         if (selIdx != -1)
            comboSites.SelectedIndex = selIdx;
      }

      private void readFTPConfiguration()
      {
         //---
         // Read settings from configuration class
         //---
         string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
         _ftpConfig = new XmlConfig(path + @"\rssbuilder.ftpconfig");

         comboSites.Items.Clear();
         StringCollection sectionNames = _ftpConfig.GetSectionNames();
         foreach (string sectionName in sectionNames)
         {
            if (sectionName != "__general__")
               comboSites.Items.Add(sectionName);
         }
         if (comboSites.Items.Count > 0)
            comboSites.SelectedIndex = 0;
      }

      private string section()
      {
         if (comboSites.SelectedIndex != -1)
         {
            string selItem = comboSites.SelectedItem.ToString();

            return selItem;
         }

         return "";
      }

      private void buttonSiteManager_Click(object sender, EventArgs e)
      {
         FormFTPSites ftp = new FormFTPSites(section());
         if (ftp.ShowDialog() == DialogResult.OK)
         {
            readFTPConfiguration();
            comboSites.SelectedIndex = comboSites.Items.IndexOf(ftp.section());
         }
      }

      private void comboSites_SelectedIndexChanged(object sender, EventArgs e)
      {
         updateControls();
      }

      private void updateControls()
      {
         if (comboSites.SelectedIndex != -1)
            buttonUpload.Enabled = true;
         else
            buttonUpload.Enabled = false;
      }

      //private const int TRANSFER_NOTIFY_INTERVAL = 100;
      //private void bytesTransferred(object sender, BytesTransferredEventArgs e)
      //{
      //   progressBar.Maximum = (int)_totalBytes / TRANSFER_NOTIFY_INTERVAL;

      //   labelMsg.Text = e.ByteCount/1024 + " of " + _totalBytes/1024 + " KB uploaded";
      //   labelMsg.Refresh();
      //}

      private void buttonPublish_Click(object sender, EventArgs e)
      {
      }

      private void Upload()
      {
         if (textFileName.Text == "")
            return;

         if (comboSites.SelectedIndex == -1)
            return;

         string sectionName = section();
         if (!_ftpConfig.HasSection(sectionName))
            return;

         _session.LocalFileName = textFileName.Text;
         _session.RemoteFileName = Path.GetFileName(_session.LocalFileName);
         _session.Server = _ftpConfig.GetValue(sectionName, "ftpServer");
         _session.PortStr = _ftpConfig.GetValue(sectionName, "ftpPort", "21");
         _session.User = Encryption.Decrypt(_ftpConfig.GetValue(sectionName, "ftpUser"));
         _session.Password = Encryption.Decrypt(_ftpConfig.GetValue(sectionName, "ftpPassword"));
         _session.RemotePath = _ftpConfig.GetValue(sectionName, "ftpRemotePath", ".");
         _session.Passive = ("1" == _ftpConfig.GetValue("__general__", "ftpPassiveFtp", "0"));

         FileInfo fileInfo = new FileInfo(_session.LocalFileName);
         _totalBytes = fileInfo.Length;

         labelMsg.Text = "Publishing... Please wait!!"; labelMsg.Refresh();
         //       textBoxLog.Refresh();


         progressBar.Maximum = 100;

         backgroundWorker.RunWorkerAsync(_session);
      }

      private void buttonBrowse_Click(object sender, EventArgs e)
      {
         openEnclosureDialog.FileName = "";

         if (openEnclosureDialog.ShowDialog() == DialogResult.OK)
         {
            textFileName.Text = Path.GetFileName(openEnclosureDialog.FileName);
            updateControls();
            comboSites.Enabled = false;
            buttonSiteManager.Enabled = false;

            Upload();

            buttonUpload.Enabled = false;
            buttonClose.Text = "Close";
            buttonClose.Enabled = false;
         }
      }

      private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         FTPSessionParameters session = (FTPSessionParameters)e.Argument;

         FileInfo fileInfo = new FileInfo(session.LocalFileName);
         int totalBytes = (int) fileInfo.Length;
         int bytesTransferred = 0;
         try
         {
            int port;
            try
            {
               port = Int16.Parse(_session.PortStr);
            }
            catch
            {
               port = 21;
            }

            FTPClient ftp = new FTPClient();
          
            ftp.BytesTransferred += delegate(object dsender, BytesTransferredEventArgs de)
                             { 
                                bytesTransferred += 1024;
                                backgroundWorker.ReportProgress(bytesTransferred*100 / totalBytes, 
                                bytesTransferred.ToString() + " of " + totalBytes.ToString());
                             };

            ftp.TransferNotifyInterval = 1024;
            ftp.RemoteHost = session.Server;
            ftp.ControlPort = port;
            ftp.Timeout = 4000; // not too small to prevent 'unable to read from transport connection'

            if (session.Passive)
               ftp.ConnectMode = FTPConnectMode.PASV;
            else
               ftp.ConnectMode = FTPConnectMode.ACTIVE;

            ftp.Connect();
            ftp.Login(session.User, session.Password);

            backgroundWorker.ReportProgress(10, ftp.LastValidReply.ReplyText);

            string path = session.RemotePath;
            if (path != @"." && path != @"/" && path != @"\" && path != "")
               ftp.ChDir(path); // only change dir when needed
            // OLD FTP               ftp.Chdir(path); // only change dir when needed

            backgroundWorker.ReportProgress(20, ftp.LastValidReply.ReplyText);

            ftp.TransferType = FTPTransferType.BINARY;

            ftp.Put(textFileName.Text, _session.RemoteFileName);

            backgroundWorker.ReportProgress(30, ftp.LastValidReply.ReplyText);

            ftp.Quit();
            e.Result = true;
         }
         catch (Exception ex)
         {
            backgroundWorker.ReportProgress(40, "Unable to upload the file. Check your FTP settings, try passive FTP mode, or do not use remote directory");
            MessageBox.Show(ex.Message, "RSS Builder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.Result = false;
         }
         finally
         {
         //   this.Cursor = Cursors.Default;
         }
      }

      private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         progressBar.Value = e.ProgressPercentage;
         labelMsg.Text = (string)e.UserState;
         labelMsg.Refresh();
      }

      public event EventHandler UploadSucceeded = null;

      private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         buttonClose.Enabled = true;
         progressBar.Value = 100;

         //      textEnclosureUrl.Text = this.textBoxWebURL.Text + "/" + remotePath + "/" + Path.GetFileName(fileName);
         //      determineEnclosureLengthAndType(fileName);

         if ((bool)(e.Result) == true)
         {
            _rssItem.EnclosureUrl = _rssFeed.WebURL + "/" + _session.RemotePath + "/" + _session.RemoteFileName;
            determineEnclosureLengthAndType();

            if (UploadSucceeded != null)
               UploadSucceeded(this, new EventArgs());

            if (checkCloseWhenFinished.Checked)
               Close();
         }
      }

      private void determineEnclosureLengthAndType()
      {
         FileInfo fileInfo = new FileInfo(_session.LocalFileName);
         _rssItem.EnclosureLength = fileInfo.Length.ToString();

         string extension = Path.GetExtension(_session.LocalFileName);
         string enclType = "";
         switch (extension.ToLower())
         {
            case ".au":
               enclType = "audio/basic";
               break;
            case ".mp3":
               enclType = "audio/mpeg";
               break;
            case ".mid":
               enclType = "audio/mid";
               break;
            case ".rmi":
               enclType = "audio/mid";
               break;
            case ".aif":
               enclType = "audio/x-aiff";
               break;
            case ".wav":
               enclType = "audio/x-wav";
               break;
            case ".ra":
               enclType = "audio/x-pn-realaudio";
               break;
            case ".ram":
               enclType = "audio/x-pn-realaudio";
               break;
            case ".bmp":
               enclType = "image/bmp";
               break;
            case ".gif":
               enclType = "image/gif";
               break;
            case ".jpg":
               enclType = "image/jpeg";
               break;
            case ".jpeg":
               enclType = "image/jpeg";
               break;
            case ".tif":
               enclType = "image/tiff";
               break;
            case ".tiff":
               enclType = "image/tiff";
               break;
            case ".mpg":
               enclType = "video/mpeg";
               break;
            case ".mpeg":
               enclType = "video/mpeg";
               break;
            case ".mpa":
               enclType = "video/mpeg";
               break;
            case ".mov":
               enclType = "video/quicktime";
               break;
            case ".qt":
               enclType = "video/quicktime";
               break;
            case ".avi":
               enclType = "video/x-msvideo";
               break;
            case ".wmv":
               enclType = "video/x-ms-wmv";
               break;
            default: enclType = "";
               break;
         }
         _rssItem.EnclosureType = enclType;
      }


      private void buttonClose_Click(object sender, EventArgs e)
      {
         Close();
      }

   }
}