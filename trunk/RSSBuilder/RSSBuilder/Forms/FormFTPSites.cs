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
	/// Summary description for FormFTPSites.
	/// </summary>
	public class FormFTPSites : System.Windows.Forms.Form
	{
      private XmlConfig ftpConfig = null;

      private bool disableChanges = false;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.GroupBox groupBoxFTP;
      private System.Windows.Forms.TextBox textBoxFile;
      private System.Windows.Forms.TextBox textBoxPath;
      private System.Windows.Forms.TextBox textBoxPasswd;
      private System.Windows.Forms.TextBox textBoxUser;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox textBoxServer;
      private System.Windows.Forms.TextBox textBoxPort;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.ColumnHeader columnSite;
      private System.Windows.Forms.Button buttonNew;
      private System.Windows.Forms.Button buttonRename;
      private System.Windows.Forms.Button buttonDelete;
      private System.Windows.Forms.ListView listSites;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.CheckBox checkSavePasswd;
      private System.Windows.Forms.CheckBox checkPassiveFTP;
      private System.Windows.Forms.Button buttonOk;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormFTPSites(string defaultSiteName)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

         //---
         // Read settings from configuration class
         //---
         if(ftpConfig == null) // reuse an existing instance
         {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            ftpConfig = new XmlConfig(path + @"\rssbuilder.ftpconfig");
         }

        
         StringCollection sectionNames = ftpConfig.GetSectionNames();
         foreach(string sectionName in sectionNames)
         {
            if(sectionName != "__general__")
            {
               ListViewItem item = listSites.Items.Add(sectionName);
               if(sectionName == defaultSiteName)
                  item.Selected = true;
            }
         }

         if(listSites.Items.Count>0 && listSites.SelectedItems.Count==0)
            listSites.Items[0].Selected = true;

         checkPassiveFTP.Checked = ("1" == ftpConfig.GetValue("__general__", "ftpPassiveFtp","0"));
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFTPSites));
         this.buttonCancel = new System.Windows.Forms.Button();
         this.groupBoxFTP = new System.Windows.Forms.GroupBox();
         this.checkSavePasswd = new System.Windows.Forms.CheckBox();
         this.label8 = new System.Windows.Forms.Label();
         this.buttonDelete = new System.Windows.Forms.Button();
         this.buttonRename = new System.Windows.Forms.Button();
         this.buttonNew = new System.Windows.Forms.Button();
         this.listSites = new System.Windows.Forms.ListView();
         this.columnSite = new System.Windows.Forms.ColumnHeader();
         this.textBoxPort = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.textBoxFile = new System.Windows.Forms.TextBox();
         this.textBoxPath = new System.Windows.Forms.TextBox();
         this.textBoxPasswd = new System.Windows.Forms.TextBox();
         this.textBoxUser = new System.Windows.Forms.TextBox();
         this.label6 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.textBoxServer = new System.Windows.Forms.TextBox();
         this.buttonOk = new System.Windows.Forms.Button();
         this.checkPassiveFTP = new System.Windows.Forms.CheckBox();
         this.groupBoxFTP.SuspendLayout();
         this.SuspendLayout();
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
         // groupBoxFTP
         // 
         this.groupBoxFTP.AccessibleDescription = null;
         this.groupBoxFTP.AccessibleName = null;
         resources.ApplyResources(this.groupBoxFTP, "groupBoxFTP");
         this.groupBoxFTP.BackgroundImage = null;
         this.groupBoxFTP.Controls.Add(this.checkSavePasswd);
         this.groupBoxFTP.Controls.Add(this.label8);
         this.groupBoxFTP.Controls.Add(this.buttonDelete);
         this.groupBoxFTP.Controls.Add(this.buttonRename);
         this.groupBoxFTP.Controls.Add(this.buttonNew);
         this.groupBoxFTP.Controls.Add(this.listSites);
         this.groupBoxFTP.Controls.Add(this.textBoxPort);
         this.groupBoxFTP.Controls.Add(this.label4);
         this.groupBoxFTP.Controls.Add(this.textBoxFile);
         this.groupBoxFTP.Controls.Add(this.textBoxPath);
         this.groupBoxFTP.Controls.Add(this.textBoxPasswd);
         this.groupBoxFTP.Controls.Add(this.textBoxUser);
         this.groupBoxFTP.Controls.Add(this.label6);
         this.groupBoxFTP.Controls.Add(this.label5);
         this.groupBoxFTP.Controls.Add(this.label3);
         this.groupBoxFTP.Controls.Add(this.label2);
         this.groupBoxFTP.Controls.Add(this.label1);
         this.groupBoxFTP.Controls.Add(this.textBoxServer);
         this.groupBoxFTP.Font = null;
         this.groupBoxFTP.Name = "groupBoxFTP";
         this.groupBoxFTP.TabStop = false;
         // 
         // checkSavePasswd
         // 
         this.checkSavePasswd.AccessibleDescription = null;
         this.checkSavePasswd.AccessibleName = null;
         resources.ApplyResources(this.checkSavePasswd, "checkSavePasswd");
         this.checkSavePasswd.BackgroundImage = null;
         this.checkSavePasswd.Font = null;
         this.checkSavePasswd.Name = "checkSavePasswd";
         this.checkSavePasswd.CheckedChanged += new System.EventHandler(this.checkSavePasswd_CheckedChanged);
         // 
         // label8
         // 
         this.label8.AccessibleDescription = null;
         this.label8.AccessibleName = null;
         resources.ApplyResources(this.label8, "label8");
         this.label8.Font = null;
         this.label8.Name = "label8";
         // 
         // buttonDelete
         // 
         this.buttonDelete.AccessibleDescription = null;
         this.buttonDelete.AccessibleName = null;
         resources.ApplyResources(this.buttonDelete, "buttonDelete");
         this.buttonDelete.BackgroundImage = null;
         this.buttonDelete.Font = null;
         this.buttonDelete.Name = "buttonDelete";
         this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
         // 
         // buttonRename
         // 
         this.buttonRename.AccessibleDescription = null;
         this.buttonRename.AccessibleName = null;
         resources.ApplyResources(this.buttonRename, "buttonRename");
         this.buttonRename.BackgroundImage = null;
         this.buttonRename.Font = null;
         this.buttonRename.Name = "buttonRename";
         this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
         // 
         // buttonNew
         // 
         this.buttonNew.AccessibleDescription = null;
         this.buttonNew.AccessibleName = null;
         resources.ApplyResources(this.buttonNew, "buttonNew");
         this.buttonNew.BackgroundImage = null;
         this.buttonNew.Font = null;
         this.buttonNew.Name = "buttonNew";
         this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
         // 
         // listSites
         // 
         this.listSites.AccessibleDescription = null;
         this.listSites.AccessibleName = null;
         resources.ApplyResources(this.listSites, "listSites");
         this.listSites.BackgroundImage = null;
         this.listSites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSite});
         this.listSites.Font = null;
         this.listSites.FullRowSelect = true;
         this.listSites.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.listSites.HideSelection = false;
         this.listSites.LabelEdit = true;
         this.listSites.MultiSelect = false;
         this.listSites.Name = "listSites";
         this.listSites.UseCompatibleStateImageBehavior = false;
         this.listSites.View = System.Windows.Forms.View.Details;
         this.listSites.SelectedIndexChanged += new System.EventHandler(this.listSites_SelectedIndexChanged);
         this.listSites.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listSites_AfterLabelEdit);
         // 
         // columnSite
         // 
         resources.ApplyResources(this.columnSite, "columnSite");
         // 
         // textBoxPort
         // 
         this.textBoxPort.AccessibleDescription = null;
         this.textBoxPort.AccessibleName = null;
         resources.ApplyResources(this.textBoxPort, "textBoxPort");
         this.textBoxPort.BackgroundImage = null;
         this.textBoxPort.Font = null;
         this.textBoxPort.Name = "textBoxPort";
         this.textBoxPort.TextChanged += new System.EventHandler(this.textBoxPort_TextChanged);
         // 
         // label4
         // 
         this.label4.AccessibleDescription = null;
         this.label4.AccessibleName = null;
         resources.ApplyResources(this.label4, "label4");
         this.label4.Font = null;
         this.label4.Name = "label4";
         // 
         // textBoxFile
         // 
         this.textBoxFile.AccessibleDescription = null;
         this.textBoxFile.AccessibleName = null;
         resources.ApplyResources(this.textBoxFile, "textBoxFile");
         this.textBoxFile.BackgroundImage = null;
         this.textBoxFile.Font = null;
         this.textBoxFile.Name = "textBoxFile";
         this.textBoxFile.TextChanged += new System.EventHandler(this.textBoxFile_TextChanged);
         // 
         // textBoxPath
         // 
         this.textBoxPath.AccessibleDescription = null;
         this.textBoxPath.AccessibleName = null;
         resources.ApplyResources(this.textBoxPath, "textBoxPath");
         this.textBoxPath.BackgroundImage = null;
         this.textBoxPath.Font = null;
         this.textBoxPath.Name = "textBoxPath";
         this.textBoxPath.TextChanged += new System.EventHandler(this.textBoxPath_TextChanged);
         // 
         // textBoxPasswd
         // 
         this.textBoxPasswd.AccessibleDescription = null;
         this.textBoxPasswd.AccessibleName = null;
         resources.ApplyResources(this.textBoxPasswd, "textBoxPasswd");
         this.textBoxPasswd.BackgroundImage = null;
         this.textBoxPasswd.Font = null;
         this.textBoxPasswd.Name = "textBoxPasswd";
         this.textBoxPasswd.TextChanged += new System.EventHandler(this.textBoxPasswd_TextChanged);
         // 
         // textBoxUser
         // 
         this.textBoxUser.AccessibleDescription = null;
         this.textBoxUser.AccessibleName = null;
         resources.ApplyResources(this.textBoxUser, "textBoxUser");
         this.textBoxUser.BackgroundImage = null;
         this.textBoxUser.Font = null;
         this.textBoxUser.Name = "textBoxUser";
         this.textBoxUser.TextChanged += new System.EventHandler(this.textBoxUser_TextChanged);
         // 
         // label6
         // 
         this.label6.AccessibleDescription = null;
         this.label6.AccessibleName = null;
         resources.ApplyResources(this.label6, "label6");
         this.label6.Font = null;
         this.label6.Name = "label6";
         // 
         // label5
         // 
         this.label5.AccessibleDescription = null;
         this.label5.AccessibleName = null;
         resources.ApplyResources(this.label5, "label5");
         this.label5.Font = null;
         this.label5.Name = "label5";
         // 
         // label3
         // 
         this.label3.AccessibleDescription = null;
         this.label3.AccessibleName = null;
         resources.ApplyResources(this.label3, "label3");
         this.label3.Font = null;
         this.label3.Name = "label3";
         // 
         // label2
         // 
         this.label2.AccessibleDescription = null;
         this.label2.AccessibleName = null;
         resources.ApplyResources(this.label2, "label2");
         this.label2.Font = null;
         this.label2.Name = "label2";
         // 
         // label1
         // 
         this.label1.AccessibleDescription = null;
         this.label1.AccessibleName = null;
         resources.ApplyResources(this.label1, "label1");
         this.label1.Font = null;
         this.label1.Name = "label1";
         // 
         // textBoxServer
         // 
         this.textBoxServer.AccessibleDescription = null;
         this.textBoxServer.AccessibleName = null;
         resources.ApplyResources(this.textBoxServer, "textBoxServer");
         this.textBoxServer.BackgroundImage = null;
         this.textBoxServer.Font = null;
         this.textBoxServer.Name = "textBoxServer";
         this.textBoxServer.TextChanged += new System.EventHandler(this.textBoxServer_TextChanged);
         // 
         // buttonOk
         // 
         this.buttonOk.AccessibleDescription = null;
         this.buttonOk.AccessibleName = null;
         resources.ApplyResources(this.buttonOk, "buttonOk");
         this.buttonOk.BackgroundImage = null;
         this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonOk.Font = null;
         this.buttonOk.Name = "buttonOk";
         this.buttonOk.Click += new System.EventHandler(this.buttonSave_Click);
         // 
         // checkPassiveFTP
         // 
         this.checkPassiveFTP.AccessibleDescription = null;
         this.checkPassiveFTP.AccessibleName = null;
         resources.ApplyResources(this.checkPassiveFTP, "checkPassiveFTP");
         this.checkPassiveFTP.BackgroundImage = null;
         this.checkPassiveFTP.Font = null;
         this.checkPassiveFTP.Name = "checkPassiveFTP";
         this.checkPassiveFTP.CheckedChanged += new System.EventHandler(this.checkPassiveFTP_CheckedChanged);
         // 
         // FormFTPSites
         // 
         this.AcceptButton = this.buttonOk;
         this.AccessibleDescription = null;
         this.AccessibleName = null;
         resources.ApplyResources(this, "$this");
         this.BackgroundImage = null;
         this.CancelButton = this.buttonCancel;
         this.Controls.Add(this.checkPassiveFTP);
         this.Controls.Add(this.buttonOk);
         this.Controls.Add(this.groupBoxFTP);
         this.Controls.Add(this.buttonCancel);
         this.Font = null;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormFTPSites";
         this.ShowInTaskbar = false;
         this.Activated += new System.EventHandler(this.FormFTPSites_Activated);
         this.groupBoxFTP.ResumeLayout(false);
         this.groupBoxFTP.PerformLayout();
         this.ResumeLayout(false);

      }
		#endregion


      private void buttonNew_Click(object sender, System.EventArgs e)
      {
         int defaultPostfix = 0;
         foreach(ListViewItem item in listSites.Items)
         {
            if(item.Text.StartsWith("New FTP Site"))
            {
               string numStr = item.Text.Substring(13).Trim();
               int num = Int32.Parse(numStr);
               if(num > defaultPostfix)
                  defaultPostfix = num;
            }
         }

         defaultPostfix++;

         string sectionName = "New FTP Site " + defaultPostfix.ToString();
         ListViewItem addedItem = listSites.Items.Add(sectionName);

         addedItem.BeginEdit();
      }

      private void buttonRename_Click(object sender, System.EventArgs e)
      {
         if(listSites.SelectedItems.Count == 1)
         {
            ListViewItem selItem = listSites.SelectedItems[0];

            selItem.BeginEdit();
         }
      }

      private void buttonDelete_Click(object sender, System.EventArgs e)
      {
         if(listSites.SelectedItems.Count == 1)
         {
            ListViewItem selItem = listSites.SelectedItems[0];

            ftpConfig.DeleteSection(selItem.Text);

            listSites.Items.Remove(selItem);
         }
      }

      private void buttonSave_Click(object sender, System.EventArgs e)
      {
         //---
         // Save settings to configuration 
         //---
         ftpConfig.Save(); 
         Close();
      }

      public string section()
      {
         if(listSites.SelectedItems.Count == 1)
         {
            ListViewItem selItem = listSites.SelectedItems[0];

            return selItem.Text;
         }
         
         return "";
      }

      private void listSites_AfterLabelEdit(object sender, System.Windows.Forms.LabelEditEventArgs e)
      {
         if(e.Label == null)
            return;

         //---
         // If the name already exists, does not allow renaming
         //---
         if(ftpConfig.HasSection(e.Label))
         {
            e.CancelEdit = true;
            return;
         }

         //---
         // Rename the section in the configuration file
         //---
         ftpConfig.RenameSection(listSites.Items[e.Item].Text,  e.Label);
      }

      private void listSites_SelectedIndexChanged(object sender, System.EventArgs e)
      {
         bool enable = listSites.SelectedItems.Count == 1;

         disableChanges = true;

         if(enable)
         {
            string sectionName = section();
            if(ftpConfig.HasSection(sectionName))
            {
               textBoxServer.Text = ftpConfig.GetValue(sectionName, "ftpServer");
               textBoxPort.Text = ftpConfig.GetValue(sectionName, "ftpPort", "21");
               textBoxUser.Text = Encryption.Decrypt(ftpConfig.GetValue(sectionName, "ftpUser"));
               textBoxPasswd.Text = Encryption.Decrypt(ftpConfig.GetValue(sectionName, "ftpPassword"));
               textBoxPath.Text = ftpConfig.GetValue(sectionName, "ftpRemotePath", ".");
               textBoxFile.Text = ftpConfig.GetValue(sectionName, "ftpRemoteFile", "news.xml");
               checkSavePasswd.Checked = (textBoxPasswd.Text != "");
            }
         }
         else
         {
            textBoxServer.Text = "";    
            textBoxPort.Text = "21";      
            textBoxUser.Text = "";
            textBoxPasswd.Text = "";
            textBoxPath.Text = ".";
            textBoxFile.Text = "";
         }

         disableChanges = false;

         textBoxServer.Enabled = enable;
         textBoxPort.Enabled = enable;
         textBoxUser.Enabled = enable;
         textBoxPasswd.Enabled = enable;
         textBoxPath.Enabled = enable;
         textBoxFile.Enabled = enable;
         buttonRename.Enabled = enable;
         buttonDelete.Enabled = enable;
         buttonOk.Enabled = enable;
         checkSavePasswd.Enabled = enable;
      }


      private void textBoxServer_TextChanged(object sender, System.EventArgs e)
      {
         if(listSites.SelectedItems.Count == 1 && !disableChanges)
            ftpConfig.SetValue(section(), "ftpServer",textBoxServer.Text);
      }

      private void textBoxUser_TextChanged(object sender, System.EventArgs e)
      {
         if(listSites.SelectedItems.Count == 1 && !disableChanges)
            ftpConfig.SetValue(section(), "ftpUser",Encryption.Encrypt(textBoxUser.Text));
      }

      private void textBoxPasswd_TextChanged(object sender, System.EventArgs e)
      {
         if(listSites.SelectedItems.Count == 1 && !disableChanges)
            if(checkSavePasswd.Checked)
               ftpConfig.SetValue(section(), "ftpPassword",Encryption.Encrypt(textBoxPasswd.Text));
      }

      private void textBoxPath_TextChanged(object sender, System.EventArgs e)
      {
         if(listSites.SelectedItems.Count == 1 && !disableChanges)
            ftpConfig.SetValue(section(), "ftpRemotePath",textBoxPath.Text);
      }

      private void textBoxPort_TextChanged(object sender, System.EventArgs e)
      {
         if(listSites.SelectedItems.Count == 1 && !disableChanges)
            ftpConfig.SetValue(section(), "ftpPort",textBoxPort.Text);
      }


      private void textBoxFile_TextChanged(object sender, System.EventArgs e)
      {
         if(listSites.SelectedItems.Count == 1 && !disableChanges)
            ftpConfig.SetValue(section(), "ftpRemoteFile",textBoxFile.Text);
      }

      private void checkSavePasswd_CheckedChanged(object sender, System.EventArgs e)
      {
         if(listSites.SelectedItems.Count == 1 && !disableChanges)
         {
            if(checkSavePasswd.Checked)
            {
               ftpConfig.SetValue(section(), "ftpPassword", Encryption.Encrypt(textBoxPasswd.Text));
            }
            else
            {
               ftpConfig.SetValue(section(), "ftpPassword", "");
            }
         }
      }

      private void checkPassiveFTP_CheckedChanged(object sender, System.EventArgs e)
      {
         string flag = "0";
         if(checkPassiveFTP.Checked)
            flag = "1";

         ftpConfig.SetValue("__general__", "ftpPassiveFtp", flag);
      }

      private void FormFTPSites_Activated(object sender, System.EventArgs e)
      {
         if(textBoxUser.Text == "")
            textBoxUser.Focus();
         else if(textBoxPasswd.Text == "")
            textBoxPasswd.Focus();     
      }





	}
}
