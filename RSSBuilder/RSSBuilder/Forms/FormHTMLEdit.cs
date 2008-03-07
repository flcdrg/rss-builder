using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace RSSBuilder
{
	/// <summary>
	/// Summary description for FormHTMLEdit.
	/// </summary>
	public class FormHTMLEdit : System.Windows.Forms.Form
	{
      private onlyconnect.HtmlEditor htmlEdit;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
      private System.Windows.Forms.NumericUpDown numCol;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.NumericUpDown numRow;
      private System.Windows.Forms.Button btnTable;
      private System.Windows.Forms.Button buttonOk;
      private System.Windows.Forms.NumericUpDown numBorder;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.GroupBox groupBoxSelection;
      private System.Windows.Forms.Button btnList;
      private System.Windows.Forms.Button btnUnderline;
      private System.Windows.Forms.Button btnImg;
      private System.Windows.Forms.Button btnURL;
      private System.Windows.Forms.Button btnBold;
      private System.Windows.Forms.Button btnItalic;
      private System.Windows.Forms.Button btnRight;
      private System.Windows.Forms.Button btnCenter;
      private System.Windows.Forms.Button btnLeft;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label7;

      public string textDesc = "";

      private void applyTagToSelection(string tagName)
      {
         mshtml.IHTMLElement el = null;
         try
         {
            el = htmlEdit.CurrentElement;
            if (el != null && el.tagName != "BODY")
            {
               el.outerHTML = "<"+tagName+">" + el.innerHTML + "</"+tagName+">";
            }
         }
         catch(Exception exc)
         {
            string sMessage = "Could not apply "+ tagName + ": " + exc.Message;
            if (el != null) 
            {
               sMessage += " The possible reason is that the tag is " + el.tagName + ".";
            }
            MessageBox.Show(sMessage);
         }

      }

		public FormHTMLEdit(string textDesc)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
         
         string s = "<html><head></head><body >" + textDesc + "</body></html>";

         byte[] preamble = UnicodeEncoding.Unicode.GetPreamble();
         String byteOrderMark = UnicodeEncoding.Unicode.GetString(preamble, 0, preamble.Length);
         s = byteOrderMark + s;

         htmlEdit.LoadDocument(s);
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHTMLEdit));
         this.numCol = new System.Windows.Forms.NumericUpDown();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.numRow = new System.Windows.Forms.NumericUpDown();
         this.btnTable = new System.Windows.Forms.Button();
         this.buttonOk = new System.Windows.Forms.Button();
         this.numBorder = new System.Windows.Forms.NumericUpDown();
         this.label4 = new System.Windows.Forms.Label();
         this.groupBoxSelection = new System.Windows.Forms.GroupBox();
         this.btnRight = new System.Windows.Forms.Button();
         this.btnCenter = new System.Windows.Forms.Button();
         this.btnLeft = new System.Windows.Forms.Button();
         this.btnList = new System.Windows.Forms.Button();
         this.btnUnderline = new System.Windows.Forms.Button();
         this.btnImg = new System.Windows.Forms.Button();
         this.btnURL = new System.Windows.Forms.Button();
         this.btnItalic = new System.Windows.Forms.Button();
         this.btnBold = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.htmlEdit = new onlyconnect.HtmlEditor();
         ((System.ComponentModel.ISupportInitialize)(this.numCol)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRow)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numBorder)).BeginInit();
         this.groupBoxSelection.SuspendLayout();
         this.SuspendLayout();
         // 
         // numCol
         // 
         this.numCol.AccessibleDescription = null;
         this.numCol.AccessibleName = null;
         resources.ApplyResources(this.numCol, "numCol");
         this.numCol.Font = null;
         this.numCol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numCol.Name = "numCol";
         this.numCol.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
         // 
         // label2
         // 
         this.label2.AccessibleDescription = null;
         this.label2.AccessibleName = null;
         resources.ApplyResources(this.label2, "label2");
         this.label2.Font = null;
         this.label2.Name = "label2";
         // 
         // label3
         // 
         this.label3.AccessibleDescription = null;
         this.label3.AccessibleName = null;
         resources.ApplyResources(this.label3, "label3");
         this.label3.Font = null;
         this.label3.Name = "label3";
         // 
         // numRow
         // 
         this.numRow.AccessibleDescription = null;
         this.numRow.AccessibleName = null;
         resources.ApplyResources(this.numRow, "numRow");
         this.numRow.Font = null;
         this.numRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numRow.Name = "numRow";
         this.numRow.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
         // 
         // btnTable
         // 
         this.btnTable.AccessibleDescription = null;
         this.btnTable.AccessibleName = null;
         resources.ApplyResources(this.btnTable, "btnTable");
         this.btnTable.BackgroundImage = null;
         this.btnTable.Font = null;
         this.btnTable.Name = "btnTable";
         this.btnTable.Click += new System.EventHandler(this.btnTable_Click);
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
         // numBorder
         // 
         this.numBorder.AccessibleDescription = null;
         this.numBorder.AccessibleName = null;
         resources.ApplyResources(this.numBorder, "numBorder");
         this.numBorder.Font = null;
         this.numBorder.Name = "numBorder";
         // 
         // label4
         // 
         this.label4.AccessibleDescription = null;
         this.label4.AccessibleName = null;
         resources.ApplyResources(this.label4, "label4");
         this.label4.Font = null;
         this.label4.Name = "label4";
         // 
         // groupBoxSelection
         // 
         this.groupBoxSelection.AccessibleDescription = null;
         this.groupBoxSelection.AccessibleName = null;
         resources.ApplyResources(this.groupBoxSelection, "groupBoxSelection");
         this.groupBoxSelection.BackgroundImage = null;
         this.groupBoxSelection.Controls.Add(this.btnRight);
         this.groupBoxSelection.Controls.Add(this.btnCenter);
         this.groupBoxSelection.Controls.Add(this.btnLeft);
         this.groupBoxSelection.Controls.Add(this.btnList);
         this.groupBoxSelection.Controls.Add(this.btnUnderline);
         this.groupBoxSelection.Controls.Add(this.btnImg);
         this.groupBoxSelection.Controls.Add(this.btnURL);
         this.groupBoxSelection.Controls.Add(this.btnItalic);
         this.groupBoxSelection.Controls.Add(this.btnBold);
         this.groupBoxSelection.Font = null;
         this.groupBoxSelection.Name = "groupBoxSelection";
         this.groupBoxSelection.TabStop = false;
         // 
         // btnRight
         // 
         this.btnRight.AccessibleDescription = null;
         this.btnRight.AccessibleName = null;
         resources.ApplyResources(this.btnRight, "btnRight");
         this.btnRight.BackgroundImage = null;
         this.btnRight.Font = null;
         this.btnRight.Name = "btnRight";
         this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
         // 
         // btnCenter
         // 
         this.btnCenter.AccessibleDescription = null;
         this.btnCenter.AccessibleName = null;
         resources.ApplyResources(this.btnCenter, "btnCenter");
         this.btnCenter.BackgroundImage = null;
         this.btnCenter.Font = null;
         this.btnCenter.Name = "btnCenter";
         this.btnCenter.Click += new System.EventHandler(this.btnCenter_Click);
         // 
         // btnLeft
         // 
         this.btnLeft.AccessibleDescription = null;
         this.btnLeft.AccessibleName = null;
         resources.ApplyResources(this.btnLeft, "btnLeft");
         this.btnLeft.BackgroundImage = null;
         this.btnLeft.Font = null;
         this.btnLeft.Name = "btnLeft";
         this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
         // 
         // btnList
         // 
         this.btnList.AccessibleDescription = null;
         this.btnList.AccessibleName = null;
         resources.ApplyResources(this.btnList, "btnList");
         this.btnList.BackgroundImage = null;
         this.btnList.Font = null;
         this.btnList.Name = "btnList";
         this.btnList.Click += new System.EventHandler(this.btnList_Click);
         // 
         // btnUnderline
         // 
         this.btnUnderline.AccessibleDescription = null;
         this.btnUnderline.AccessibleName = null;
         resources.ApplyResources(this.btnUnderline, "btnUnderline");
         this.btnUnderline.BackgroundImage = null;
         this.btnUnderline.Name = "btnUnderline";
         this.btnUnderline.Click += new System.EventHandler(this.btnUnderline_Click);
         // 
         // btnImg
         // 
         this.btnImg.AccessibleDescription = null;
         this.btnImg.AccessibleName = null;
         resources.ApplyResources(this.btnImg, "btnImg");
         this.btnImg.BackgroundImage = null;
         this.btnImg.Font = null;
         this.btnImg.Name = "btnImg";
         this.btnImg.Click += new System.EventHandler(this.btnImg_Click);
         // 
         // btnURL
         // 
         this.btnURL.AccessibleDescription = null;
         this.btnURL.AccessibleName = null;
         resources.ApplyResources(this.btnURL, "btnURL");
         this.btnURL.BackgroundImage = null;
         this.btnURL.Font = null;
         this.btnURL.Name = "btnURL";
         this.btnURL.Click += new System.EventHandler(this.btnURL_Click);
         // 
         // btnItalic
         // 
         this.btnItalic.AccessibleDescription = null;
         this.btnItalic.AccessibleName = null;
         resources.ApplyResources(this.btnItalic, "btnItalic");
         this.btnItalic.BackgroundImage = null;
         this.btnItalic.Name = "btnItalic";
         this.btnItalic.Click += new System.EventHandler(this.btnItalic_Click);
         // 
         // btnBold
         // 
         this.btnBold.AccessibleDescription = null;
         this.btnBold.AccessibleName = null;
         resources.ApplyResources(this.btnBold, "btnBold");
         this.btnBold.BackgroundImage = null;
         this.btnBold.Name = "btnBold";
         this.btnBold.Click += new System.EventHandler(this.btnBold_Click);
         // 
         // label1
         // 
         this.label1.AccessibleDescription = null;
         this.label1.AccessibleName = null;
         resources.ApplyResources(this.label1, "label1");
         this.label1.Name = "label1";
         // 
         // label5
         // 
         this.label5.AccessibleDescription = null;
         this.label5.AccessibleName = null;
         resources.ApplyResources(this.label5, "label5");
         this.label5.Name = "label5";
         // 
         // label6
         // 
         this.label6.AccessibleDescription = null;
         this.label6.AccessibleName = null;
         resources.ApplyResources(this.label6, "label6");
         this.label6.Name = "label6";
         // 
         // label7
         // 
         this.label7.AccessibleDescription = null;
         this.label7.AccessibleName = null;
         resources.ApplyResources(this.label7, "label7");
         this.label7.Name = "label7";
         // 
         // htmlEdit
         // 
         this.htmlEdit.AccessibleDescription = null;
         this.htmlEdit.AccessibleName = null;
         resources.ApplyResources(this.htmlEdit, "htmlEdit");
         this.htmlEdit.BackgroundImage = null;
         this.htmlEdit.DefaultComposeSettings.BackColor = System.Drawing.Color.White;
         this.htmlEdit.DefaultComposeSettings.DefaultFont = new System.Drawing.Font("Arial", 10F);
         this.htmlEdit.DefaultComposeSettings.Enabled = false;
         this.htmlEdit.DefaultComposeSettings.ForeColor = System.Drawing.Color.Black;
         this.htmlEdit.DocumentEncoding = onlyconnect.EncodingType.WindowsCurrent;
         this.htmlEdit.Font = null;
         this.htmlEdit.IsDesignMode = true;
         this.htmlEdit.Name = "htmlEdit";
         this.htmlEdit.OpenLinksInNewWindow = true;
         this.htmlEdit.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Left;
         this.htmlEdit.SelectionBackColor = System.Drawing.Color.Empty;
         this.htmlEdit.SelectionBullets = false;
         this.htmlEdit.SelectionFont = null;
         this.htmlEdit.SelectionForeColor = System.Drawing.Color.Empty;
         this.htmlEdit.SelectionNumbering = false;
         // 
         // FormHTMLEdit
         // 
         this.AccessibleDescription = null;
         this.AccessibleName = null;
         resources.ApplyResources(this, "$this");
         this.BackgroundImage = null;
         this.Controls.Add(this.label7);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.groupBoxSelection);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.numBorder);
         this.Controls.Add(this.buttonOk);
         this.Controls.Add(this.btnTable);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.numRow);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.numCol);
         this.Controls.Add(this.htmlEdit);
         this.Font = null;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormHTMLEdit";
         this.ShowInTaskbar = false;
         this.Closed += new System.EventHandler(this.FormHTMLEdit_Closed);
         ((System.ComponentModel.ISupportInitialize)(this.numCol)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRow)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numBorder)).EndInit();
         this.groupBoxSelection.ResumeLayout(false);
         this.ResumeLayout(false);

      }
		#endregion

      private void btnBold_Click(object sender, System.EventArgs e)
      {
         htmlEdit.Document.execCommand("bold",false,null);
         htmlEdit.Document.focus();
      }

      private void btnItalic_Click(object sender, System.EventArgs e)
      {
         try
         {
            htmlEdit.Document.execCommand("italic",false,null);
            htmlEdit.Document.focus();
         }
         catch
         {
         }
      }

      private void btnList_Click(object sender, System.EventArgs e)
      {
         htmlEdit.Document.execCommand("insertUnorderedList",false,null);
         htmlEdit.Document.focus();
      }

      private void btnLeft_Click(object sender, System.EventArgs e)
      {
         htmlEdit.Document.execCommand("justifyLeft",false,null);
         htmlEdit.Document.focus();
      }

      private void btnCenter_Click(object sender, System.EventArgs e)
      {
         htmlEdit.Document.execCommand("justifyCenter",false,null);
         htmlEdit.Document.focus();
      }

      private void btnRight_Click(object sender, System.EventArgs e)
      {
         htmlEdit.Document.execCommand("justifyRight",false,null);
         htmlEdit.Document.focus();
      }

      private void btnURL_Click(object sender, System.EventArgs e)
      {   
         try
         {
            htmlEdit.Document.execCommand("createLink",false,null);
            htmlEdit.Document.focus();     
         }
         catch
         {
         }

      }

      private void btnImg_Click(object sender, System.EventArgs e)
      {
         FormInput formInput = new FormInput();
         formInput.Text = "Image URL";
         formInput.UserInput = "";
         formInput.ShowDialog();

         htmlEdit.Document.execCommand("insertImage",false,formInput.UserInput);
         htmlEdit.Document.focus();     
      }

      private void FormHTMLEdit_Closed(object sender, System.EventArgs e)
      {
         if(htmlEdit.Document.body != null)
         {
            /**
            textDesc = htmlEdit.Document.body.innerHTML;
            textDesc = textDesc.Replace("</P>","");
            if(textDesc.StartsWith("<P>"))
               textDesc = textDesc.Remove(0, 3);
               **/


            textDesc = htmlEdit.Document.body.innerHTML;
            if(textDesc == null)
               textDesc = "";

            // Starting with a paragraph is not nice: remove 
            // this paragraph and its closing tag.
            if(textDesc.StartsWith("<P>"))
            {
               textDesc = textDesc.Remove(0, 3);

               int index = 0;
               while(index < textDesc.Length)
               {
                  int closeIndex = textDesc.IndexOf("</P>", index);
                  int openIndex = textDesc.IndexOf("<P>", index);

                  // if there is an open index before the closing index, 
                  // go on searching from the closing index

                  if(openIndex>0 && openIndex<closeIndex)
                  {
                     index = closeIndex+4;
                     continue;
                  }
                  else
                  {
                     // the </P> tag belonging to the first <P> tag was found at
                     // index closeIndex;
                     textDesc = textDesc.Remove(closeIndex,4);
                     break;
                  }
               }
            }

            textDesc = textDesc.Replace("<BR>", "<br/>");
            textDesc = textDesc.Replace("UL>", "ul>");
            textDesc = textDesc.Replace("LI>", "li>");
            textDesc = textDesc.Replace("P>", "p>");
            textDesc = textDesc.Replace("<P", "<p");
            textDesc = textDesc.Replace("STRONG>", "strong>");
            textDesc = textDesc.Replace("<TABLE", "<table");
            textDesc = textDesc.Replace("TABLE>", "table>");
            textDesc = textDesc.Replace("TBODY>", "tbody>");
            textDesc = textDesc.Replace("TR>", "tr>");
            textDesc = textDesc.Replace("TD>", "td>");
            textDesc = textDesc.Replace("EM>", "em>");
            textDesc = textDesc.Replace("<IMG", "<img");
            textDesc = textDesc.Replace("<A", "<a");
            textDesc = textDesc.Replace("</A", "</a");
            textDesc = textDesc.Replace("<DIV", "<div");
            textDesc = textDesc.Replace("</DIV", "</div");
            textDesc = textDesc.Replace("align=left", "align=\"left\"");
            textDesc = textDesc.Replace("align=right", "align=\"right\"");
            textDesc = textDesc.Replace("align=center", "align=\"center\"");
         }
         else
            textDesc = "";
      }

      private void btnUnderline_Click(object sender, System.EventArgs e)
      {
         try
         {
            htmlEdit.Document.execCommand("underline",false,null);
            htmlEdit.Document.focus();
         }
         catch
         {
         }
      }

      private void btnTable_Click(object sender, System.EventArgs e)
      {
         try
         {
            mshtml.IHTMLTable t = (mshtml.IHTMLTable)htmlEdit.Document.createElement("table");

            //set the cols
            int cols = (int)numCol.Value;
            int rows = (int)numRow.Value;
  
            t.cols = cols;
            t.border = (int)numBorder.Value;

            for (int i = 1; i <= rows; i++) 
            {
               mshtml.IHTMLTableRow tr = (mshtml.IHTMLTableRow) t.insertRow(-1);
               for (int j = 1; j <= cols; j++) 
               {
                  mshtml.IHTMLElement c = (mshtml.IHTMLElement) tr.insertCell(-1);
                  c.innerHTML = j.ToString();
               }
            }
				
            mshtml.IHTMLDOMNode nd = (mshtml.IHTMLDOMNode) t;
            mshtml.IHTMLDOMNode body = (mshtml.IHTMLDOMNode)htmlEdit.Document.body;
            body.appendChild(nd);
				
				
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }      
      }

      private void buttonOk_Click(object sender, System.EventArgs e)
      {
      
      }



	}
}
