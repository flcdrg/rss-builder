using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using System.Reflection;



namespace RSSBuilder
{
	/// <summary>
	/// Summary description for FormAbout.
	/// </summary>
	public class FormAbout : System.Windows.Forms.Form
	{
      private System.Windows.Forms.Button buttonOk;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.PictureBox pictureBox1;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Label labelVersion;
      private System.Windows.Forms.LinkLabel linkLabel;
      private System.Windows.Forms.Label label3;
      private Label label1;
      private Label label2;
      private Label label4;
      private Label label6;
      private Label label7;
      private Label label8;
      private Label label9;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

         Version version = Assembly.GetExecutingAssembly().GetName().Version;

         labelVersion.Text = "Version " + version.Major.ToString() + 
                             "." + version.Minor.ToString() + 
                             "." + version.Build.ToString();
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
         this.buttonOk = new System.Windows.Forms.Button();
         this.label5 = new System.Windows.Forms.Label();
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         this.panel1 = new System.Windows.Forms.Panel();
         this.labelVersion = new System.Windows.Forms.Label();
         this.linkLabel = new System.Windows.Forms.LinkLabel();
         this.label3 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
         this.panel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // buttonOk
         // 
         this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonOk.Location = new System.Drawing.Point(211, 156);
         this.buttonOk.Name = "buttonOk";
         this.buttonOk.Size = new System.Drawing.Size(75, 23);
         this.buttonOk.TabIndex = 0;
         this.buttonOk.Text = "OK";
         // 
         // label5
         // 
         this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.label5.Location = new System.Drawing.Point(6, 192);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(280, 56);
         this.label5.TabIndex = 6;
         this.label5.Tag = "";
         this.label5.Text = "This software is FREEWARE.  B!Soft doesn\'t take any responsibility for any damage" +
             " to your computer caused by installing or inappropriate use of this program. ";
         // 
         // pictureBox1
         // 
         this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
         this.pictureBox1.Location = new System.Drawing.Point(16, 8);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(32, 32);
         this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
         this.pictureBox1.TabIndex = 7;
         this.pictureBox1.TabStop = false;
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.White;
         this.panel1.Controls.Add(this.labelVersion);
         this.panel1.Controls.Add(this.pictureBox1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(292, 48);
         this.panel1.TabIndex = 8;
         // 
         // labelVersion
         // 
         this.labelVersion.Location = new System.Drawing.Point(72, 16);
         this.labelVersion.Name = "labelVersion";
         this.labelVersion.Size = new System.Drawing.Size(216, 16);
         this.labelVersion.TabIndex = 7;
         this.labelVersion.Text = "Version 1.1";
         // 
         // linkLabel
         // 
         this.linkLabel.Location = new System.Drawing.Point(14, 72);
         this.linkLabel.Name = "linkLabel";
         this.linkLabel.Size = new System.Drawing.Size(208, 16);
         this.linkLabel.TabIndex = 11;
         this.linkLabel.TabStop = true;
         this.linkLabel.Text = "http://home.hetnet.nl/~bsoft/rssbuilder";
         this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
         // 
         // label3
         // 
         this.label3.Location = new System.Drawing.Point(14, 56);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(242, 23);
         this.label3.TabIndex = 10;
         this.label3.Text = "Copyright 2004-2006,  B!Soft  ";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(14, 104);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(41, 13);
         this.label1.TabIndex = 12;
         this.label1.Text = "Author:";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(14, 128);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(68, 13);
         this.label2.TabIndex = 13;
         this.label2.Text = "Icon design: ";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(14, 153);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(62, 13);
         this.label4.TabIndex = 14;
         this.label4.Text = "Translation:";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(81, 104);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(70, 13);
         this.label6.TabIndex = 15;
         this.label6.Text = "Wim Bokkers";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(81, 128);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(48, 13);
         this.label7.TabIndex = 16;
         this.label7.Text = "Ivo Blom";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(81, 153);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(82, 13);
         this.label8.TabIndex = 17;
         this.label8.Text = "Buchtič (Czech)";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(81, 166);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(117, 13);
         this.label9.TabIndex = 18;
         this.label9.Text = "Amos Hooder (German)";
         // 
         // FormAbout
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(292, 257);
         this.Controls.Add(this.label9);
         this.Controls.Add(this.label8);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.linkLabel);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.buttonOk);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormAbout";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "About RSS Builder";
         this.Load += new System.EventHandler(this.FormAbout_Load);
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
		#endregion

      private void linkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
      {
         try
         {
            System.Diagnostics.Process.Start(linkLabel.Text);
         }
         catch
         {
         }
      }
   
      private void FormAbout_Load(object sender, System.EventArgs e)
      {

         
      }
	}
}
