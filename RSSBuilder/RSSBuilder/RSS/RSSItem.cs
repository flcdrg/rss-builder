using System;

namespace RSSBuilder
{
	/// <summary>
	/// A news topic in an RSS feed.
	/// </summary>
	public class RSSItem
	{
      private string title = "New Topic";
      private string link = "http://";
      private string guid = "";
      private bool isPermaLink = true;
      private string comments = "";
      private string author = "";
      private string description = "";
      private string pubDate = "";
      private string category = "";
      private string enclosureUrl = "";
      private string enclosureLength = "";
      private string enclosureType = "";

      private int timeOffset; 

      public RSSItem()
		{
		}

      public string Title
      {
         get { return title; }
         set { title = value; }
      }

      public string Link
      {
         get { return link; }
         set { link = value; }
      }

      public string GUID
      {
          get { return guid; }
          set { guid = value; }
      }

      public bool IsPermaLink
      {
         get { return isPermaLink; }
         set { isPermaLink = value; }
      }

      public string Category
      {
         get { return category; }
         set { category = value; }
      }

      public string Description
      {
         get { return description; }
         set { description = value; }
      }

      public string PubDate
      {
         get { return pubDate; }
         set { pubDate = value; }
      }

      public int TimeOffset
      {
         get { return timeOffset; }
         set { timeOffset = value; }
      }

      public string Author
      {
         get { return author; }
         set { author = value; }
      }

      public string Comments
      {
         get { return comments; }
         set { comments = value; }
      }

      public string EnclosureUrl
      {
         get { return enclosureUrl; }
         set { enclosureUrl = value; }
      }

      public string EnclosureLength
      {
         get { return enclosureLength; }
         set { enclosureLength = value; }
      }

      public string EnclosureType
      {
         get { return enclosureType; }
         set { enclosureType = value; }
      }
   }
}
