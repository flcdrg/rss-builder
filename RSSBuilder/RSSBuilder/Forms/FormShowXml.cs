using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace RSSBuilder
{
	/// <summary>
	/// Summary description for FormShowXml.
	/// </summary>
	public class FormShowXml : System.Windows.Forms.Form
	{
      private System.Windows.Forms.TextBox textXml;
      private System.Windows.Forms.Panel pnlBottom;
      private System.Windows.Forms.Button btnOk;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormShowXml(RSSFeed rssFeed)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

         string tempFeedFileName = Path.GetTempFileName();

            rssFeed.saveFeed(tempFeedFileName);

            StreamReader sr = new StreamReader(tempFeedFileName);
            textXml.Text = sr.ReadToEnd();
            textXml.Select(0,0);
            sr.Close();

            File.Delete(tempFeedFileName);

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
         System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormShowXml));
         this.textXml = new System.Windows.Forms.TextBox();
         this.pnlBottom = new System.Windows.Forms.Panel();
         this.btnOk = new System.Windows.Forms.Button();
         this.pnlBottom.SuspendLayout();
         this.SuspendLayout();
         // 
         // textXml
         // 
         this.textXml.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.textXml.BackColor = System.Drawing.Color.White;
         this.textXml.Location = new System.Drawing.Point(0, 0);
         this.textXml.Multiline = true;
         this.textXml.Name = "textXml";
         this.textXml.ReadOnly = true;
         this.textXml.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.textXml.Size = new System.Drawing.Size(560, 312);
         this.textXml.TabIndex = 0;
         this.textXml.Text = "";
         // 
         // pnlBottom
         // 
         this.pnlBottom.Controls.Add(this.btnOk);
         this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.pnlBottom.Location = new System.Drawing.Point(0, 315);
         this.pnlBottom.Name = "pnlBottom";
         this.pnlBottom.Size = new System.Drawing.Size(560, 40);
         this.pnlBottom.TabIndex = 2;
         // 
         // btnOk
         // 
         this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOk.Location = new System.Drawing.Point(472, 8);
         this.btnOk.Name = "btnOk";
         this.btnOk.TabIndex = 2;
         this.btnOk.Text = "Close";
         // 
         // FormShowXml
         // 
         this.AcceptButton = this.btnOk;
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(560, 355);
         this.Controls.Add(this.pnlBottom);
         this.Controls.Add(this.textXml);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormShowXml";
         this.ShowInTaskbar = false;
         this.Text = "View XML";
         this.pnlBottom.ResumeLayout(false);
         this.ResumeLayout(false);

      }
		#endregion
	}
}
