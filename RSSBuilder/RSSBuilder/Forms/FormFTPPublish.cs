using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using EnterpriseDT.Net.Ftp;
using BSoft.Xml;
using System.Threading;
using System.Globalization;
using System.Resources;

namespace RSSBuilder
{
	/// <summary>
	/// Summary description for FormFTP.
	/// </summary>
	public class FormFTPPublish : System.Windows.Forms.Form
	{
      /// <summary>
      /// The rss feed to publish
      /// </summary>
      private RSSFeed rssFeed; 

      private XmlConfig ftpConfig = null;

      private ResourceManager res = new ResourceManager("RSSBuilder.RSSBuilderStrings", typeof(FormFTPPublish).Assembly);

      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Button buttonPublish;
      private System.Windows.Forms.ProgressBar progressBar;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.NumericUpDown numItemCount;
      private System.Windows.Forms.Label labelMsg;
      private System.Windows.Forms.ComboBox comboSites;
      private System.Windows.Forms.Button buttonSiteManager;
      private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormFTPPublish(RSSFeed theRssFeed, string defaultSiteName)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

         rssFeed = theRssFeed;
         numItemCount.Value = rssFeed.NewsItemCount;
         numItemCount.Maximum = rssFeed.NewsItemCount;

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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFTPPublish));
         this.label7 = new System.Windows.Forms.Label();
         this.buttonPublish = new System.Windows.Forms.Button();
         this.progressBar = new System.Windows.Forms.ProgressBar();
         this.buttonCancel = new System.Windows.Forms.Button();
         this.numItemCount = new System.Windows.Forms.NumericUpDown();
         this.labelMsg = new System.Windows.Forms.Label();
         this.comboSites = new System.Windows.Forms.ComboBox();
         this.buttonSiteManager = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.numItemCount)).BeginInit();
         this.SuspendLayout();
         // 
         // label7
         // 
         this.label7.AccessibleDescription = null;
         this.label7.AccessibleName = null;
         resources.ApplyResources(this.label7, "label7");
         this.label7.Font = null;
         this.label7.Name = "label7";
         this.label7.Click += new System.EventHandler(this.label7_Click);
         // 
         // buttonPublish
         // 
         this.buttonPublish.AccessibleDescription = null;
         this.buttonPublish.AccessibleName = null;
         resources.ApplyResources(this.buttonPublish, "buttonPublish");
         this.buttonPublish.BackgroundImage = null;
         this.buttonPublish.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonPublish.Font = null;
         this.buttonPublish.Name = "buttonPublish";
         this.buttonPublish.Click += new System.EventHandler(this.buttonPublish_Click);
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
         this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
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
         this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
         // 
         // numItemCount
         // 
         this.numItemCount.AccessibleDescription = null;
         this.numItemCount.AccessibleName = null;
         resources.ApplyResources(this.numItemCount, "numItemCount");
         this.numItemCount.Font = null;
         this.numItemCount.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
         this.numItemCount.Name = "numItemCount";
         this.numItemCount.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
         this.numItemCount.ValueChanged += new System.EventHandler(this.numItemCount_ValueChanged);
         // 
         // labelMsg
         // 
         this.labelMsg.AccessibleDescription = null;
         this.labelMsg.AccessibleName = null;
         resources.ApplyResources(this.labelMsg, "labelMsg");
         this.labelMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.labelMsg.Font = null;
         this.labelMsg.Name = "labelMsg";
         this.labelMsg.Click += new System.EventHandler(this.labelMsg_Click);
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
         this.label1.Click += new System.EventHandler(this.label1_Click);
         // 
         // FormFTPPublish
         // 
         this.AcceptButton = this.buttonPublish;
         this.AccessibleDescription = null;
         this.AccessibleName = null;
         resources.ApplyResources(this, "$this");
         this.BackgroundImage = null;
         this.CancelButton = this.buttonCancel;
         this.Controls.Add(this.label1);
         this.Controls.Add(this.buttonSiteManager);
         this.Controls.Add(this.comboSites);
         this.Controls.Add(this.labelMsg);
         this.Controls.Add(this.numItemCount);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.progressBar);
         this.Controls.Add(this.buttonPublish);
         this.Controls.Add(this.label7);
         this.Font = null;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormFTPPublish";
         this.ShowInTaskbar = false;
         ((System.ComponentModel.ISupportInitialize)(this.numItemCount)).EndInit();
         this.ResumeLayout(false);

      }
		#endregion

      private void buttonPublish_Click(object sender, System.EventArgs e)
      {
         if(comboSites.SelectedIndex == -1)
            return;

         string sectionName = section();
         if(!ftpConfig.HasSection(sectionName))
            return;

         //
         // If the feed was published to a different site before, and
         // the selected FTP site differs from this site, ask the user what to do.
         //
         if(rssFeed.FtpSite =="")
         {
            if( MessageBox.Show(res.GetString("msgFirstPublish"), "RSS Builder", 
               MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
               return;
         }
         else if(sectionName != rssFeed.FtpSite)
         {
            if( MessageBox.Show(res.GetString("msgAskChangeSite"), "RSS Builder", 
                            MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
               return;
         }

         rssFeed.FtpSite = sectionName;

         string server = ftpConfig.GetValue(sectionName, "ftpServer");
         string portStr = ftpConfig.GetValue(sectionName, "ftpPort", "21");
         string user = Encryption.Decrypt(ftpConfig.GetValue(sectionName, "ftpUser"));
         string password = Encryption.Decrypt(ftpConfig.GetValue(sectionName, "ftpPassword"));
         string path = ftpConfig.GetValue(sectionName, "ftpRemotePath", ".");
         string file = ftpConfig.GetValue(sectionName, "ftpRemoteFile", "news.xml");

         bool passiveFTP = ("1" == ftpConfig.GetValue("__general__", "ftpPassiveFtp","0"));

         string tempFeedFileName = Path.GetTempFileName();

         int maxItemCount = (int)numItemCount.Value;
         rssFeed.saveFeed(tempFeedFileName, maxItemCount);

         labelMsg.Text = "Publishing... Please wait!!" ; labelMsg.Refresh();
         progressBar.Increment(1);
         //       textBoxLog.Refresh();


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

            ftp.Timeout = 3500; // not too small to prevent 'unable to read from transport connection'

            ftp.Login(user, password);

            if(passiveFTP)
               ftp.ConnectMode = FTPConnectMode.PASV;
            else
               ftp.ConnectMode = FTPConnectMode.ACTIVE;
            
            progressBar.Increment(1);
            labelMsg.Text = ftp.LastValidReply.ReplyText; labelMsg.Refresh();

            if(path != @"."  && path != @"/" && path != @"\" && path != "")
               ftp.ChDir(path); // only change dir when needed
// OLD FTP               ftp.Chdir(path); // only change dir when needed
            
            progressBar.Increment(1);
            labelMsg.Text = ftp.LastValidReply.ReplyText; labelMsg.Refresh();

            ftp.Put(tempFeedFileName, file);
            
            progressBar.Increment(1);
            labelMsg.Text = ftp.LastValidReply.ReplyText; labelMsg.Refresh();

            ftp.Quit();
            progressBar.Increment(1);

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
            labelMsg.Text = "Unable to publish the feed. Check your FTP settings, try passive FTP mode, or do not use remote directory";
            MessageBox.Show(ex.Message, "RSS Builder", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            this.Cursor = Cursors.Default;
         }

         ftpConfig.Save(); 
         Close();
      }

      private string section()
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
            buttonPublish.Enabled = true;
         else
            buttonPublish.Enabled = false;
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

      private void progressBar_Click(object sender, EventArgs e)
      {

      }

      private void buttonCancel_Click(object sender, EventArgs e)
      {

      }

      private void numItemCount_ValueChanged(object sender, EventArgs e)
      {

      }

      private void labelMsg_Click(object sender, EventArgs e)
      {

      }

      private void label7_Click(object sender, EventArgs e)
      {

      }

      private void label1_Click(object sender, EventArgs e)
      {

      }









	}
}
