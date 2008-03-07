using System;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;

namespace RSSBuilder
{
	/// <summary>
	/// Summary description for RSS2HTML.
	/// </summary>
	public class RSS2HTML
	{
      private RSSFeed rssFeed = null;

		public RSS2HTML(RSSFeed rssF)
		{
         rssFeed = rssF;
		}

      public void translate(string xsltFile, string htmlFile)
      {
         //---
         // Save the rss feed to a temp file
         //---
         string tempFeedFileName = Path.GetTempFileName();
         rssFeed.saveFeed(tempFeedFileName);

         //---
         // Open the rss feeed in an XML document
         //---
         XmlDocument xmlDoc = new XmlDocument();
         xmlDoc.Load(tempFeedFileName);

         //---
         // Open a XSLT file
         //---
        // XslTransform xslTransform = new XslTransform();
         XslCompiledTransform xslTransform = new XslCompiledTransform();
         xslTransform.Load(xsltFile);

         //---
         // Transform the feed into an HTML document
         //---
         XPathNavigator nav = xmlDoc.DocumentElement.CreateNavigator();
         XmlTextWriter writer = new XmlTextWriter(htmlFile, null);
         
         //xslTransform.Transform(nav, null, writer, null);
         xslTransform.Transform(nav, writer);
         writer.Close();

         //---
         // Delete the temp. feed
         //---
         try
         {
            File.Delete(tempFeedFileName);
         }
         catch
         {
         }

      }
	}
}
