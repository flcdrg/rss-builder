using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Web;
using System.Text;

namespace RSSBuilder
{
   /// <summary>
   /// An RSS news feed.
   /// </summary>
   public class RSSFeed
   {
      private string title = "";
      private string webURL = "";
      private string feedURL = "";
      private string hubURL = "";


      private string copyright = "";
      private string language = "en-us";
      private string editor = "";
      private string webmaster = "";
      private string description = "";

      private string imgURL = "";
      private string imgWidth = "";
      private string imgHeight = "";


      // some non RSS information
      private string ftpSite = "";
      private string styleSheet = "";
      private string styleType = "";

      private List<RSSItem> newsItems = new List<RSSItem>();

      public RSSFeed()
      {
      }

      /// <summary>
      /// Title of the feed
      /// </summary>
      public string Title
      {
         get { return title; }
         set { title = value; }
      }

      public string FeedURL
      {
          get { return feedURL; }
          set { feedURL = value; }
      }
      public string HubURL
      {
          get { return hubURL; }
          set { hubURL = value; }
      }
      public string WebURL
      {
          get { return webURL; }
          set { webURL = value; }
      }

      public string Copyright
      {
         get { return copyright; }
         set { copyright = value; }
      }

      public string Language
      {
         get { return language; }
         set { language = value.Split(' ')[0]; }
      }

      public string Editor
      {
         get { return editor; }
         set { editor = value; }
      }

      public string Webmaster
      {
         get { return webmaster; }
         set { webmaster = value; }
      }

      public string Description
      {
         get { return description; }
         set { description = value; }
      }

      public string ImgURL
      {
         get { return imgURL; }
         set { imgURL = value; }
      }

      public string ImgWidth
      {
         get { return imgWidth; }
         set { imgWidth = value; }
      }

      public string ImgHeight
      {
         get { return imgHeight; }
         set { imgHeight = value; }
      }


      public string FtpSite
      {
         get { return ftpSite; }
         set { ftpSite = value; }
      }
      
      public string StyleSheet
      {
         get { return styleSheet; }
         set { styleSheet = value; }
      }

      public string StyleType
      {
         get { return styleType; }
         set { styleType = value; }
      }

      public int NewsItemCount
      {
         get { return newsItems.Count; }
      }

      /// <summary>
      /// Indexer of this class
      /// </summary>
      public RSSItem this [int index]
      {
         get 
         {
            if(index < newsItems.Count)
               return (RSSItem)newsItems[index]; 
            else
               return null;
         }
      }

      public RSSItem addItem()
      {
         RSSItem newItem = new RSSItem();
         newsItems.Insert(0, newItem );

         return newItem;
      }

      public void deleteItem(RSSItem rssItem)
      {
         newsItems.Remove(rssItem);
      }

      public void swapItems(int index1, int index2)
      {
         int count = newsItems.Count;
         if(index1 >= 0 && index2 >= 0 && index1 < count && index2 < count)
         {
            RSSItem tempRSSItem = newsItems[index1];

            newsItems[index1] = newsItems[index2];
            newsItems[index2] = tempRSSItem;
         }
      }

      public void saveFeed(string fileName)
      {
         saveFeed(fileName, 0);
      }

      public void saveFeed(string fileName, int maxItemCount)
      {
         XmlTextWriter w = new XmlTextWriter(fileName, null);
         w.Formatting = Formatting.Indented;
     
         string encodingHeader = Encoding.UTF8.HeaderName;
         //string encodingHeader = Encoding.GetEncoding("ISO-8859-1").HeaderName;
         //w.WriteStartDocument();
         w.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"" + encodingHeader + "\"");
         //w.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
//         w.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"ISO-8859-1\"");

         if(StyleSheet != "")
         {
            string xsl = "type=\"" + StyleType + "\" href=\"" + StyleSheet + "\" media=\"screen\"";
            w.WriteProcessingInstruction("xml-stylesheet", xsl );
         }

         //<?xml-stylesheet type='text/xsl' href='rss.xsl' media='screen' ?>

         w.WriteStartElement("rss");
         w.WriteAttributeString("version", "2.0");
         w.WriteAttributeString("xmlns", "atom", null, "http://www.w3.org/2005/Atom"); // <rss xmlns:atom="http://www.w3.org/2005/Atom">

         if(FtpSite != "") w.WriteComment("FTPSite " + FtpSite);
        // w.WriteComment("Created by RSS Builder (http://home.hetnet.nl/~bsoft/rssbuilder)");

         w.WriteStartElement("channel");
         w.WriteElementString("generator", "RSS Builder by B!Soft");
         w.WriteElementString("title", title);
         w.WriteElementString("link", webURL);

         if (!string.IsNullOrEmpty(feedURL) && feedURL.StartsWith("http"))
         {
             // <atom:link rel="self" href="http://example.tld/news.rss" type="application/rss+xml" />
             w.WriteStartElement("atom", "link", null);
             w.WriteAttributeString("rel", "self");
             w.WriteAttributeString("href", feedURL);
             w.WriteAttributeString("type", "application/rss+xml");
             w.WriteEndElement();
         }

         if (!string.IsNullOrEmpty(hubURL) && hubURL.StartsWith("http"))
         {
             // <atom:link rel="hub" href="http://pubsubhubbub.appspot.com/" />
             w.WriteStartElement("atom", "link", null);
             w.WriteAttributeString("rel", "hub");
             w.WriteAttributeString("href", hubURL);
             w.WriteEndElement();
         }

         //Encoding isoEncoding = Encoding.GetEncoding("ISO-8859-1");
         //byte[] utfDescription = Encoding.UTF8.GetBytes(description);
         //byte[] encodedDesc = Encoding.Convert(Encoding.UTF8, isoEncoding, utfDescription);
         //char[] encodedChars = isoEncoding.GetChars(encodedDesc);
         //string encodedDescription = new string(encodedChars);
         //w.WriteElementString("description", encodedDescription );
         w.WriteElementString("description", description );
         w.WriteElementString("language", language);
         if(editor!="") w.WriteElementString("managingEditor", editor);
         if(webmaster!="") w.WriteElementString("webMaster", webmaster);
         if(copyright!="") w.WriteElementString("copyright", copyright);

         if(imgURL != "")
         {
            w.WriteStartElement("image");
            w.WriteElementString("title", title);
            w.WriteElementString("link", webURL);
            w.WriteElementString("url", imgURL);
            if(imgWidth != "")
            {
               w.WriteElementString("width", imgWidth);
               w.WriteElementString("height", imgHeight);
            }
            w.WriteEndElement();
         }

         writeItems(w, maxItemCount);

         w.WriteEndElement(); // channel
         w.WriteEndElement(); // rss

         //w.WriteEndDocument();
         w.Close();
      }

      private void writeItems(XmlTextWriter w, int maxItemCount)
      {
         int writeItemCount = newsItems.Count;

         if(maxItemCount!= 0  && maxItemCount<writeItemCount)
            writeItemCount = maxItemCount;

         for(int i=0; i<writeItemCount; i++)
         {
            RSSItem rssItem = (RSSItem)newsItems[i];

            w.WriteStartElement("item");
            w.WriteElementString("title", rssItem.Title);

            string timeZone = " GMT";
            if(rssItem.TimeOffset != 0)
            {
               if(rssItem.TimeOffset > 0)
                  timeZone = " +"; // make positive numbers explicit
               else
                  timeZone = " "; // negative numbers are already explicit

               timeZone += String.Format("{0:D2}00", rssItem.TimeOffset);
            }

            w.WriteElementString("pubDate", rssItem.PubDate + timeZone);

            if(rssItem.Link != "")
               w.WriteElementString("link", rssItem.Link);

            if(rssItem.GUID != "")
            {
               w.WriteStartElement("guid");
               string isPermaLink = rssItem.IsPermaLink ? "true" : "false";
               w.WriteAttributeString("isPermaLink", isPermaLink);
               w.WriteString(rssItem.GUID);
               w.WriteEndElement();
            }
               
            if(rssItem.Author != "")
               w.WriteElementString("author", rssItem.Author);

            if(rssItem.Comments != "")
               w.WriteElementString("comments",rssItem.Comments);

            if(rssItem.Category != "")
               w.WriteElementString("category", rssItem.Category);

            if (rssItem.EnclosureUrl != "")
            {
               w.WriteStartElement("enclosure");
                  w.WriteAttributeString("url", rssItem.EnclosureUrl);
                  w.WriteAttributeString("length", rssItem.EnclosureLength);
                  w.WriteAttributeString("type", rssItem.EnclosureType);
               w.WriteEndElement(); // enclosure
            }

            w.WriteStartElement("description");
               w.WriteCData(rssItem.Description);
            w.WriteEndElement(); // description

            w.WriteEndElement(); // item
         }
      }


      public void saveFeedAsHTML(string fileName)
      {
         saveFeedAsHTML(fileName, this.NewsItemCount, true, true, true);
      }

      public void saveFeedAsHTML(string fileName, int maxItemCount)
      {
         saveFeedAsHTML(fileName, maxItemCount, true, true, true);
      }

      public void saveFeedAsHTML(string fileName, int maxItemCount, bool saveTitle, bool saveImage, bool saveDates)
      {
         using(StreamWriter sw = new StreamWriter(fileName))
         {
            sw.WriteLine("<HTML><HEAD>");
            sw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\">");
            sw.WriteLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"rss_style.css\" />");
            sw.WriteLine("</HEAD>");

            sw.WriteLine("<BODY>");

            if(saveTitle || saveImage)
               sw.WriteLine("<div class=\"ChannelTitle\">");

            if(saveTitle)
               sw.WriteLine(title);


            if(saveImage && imgURL != "")
            {
               sw.Write("<BR/><IMG SRC='");
               sw.Write(imgURL);
               sw.WriteLine("'</IMG>");
            }

            if(saveTitle || saveImage)
               sw.WriteLine("</div><br/>");

            writeItemsAsHTML(sw, maxItemCount, saveDates);

            sw.WriteLine("</BODY></HTML>");
         }
      }

      private void writeItemsAsHTML(StreamWriter sw, int maxItemCount, bool saveDates)
      {
         int writeItemCount = newsItems.Count;

         if(maxItemCount!= 0  && maxItemCount<writeItemCount)
            writeItemCount = maxItemCount;

         for(int i=0; i<writeItemCount; i++)
         {
            RSSItem rssItem = (RSSItem)newsItems[i];

            sw.WriteLine("<div class=\"ArticleEntry\">");

            sw.WriteLine("<div class=\"ArticleTitle\">");
            sw.Write("<a href=");   
            sw.Write(rssItem.Link);
            sw.Write(">");
            sw.Write(rssItem.Title);
            sw.WriteLine("</a>");
            sw.WriteLine("</div>");

            if(saveDates)
            {
               sw.WriteLine("<div class=\"ArticleDate\">");
               sw.WriteLine(rssItem.PubDate);
               sw.WriteLine("</div>");
            }

            sw.WriteLine("<div class=\"ArticleDescription\">");
            sw.Write(rssItem.Description);
            sw.WriteLine("</div>");
            
            sw.WriteLine("</div><br/>");
            
         }
      }

      private string getSubElem(XmlNode node, string element)
      {
         if(node == null)
            return "";

         XmlNode elemNode = node[element];
         if(elemNode == null)
            return "";

         return elemNode.InnerText;
      }


       /// <remarks><atom:link rel="hub" href="http://pubsubhubbub.appspot.com/" /></remarks>
      private string getAtomElem(XmlNode node, string relation)
      {
          if (node == null)
              return "";

          XmlNamespaceManager nsmgr = new XmlNamespaceManager(node.OwnerDocument.NameTable);
          nsmgr.AddNamespace("at", "http://www.w3.org/2005/Atom");
          XmlNodeList atomLinks = node.SelectNodes("at:link", nsmgr);

          foreach (XmlNode atomLink in atomLinks)
          {
              var rel = atomLink.Attributes["rel"];
              if (rel != null && rel.Value == relation)
                  return atomLink.Attributes["href"].Value;
          }

          return "";
      }

      public bool openFeed(string fileName)
      {
         XmlDocument xmlDoc = new XmlDocument();
         xmlDoc.Load(fileName);
         int topLength = Math.Min(xmlDoc.OuterXml.Length, 400);
         string xmlTop = xmlDoc.OuterXml.Substring(0, topLength);



         // Extract Stylesheet information
         // <?xml-stylesheet type="text/xsl" href="rss.xsl" media="screen"?>
         //
         int startIdx = xmlTop.IndexOf("xml-stylesheet");
         if(startIdx > 0)
         {
            int startIdxType = xmlTop.IndexOf("type=", startIdx, 40) + 6;
            int endIdxType = xmlTop.IndexOfAny("\"'".ToCharArray(), startIdxType, 40)-1;

            StyleType =  xmlTop.Substring(startIdxType, endIdxType - startIdxType+1);

            int startIdxHref = xmlTop.IndexOf("href=", startIdx, 100) + 6;
            int endIdxHref   = xmlTop.IndexOfAny("\"'".ToCharArray(), startIdxHref, 300) -1;

            StyleSheet = xmlTop.Substring(startIdxHref, endIdxHref - startIdxHref+1);
         }

         // Extract FTP Site information (stored in a comment)
         // <!--FTPSite xxxx-->
         //
         startIdx = xmlTop.IndexOf("<!--FTPSite");
         if(startIdx > 0)
         {
            int startIdxSite = startIdx + 12;
            int endIdxSite = xmlTop.IndexOf("-->", startIdxSite, 100)-1;

            FtpSite =  xmlTop.Substring(startIdxSite, endIdxSite - startIdxSite+1);
         }


         XmlElement root = xmlDoc.DocumentElement;

         XmlNode channel = root["channel"];
         if(channel == null)
            return false;

         title = getSubElem(channel, "title");
         webURL = getSubElem(channel, "link");
         feedURL = getAtomElem(channel, "self");
         hubURL = getAtomElem(channel, "hub");

         description = getSubElem(channel,"description");
         language = getSubElem(channel,"language");
         editor = getSubElem(channel,"managingEditor");
         webmaster = getSubElem(channel,"webMaster");
         copyright = getSubElem(channel,"copyright");

         XmlNode image = channel["image"];
         if(image != null)
         {
            imgURL = getSubElem(image, "url");
            imgWidth = getSubElem(image, "width");
            imgHeight = getSubElem(image, "height");
         }

         newsItems.Clear();
         XmlNodeList itemNodes = channel.SelectNodes("item");
         foreach(XmlNode item in itemNodes) 
         {
            RSSItem rssItem = new RSSItem();
            newsItems.Add(rssItem );

            rssItem.Title = getSubElem(item, "title");

            string dateWithTZ = getSubElem(item, "pubDate");
            rssItem.PubDate = dateWithoutTZ(dateWithTZ);

            string TZ = getTZFromDate(dateWithTZ);
            if(TZ == "GMT")
               rssItem.TimeOffset = 0;
            else
            {
               //---
               // Try to get time zone offset from the date
               // -XXYY: YY is stripped and XX is converted to an integer
               //---
               try
               {
                  rssItem.TimeOffset = Int16.Parse(TZ.TrimEnd('0'));
               }
               catch
               {
                  rssItem.TimeOffset = 0;
               }
            }

            rssItem.Description = getSubElem(item,"description");
            rssItem.Comments = getSubElem(item, "comments");
            rssItem.Category = getSubElem(item, "category");
            rssItem.Author = getSubElem(item, "author");
            rssItem.Link = getSubElem(item,"link");

            XmlNode enclosureNode = item["enclosure"];
            if (enclosureNode != null)
            {
               if (enclosureNode.Attributes.Count == 3)
               {
                  rssItem.EnclosureUrl = enclosureNode.Attributes["url"].Value;
                  rssItem.EnclosureLength = enclosureNode.Attributes["length"].Value;
                  rssItem.EnclosureType = enclosureNode.Attributes["type"].Value;
               }
            }

            XmlNode GuidNode = item["guid"];
            if (GuidNode != null)
            {
               if (GuidNode.Attributes.Count == 1)
                  rssItem.IsPermaLink = (GuidNode.Attributes["isPermaLink"].Value == "true");

               rssItem.GUID = GuidNode.InnerText;
            }

         }

         return true;
      }


      private string getTZFromDate(string date)
      {
         string[] tokens = date.Split(' ');
         if(tokens.Length != 6)
         {
            return "";
         }
         else
         {
            return tokens[5];
         }
      }

      private string dateWithoutTZ(string date)
      {
         string[] tokens = date.Split(' ');
         if(tokens.Length != 6)
         {
            return date;
         }
         else
         {
            return tokens[0]+" "+tokens[1]+" "+tokens[2]+" "+tokens[3]+" "+tokens[4];
         }
      }


   } // class
} // namespace
