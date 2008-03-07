using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace RSSBuilder
{
	/// <summary>
	/// Summary description for FormSaveHTML.
	/// </summary>
	public class FormSaveHTML : System.Windows.Forms.Form
	{
      private RSSFeed rssFeed = null;

      private System.Windows.Forms.NumericUpDown numItemCount;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.CheckBox checkTitle;
      private System.Windows.Forms.CheckBox checkImage;
      private System.Windows.Forms.CheckBox checkDate;
      private System.Windows.Forms.Button buttonOk;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.ComboBox comboStyle;
      private System.Windows.Forms.SaveFileDialog saveFileDialog;
      private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormSaveHTML(RSSFeed rssF, string feedFileName)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

         //
         // 
         //
         rssFeed = rssF;

         if(feedFileName != "")
            saveFileDialog.FileName = Path.GetFileNameWithoutExtension(feedFileName) + ".htm";
         else
            saveFileDialog.FileName = "";
         
         numItemCount.Value = rssFeed.NewsItemCount;

         string appPath = Application.StartupPath;
         string[] cssFiles = Directory.GetFiles(appPath, "*.css");
         comboStyle.Items.Clear();

         foreach (string cssFileName in cssFiles)
         {
            comboStyle.Items.Add(Path.GetFileName(cssFileName));
         }

         if(comboStyle.Items.Count > 0)
            comboStyle.SelectedIndex = 0;
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSaveHTML));
         this.numItemCount = new System.Windows.Forms.NumericUpDown();
         this.label7 = new System.Windows.Forms.Label();
         this.checkTitle = new System.Windows.Forms.CheckBox();
         this.checkImage = new System.Windows.Forms.CheckBox();
         this.checkDate = new System.Windows.Forms.CheckBox();
         this.buttonOk = new System.Windows.Forms.Button();
         this.buttonCancel = new System.Windows.Forms.Button();
         this.label2 = new System.Windows.Forms.Label();
         this.comboStyle = new System.Windows.Forms.ComboBox();
         this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
         this.label1 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.numItemCount)).BeginInit();
         this.SuspendLayout();
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
         // 
         // label7
         // 
         this.label7.AccessibleDescription = null;
         this.label7.AccessibleName = null;
         resources.ApplyResources(this.label7, "label7");
         this.label7.Font = null;
         this.label7.Name = "label7";
         // 
         // checkTitle
         // 
         this.checkTitle.AccessibleDescription = null;
         this.checkTitle.AccessibleName = null;
         resources.ApplyResources(this.checkTitle, "checkTitle");
         this.checkTitle.BackgroundImage = null;
         this.checkTitle.Font = null;
         this.checkTitle.Name = "checkTitle";
         // 
         // checkImage
         // 
         this.checkImage.AccessibleDescription = null;
         this.checkImage.AccessibleName = null;
         resources.ApplyResources(this.checkImage, "checkImage");
         this.checkImage.BackgroundImage = null;
         this.checkImage.Font = null;
         this.checkImage.Name = "checkImage";
         // 
         // checkDate
         // 
         this.checkDate.AccessibleDescription = null;
         this.checkDate.AccessibleName = null;
         resources.ApplyResources(this.checkDate, "checkDate");
         this.checkDate.BackgroundImage = null;
         this.checkDate.Font = null;
         this.checkDate.Name = "checkDate";
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
         this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
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
         // label2
         // 
         this.label2.AccessibleDescription = null;
         this.label2.AccessibleName = null;
         resources.ApplyResources(this.label2, "label2");
         this.label2.Font = null;
         this.label2.Name = "label2";
         // 
         // comboStyle
         // 
         this.comboStyle.AccessibleDescription = null;
         this.comboStyle.AccessibleName = null;
         resources.ApplyResources(this.comboStyle, "comboStyle");
         this.comboStyle.BackgroundImage = null;
         this.comboStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.comboStyle.Font = null;
         this.comboStyle.Items.AddRange(new object[] {
            resources.GetString("comboStyle.Items")});
         this.comboStyle.Name = "comboStyle";
         // 
         // saveFileDialog
         // 
         this.saveFileDialog.DefaultExt = "rss";
         resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
         // 
         // label1
         // 
         this.label1.AccessibleDescription = null;
         this.label1.AccessibleName = null;
         resources.ApplyResources(this.label1, "label1");
         this.label1.Name = "label1";
         // 
         // FormSaveHTML
         // 
         this.AcceptButton = this.buttonOk;
         this.AccessibleDescription = null;
         this.AccessibleName = null;
         resources.ApplyResources(this, "$this");
         this.BackgroundImage = null;
         this.CancelButton = this.buttonCancel;
         this.Controls.Add(this.label1);
         this.Controls.Add(this.comboStyle);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.buttonOk);
         this.Controls.Add(this.checkDate);
         this.Controls.Add(this.checkImage);
         this.Controls.Add(this.checkTitle);
         this.Controls.Add(this.numItemCount);
         this.Controls.Add(this.label7);
         this.Font = null;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormSaveHTML";
         this.ShowInTaskbar = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         ((System.ComponentModel.ISupportInitialize)(this.numItemCount)).EndInit();
         this.ResumeLayout(false);

      }
		#endregion

      private void buttonOk_Click(object sender, System.EventArgs e)
      {
         if(saveFileDialog.ShowDialog() == DialogResult.OK)
         {
            try
            {
               //
               // Copy CSS file from startup path to destination directory
               //
               string cssFolder = Application.StartupPath;
               string destFolder = Path.GetDirectoryName(saveFileDialog.FileName);
               if(comboStyle.Items.Count > 0)
               {
                  string cssFile = comboStyle.Items[ comboStyle.SelectedIndex ].ToString();
                  File.Copy(cssFolder+@"\"+cssFile, destFolder+@"\rss_style.css", true);
               }

               rssFeed.saveFeedAsHTML(saveFileDialog.FileName, (int)numItemCount.Value, 
                                      checkTitle.Checked,checkImage.Checked, checkDate.Checked);
               saveFileDialog.InitialDirectory = "";
            }
            catch
            {
               MessageBox.Show(this, "Unable to save to this location", "RSS Builder", 
                  MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
         } 
      }


	}
}
