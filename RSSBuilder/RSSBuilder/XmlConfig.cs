using System;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Specialized;

namespace BSoft.Xml
{
	/// <summary>
	/// Summary description for XmlConfig.
	/// </summary>
	public class XmlConfig
	{
      private string fileName;
      private XmlDocument xmlDoc = new XmlDocument();

		public XmlConfig(string filename) 
		{
         if(File.Exists(filename))
         {
            xmlDoc.Load(filename);
         }
         else
         {
            xmlDoc.LoadXml("<configuration></configuration>");
            xmlDoc.Save(filename);
         }

         fileName = filename;
		}

      public void Save()
      {
         xmlDoc.Save(fileName);
      }

      public StringCollection GetSectionNames()
      {
         StringCollection sectionNames = new StringCollection();

         XmlNodeList sections = xmlDoc.SelectNodes("//section");
         foreach(XmlNode section in sections)
         {
            sectionNames.Add(section.Attributes["id"].Value);
         }

         return sectionNames;
      }
      private XmlNode findSection(string sectionName)
      {
         if(sectionName != null && sectionName != "")
         {
            XmlNode root = xmlDoc.DocumentElement;
            return root.SelectSingleNode("//section[@id='"+sectionName+"']");
         }
         else
            return null;   
      }

      public bool HasSection(string sectionName)
      {
         return findSection(sectionName) != null;
      }

      public void RenameSection(string oldName, string newName)
      {
         XmlNode section = findSection(oldName);
         if(section != null)
         {
            section.Attributes["id"].Value = newName;
         }
      }

      public void DeleteSection(string sectionName)
      {
         XmlNode section = findSection(sectionName);
         if(section != null)
         {
            section.ParentNode.RemoveChild(section);
         }
      }

      private XmlNode createSection(string sectionName)
      {
         XmlNode root = xmlDoc.DocumentElement;

         XmlElement sectionNode = xmlDoc.CreateElement("section");
         XmlAttribute idAttr = xmlDoc.CreateAttribute("id");
         idAttr.Value = sectionName;
         sectionNode.Attributes.Append(idAttr);

         root.AppendChild(sectionNode);

         return sectionNode;
      }

      private XmlNode createKey(XmlNode section, string keyName, string keyValue)
      {
         XmlElement keyNode = xmlDoc.CreateElement("key");
         XmlAttribute nameAttr = xmlDoc.CreateAttribute("name");
         nameAttr.Value = keyName;
         XmlAttribute valueAttr = xmlDoc.CreateAttribute("value");
         valueAttr.Value = keyValue;

         keyNode.Attributes.Append(nameAttr);
         keyNode.Attributes.Append(valueAttr);

         section.AppendChild(keyNode);

         return keyNode;
      }

      private XmlNode findKey(XmlNode section, string keyName)
      {
         if(section != null && keyName != null && keyName != "")
            return section.SelectSingleNode("key[@name='"+keyName+"']");
         else
            return null;   
      }

      public string GetValue(string sectionName, string keyName, string defaultValue)
      {
         string keyValue = defaultValue;

         if(sectionName != null && keyName != null && sectionName != "" && keyName != "")
         {
            XmlNode section = findSection(sectionName);

            XmlNode key = findKey( section, keyName );
            if(key != null)
            {
               keyValue = key.Attributes["value"].Value;
            }
         }

         return keyValue;
      }

      public string GetValue(string sectionName, string keyName)
      {
         return GetValue(sectionName, keyName, "");
      }

      public void SetValue(string sectionName, string keyName, string keyValue)
      {
         if(sectionName==null || keyName==null || keyValue==null ||
            sectionName=="" || keyName=="")
            return;

         XmlNode section = findSection(sectionName);
         if(section == null) 
            section = createSection(sectionName);

         XmlNode key = findKey(section, keyName);
         if(key == null)
            key = createKey(section, keyName, keyValue);
         else
            key.Attributes["value"].Value = keyValue;
      }
	}
}
