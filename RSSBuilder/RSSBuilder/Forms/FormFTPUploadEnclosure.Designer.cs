namespace RSSBuilder
{
   partial class FormFTPUploadEnclosure
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.label1 = new System.Windows.Forms.Label();
         this.buttonSiteManager = new System.Windows.Forms.Button();
         this.comboSites = new System.Windows.Forms.ComboBox();
         this.labelMsg = new System.Windows.Forms.Label();
         this.buttonClose = new System.Windows.Forms.Button();
         this.progressBar = new System.Windows.Forms.ProgressBar();
         this.textFileName = new System.Windows.Forms.TextBox();
         this.openEnclosureDialog = new System.Windows.Forms.OpenFileDialog();
         this.buttonUpload = new System.Windows.Forms.Button();
         this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
         this.checkCloseWhenFinished = new System.Windows.Forms.CheckBox();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.label1.Location = new System.Drawing.Point(16, 20);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(64, 16);
         this.label1.TabIndex = 33;
         this.label1.Text = "FTP Site";
         // 
         // buttonSiteManager
         // 
         this.buttonSiteManager.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.buttonSiteManager.Location = new System.Drawing.Point(314, 14);
         this.buttonSiteManager.Name = "buttonSiteManager";
         this.buttonSiteManager.Size = new System.Drawing.Size(96, 23);
         this.buttonSiteManager.TabIndex = 32;
         this.buttonSiteManager.Text = "Site Manager...";
         this.buttonSiteManager.Click += new System.EventHandler(this.buttonSiteManager_Click);
         // 
         // comboSites
         // 
         this.comboSites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.comboSites.ItemHeight = 13;
         this.comboSites.Location = new System.Drawing.Point(86, 15);
         this.comboSites.Name = "comboSites";
         this.comboSites.Size = new System.Drawing.Size(218, 21);
         this.comboSites.TabIndex = 31;
         this.comboSites.SelectedIndexChanged += new System.EventHandler(this.comboSites_SelectedIndexChanged);
         // 
         // labelMsg
         // 
         this.labelMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.labelMsg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.labelMsg.Location = new System.Drawing.Point(16, 90);
         this.labelMsg.Name = "labelMsg";
         this.labelMsg.Size = new System.Drawing.Size(392, 16);
         this.labelMsg.TabIndex = 30;
         // 
         // buttonClose
         // 
         this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.buttonClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.buttonClose.Location = new System.Drawing.Point(326, 119);
         this.buttonClose.Name = "buttonClose";
         this.buttonClose.Size = new System.Drawing.Size(82, 23);
         this.buttonClose.TabIndex = 27;
         this.buttonClose.Text = "Cancel";
         this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
         // 
         // progressBar
         // 
         this.progressBar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.progressBar.Location = new System.Drawing.Point(16, 71);
         this.progressBar.Maximum = 5;
         this.progressBar.Name = "progressBar";
         this.progressBar.Size = new System.Drawing.Size(392, 16);
         this.progressBar.Step = 1;
         this.progressBar.TabIndex = 29;
         // 
         // textFileName
         // 
         this.textFileName.Location = new System.Drawing.Point(111, 44);
         this.textFileName.Name = "textFileName";
         this.textFileName.ReadOnly = true;
         this.textFileName.Size = new System.Drawing.Size(299, 20);
         this.textFileName.TabIndex = 35;
         // 
         // openEnclosureDialog
         // 
         this.openEnclosureDialog.Title = "Open Enclosure";
         // 
         // buttonUpload
         // 
         this.buttonUpload.Location = new System.Drawing.Point(16, 42);
         this.buttonUpload.Name = "buttonUpload";
         this.buttonUpload.Size = new System.Drawing.Size(89, 23);
         this.buttonUpload.TabIndex = 36;
         this.buttonUpload.Text = "Upload File ...";
         this.buttonUpload.UseVisualStyleBackColor = true;
         this.buttonUpload.Click += new System.EventHandler(this.buttonBrowse_Click);
         // 
         // backgroundWorker
         // 
         this.backgroundWorker.WorkerReportsProgress = true;
         this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
         this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
         this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
         // 
         // checkCloseWhenFinished
         // 
         this.checkCloseWhenFinished.AutoSize = true;
         this.checkCloseWhenFinished.Checked = true;
         this.checkCloseWhenFinished.CheckState = System.Windows.Forms.CheckState.Checked;
         this.checkCloseWhenFinished.Location = new System.Drawing.Point(16, 123);
         this.checkCloseWhenFinished.Name = "checkCloseWhenFinished";
         this.checkCloseWhenFinished.Size = new System.Drawing.Size(120, 17);
         this.checkCloseWhenFinished.TabIndex = 37;
         this.checkCloseWhenFinished.Text = "Close when finished";
         this.checkCloseWhenFinished.UseVisualStyleBackColor = true;
         // 
         // FormFTPUploadEnclosure
         // 
         this.AcceptButton = this.buttonUpload;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.buttonClose;
         this.ClientSize = new System.Drawing.Size(424, 151);
         this.ControlBox = false;
         this.Controls.Add(this.checkCloseWhenFinished);
         this.Controls.Add(this.buttonUpload);
         this.Controls.Add(this.textFileName);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.buttonSiteManager);
         this.Controls.Add(this.comboSites);
         this.Controls.Add(this.labelMsg);
         this.Controls.Add(this.buttonClose);
         this.Controls.Add(this.progressBar);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.Name = "FormFTPUploadEnclosure";
         this.Opacity = 0.95;
         this.ShowIcon = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = " Upload File for";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button buttonSiteManager;
      private System.Windows.Forms.ComboBox comboSites;
      private System.Windows.Forms.Label labelMsg;
      private System.Windows.Forms.Button buttonClose;
      private System.Windows.Forms.ProgressBar progressBar;
      private System.Windows.Forms.TextBox textFileName;
      private System.Windows.Forms.OpenFileDialog openEnclosureDialog;
      private System.Windows.Forms.Button buttonUpload;
      private System.ComponentModel.BackgroundWorker backgroundWorker;
      private System.Windows.Forms.CheckBox checkCloseWhenFinished;
   }
}