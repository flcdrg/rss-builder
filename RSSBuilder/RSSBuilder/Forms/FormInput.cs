using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RSSBuilder
{
	/// <summary>
	/// Summary description for FormInput.
	/// </summary>
	public class FormInput : System.Windows.Forms.Form
	{
      public string UserInput
      {
         get { return textBox.Text; }
         set { textBox.Text = value; }
      }

      private System.Windows.Forms.TextBox textBox;
      private System.Windows.Forms.Button buttonOk;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormInput()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
         this.textBox = new System.Windows.Forms.TextBox();
         this.buttonOk = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // textBox
         // 
         this.textBox.Location = new System.Drawing.Point(8, 8);
         this.textBox.Name = "textBox";
         this.textBox.Size = new System.Drawing.Size(272, 20);
         this.textBox.TabIndex = 0;
         this.textBox.Text = "";
         // 
         // buttonOk
         // 
         this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonOk.Location = new System.Drawing.Point(205, 40);
         this.buttonOk.Name = "buttonOk";
         this.buttonOk.TabIndex = 1;
         this.buttonOk.Text = "OK";
         // 
         // FormInput
         // 
         this.AcceptButton = this.buttonOk;
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(292, 77);
         this.ControlBox = false;
         this.Controls.Add(this.buttonOk);
         this.Controls.Add(this.textBox);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormInput";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "URL";
         this.ResumeLayout(false);

      }
		#endregion
	}
}
