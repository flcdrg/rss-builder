// edtFTPnet
// 
// Copyright (C) 2004 Enterprise Distributed Technologies Ltd
// 
// www.enterprisedt.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// Bug fixes, suggestions and comments should posted on 
// http://www.enterprisedt.com/forums/index.php
// 
// Change Log:
// 
// $Log: FileAppender.cs,v $
// Revision 1.7  2006/03/16 22:13:37  hans
// Added a FileName property and support for InnerExceptions.
//
// Revision 1.6  2006/02/16 20:16:53  hans
// Changed logging of exceptions to include inner exceptions
//
// Revision 1.5  2005/07/08 09:29:48  bruceb
// add flushing to Log methods
//
// Revision 1.4  2004/11/06 11:15:24  bruceb
// namespace tidying up
//
// Revision 1.3  2004/10/29 09:42:30  bruceb
// removed /// from file headers
//
//
//

using System;
using System.IO;

namespace EnterpriseDT.Util.Debug
{    
	/// <summary>  
	/// Appends log statements to a file
	/// </summary>
	/// <author>       Bruce Blackshaw
	/// </author>
	/// <version>      $Revision: 1.7 $
	/// </version>
	public class FileAppender : Appender
	{
		
		/// <summary> Destination</summary>
		private StreamWriter log;

		/// <summary> Log file</summary>
		private string fileName;
		
		/// <summary>Constructor</summary>
		/// <param name="fileName">name of file to log to</param>
		/// <throws>  IOException </throws>
		public FileAppender(string fileName)
		{
			this.fileName = fileName;
			log = new StreamWriter(fileName, true);
		}

		/// <summary>
		/// Name of file to log to.
		/// </summary>
		public string FileName
		{
			get
			{
				return fileName;
			}
		}
		
		/// <summary> 
		/// Log a message
		/// </summary>
		/// <param name="msg"> message to log
		/// </param>
		public virtual void Log(string msg)
		{
			log.WriteLine(msg);
            log.Flush();
		}
		
		/// <summary> 
		/// Log a stack trace
		/// </summary>
		/// <param name="t"> throwable object
		/// </param>
		public virtual void Log(Exception t)
		{
			log.WriteLine(t.GetType().FullName + ": " + t.Message);
			log.WriteLine(t.StackTrace.ToString());
			log.Flush();
		}
		
		/// <summary> 
		/// Close this appender
		/// </summary>
		public virtual void Close()
		{
			log.Flush();
			log.Close();
		}
	}
}
