using System;
using System.Collections.Generic;
using System.Text;

namespace RSSBuilder
{
   class FTPSessionParameters
   {
      private string _server;

      public string Server
      {
         get { return _server; }
         set { _server = value; }
      }

      private string _portStr;

      public string PortStr
      {
         get { return _portStr; }
         set { _portStr = value; }
      }

      private bool _passive;

      public bool Passive
      {
         get { return _passive; }
         set { _passive = value; }
      }

      private string _user;

      public string User
      {
         get { return _user; }
         set { _user = value; }
      }

      private string _password;

      public string Password
      {
         get { return _password; }
         set { _password = value; }
      }

      private string _remotePath;

      public string RemotePath
      {
         get { return _remotePath; }
         set { _remotePath = value; }
      }

      private string _localFileName;

      public string LocalFileName
      {
         get { return _localFileName; }
         set { _localFileName = value; }
      }

      private string _remoteFileName;

      public string RemoteFileName
      {
         get { return _remoteFileName; }
         set { _remoteFileName = value; }
      }
	
	
   }
}
