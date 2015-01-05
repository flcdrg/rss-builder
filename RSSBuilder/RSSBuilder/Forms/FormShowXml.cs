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
      private Button btnValidate;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShowXml));
            this.textXml = new System.Windows.Forms.TextBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
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
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnValidate);
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
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Close";
            // 
            // btnValidate
            // 
            this.btnValidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidate.Location = new System.Drawing.Point(12, 8);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(75, 23);
            this.btnValidate.TabIndex = 3;
            this.btnValidate.Text = "Validate";
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
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
            this.PerformLayout();

      }
		#endregion

        private void btnValidate_Click(object sender, EventArgs e)
        {

            using (var client = new System.Net.WebClient())
            {
                client.Headers["User-Agent"] = "RSS Builder by B!Soft";

                string tempFileName = Path.GetTempFileName();
                File.Move(tempFileName, tempFileName + ".html");
                tempFileName = tempFileName + ".html";

                var values = new System.Collections.Specialized.NameValueCollection();
                values["manual"] = "1";
                values["rawdata"] = textXml.Text; // UTF-8 in

                var response = client.UploadValues("http://validator.w3.org/feed/check.cgi", values);

                var responseString = System.Text.Encoding.UTF8.GetString(response); // UTF-8 out

                responseString = responseString.Replace(@"<head>", "<head><base href=\"http://validator.w3.org/feed/check.cgi\" target=\"_blank\">"); // make online validator's CSS work on local file
                File.WriteAllText(tempFileName, responseString); 

                var sInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C start " + tempFileName);
                sInfo.CreateNoWindow = true;
                sInfo.UseShellExecute = false;
                System.Diagnostics.Process.Start(sInfo);
            }
            
        }
	}
}
