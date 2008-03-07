using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using EnterpriseDT.Net.Ftp;
using BSoft.Xml;

namespace RSSBuilder
{
	/// <summary>
	/// Summary description for FormFTP.
	/// </summary>
	public class FormFTPDownload : System.Windows.Forms.Form
	{
      /// <summary>
      /// The rss feed to download to
      /// </summary>
      private RSSFeed rssFeed; 

      private XmlConfig ftpConfig = null;
      private System.Windows.Forms.ProgressBar progressBar;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.Label labelMsg;
      private System.Windows.Forms.ComboBox comboSites;
      private System.Windows.Forms.Button buttonSiteManager;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button buttonDownload;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormFTPDownload(RSSFeed theRssFeed, string defaultSiteName)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

         rssFeed = theRssFeed;

         readFTPConfiguration();

         int selIdx = comboSites.Items.IndexOf(defaultSiteName);
         if(selIdx != -1)
            comboSites.SelectedIndex = selIdx;
		}

      private void readFTPConfiguration()
      {
         //---
         // Read settings from configuration class
         //---
         string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
         ftpConfig = new XmlConfig(path + @"\rssbuilder.ftpconfig");

         comboSites.Items.Clear();
         StringCollection sectionNames = ftpConfig.GetSectionNames();
         foreach(string sectionName in sectionNames)
         {
            if(sectionName != "__general__")
               comboSites.Items.Add(sectionName);
         }
         if(comboSites.Items.Count > 0)
            comboSites.SelectedIndex = 0;
      }



		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFTPDownload));
         this.buttonDownload = new System.Windows.Forms.Button();
         this.progressBar = new System.Windows.Forms.ProgressBar();
         this.buttonCancel = new System.Windows.Forms.Button();
         this.labelMsg = new System.Windows.Forms.Label();
         this.comboSites = new System.Windows.Forms.ComboBox();
         this.buttonSiteManager = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // buttonDownload
         // 
         this.buttonDownload.AccessibleDescription = null;
         this.buttonDownload.AccessibleName = null;
         resources.ApplyResources(this.buttonDownload, "buttonDownload");
         this.buttonDownload.BackgroundImage = null;
         this.buttonDownload.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonDownload.Font = null;
         this.buttonDownload.Name = "buttonDownload";
         this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
         // 
         // progressBar
         // 
         this.progressBar.AccessibleDescription = null;
         this.progressBar.AccessibleName = null;
         resources.ApplyResources(this.progressBar, "progressBar");
         this.progressBar.BackgroundImage = null;
         this.progressBar.Font = null;
         this.progressBar.Maximum = 5;
         this.progressBar.Name = "progressBar";
         this.progressBar.Step = 1;
         // 
         // buttonCancel
         // 
         this.buttonCancel.AccessibleDescription = null;
         this.buttonCancel.AccessibleName = null;
         resources.ApplyResources(this.buttonCancel, "buttonCancel");
         this.buttonCancel.BackgroundImage = null;
         this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.buttonCancel.Font = null;
         this.buttonCancel.Name = "buttonCancel";
         // 
         // labelMsg
         // 
         this.labelMsg.AccessibleDescription = null;
         this.labelMsg.AccessibleName = null;
         resources.ApplyResources(this.labelMsg, "labelMsg");
         this.labelMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.labelMsg.Font = null;
         this.labelMsg.Name = "labelMsg";
         // 
         // comboSites
         // 
         this.comboSites.AccessibleDescription = null;
         this.comboSites.AccessibleName = null;
         resources.ApplyResources(this.comboSites, "comboSites");
         this.comboSites.BackgroundImage = null;
         this.comboSites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.comboSites.Font = null;
         this.comboSites.Name = "comboSites";
         this.comboSites.SelectedIndexChanged += new System.EventHandler(this.comboSites_SelectedIndexChanged);
         // 
         // buttonSiteManager
         // 
         this.buttonSiteManager.AccessibleDescription = null;
         this.buttonSiteManager.AccessibleName = null;
         resources.ApplyResources(this.buttonSiteManager, "buttonSiteManager");
         this.buttonSiteManager.BackgroundImage = null;
         this.buttonSiteManager.Font = null;
         this.buttonSiteManager.Name = "buttonSiteManager";
         this.buttonSiteManager.Click += new System.EventHandler(this.buttonSiteManager_Click);
         // 
         // label1
         // 
         this.label1.AccessibleDescription = null;
         this.label1.AccessibleName = null;
         resources.ApplyResources(this.label1, "label1");
         this.label1.Font = null;
         this.label1.Name = "label1";
         // 
         // FormFTPDownload
         // 
         this.AcceptButton = this.buttonDownload;
         this.AccessibleDescription = null;
         this.AccessibleName = null;
         resources.ApplyResources(this, "$this");
         this.BackgroundImage = null;
         this.CancelButton = this.buttonCancel;
         this.Controls.Add(this.buttonSiteManager);
         this.Controls.Add(this.comboSites);
         this.Controls.Add(this.labelMsg);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.progressBar);
         this.Controls.Add(this.buttonDownload);
         this.Controls.Add(this.label1);
         this.Font = null;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormFTPDownload";
         this.ShowInTaskbar = false;
         this.ResumeLayout(false);

      }
		#endregion

      private void buttonDownload_Click(object sender, System.EventArgs e)
      {
         if(comboSites.SelectedIndex == -1)
            return;

         string sectionName = section();
         if(!ftpConfig.HasSection(sectionName))
            return;

         string server = ftpConfig.GetValue(sectionName, "ftpServer");
         string portStr = ftpConfig.GetValue(sectionName, "ftpPort", "21");
         string user = Encryption.Decrypt(ftpConfig.GetValue(sectionName, "ftpUser"));
         string password = Encryption.Decrypt(ftpConfig.GetValue(sectionName, "ftpPassword"));
         string path = ftpConfig.GetValue(sectionName, "ftpRemotePath", ".");
         string file = ftpConfig.GetValue(sectionName, "ftpRemoteFile", "news.xml");

         bool passiveFTP = ("1" == ftpConfig.GetValue("__general__", "ftpPassiveFtp","0"));


         string tempFeedFileName = Path.GetTempFileName();

//         rssFeed.saveFeed(tempFeedFileName, maxItemCount);

         labelMsg.Text = "Downloading... Please wait!!" ; labelMsg.Refresh();
         progressBar.Increment(1);

         try
         {
            this.Cursor = Cursors.WaitCursor;
            int port;
            try
            {
               port = Int16.Parse(portStr);
            }
            catch
            {
               port = 21;
            }

            FTPClient ftp = new FTPClient(server, port);

            ftp.Timeout = 3500;

            ftp.Login(user, password);

            if(passiveFTP)
               ftp.ConnectMode = FTPConnectMode.PASV;
            else
               ftp.ConnectMode = FTPConnectMode.ACTIVE;
            
            progressBar.Increment(1);
            labelMsg.Text = ftp.LastValidReply.ReplyText; labelMsg.Refresh();

            if(path != @"."  && path != @"/" && path != @"\" && path != "")
               ftp.ChDir(path);

// OLD               ftp.Chdir(path);
            
            progressBar.Increment(1);
            labelMsg.Text = ftp.LastValidReply.ReplyText; labelMsg.Refresh();

            ftp.Get(tempFeedFileName, file);
            
            progressBar.Increment(1);
            labelMsg.Text = ftp.LastValidReply.ReplyText; labelMsg.Refresh();

            ftp.Quit();
            progressBar.Increment(1);

            //---
            // Read temp file into feed
            //---
            rssFeed.openFeed(tempFeedFileName);

            try
            {
               File.Delete(tempFeedFileName);
            }
            catch
            {
            }
         }
         catch(Exception ex)
         {
            labelMsg.Text = "Unable to download the feed. Check your FTP settings, or try using passive FTP mode";
            MessageBox.Show(ex.Message, "RSS Builder", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            this.Cursor = Cursors.Default;
         }

         ftpConfig.Save(); 
         Close();
      }





      public string section()
      {
         if(comboSites.SelectedIndex != -1)
         {
            string selItem = comboSites.SelectedItem.ToString();

            return selItem;
         }
         
         return "";
      }

      private void comboSites_SelectedIndexChanged(object sender, System.EventArgs e)
      {
         if(comboSites.SelectedIndex != -1)
            buttonDownload.Enabled = true;
         else
            buttonDownload.Enabled = false;
      }

      private void buttonSiteManager_Click(object sender, System.EventArgs e)
      {
         FormFTPSites ftp = new FormFTPSites( section() );
         if(ftp.ShowDialog() == DialogResult.OK)
         {
            readFTPConfiguration();
            comboSites.SelectedIndex = comboSites.Items.IndexOf(ftp.section());
         }
      }










	}
}
