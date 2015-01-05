using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Data;
using System.IO;
using System.Resources;

using BSoft.Xml;


namespace RSSBuilder
{
      
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
   public class FormRSSBuilder : System.Windows.Forms.Form
   {
       private ResourceManager _res = new ResourceManager("RSSBuilder.MessageStrings.RSSBuilderStrings", typeof(FormRSSBuilder).Assembly);
      private static string _dateTimeFormat = "ddd, d MMM yyyy HH:mm:ss";
      private RSSFeed _rssFeed = null;
      private bool   _changed = false;
      private string _fileName = "";
//      private string siteName = "";
      private XmlConfig _settings = null;
      private int _localTimeOffset = 0;

      #region Form control declarations

      private System.Windows.Forms.StatusBar statusBar;
      private System.Windows.Forms.Panel panelContent;
      private System.Windows.Forms.Panel panelRight;
      private System.Windows.Forms.Panel panelItemProperties;
      private System.Windows.Forms.TextBox textAuthor;
      private System.Windows.Forms.TextBox textLink;
      private System.Windows.Forms.TextBox textTitle;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox textDesc;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Splitter splitterV;
      private System.Windows.Forms.Panel panelLeft;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.Label label14;
      private System.Windows.Forms.Label label16;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.MainMenu mainMenu;
      public System.Windows.Forms.ImageList imageList;
      private System.Windows.Forms.MenuItem menuItemFile;
      private System.Windows.Forms.MenuItem menuItemOpen;
      private System.Windows.Forms.MenuItem menuItemSave;
      private System.Windows.Forms.MenuItem menuItemNew;
      private System.Windows.Forms.TextBox textBoxImgHeight;
      private System.Windows.Forms.TextBox textBoxImgWidth;
      private System.Windows.Forms.TextBox textBoxImgURL;
      private System.Windows.Forms.TextBox textBoxDescription;
      private System.Windows.Forms.TextBox textBoxWebmaster;
      private System.Windows.Forms.TextBox textBoxEditor;
      private System.Windows.Forms.TextBox textBoxCopyright;
      private System.Windows.Forms.TextBox textBoxWebURL;
      private System.Windows.Forms.TextBox textBoxTitle;
      private System.Windows.Forms.ComboBox comboBoxLanguage;
      private System.Windows.Forms.DateTimePicker dateTimePicker;
      private System.Windows.Forms.MenuItem menuItem1;
      private System.Windows.Forms.MenuItem menuItemExit;
      private System.Windows.Forms.MenuItem menuItem2;
      private System.Windows.Forms.MenuItem menuItemAddTopic;
      private System.Windows.Forms.MenuItem menuItemDeleteTopic;
      private System.Windows.Forms.MenuItem menuItem5;
      private System.Windows.Forms.MenuItem menuItemViewToolBar;
      private System.Windows.Forms.MenuItem menuItemViewFeedProp;
      private System.Windows.Forms.MenuItem menuItem6;
      private System.Windows.Forms.OpenFileDialog openFileDialog;
      private System.Windows.Forms.SaveFileDialog saveFileDialog;
      private System.Windows.Forms.MenuItem menuItemSaveAs;
      private System.Windows.Forms.MenuItem menuItemAbout;
      private System.Windows.Forms.MenuItem menuItemRSSBuilderWeb;
      private System.Windows.Forms.ToolTip toolTip;
      private System.Windows.Forms.ErrorProvider errorProvider;
      private System.Windows.Forms.NumericUpDown numTimeOffset;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.Button buttonLocalOffset;
      private System.Windows.Forms.TextBox textComments;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.Button buttonHTML;
      private System.Windows.Forms.MenuItem menuItemSaveAsHTML;
      private System.Windows.Forms.MenuItem menuItem9;
      private System.Windows.Forms.MenuItem menuItemSiteManager;
      private System.Windows.Forms.MenuItem menuItemSettings;
      private System.Windows.Forms.MenuItem menuItem12;
      private System.Windows.Forms.MenuItem menuItemDownloadFeed;
      private System.Windows.Forms.MenuItem menuItemPublishFeed;
      private System.Windows.Forms.MenuItem menuItemTopicUp;
      private System.Windows.Forms.MenuItem menuItemTopicDown;
      private System.Windows.Forms.MenuItem menuItem7;
      private System.Windows.Forms.MenuItem menuItemEnglish;
      private System.Windows.Forms.MenuItem menuItemGerman;
      private System.Windows.Forms.MenuItem menuItemDutch;
      private System.Windows.Forms.TextBox textCategory;
      private System.Windows.Forms.Label label18;
      private System.Windows.Forms.MenuItem menuItem8;
      private System.Windows.Forms.MenuItem menuItemViewXml;
      private System.Windows.Forms.TextBox textBoxStyleSheet;
      private System.Windows.Forms.Label label19;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.Label label20;
      private System.Windows.Forms.ComboBox comboStyleType;
      private MenuItem menuItemCzech;
      private ToolStrip toolStripItems;
      private ListView listViewItems;
      private ColumnHeader columnTitle;
      private ColumnHeader columnDate;
      private ToolStripContainer toolStripContainer;
      private ToolStrip toolStrip;
      private ToolStripButton btnAddItem;
      private ToolStripButton btnDeleteItem;
      private ToolStripButton btnMoveUpItem;
      private ToolStripButton btnMoveDownItem;
      private ToolStripButton btnNew;
      private ToolStripButton btnOpen;
      private ToolStripButton btnSave;
      private ToolStripSeparator toolStripSeparator1;
      private ToolStripButton btnDownload;
      private ToolStripButton btnPublish;
      private ToolStripButton btnPreview;
      private ToolStripSeparator toolStripSeparator2;
      private ToolStripButton btnSiteManager;
      private SplitContainer splitContainer1;
      private TabControl tabControl;
      private TabPage tabPageGeneral;
      private TabPage tabPageEnclosure;
      private Label label21;
      private TextBox textEnclosureUrl;
      private FolderBrowserDialog folderBrowserDialog1;
      private Label label23;
      private TextBox textEnclosureLength;
      private Label label22;
      private Label label24;
      private ComboBox comboEnclosureType;
      private OpenFileDialog openEnclosureDialog;
      private LinkLabel linkDetermineFromURL;
      private Label label26;
      private Label label28;
      private Label label27;
      private Label labelWait;
      private TabPage tabPageAdvanced;
      private TableLayoutPanel tableLayoutPanelAdvanced;
      private TextBox textGUID;
      private Label labelGUID;
      private CheckBox checkPermaLink;
      private Button buttonUseLinkAsGuid;
      private Button btnUploadEnclosure;
      private ToolStripSeparator toolStripSeparator3;
      private ToolStripButton btnDonate;
      private ComboBox cmbHubs;
      private Label lblHub;
      private TextBox textBoxFeedURL;
      private Label lblFeedURL;
      private System.ComponentModel.IContainer components;

      #endregion Form control declarations

      private bool askSaveChanges()
      {
         if(_changed)
         {
            DialogResult dr = MessageBox.Show(_res.GetString("msgChanged"),"RSS Builder", 
               MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            
            if(dr == DialogResult.Yes)
            {
               saveFeed();
            }

            return (dr != DialogResult.Cancel); // return false if user cliked cancel
         }

         return true;
      }

      private void newFeed()
      {
         if(! askSaveChanges() )
            return;

         _rssFeed = new RSSFeed();
         showRSSFeed();

         _fileName = "";
//         siteName = "";
         this.Text = "RSS Builder";
      }

      public void OpenFeed(string rssFileName)
      {
         RSSFeed newRssFeed = new RSSFeed();
         if(newRssFeed.openFeed(rssFileName))
         {
            _rssFeed = newRssFeed;
            showRSSFeed();

            openFileDialog.InitialDirectory = "";

            _fileName = rssFileName;
//            siteName = rssFeed.FtpSite;
            if(_rssFeed.FtpSite != "")
               this.Text = "RSS Builder - " + Path.GetFileName(rssFileName) + " @ " + _rssFeed.FtpSite;
            else
               this.Text = "RSS Builder - " + Path.GetFileName(rssFileName);
         
            _changed = false;
         }
         else
         {
            MessageBox.Show(_res.GetString("msgInvalidRSS"));
         }
      }

      private void openFeed()
      {
         if(! askSaveChanges() )
            return;

         openFileDialog.FileName ="";

         if(openFileDialog.ShowDialog() == DialogResult.OK)
         {
            OpenFeed(openFileDialog.FileName);
         }
      }

      private void downloadFeed()
      {
         if(! askSaveChanges() )
            return;

         RSSFeed newRssFeed = new RSSFeed();

         FormFTPDownload ftp = new FormFTPDownload(newRssFeed, _rssFeed.FtpSite);

         if(ftp.ShowDialog() == DialogResult.OK)
         {
            _rssFeed= newRssFeed;
            showRSSFeed();

            _fileName = "";
//            siteName = ftp.section();
//            if(rssFeed.FtpSite == "") rssFeed.FtpSite = siteName;
            _rssFeed.FtpSite = ftp.section();

            this.Text = "RSS Builder -  @ " + _rssFeed.FtpSite;
            _changed = false;
         }
      }


      private void saveFeed()
      {
         if(_fileName == "")
         {
            saveFeedAs();
         }
         else
         {
            try
            {
               _rssFeed.saveFeed(_fileName);
            }
            catch
            {
               _fileName = ""; // on error, try saving as different name
               saveFeedAs();
            }
         }

         _changed = false;
      }

      private void saveFeedAs()
      {
         saveFileDialog.FileName = _fileName;

         if(saveFileDialog.ShowDialog() == DialogResult.OK)
         {
            try
            {
               _rssFeed.saveFeed(saveFileDialog.FileName);

               saveFileDialog.InitialDirectory = "";

               _fileName = saveFileDialog.FileName;
               if(_rssFeed.FtpSite != "")
                  this.Text = "RSS Builder - " + Path.GetFileName(_fileName) + " @ " + _rssFeed.FtpSite;
               else
                  this.Text = "RSS Builder - " + Path.GetFileName(_fileName);
            }
            catch
            {
               MessageBox.Show(this, _res.GetString("msgCannotSave"), "RSS Builder", 
                  MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
         } 
     
         _changed = false;
      }


      private void showRSSFeed()
      {
         if(_rssFeed == null) return;

         textBoxTitle.Text = _rssFeed.Title;
         textBoxWebURL.Text = _rssFeed.WebURL;
         textBoxFeedURL.Text = _rssFeed.FeedURL;
         cmbHubs.SelectedIndex = cmbHubs.FindString(_rssFeed.HubURL);
         
         textBoxCopyright.Text = _rssFeed.Copyright;
         comboBoxLanguage.SelectedIndex = comboBoxLanguage.FindString(_rssFeed.Language);
         textBoxEditor.Text = _rssFeed.Editor;
         textBoxWebmaster.Text = _rssFeed.Webmaster;
         textBoxDescription.Text = _rssFeed.Description;
         textBoxStyleSheet.Text = _rssFeed.StyleSheet;
         comboStyleType.Text = _rssFeed.StyleType;

         textBoxImgURL.Text = _rssFeed.ImgURL;
         textBoxImgWidth.Text = _rssFeed.ImgWidth;
         textBoxImgHeight.Text = _rssFeed.ImgHeight;

         tabControl.SelectedTab = tabPageGeneral;
         tabControl.Enabled = false;
//         panelItemProperties.Enabled = false;
         listViewItems.Items.Clear();
         clearItemProperties();
         listViewItems.Enabled = false;
         for(int i=0; i<_rssFeed.NewsItemCount; i++)
         {
            RSSItem rssItem = _rssFeed[i];
            ListViewItem newItem = listViewItems.Items.Add(rssItem.Title);
         
            newItem.Tag = rssItem;
            newItem.SubItems.Add(rssItem.PubDate);
         }
         listViewItems.Enabled = true;

         if(listViewItems.Items.Count > 0)
         {
            listViewItems.Items[0].Selected = true;
//            panelItemProperties.Enabled = true;
            tabControl.Enabled = true;
         }

         textBoxTitle.Focus();

         errorProvider.Clear();
      }


      private void publishFeed()
      {
         FormFTPPublish ftp = new FormFTPPublish(_rssFeed, _rssFeed.FtpSite);
         ftp.ShowDialog();

         if(_fileName != "")
            this.Text = "RSS Builder - " + Path.GetFileName(_fileName) + " @ " + _rssFeed.FtpSite;
         else
            this.Text = "RSS Builder -  @ " + _rssFeed.FtpSite;
      }

      private void startSiteManager()
      {
         FormFTPSites ftp = new FormFTPSites(_rssFeed.FtpSite);
         ftp.ShowDialog();
      }

      private void clearItemProperties()
      {
         textTitle.Text="";
         textLink.Text="";
         textAuthor.Text="";
         textDesc.Text="";
         textAuthor.Text ="";
         textComments.Text="";
         textEnclosureLength.Text = "";
         textEnclosureUrl.Text = "";
         comboEnclosureType.Text = "";
         dateTimePicker.ResetText();
         textGUID.Text = "";
         checkPermaLink.Checked = true;

         errorProvider.SetError(textEnclosureUrl, "");
      }

      private void showSelectedRSSItem()
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            textTitle.Text = selRssItem.Title;
            textLink.Text = selRssItem.Link;
            textAuthor.Text = selRssItem.Author;
            textComments.Text = selRssItem.Comments;
            textDesc.Text = selRssItem.Description;
            textCategory.Text = selRssItem.Category;
            try
            {
               dateTimePicker.Text = selRssItem.PubDate;
            }
            catch
            {
               dateTimePicker.ResetText(); // if date is invalid RFC date, set to current time
            }
            numTimeOffset.Value = selRssItem.TimeOffset;

            textEnclosureUrl.Text = selRssItem.EnclosureUrl;
            textEnclosureLength.Text = selRssItem.EnclosureLength;
            comboEnclosureType.Text = selRssItem.EnclosureType;

            textGUID.Text = selRssItem.GUID;
            checkPermaLink.Checked = selRssItem.IsPermaLink;
         }
         else
         {
            clearItemProperties();
         }
      }

      private void addNewsItem()
      {
         RSSItem newRSSItem = _rssFeed.addItem();
         newRSSItem.Title = "<new>";
         newRSSItem.Link = "";

         ListViewItem newItem = listViewItems.Items.Insert(0, newRSSItem.Title);
         newItem.Tag = newRSSItem;

         newItem.SubItems.Add(newRSSItem.PubDate);

         textTitle.Focus();
         newItem.Selected = true;

         dateTimePicker.ResetText();
         newRSSItem.PubDate = dateTimePicker.Value.ToString(_dateTimeFormat, CultureInfo.CreateSpecificCulture("en-US")); // RFC822 date-time, e.g. name of day of week regardless of locale language

         TimeZone tz = TimeZone.CurrentTimeZone;
         int hourOffset = tz.GetUtcOffset(DateTime.Now).Hours;
         numTimeOffset.Value = hourOffset;

//         panelItemProperties.Enabled = true;
         tabControl.Enabled = true;
         tabControl.SelectedTab = tabPageGeneral;

         _changed = true;
      }

      private void deleteNewsItem()
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            if( MessageBox.Show(_res.GetString("msgAskDelete"), "RSS Builder", 
                            MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
               return;

            //---
            // Get the selected list item and remove it.
            // Delete the associated RSSItem object
            //---
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            int delIndex = selItem.Index;

            listViewItems.Items.Remove(selItem);
            _rssFeed.deleteItem(selRssItem);

            if (listViewItems.Items.Count == 0)
            {
               tabControl.SelectedTab = tabPageGeneral;
               tabControl.Enabled = false;
            }

            //---
            // Select the item before the deleted item
            // Otherwise try to select the first item.
            //---
            if(delIndex-1 >= 0)
               listViewItems.Items[delIndex-1].Selected = true;
            else if(listViewItems.Items.Count > 0)
               listViewItems.Items[0].Selected = true;

            _changed = true;
         }
      }

      public void swapNewsItems(int index1, int index2)
      {
         int count = listViewItems.Items.Count;
         if(index1 >= 0 && index2 >= 0 && index1 < count && index2 < count)
         {
            string tempLabel = listViewItems.Items[index1].Text;
            string tempDate  = listViewItems.Items[index1].SubItems[1].Text;
            RSSItem tempRSSItem = (RSSItem)listViewItems.Items[index1].Tag;

            listViewItems.Items[index1].Text = listViewItems.Items[index2].Text;
            listViewItems.Items[index1].Tag = listViewItems.Items[index2].Tag;
            listViewItems.Items[index1].SubItems[1].Text = listViewItems.Items[index2].SubItems[1].Text;

            listViewItems.Items[index2].Text = tempLabel;
            listViewItems.Items[index2].Tag = tempRSSItem;
            listViewItems.Items[index2].SubItems[1].Text = tempDate;
         }
      }

      public void shiftNewsItemUp()
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            int selIndex = listViewItems.SelectedIndices[0];

            if(selIndex-1 >=0)
            {
               swapNewsItems(selIndex, selIndex-1);
               _rssFeed.swapItems(selIndex, selIndex-1);
               listViewItems.Items[selIndex-1].Selected = true;

            }
         }
      }

      public void shiftNewsItemDown()
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            int selIndex = listViewItems.SelectedIndices[0];

            if(selIndex+1 < listViewItems.Items.Count)
            {
               swapNewsItems(selIndex, selIndex+1);
               _rssFeed.swapItems(selIndex, selIndex+1);
               listViewItems.Items[selIndex+1].Selected = true;
            }
         }
      }

      public FormRSSBuilder()
      {
         //
         // Read language setting from rssbuilder.config
         // and change the UI culture based on the language.
         //
         // This must be done before InitializeComponent, because
         // it affects the used resources.
         //
         string language = "english";

         try
         {
            string configPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _settings = new XmlConfig(configPath + @"\rssbuilder.config");

            language = _settings.GetValue("UserInterface", "language", "english");

            if (language == "english")
            {
               Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            }
            else if (language == "german")
            {
               Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            }
            else if (language == "dutch")
            {
               Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-NL");
            }
            else if (language == "czech")
            {
               Thread.CurrentThread.CurrentUICulture = new CultureInfo("cs-CZ");
            }
         }
         catch
         {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
         }

         //
         // Required for Windows Form Designer support
         //
         InitializeComponent();

         //
         // Depending on the language, check the appropriate menu item
         //
         menuItemEnglish.Checked = (language == "english");
         menuItemGerman.Checked = (language == "german");
         menuItemDutch.Checked = (language == "dutch");
         menuItemCzech.Checked = (language == "czech");

         _rssFeed = new RSSFeed();

         comboBoxLanguage.SelectedIndex = comboBoxLanguage.FindString("en-us");
      }

      private void FormRSSBuilder_Load(object sender, EventArgs e)
      {
         //---
         // IMPORTANT: Set culture info to english for translating
         // dates into english format (needed in RSS feeds)
         //---
         Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

         dateTimePicker.CustomFormat = _dateTimeFormat;
       
         openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
         saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

         TimeZone tz = TimeZone.CurrentTimeZone;

         _localTimeOffset = tz.GetUtcOffset(DateTime.Now).Hours;

         numTimeOffset.Minimum = -100;
         numTimeOffset.Maximum = 100;
         numTimeOffset.Value = _localTimeOffset;

         _changed = false;
      }

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose( bool disposing )
      {
         if( disposing )
         {
            if (components != null) 
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRSSBuilder));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listViewItems = new System.Windows.Forms.ListView();
            this.columnTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.panelItemProperties = new System.Windows.Forms.Panel();
            this.buttonUseLinkAsGuid = new System.Windows.Forms.Button();
            this.textCategory = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.buttonHTML = new System.Windows.Forms.Button();
            this.textComments = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.buttonLocalOffset = new System.Windows.Forms.Button();
            this.numTimeOffset = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.textAuthor = new System.Windows.Forms.TextBox();
            this.textLink = new System.Windows.Forms.TextBox();
            this.textTitle = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textDesc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageEnclosure = new System.Windows.Forms.TabPage();
            this.btnUploadEnclosure = new System.Windows.Forms.Button();
            this.labelWait = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.linkDetermineFromURL = new System.Windows.Forms.LinkLabel();
            this.label24 = new System.Windows.Forms.Label();
            this.comboEnclosureType = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.textEnclosureLength = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.textEnclosureUrl = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tabPageAdvanced = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelAdvanced = new System.Windows.Forms.TableLayoutPanel();
            this.labelGUID = new System.Windows.Forms.Label();
            this.textGUID = new System.Windows.Forms.TextBox();
            this.checkPermaLink = new System.Windows.Forms.CheckBox();
            this.toolStripItems = new System.Windows.Forms.ToolStrip();
            this.btnAddItem = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.btnMoveUpItem = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDownItem = new System.Windows.Forms.ToolStripButton();
            this.splitterV = new System.Windows.Forms.Splitter();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.comboStyleType = new System.Windows.Forms.ComboBox();
            this.textBoxStyleSheet = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxImgHeight = new System.Windows.Forms.TextBox();
            this.textBoxImgWidth = new System.Windows.Forms.TextBox();
            this.textBoxImgURL = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblHub = new System.Windows.Forms.Label();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.cmbHubs = new System.Windows.Forms.ComboBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.textBoxWebmaster = new System.Windows.Forms.TextBox();
            this.textBoxEditor = new System.Windows.Forms.TextBox();
            this.textBoxCopyright = new System.Windows.Forms.TextBox();
            this.textBoxWebURL = new System.Windows.Forms.TextBox();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnPreview = new System.Windows.Forms.ToolStripButton();
            this.btnDownload = new System.Windows.Forms.ToolStripButton();
            this.btnPublish = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSiteManager = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDonate = new System.Windows.Forms.ToolStripButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemNew = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemDownloadFeed = new System.Windows.Forms.MenuItem();
            this.menuItemPublishFeed = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItemSaveAsHTML = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemAddTopic = new System.Windows.Forms.MenuItem();
            this.menuItemDeleteTopic = new System.Windows.Forms.MenuItem();
            this.menuItemTopicUp = new System.Windows.Forms.MenuItem();
            this.menuItemTopicDown = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItemViewToolBar = new System.Windows.Forms.MenuItem();
            this.menuItemViewFeedProp = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItemViewXml = new System.Windows.Forms.MenuItem();
            this.menuItemSettings = new System.Windows.Forms.MenuItem();
            this.menuItemSiteManager = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItemEnglish = new System.Windows.Forms.MenuItem();
            this.menuItemGerman = new System.Windows.Forms.MenuItem();
            this.menuItemDutch = new System.Windows.Forms.MenuItem();
            this.menuItemCzech = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.menuItemRSSBuilderWeb = new System.Windows.Forms.MenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openEnclosureDialog = new System.Windows.Forms.OpenFileDialog();
            this.textBoxFeedURL = new System.Windows.Forms.TextBox();
            this.lblFeedURL = new System.Windows.Forms.Label();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.panelItemProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeOffset)).BeginInit();
            this.tabPageEnclosure.SuspendLayout();
            this.tabPageAdvanced.SuspendLayout();
            this.tableLayoutPanelAdvanced.SuspendLayout();
            this.toolStripItems.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            resources.ApplyResources(this.toolStripContainer.ContentPanel, "toolStripContainer.ContentPanel");
            this.toolStripContainer.ContentPanel.Controls.Add(this.panelContent);
            this.toolStripContainer.ContentPanel.Controls.Add(this.statusBar);
            resources.ApplyResources(this.toolStripContainer, "toolStripContainer");
            this.toolStripContainer.LeftToolStripPanelVisible = false;
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.RightToolStripPanelVisible = false;
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.panelRight);
            this.panelContent.Controls.Add(this.splitterV);
            this.panelContent.Controls.Add(this.panelLeft);
            resources.ApplyResources(this.panelContent, "panelContent");
            this.panelContent.Name = "panelContent";
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.splitContainer1);
            this.panelRight.Controls.Add(this.toolStripItems);
            resources.ApplyResources(this.panelRight, "panelRight");
            this.panelRight.Name = "panelRight";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listViewItems);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            // 
            // listViewItems
            // 
            this.listViewItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTitle,
            this.columnDate});
            resources.ApplyResources(this.listViewItems, "listViewItems");
            this.listViewItems.FullRowSelect = true;
            this.listViewItems.HideSelection = false;
            this.listViewItems.MultiSelect = false;
            this.listViewItems.Name = "listViewItems";
            this.listViewItems.UseCompatibleStateImageBehavior = false;
            this.listViewItems.View = System.Windows.Forms.View.Details;
            this.listViewItems.SelectedIndexChanged += new System.EventHandler(this.listViewItems_SelectedIndexChanged);
            // 
            // columnTitle
            // 
            resources.ApplyResources(this.columnTitle, "columnTitle");
            // 
            // columnDate
            // 
            resources.ApplyResources(this.columnDate, "columnDate");
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageGeneral);
            this.tabControl.Controls.Add(this.tabPageEnclosure);
            this.tabControl.Controls.Add(this.tabPageAdvanced);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.panelItemProperties);
            resources.ApplyResources(this.tabPageGeneral, "tabPageGeneral");
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // panelItemProperties
            // 
            this.panelItemProperties.Controls.Add(this.buttonUseLinkAsGuid);
            this.panelItemProperties.Controls.Add(this.textCategory);
            this.panelItemProperties.Controls.Add(this.label18);
            this.panelItemProperties.Controls.Add(this.buttonHTML);
            this.panelItemProperties.Controls.Add(this.textComments);
            this.panelItemProperties.Controls.Add(this.label15);
            this.panelItemProperties.Controls.Add(this.buttonLocalOffset);
            this.panelItemProperties.Controls.Add(this.numTimeOffset);
            this.panelItemProperties.Controls.Add(this.label13);
            this.panelItemProperties.Controls.Add(this.dateTimePicker);
            this.panelItemProperties.Controls.Add(this.textAuthor);
            this.panelItemProperties.Controls.Add(this.textLink);
            this.panelItemProperties.Controls.Add(this.textTitle);
            this.panelItemProperties.Controls.Add(this.label5);
            this.panelItemProperties.Controls.Add(this.textDesc);
            this.panelItemProperties.Controls.Add(this.label4);
            this.panelItemProperties.Controls.Add(this.label3);
            this.panelItemProperties.Controls.Add(this.label2);
            this.panelItemProperties.Controls.Add(this.label1);
            resources.ApplyResources(this.panelItemProperties, "panelItemProperties");
            this.panelItemProperties.Name = "panelItemProperties";
            // 
            // buttonUseLinkAsGuid
            // 
            resources.ApplyResources(this.buttonUseLinkAsGuid, "buttonUseLinkAsGuid");
            this.buttonUseLinkAsGuid.Name = "buttonUseLinkAsGuid";
            this.toolTip.SetToolTip(this.buttonUseLinkAsGuid, resources.GetString("buttonUseLinkAsGuid.ToolTip"));
            this.buttonUseLinkAsGuid.UseVisualStyleBackColor = true;
            this.buttonUseLinkAsGuid.Click += new System.EventHandler(this.buttonUseLinkAsGuid_Click);
            // 
            // textCategory
            // 
            resources.ApplyResources(this.textCategory, "textCategory");
            this.textCategory.Name = "textCategory";
            this.toolTip.SetToolTip(this.textCategory, resources.GetString("textCategory.ToolTip"));
            this.textCategory.TextChanged += new System.EventHandler(this.textCategory_TextChanged);
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // buttonHTML
            // 
            resources.ApplyResources(this.buttonHTML, "buttonHTML");
            this.buttonHTML.Name = "buttonHTML";
            this.buttonHTML.Click += new System.EventHandler(this.buttonHTML_Click);
            // 
            // textComments
            // 
            resources.ApplyResources(this.textComments, "textComments");
            this.textComments.Name = "textComments";
            this.toolTip.SetToolTip(this.textComments, resources.GetString("textComments.ToolTip"));
            this.textComments.TextChanged += new System.EventHandler(this.textComments_TextChanged);
            this.textComments.Validating += new System.ComponentModel.CancelEventHandler(this.textComments_Validating);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // buttonLocalOffset
            // 
            resources.ApplyResources(this.buttonLocalOffset, "buttonLocalOffset");
            this.buttonLocalOffset.Name = "buttonLocalOffset";
            this.toolTip.SetToolTip(this.buttonLocalOffset, resources.GetString("buttonLocalOffset.ToolTip"));
            this.buttonLocalOffset.Click += new System.EventHandler(this.buttonLocalOffset_Click);
            // 
            // numTimeOffset
            // 
            resources.ApplyResources(this.numTimeOffset, "numTimeOffset");
            this.numTimeOffset.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
            this.numTimeOffset.Minimum = new decimal(new int[] {
            48,
            0,
            0,
            -2147483648});
            this.numTimeOffset.Name = "numTimeOffset";
            this.toolTip.SetToolTip(this.numTimeOffset, resources.GetString("numTimeOffset.ToolTip"));
            this.numTimeOffset.ValueChanged += new System.EventHandler(this.numTimeOffset_ValueChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            this.toolTip.SetToolTip(this.label13, resources.GetString("label13.ToolTip"));
            // 
            // dateTimePicker
            // 
            resources.ApplyResources(this.dateTimePicker, "dateTimePicker");
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.Name = "dateTimePicker";
            this.toolTip.SetToolTip(this.dateTimePicker, resources.GetString("dateTimePicker.ToolTip"));
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // textAuthor
            // 
            resources.ApplyResources(this.textAuthor, "textAuthor");
            this.textAuthor.Name = "textAuthor";
            this.toolTip.SetToolTip(this.textAuthor, resources.GetString("textAuthor.ToolTip"));
            this.textAuthor.TextChanged += new System.EventHandler(this.textAuthor_TextChanged);
            this.textAuthor.Validating += new System.ComponentModel.CancelEventHandler(this.textAuthor_Validating);
            // 
            // textLink
            // 
            resources.ApplyResources(this.textLink, "textLink");
            this.textLink.Name = "textLink";
            this.toolTip.SetToolTip(this.textLink, resources.GetString("textLink.ToolTip"));
            this.textLink.TextChanged += new System.EventHandler(this.textLink_TextChanged);
            this.textLink.Validating += new System.ComponentModel.CancelEventHandler(this.textLink_Validating);
            // 
            // textTitle
            // 
            resources.ApplyResources(this.textTitle, "textTitle");
            this.textTitle.Name = "textTitle";
            this.toolTip.SetToolTip(this.textTitle, resources.GetString("textTitle.ToolTip"));
            this.textTitle.TextChanged += new System.EventHandler(this.textTitle_TextChanged);
            this.textTitle.Validating += new System.ComponentModel.CancelEventHandler(this.textTitle_Validating);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // textDesc
            // 
            resources.ApplyResources(this.textDesc, "textDesc");
            this.textDesc.Name = "textDesc";
            this.toolTip.SetToolTip(this.textDesc, resources.GetString("textDesc.ToolTip"));
            this.textDesc.TextChanged += new System.EventHandler(this.textDesc_TextChanged);
            this.textDesc.Validating += new System.ComponentModel.CancelEventHandler(this.textDesc_Validating);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tabPageEnclosure
            // 
            this.tabPageEnclosure.Controls.Add(this.btnUploadEnclosure);
            this.tabPageEnclosure.Controls.Add(this.labelWait);
            this.tabPageEnclosure.Controls.Add(this.label28);
            this.tabPageEnclosure.Controls.Add(this.label27);
            this.tabPageEnclosure.Controls.Add(this.label26);
            this.tabPageEnclosure.Controls.Add(this.linkDetermineFromURL);
            this.tabPageEnclosure.Controls.Add(this.label24);
            this.tabPageEnclosure.Controls.Add(this.comboEnclosureType);
            this.tabPageEnclosure.Controls.Add(this.label23);
            this.tabPageEnclosure.Controls.Add(this.textEnclosureLength);
            this.tabPageEnclosure.Controls.Add(this.label22);
            this.tabPageEnclosure.Controls.Add(this.textEnclosureUrl);
            this.tabPageEnclosure.Controls.Add(this.label21);
            resources.ApplyResources(this.tabPageEnclosure, "tabPageEnclosure");
            this.tabPageEnclosure.Name = "tabPageEnclosure";
            // 
            // btnUploadEnclosure
            // 
            resources.ApplyResources(this.btnUploadEnclosure, "btnUploadEnclosure");
            this.btnUploadEnclosure.Name = "btnUploadEnclosure";
            this.btnUploadEnclosure.UseVisualStyleBackColor = true;
            this.btnUploadEnclosure.Click += new System.EventHandler(this.btnUploadEnclosure_Click);
            // 
            // labelWait
            // 
            resources.ApplyResources(this.labelWait, "labelWait");
            this.labelWait.ForeColor = System.Drawing.Color.LightCoral;
            this.labelWait.Name = "labelWait";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label28.Name = "label28";
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label27.Name = "label27";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // linkDetermineFromURL
            // 
            resources.ApplyResources(this.linkDetermineFromURL, "linkDetermineFromURL");
            this.linkDetermineFromURL.Name = "linkDetermineFromURL";
            this.linkDetermineFromURL.TabStop = true;
            this.linkDetermineFromURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDetermineFromURL_LinkClicked);
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // comboEnclosureType
            // 
            resources.ApplyResources(this.comboEnclosureType, "comboEnclosureType");
            this.comboEnclosureType.Items.AddRange(new object[] {
            resources.GetString("comboEnclosureType.Items"),
            resources.GetString("comboEnclosureType.Items1"),
            resources.GetString("comboEnclosureType.Items2"),
            resources.GetString("comboEnclosureType.Items3"),
            resources.GetString("comboEnclosureType.Items4"),
            resources.GetString("comboEnclosureType.Items5"),
            resources.GetString("comboEnclosureType.Items6"),
            resources.GetString("comboEnclosureType.Items7"),
            resources.GetString("comboEnclosureType.Items8"),
            resources.GetString("comboEnclosureType.Items9"),
            resources.GetString("comboEnclosureType.Items10"),
            resources.GetString("comboEnclosureType.Items11")});
            this.comboEnclosureType.Name = "comboEnclosureType";
            this.toolTip.SetToolTip(this.comboEnclosureType, resources.GetString("comboEnclosureType.ToolTip"));
            this.comboEnclosureType.TextChanged += new System.EventHandler(this.comboEnclosureType_TextChanged);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // textEnclosureLength
            // 
            resources.ApplyResources(this.textEnclosureLength, "textEnclosureLength");
            this.textEnclosureLength.Name = "textEnclosureLength";
            this.toolTip.SetToolTip(this.textEnclosureLength, resources.GetString("textEnclosureLength.ToolTip"));
            this.textEnclosureLength.TextChanged += new System.EventHandler(this.textEnclosureLength_TextChanged);
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // textEnclosureUrl
            // 
            resources.ApplyResources(this.textEnclosureUrl, "textEnclosureUrl");
            this.textEnclosureUrl.Name = "textEnclosureUrl";
            this.toolTip.SetToolTip(this.textEnclosureUrl, resources.GetString("textEnclosureUrl.ToolTip"));
            this.textEnclosureUrl.TextChanged += new System.EventHandler(this.textEnclosureUrl_TextChanged);
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // tabPageAdvanced
            // 
            this.tabPageAdvanced.Controls.Add(this.tableLayoutPanelAdvanced);
            resources.ApplyResources(this.tabPageAdvanced, "tabPageAdvanced");
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            // 
            // tableLayoutPanelAdvanced
            // 
            resources.ApplyResources(this.tableLayoutPanelAdvanced, "tableLayoutPanelAdvanced");
            this.tableLayoutPanelAdvanced.Controls.Add(this.labelGUID, 0, 0);
            this.tableLayoutPanelAdvanced.Controls.Add(this.textGUID, 1, 0);
            this.tableLayoutPanelAdvanced.Controls.Add(this.checkPermaLink, 1, 1);
            this.tableLayoutPanelAdvanced.Name = "tableLayoutPanelAdvanced";
            // 
            // labelGUID
            // 
            resources.ApplyResources(this.labelGUID, "labelGUID");
            this.labelGUID.Name = "labelGUID";
            // 
            // textGUID
            // 
            resources.ApplyResources(this.textGUID, "textGUID");
            this.textGUID.Name = "textGUID";
            this.toolTip.SetToolTip(this.textGUID, resources.GetString("textGUID.ToolTip"));
            this.textGUID.TextChanged += new System.EventHandler(this.textGUID_TextChanged);
            // 
            // checkPermaLink
            // 
            resources.ApplyResources(this.checkPermaLink, "checkPermaLink");
            this.checkPermaLink.Checked = true;
            this.checkPermaLink.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkPermaLink.Name = "checkPermaLink";
            this.checkPermaLink.UseVisualStyleBackColor = true;
            this.checkPermaLink.CheckedChanged += new System.EventHandler(this.checkPermaLink_CheckedChanged);
            // 
            // toolStripItems
            // 
            resources.ApplyResources(this.toolStripItems, "toolStripItems");
            this.toolStripItems.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripItems.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddItem,
            this.btnDeleteItem,
            this.btnMoveUpItem,
            this.btnMoveDownItem});
            this.toolStripItems.Name = "toolStripItems";
            // 
            // btnAddItem
            // 
            this.btnAddItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnAddItem, "btnAddItem");
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Click += new System.EventHandler(this.menuItemAddTopic_Click);
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDeleteItem, "btnDeleteItem");
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Click += new System.EventHandler(this.menuItemDeleteTopic_Click);
            // 
            // btnMoveUpItem
            // 
            this.btnMoveUpItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveUpItem, "btnMoveUpItem");
            this.btnMoveUpItem.Name = "btnMoveUpItem";
            this.btnMoveUpItem.Click += new System.EventHandler(this.menuItemTopicUp_Click);
            // 
            // btnMoveDownItem
            // 
            this.btnMoveDownItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnMoveDownItem, "btnMoveDownItem");
            this.btnMoveDownItem.Name = "btnMoveDownItem";
            this.btnMoveDownItem.Click += new System.EventHandler(this.menuItemTopicDown_Click);
            // 
            // splitterV
            // 
            resources.ApplyResources(this.splitterV, "splitterV");
            this.splitterV.Name = "splitterV";
            this.splitterV.TabStop = false;
            // 
            // panelLeft
            // 
            resources.ApplyResources(this.panelLeft, "panelLeft");
            this.panelLeft.Controls.Add(this.groupBox3);
            this.panelLeft.Controls.Add(this.groupBox2);
            this.panelLeft.Controls.Add(this.groupBox1);
            this.panelLeft.Name = "panelLeft";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.comboStyleType);
            this.groupBox3.Controls.Add(this.textBoxStyleSheet);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // comboStyleType
            // 
            resources.ApplyResources(this.comboStyleType, "comboStyleType");
            this.comboStyleType.Items.AddRange(new object[] {
            resources.GetString("comboStyleType.Items"),
            resources.GetString("comboStyleType.Items1")});
            this.comboStyleType.Name = "comboStyleType";
            this.comboStyleType.TextChanged += new System.EventHandler(this.comboStyleType_TextChanged);
            // 
            // textBoxStyleSheet
            // 
            resources.ApplyResources(this.textBoxStyleSheet, "textBoxStyleSheet");
            this.textBoxStyleSheet.Name = "textBoxStyleSheet";
            this.toolTip.SetToolTip(this.textBoxStyleSheet, resources.GetString("textBoxStyleSheet.ToolTip"));
            this.textBoxStyleSheet.TextChanged += new System.EventHandler(this.textBoxStyleSheet_TextChanged);
            this.textBoxStyleSheet.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxStyleSheet_Validating);
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.textBoxImgHeight);
            this.groupBox2.Controls.Add(this.textBoxImgWidth);
            this.groupBox2.Controls.Add(this.textBoxImgURL);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // textBoxImgHeight
            // 
            resources.ApplyResources(this.textBoxImgHeight, "textBoxImgHeight");
            this.textBoxImgHeight.Name = "textBoxImgHeight";
            this.toolTip.SetToolTip(this.textBoxImgHeight, resources.GetString("textBoxImgHeight.ToolTip"));
            this.textBoxImgHeight.TextChanged += new System.EventHandler(this.textBoxImgHeight_TextChanged);
            // 
            // textBoxImgWidth
            // 
            resources.ApplyResources(this.textBoxImgWidth, "textBoxImgWidth");
            this.textBoxImgWidth.Name = "textBoxImgWidth";
            this.toolTip.SetToolTip(this.textBoxImgWidth, resources.GetString("textBoxImgWidth.ToolTip"));
            this.textBoxImgWidth.TextChanged += new System.EventHandler(this.textBoxImgWidth_TextChanged);
            // 
            // textBoxImgURL
            // 
            resources.ApplyResources(this.textBoxImgURL, "textBoxImgURL");
            this.textBoxImgURL.Name = "textBoxImgURL";
            this.toolTip.SetToolTip(this.textBoxImgURL, resources.GetString("textBoxImgURL.ToolTip"));
            this.textBoxImgURL.TextChanged += new System.EventHandler(this.textBoxImgURL_TextChanged);
            this.textBoxImgURL.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxImgURL_Validating);
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.textBoxFeedURL);
            this.groupBox1.Controls.Add(this.lblFeedURL);
            this.groupBox1.Controls.Add(this.lblHub);
            this.groupBox1.Controls.Add(this.comboBoxLanguage);
            this.groupBox1.Controls.Add(this.cmbHubs);
            this.groupBox1.Controls.Add(this.textBoxDescription);
            this.groupBox1.Controls.Add(this.textBoxWebmaster);
            this.groupBox1.Controls.Add(this.textBoxEditor);
            this.groupBox1.Controls.Add(this.textBoxCopyright);
            this.groupBox1.Controls.Add(this.textBoxWebURL);
            this.groupBox1.Controls.Add(this.textBoxTitle);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lblHub
            // 
            resources.ApplyResources(this.lblHub, "lblHub");
            this.lblHub.Name = "lblHub";
            // 
            // comboBoxLanguage
            // 
            resources.ApplyResources(this.comboBoxLanguage, "comboBoxLanguage");
            this.comboBoxLanguage.Items.AddRange(new object[] {
            resources.GetString("comboBoxLanguage.Items"),
            resources.GetString("comboBoxLanguage.Items1"),
            resources.GetString("comboBoxLanguage.Items2"),
            resources.GetString("comboBoxLanguage.Items3"),
            resources.GetString("comboBoxLanguage.Items4"),
            resources.GetString("comboBoxLanguage.Items5"),
            resources.GetString("comboBoxLanguage.Items6"),
            resources.GetString("comboBoxLanguage.Items7"),
            resources.GetString("comboBoxLanguage.Items8"),
            resources.GetString("comboBoxLanguage.Items9"),
            resources.GetString("comboBoxLanguage.Items10"),
            resources.GetString("comboBoxLanguage.Items11"),
            resources.GetString("comboBoxLanguage.Items12"),
            resources.GetString("comboBoxLanguage.Items13"),
            resources.GetString("comboBoxLanguage.Items14"),
            resources.GetString("comboBoxLanguage.Items15"),
            resources.GetString("comboBoxLanguage.Items16"),
            resources.GetString("comboBoxLanguage.Items17"),
            resources.GetString("comboBoxLanguage.Items18"),
            resources.GetString("comboBoxLanguage.Items19"),
            resources.GetString("comboBoxLanguage.Items20"),
            resources.GetString("comboBoxLanguage.Items21"),
            resources.GetString("comboBoxLanguage.Items22"),
            resources.GetString("comboBoxLanguage.Items23"),
            resources.GetString("comboBoxLanguage.Items24"),
            resources.GetString("comboBoxLanguage.Items25"),
            resources.GetString("comboBoxLanguage.Items26"),
            resources.GetString("comboBoxLanguage.Items27"),
            resources.GetString("comboBoxLanguage.Items28"),
            resources.GetString("comboBoxLanguage.Items29"),
            resources.GetString("comboBoxLanguage.Items30"),
            resources.GetString("comboBoxLanguage.Items31"),
            resources.GetString("comboBoxLanguage.Items32"),
            resources.GetString("comboBoxLanguage.Items33"),
            resources.GetString("comboBoxLanguage.Items34"),
            resources.GetString("comboBoxLanguage.Items35"),
            resources.GetString("comboBoxLanguage.Items36"),
            resources.GetString("comboBoxLanguage.Items37"),
            resources.GetString("comboBoxLanguage.Items38"),
            resources.GetString("comboBoxLanguage.Items39"),
            resources.GetString("comboBoxLanguage.Items40"),
            resources.GetString("comboBoxLanguage.Items41"),
            resources.GetString("comboBoxLanguage.Items42"),
            resources.GetString("comboBoxLanguage.Items43"),
            resources.GetString("comboBoxLanguage.Items44"),
            resources.GetString("comboBoxLanguage.Items45"),
            resources.GetString("comboBoxLanguage.Items46"),
            resources.GetString("comboBoxLanguage.Items47"),
            resources.GetString("comboBoxLanguage.Items48"),
            resources.GetString("comboBoxLanguage.Items49"),
            resources.GetString("comboBoxLanguage.Items50"),
            resources.GetString("comboBoxLanguage.Items51"),
            resources.GetString("comboBoxLanguage.Items52"),
            resources.GetString("comboBoxLanguage.Items53"),
            resources.GetString("comboBoxLanguage.Items54"),
            resources.GetString("comboBoxLanguage.Items55"),
            resources.GetString("comboBoxLanguage.Items56"),
            resources.GetString("comboBoxLanguage.Items57"),
            resources.GetString("comboBoxLanguage.Items58"),
            resources.GetString("comboBoxLanguage.Items59"),
            resources.GetString("comboBoxLanguage.Items60"),
            resources.GetString("comboBoxLanguage.Items61"),
            resources.GetString("comboBoxLanguage.Items62"),
            resources.GetString("comboBoxLanguage.Items63"),
            resources.GetString("comboBoxLanguage.Items64"),
            resources.GetString("comboBoxLanguage.Items65"),
            resources.GetString("comboBoxLanguage.Items66"),
            resources.GetString("comboBoxLanguage.Items67"),
            resources.GetString("comboBoxLanguage.Items68"),
            resources.GetString("comboBoxLanguage.Items69"),
            resources.GetString("comboBoxLanguage.Items70"),
            resources.GetString("comboBoxLanguage.Items71"),
            resources.GetString("comboBoxLanguage.Items72"),
            resources.GetString("comboBoxLanguage.Items73"),
            resources.GetString("comboBoxLanguage.Items74"),
            resources.GetString("comboBoxLanguage.Items75"),
            resources.GetString("comboBoxLanguage.Items76"),
            resources.GetString("comboBoxLanguage.Items77"),
            resources.GetString("comboBoxLanguage.Items78"),
            resources.GetString("comboBoxLanguage.Items79"),
            resources.GetString("comboBoxLanguage.Items80"),
            resources.GetString("comboBoxLanguage.Items81"),
            resources.GetString("comboBoxLanguage.Items82"),
            resources.GetString("comboBoxLanguage.Items83"),
            resources.GetString("comboBoxLanguage.Items84"),
            resources.GetString("comboBoxLanguage.Items85"),
            resources.GetString("comboBoxLanguage.Items86"),
            resources.GetString("comboBoxLanguage.Items87"),
            resources.GetString("comboBoxLanguage.Items88"),
            resources.GetString("comboBoxLanguage.Items89"),
            resources.GetString("comboBoxLanguage.Items90"),
            resources.GetString("comboBoxLanguage.Items91"),
            resources.GetString("comboBoxLanguage.Items92"),
            resources.GetString("comboBoxLanguage.Items93"),
            resources.GetString("comboBoxLanguage.Items94")});
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.toolTip.SetToolTip(this.comboBoxLanguage, resources.GetString("comboBoxLanguage.ToolTip"));
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBoxLang_SelectedIndexChanged);
            // 
            // cmbHubs
            // 
            resources.ApplyResources(this.cmbHubs, "cmbHubs");
            this.cmbHubs.Items.AddRange(new object[] {
            resources.GetString("cmbHubs.Items"),
            resources.GetString("cmbHubs.Items1")});
            this.cmbHubs.Name = "cmbHubs";
            this.toolTip.SetToolTip(this.cmbHubs, resources.GetString("cmbHubs.ToolTip"));
            this.cmbHubs.SelectedIndexChanged += new System.EventHandler(this.cmbHubs_SelectedIndexChanged);
            // 
            // textBoxDescription
            // 
            resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
            this.textBoxDescription.Name = "textBoxDescription";
            this.toolTip.SetToolTip(this.textBoxDescription, resources.GetString("textBoxDescription.ToolTip"));
            this.textBoxDescription.TextChanged += new System.EventHandler(this.textBoxDescription_TextChanged);
            this.textBoxDescription.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDescription_Validating);
            // 
            // textBoxWebmaster
            // 
            resources.ApplyResources(this.textBoxWebmaster, "textBoxWebmaster");
            this.textBoxWebmaster.Name = "textBoxWebmaster";
            this.toolTip.SetToolTip(this.textBoxWebmaster, resources.GetString("textBoxWebmaster.ToolTip"));
            this.textBoxWebmaster.TextChanged += new System.EventHandler(this.textBoxWebmaster_TextChanged);
            this.textBoxWebmaster.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxWebmaster_Validating);
            // 
            // textBoxEditor
            // 
            resources.ApplyResources(this.textBoxEditor, "textBoxEditor");
            this.textBoxEditor.Name = "textBoxEditor";
            this.toolTip.SetToolTip(this.textBoxEditor, resources.GetString("textBoxEditor.ToolTip"));
            this.textBoxEditor.TextChanged += new System.EventHandler(this.textBoxEditor_TextChanged);
            this.textBoxEditor.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxEditor_Validating);
            // 
            // textBoxCopyright
            // 
            resources.ApplyResources(this.textBoxCopyright, "textBoxCopyright");
            this.textBoxCopyright.Name = "textBoxCopyright";
            this.toolTip.SetToolTip(this.textBoxCopyright, resources.GetString("textBoxCopyright.ToolTip"));
            this.textBoxCopyright.TextChanged += new System.EventHandler(this.textBoxCopyright_TextChanged);
            // 
            // textBoxWebURL
            // 
            resources.ApplyResources(this.textBoxWebURL, "textBoxWebURL");
            this.textBoxWebURL.Name = "textBoxWebURL";
            this.toolTip.SetToolTip(this.textBoxWebURL, resources.GetString("textBoxWebURL.ToolTip"));
            this.textBoxWebURL.TextChanged += new System.EventHandler(this.textBoxWebURL_TextChanged);
            this.textBoxWebURL.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxWebURL_Validating);
            // 
            // textBoxTitle
            // 
            resources.ApplyResources(this.textBoxTitle, "textBoxTitle");
            this.textBoxTitle.Name = "textBoxTitle";
            this.toolTip.SetToolTip(this.textBoxTitle, resources.GetString("textBoxTitle.ToolTip"));
            this.textBoxTitle.TextChanged += new System.EventHandler(this.textBoxTitle_TextChanged);
            this.textBoxTitle.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxTitle_Validating);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // statusBar
            // 
            resources.ApplyResources(this.statusBar, "statusBar");
            this.statusBar.Name = "statusBar";
            // 
            // toolStrip
            // 
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnSave,
            this.toolStripSeparator1,
            this.btnOpen,
            this.btnPreview,
            this.btnDownload,
            this.btnPublish,
            this.toolStripSeparator2,
            this.btnSiteManager,
            this.toolStripSeparator3,
            this.btnDonate});
            this.toolStrip.Name = "toolStrip";
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnNew, "btnNew");
            this.btnNew.Name = "btnNew";
            this.btnNew.Click += new System.EventHandler(this.menuItemNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnOpen, "btnOpen");
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnPreview, "btnPreview");
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Click += new System.EventHandler(this.menuItemViewXml_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDownload, "btnDownload");
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Click += new System.EventHandler(this.menuItemDownloadFeed_Click);
            // 
            // btnPublish
            // 
            this.btnPublish.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnPublish, "btnPublish");
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Click += new System.EventHandler(this.menuItemPublishFeed_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnSiteManager
            // 
            this.btnSiteManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnSiteManager, "btnSiteManager");
            this.btnSiteManager.Name = "btnSiteManager";
            this.btnSiteManager.Click += new System.EventHandler(this.menuItemSiteManager_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // btnDonate
            // 
            this.btnDonate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDonate, "btnDonate");
            this.btnDonate.Name = "btnDonate";
            this.btnDonate.Click += new System.EventHandler(this.btnDonate_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Silver;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "");
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItem2,
            this.menuItem5,
            this.menuItemSettings,
            this.menuItem6});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNew,
            this.menuItemOpen,
            this.menuItemSave,
            this.menuItemSaveAs,
            this.menuItem1,
            this.menuItemDownloadFeed,
            this.menuItemPublishFeed,
            this.menuItem9,
            this.menuItemSaveAsHTML,
            this.menuItem12,
            this.menuItemExit});
            resources.ApplyResources(this.menuItemFile, "menuItemFile");
            // 
            // menuItemNew
            // 
            this.menuItemNew.Index = 0;
            resources.ApplyResources(this.menuItemNew, "menuItemNew");
            this.menuItemNew.Click += new System.EventHandler(this.menuItemNew_Click);
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 1;
            resources.ApplyResources(this.menuItemOpen, "menuItemOpen");
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // menuItemSave
            // 
            this.menuItemSave.Index = 2;
            resources.ApplyResources(this.menuItemSave, "menuItemSave");
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemSaveAs
            // 
            this.menuItemSaveAs.Index = 3;
            resources.ApplyResources(this.menuItemSaveAs, "menuItemSaveAs");
            this.menuItemSaveAs.Click += new System.EventHandler(this.menuItemSaveAs_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 4;
            resources.ApplyResources(this.menuItem1, "menuItem1");
            // 
            // menuItemDownloadFeed
            // 
            this.menuItemDownloadFeed.Index = 5;
            resources.ApplyResources(this.menuItemDownloadFeed, "menuItemDownloadFeed");
            this.menuItemDownloadFeed.Click += new System.EventHandler(this.menuItemDownloadFeed_Click);
            // 
            // menuItemPublishFeed
            // 
            this.menuItemPublishFeed.Index = 6;
            resources.ApplyResources(this.menuItemPublishFeed, "menuItemPublishFeed");
            this.menuItemPublishFeed.Click += new System.EventHandler(this.menuItemPublishFeed_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 7;
            resources.ApplyResources(this.menuItem9, "menuItem9");
            // 
            // menuItemSaveAsHTML
            // 
            this.menuItemSaveAsHTML.Index = 8;
            resources.ApplyResources(this.menuItemSaveAsHTML, "menuItemSaveAsHTML");
            this.menuItemSaveAsHTML.Click += new System.EventHandler(this.menuItemSaveAsHTML_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 9;
            resources.ApplyResources(this.menuItem12, "menuItem12");
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 10;
            resources.ApplyResources(this.menuItemExit, "menuItemExit");
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAddTopic,
            this.menuItemDeleteTopic,
            this.menuItemTopicUp,
            this.menuItemTopicDown});
            resources.ApplyResources(this.menuItem2, "menuItem2");
            // 
            // menuItemAddTopic
            // 
            this.menuItemAddTopic.Index = 0;
            resources.ApplyResources(this.menuItemAddTopic, "menuItemAddTopic");
            this.menuItemAddTopic.Click += new System.EventHandler(this.menuItemAddTopic_Click);
            // 
            // menuItemDeleteTopic
            // 
            this.menuItemDeleteTopic.Index = 1;
            resources.ApplyResources(this.menuItemDeleteTopic, "menuItemDeleteTopic");
            this.menuItemDeleteTopic.Click += new System.EventHandler(this.menuItemDeleteTopic_Click);
            // 
            // menuItemTopicUp
            // 
            this.menuItemTopicUp.Index = 2;
            resources.ApplyResources(this.menuItemTopicUp, "menuItemTopicUp");
            this.menuItemTopicUp.Click += new System.EventHandler(this.menuItemTopicUp_Click);
            // 
            // menuItemTopicDown
            // 
            this.menuItemTopicDown.Index = 3;
            resources.ApplyResources(this.menuItemTopicDown, "menuItemTopicDown");
            this.menuItemTopicDown.Click += new System.EventHandler(this.menuItemTopicDown_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 2;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemViewToolBar,
            this.menuItemViewFeedProp,
            this.menuItem8,
            this.menuItemViewXml});
            resources.ApplyResources(this.menuItem5, "menuItem5");
            // 
            // menuItemViewToolBar
            // 
            this.menuItemViewToolBar.Checked = true;
            this.menuItemViewToolBar.Index = 0;
            resources.ApplyResources(this.menuItemViewToolBar, "menuItemViewToolBar");
            this.menuItemViewToolBar.Click += new System.EventHandler(this.menuItemViewToolBar_Click);
            // 
            // menuItemViewFeedProp
            // 
            this.menuItemViewFeedProp.Checked = true;
            this.menuItemViewFeedProp.Index = 1;
            resources.ApplyResources(this.menuItemViewFeedProp, "menuItemViewFeedProp");
            this.menuItemViewFeedProp.Click += new System.EventHandler(this.menuItemViewFeedProp_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 2;
            resources.ApplyResources(this.menuItem8, "menuItem8");
            // 
            // menuItemViewXml
            // 
            this.menuItemViewXml.Index = 3;
            resources.ApplyResources(this.menuItemViewXml, "menuItemViewXml");
            this.menuItemViewXml.Click += new System.EventHandler(this.menuItemViewXml_Click);
            // 
            // menuItemSettings
            // 
            this.menuItemSettings.Index = 3;
            this.menuItemSettings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSiteManager,
            this.menuItem7});
            resources.ApplyResources(this.menuItemSettings, "menuItemSettings");
            // 
            // menuItemSiteManager
            // 
            this.menuItemSiteManager.Index = 0;
            resources.ApplyResources(this.menuItemSiteManager, "menuItemSiteManager");
            this.menuItemSiteManager.Click += new System.EventHandler(this.menuItemSiteManager_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 1;
            this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemEnglish,
            this.menuItemGerman,
            this.menuItemDutch,
            this.menuItemCzech});
            resources.ApplyResources(this.menuItem7, "menuItem7");
            // 
            // menuItemEnglish
            // 
            this.menuItemEnglish.Index = 0;
            resources.ApplyResources(this.menuItemEnglish, "menuItemEnglish");
            this.menuItemEnglish.Click += new System.EventHandler(this.menuItemEnglish_Click);
            // 
            // menuItemGerman
            // 
            this.menuItemGerman.Index = 1;
            resources.ApplyResources(this.menuItemGerman, "menuItemGerman");
            this.menuItemGerman.Click += new System.EventHandler(this.menuItemGerman_Click);
            // 
            // menuItemDutch
            // 
            this.menuItemDutch.Index = 2;
            resources.ApplyResources(this.menuItemDutch, "menuItemDutch");
            this.menuItemDutch.Click += new System.EventHandler(this.menuItemDutch_Click);
            // 
            // menuItemCzech
            // 
            this.menuItemCzech.Index = 3;
            resources.ApplyResources(this.menuItemCzech, "menuItemCzech");
            this.menuItemCzech.Click += new System.EventHandler(this.menuItemCzech_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 4;
            this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAbout,
            this.menuItemRSSBuilderWeb});
            resources.ApplyResources(this.menuItem6, "menuItem6");
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            resources.ApplyResources(this.menuItemAbout, "menuItemAbout");
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // menuItemRSSBuilderWeb
            // 
            this.menuItemRSSBuilderWeb.Index = 1;
            resources.ApplyResources(this.menuItemRSSBuilderWeb, "menuItemRSSBuilderWeb");
            this.menuItemRSSBuilderWeb.Click += new System.EventHandler(this.menuItemRSSBuilderWeb_Click);
            // 
            // openFileDialog
            // 
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "rss";
            resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // textBoxFeedURL
            // 
            resources.ApplyResources(this.textBoxFeedURL, "textBoxFeedURL");
            this.textBoxFeedURL.Name = "textBoxFeedURL";
            this.toolTip.SetToolTip(this.textBoxFeedURL, resources.GetString("textBoxFeedURL.ToolTip"));
            this.textBoxFeedURL.TextChanged += new System.EventHandler(this.textBoxFeedURL_TextChanged);
            // 
            // lblFeedURL
            // 
            resources.ApplyResources(this.lblFeedURL, "lblFeedURL");
            this.lblFeedURL.Name = "lblFeedURL";
            // 
            // FormRSSBuilder
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.toolStripContainer);
            this.Menu = this.mainMenu;
            this.Name = "FormRSSBuilder";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormRSSBuilder_Closing);
            this.Closed += new System.EventHandler(this.FormRSSBuilder_Closed);
            this.Load += new System.EventHandler(this.FormRSSBuilder_Load);
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.panelItemProperties.ResumeLayout(false);
            this.panelItemProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeOffset)).EndInit();
            this.tabPageEnclosure.ResumeLayout(false);
            this.tabPageEnclosure.PerformLayout();
            this.tabPageAdvanced.ResumeLayout(false);
            this.tabPageAdvanced.PerformLayout();
            this.tableLayoutPanelAdvanced.ResumeLayout(false);
            this.tableLayoutPanelAdvanced.PerformLayout();
            this.toolStripItems.ResumeLayout(false);
            this.toolStripItems.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

      }
      #endregion

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main(string[] args) 
      {
         FormRSSBuilder mainForm = new FormRSSBuilder();
         
         if(args.Length > 0)
         {
            string associatedFile = args[0];
            mainForm.OpenFeed(associatedFile);
         }

         Application.Run(mainForm);
      }

      private void comboBoxLang_SelectedIndexChanged(object sender, System.EventArgs e)
      {
         _rssFeed.Language = comboBoxLanguage.Text;
         _changed = true;
      }

      private void textBoxTitle_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.Title = textBoxTitle.Text;
         _changed = true;
      }

      private void textBoxWebURL_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.WebURL = textBoxWebURL.Text;
         _changed = true;
      }

      private void textBoxCopyright_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.Copyright = textBoxCopyright.Text;
         _changed = true;
      }

      private void textBoxEditor_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.Editor = textBoxEditor.Text;
         _changed = true;
      }

      private void textBoxWebmaster_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.Webmaster = textBoxWebmaster.Text;
         _changed = true;
      }

      private void textBoxDescription_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.Description = textBoxDescription.Text;
         _changed = true;
      }

      private void textBoxStyleSheet_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.StyleSheet = textBoxStyleSheet.Text;
         _changed = true;
      }

      private void textBoxImgURL_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.ImgURL = textBoxImgURL.Text;
         _changed = true;
      }

      private void textBoxImgWidth_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.ImgWidth = textBoxImgWidth.Text;
         _changed = true;
      }

      private void textBoxImgHeight_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.ImgHeight = textBoxImgHeight.Text;
         _changed = true;
      }


      private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
      {
         /**
         if(e.Button == toolBarButtonNew)
         {
            newFeed();
         }
         else if(e.Button == toolBarButtonOpen)
         {
            openFeed();
         }
         else if(e.Button == toolBarButtonSave)
         {
            saveFeed();
            this.Validate();
         }
         else if(e.Button == toolBarButtonPublish)
         {
            publishFeed();
         }
         else if(e.Button == toolBarButtonAddItem)
         {
            addNewsItem();
         }
         else if(e.Button == toolBarButtonDelItem)
         {
            deleteNewsItem();
         }
         else if(e.Button == toolBarButtonUp)
         {
            shiftNewsItemUp();
         }
         else if(e.Button == toolBarButtonDown)
         {
            shiftNewsItemDown();
         }
             */
      }

      private void menuItemViewFeedProp_Click(object sender, System.EventArgs e)
      {
         menuItemViewFeedProp.Checked = !menuItemViewFeedProp.Checked;
         panelLeft.Visible = menuItemViewFeedProp.Checked;
      }

      private void menuItemViewToolBar_Click(object sender, System.EventArgs e)
      {
         menuItemViewToolBar.Checked = !menuItemViewToolBar.Checked;
         toolStripContainer.TopToolStripPanel.Visible = menuItemViewToolBar.Checked;
      }

      private void listViewItems_SelectedIndexChanged(object sender, System.EventArgs e)
      {
         bool wasChanged = _changed;
         
         showSelectedRSSItem();

         _changed = wasChanged;

      }


      private void textTitle_TextChanged(object sender, System.EventArgs e)
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selItem.Text = textTitle.Text;
            selRssItem.Title = textTitle.Text;
         }
      }

      private void dateTimePicker_ValueChanged(object sender, System.EventArgs e)
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

         //   selItem.SubItems[1].Text = dateTimePicker.Text;
            // ddd, d MMM yyyy HH:mm:ss
            string dateTime = dateTimePicker.Value.ToString(_dateTimeFormat);
            selItem.SubItems[1].Text = dateTime;
            selRssItem.PubDate = dateTime;

            _changed = true;
         }
      }

      private void textLink_TextChanged(object sender, System.EventArgs e)
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.Link = textLink.Text;

            _changed = true;
         }
      }

      private void textAuthor_TextChanged(object sender, System.EventArgs e)
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.Author = textAuthor.Text;
            _changed = true;
         }
      }

      private void textComments_TextChanged(object sender, System.EventArgs e)
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.Comments = textComments.Text;
            _changed = true;
         }
      }

      private void textDesc_TextChanged(object sender, System.EventArgs e)
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.Description = textDesc.Text;
            _changed = true;
         }
      }

      private void textCategory_TextChanged(object sender, System.EventArgs e)
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.Category = textCategory.Text;
            _changed = true;
         }
      }

      private void numTimeOffset_ValueChanged(object sender, System.EventArgs e)
      {
         if(listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.TimeOffset = (int)numTimeOffset.Value;
            _changed = true;
         }
      }

      private void menuItemNew_Click(object sender, System.EventArgs e)
      {
         newFeed(); 
      }

      private void menuItemOpen_Click(object sender, System.EventArgs e)
      {
         openFeed(); 
      }

      private void menuItemSave_Click(object sender, System.EventArgs e)
      {
         saveFeed(); 
      }

      private void menuItemSaveAs_Click(object sender, System.EventArgs e)
      {
         saveFeedAs(); 
      }

      private void menuItemAddTopic_Click(object sender, System.EventArgs e)
      {
         addNewsItem();
      }

      private void menuItemDeleteTopic_Click(object sender, System.EventArgs e)
      {
         deleteNewsItem();
      }

      private void menuItemViewXml_Click(object sender, System.EventArgs e)
      {
         FormShowXml formXml = new FormShowXml(this._rssFeed);
         formXml.ShowDialog(this);
      }


      private void menuItemExit_Click(object sender, System.EventArgs e)
      {
         Close();
      }

      private void menuItemAbout_Click(object sender, System.EventArgs e)
      {
         (new FormAbout()).ShowDialog();
      }

      private void menuItemRSSBuilderWeb_Click(object sender, System.EventArgs e)
      {
         try
         {
            System.Diagnostics.Process.Start("http://home.hetnet.nl/~bsoft/rssbuilder/index.htm" );
         }
         catch
         {
         }
      }


      private void requireText(TextBox textBox)
      {
         if(textBox.Text == "")
            errorProvider.SetError(textBox, "Required");
         else
            errorProvider.SetError(textBox, "");     
      }

      private void requireValidEmail(TextBox textBox, bool optional)
      {
         errorProvider.SetError(textBox, "");

         if(optional && textBox.Text == "")
            return;

         if( (textBox.Text.IndexOf('.') == -1) || (textBox.Text.IndexOf('@') == -1))
            errorProvider.SetError(textBox, "Must be a valid email address");
      }

      private void requireValidURL(TextBox textBox, bool optional)
      {
         errorProvider.SetError(textBox, "");

         if(optional && textBox.Text == "")
            return;

         if(textBox.Text.IndexOf('.') == -1)
            errorProvider.SetError(textBox, "Must be a valid URL");
      }

      private void textBoxTitle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireText(textBoxTitle);
      }

      private void textBoxWebURL_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireText(textBoxWebURL);
      }

      private void textBoxDescription_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireText(textBoxDescription);
      }

      private void textBoxStyleSheet_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         errorProvider.SetError(textBoxStyleSheet, "");

         if(textBoxStyleSheet.Text.IndexOfAny(Path.GetInvalidPathChars()) > 0)
            errorProvider.SetError(textBoxStyleSheet, "Must be a valid file name");
      }

      private void textTitle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireText(textTitle);
      }

      private void textDesc_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireText(textDesc);
      }

      private void textLink_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireValidURL(textLink, true);
 
      }

      private void textBoxEditor_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireValidEmail(textBoxEditor, true);
      }

      private void textBoxWebmaster_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireValidEmail(textBoxWebmaster, true);
      }

      private void textComments_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireValidURL(textComments, true);      
      }

      private void textBoxImgURL_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireValidURL(textBoxImgURL, true);
      }

      private void textAuthor_Validating(object sender, System.ComponentModel.CancelEventArgs e)
      {
         requireValidEmail(textAuthor, true);
      }

      private void FormRSSBuilder_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         if(! askSaveChanges())
            e.Cancel = true;
      }

      private void buttonLocalOffset_Click(object sender, System.EventArgs e)
      {
         numTimeOffset.Value = _localTimeOffset;
      }

      private void buttonHTML_Click(object sender, System.EventArgs e)
      {
         FormHTMLEdit formHtml = new FormHTMLEdit(textDesc.Text);
         formHtml.ShowDialog();

         textDesc.Text = formHtml.textDesc;
      }

      private void menuItemSaveAsHTML_Click(object sender, System.EventArgs e)
      {
         FormSaveHTML form = new FormSaveHTML(_rssFeed, _fileName);
         form.ShowDialog();
         //rssFeed.saveFeedAsHTML("test.htm", 100);
//         RSS2HTML rss2html = new RSS2HTML(rssFeed);

  //       rss2html.translate(Environment.GetFolderPath(Environment.SpecialFolder.Personal)+"/rss2html_1.xslt", "test.htm");
      }

      private void menuItemSiteManager_Click(object sender, System.EventArgs e)
      {
         startSiteManager();
      }

      private void menuItemDownloadFeed_Click(object sender, System.EventArgs e)
      {
         downloadFeed();
      }

      private void menuItemPublishFeed_Click(object sender, System.EventArgs e)
      {
         publishFeed();
      }

      private void menuItemTopicUp_Click(object sender, System.EventArgs e)
      {
         shiftNewsItemUp();
      }

      private void menuItemTopicDown_Click(object sender, System.EventArgs e)
      {
         shiftNewsItemDown();
      }


      private void menuItemEnglish_Click(object sender, System.EventArgs e)
      {
         menuItemEnglish.Checked = true;
         menuItemGerman.Checked = false;
         menuItemCzech.Checked = false;
         menuItemDutch.Checked = false;
         _settings.SetValue("UserInterface","language", "english");

         MessageBox.Show(_res.GetString("msgLanguage"),"RSS Builder", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      private void menuItemGerman_Click(object sender, System.EventArgs e)
      {
         menuItemEnglish.Checked = false;
         menuItemGerman.Checked = true;
         menuItemCzech.Checked = false;
         menuItemDutch.Checked = false;
         _settings.SetValue("UserInterface","language", "german");

         MessageBox.Show(_res.GetString("msgLanguage"),"RSS Builder", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      private void menuItemDutch_Click(object sender, System.EventArgs e)
      {
         menuItemEnglish.Checked = false;
         menuItemGerman.Checked = false;
         menuItemCzech.Checked = false;
         menuItemDutch.Checked = true;
         _settings.SetValue("UserInterface","language", "dutch");

         MessageBox.Show(_res.GetString("msgLanguage"),"RSS Builder", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

       private void menuItemCzech_Click(object sender, EventArgs e)
       {
           menuItemEnglish.Checked = false;
           menuItemGerman.Checked = false;
           menuItemCzech.Checked = true;
           menuItemDutch.Checked = false;
           _settings.SetValue("UserInterface", "language", "czech");

           MessageBox.Show(_res.GetString("msgLanguage"), "RSS Builder",
              MessageBoxButtons.OK, MessageBoxIcon.Information);
       }

      private void FormRSSBuilder_Closed(object sender, System.EventArgs e)
      {
         _settings.Save();
      }

      private void comboStyleType_TextChanged(object sender, System.EventArgs e)
      {
         _rssFeed.StyleType = comboStyleType.Text;
         _changed = true;
      }

      private void textEnclosureUrl_TextChanged(object sender, EventArgs e)
      {
         if (listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.EnclosureUrl = textEnclosureUrl.Text;
            _changed = true;
         }
      }

      private void textEnclosureLength_TextChanged(object sender, EventArgs e)
      {
         if (listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.EnclosureLength = textEnclosureLength.Text;
            _changed = true;
         }
      }

      private void comboEnclosureType_TextChanged(object sender, EventArgs e)
      {
         if (listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.EnclosureType = comboEnclosureType.Text;
            _changed = true;
         }
      }

      private void textGUID_TextChanged(object sender, EventArgs e)
      {
         if (listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.GUID = textGUID.Text;
            _changed = true;
         }
      }

      private void checkPermaLink_CheckedChanged(object sender, EventArgs e)
      {
         if (listViewItems.SelectedItems.Count > 0)
         {
            ListViewItem selItem = listViewItems.SelectedItems[0];
            RSSItem selRssItem = (RSSItem)selItem.Tag;

            selRssItem.IsPermaLink = checkPermaLink.Checked;
            _changed = true;
         }
      }


      private void determineEnclosureLengthAndType(string fileName)
      {
         FileInfo fileInfo = new FileInfo(fileName);
         textEnclosureLength.Text = fileInfo.Length.ToString();

         string extension = Path.GetExtension(fileName);
         string enclType = "";
         switch (extension.ToLower())
         {
            case ".au":
               enclType = "audio/basic";
               break;
            case ".mp3":
               enclType = "audio/mpeg";
               break;
            case ".mid":
               enclType = "audio/mid";
               break;
            case ".rmi":
               enclType = "audio/mid";
               break;
            case ".aif":
               enclType = "audio/x-aiff";
               break;
            case ".wav":
               enclType = "audio/x-wav";
               break;
            case ".ra":
               enclType = "audio/x-pn-realaudio";
               break;
            case ".ram":
               enclType = "audio/x-pn-realaudio";
               break;
            case ".bmp":
               enclType = "image/bmp";
               break;
            case ".gif":
               enclType = "image/gif";
               break;
            case ".jpg":
               enclType = "image/jpeg";
               break;
            case ".jpeg":
               enclType = "image/jpeg";
               break;
            case ".tif":
               enclType = "image/tiff";
               break;
            case ".tiff":
               enclType = "image/tiff";
               break;
            case ".mpg":
               enclType = "video/mpeg";
               break;
            case ".mpeg":
               enclType = "video/mpeg";
               break;
            case ".mpa":
               enclType = "video/mpeg";
               break;
            case ".mov":
               enclType = "video/quicktime";
               break;
            case ".qt":
               enclType = "video/quicktime";
               break;
            case ".avi":
               enclType = "video/x-msvideo";
               break;
            case ".wmv":
               enclType = "video/x-ms-wmv";
               break;
            default: enclType = "";
               break;
         }
         comboEnclosureType.Text = enclType;
      }
      
      private void btnDetermineEnclosure_Click(object sender, EventArgs e)
      {
         openEnclosureDialog.FileName = "";

         if (openEnclosureDialog.ShowDialog() == DialogResult.OK)
         {
            string fileName = openEnclosureDialog.FileName;

            if (fileName != "")
            {
               textEnclosureUrl.Text = this.textBoxWebURL.Text + "/" + Path.GetFileName(fileName);
               determineEnclosureLengthAndType(fileName);
            }
         }
      }

      private void linkDetermineFromURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         string errorStr = "";

         if (Uri.IsWellFormedUriString(textEnclosureUrl.Text, UriKind.Absolute))
         {
            string uriFileName = textEnclosureUrl.Text.Replace("http:", "");
            UriBuilder uriBuild = new UriBuilder(textEnclosureUrl.Text);

            
            
            labelWait.Show();
            labelWait.Refresh();
            try
            {
               determineEnclosureLengthAndType(uriFileName);
            }
            catch(Exception ex)
            {
               errorStr = ex.Message;
            }
            labelWait.Hide();
         }
         else
         {
            errorStr = "URL is invalid";
         }

         errorProvider.SetError(textEnclosureUrl, errorStr);
      }

      private void buttonUseLinkAsGuid_Click(object sender, EventArgs e)
      {
          if (string.IsNullOrEmpty(textLink.Text))
          {
              // set the entry's GUID
              textGUID.Text = Guid.NewGuid().ToString();
              checkPermaLink.Checked = false;
              textLink.BackColor = SystemColors.InactiveCaption; // don't disable, just visually
          }
          else
          {
              // use this entry URL as its permalink/GUID
              textGUID.Text = textLink.Text;
              checkPermaLink.Checked = true;
          }
      }

      private void btnUploadEnclosure_Click(object sender, EventArgs e)
      {
         if (listViewItems.SelectedItems.Count == 0)
            return;

         ListViewItem selItem = listViewItems.SelectedItems[0];
         RSSItem selRssItem = (RSSItem)selItem.Tag;

         FormFTPUploadEnclosure formFTPUploadEnclosure = 
                                      new FormFTPUploadEnclosure(_rssFeed, selRssItem);

         formFTPUploadEnclosure.UploadSucceeded += delegate(object dsender, EventArgs de)
                  {
                     showSelectedRSSItem();
                  };

         formFTPUploadEnclosure.Show(this);
      }

      private void btnDonate_Click(object sender, EventArgs e)
      {
         try
         {
            System.Diagnostics.Process.Start("http://home.hetnet.nl/~bsoft/rssbuilder");
         }
         catch
         {
         }
      }

      private void textBoxFeedURL_TextChanged(object sender, EventArgs e)
      {
          _rssFeed.FeedURL = textBoxFeedURL.Text;
          _changed = true;
      }

      private void cmbHubs_SelectedIndexChanged(object sender, EventArgs e)
      {
          _rssFeed.HubURL = cmbHubs.Text;
          _changed = true;
      }








   }
}
