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
// $Log: FTPFileFactory.cs,v $
// Revision 1.10  2006/06/14 10:08:21  bruceb
// VMS fixes
//
// Revision 1.9  2005/08/04 21:57:35  bruceb
// throw change
//
// Revision 1.8  2005/06/10 15:47:51  bruceb
// more vms tweaks
//
// Revision 1.7  2005/06/03 11:32:47  bruceb
// vms changes
//
// Revision 1.6  2005/02/07 17:19:34  bruceb
// support for setting the CultureInfo
//
// Revision 1.5  2004/12/22 22:41:50  bruceb
// made methods public
//
// Revision 1.4  2004/11/05 20:00:28  bruceb
// cleaned up namespaces
//
// Revision 1.3  2004/10/29 09:41:44  bruceb
// removed /// in file header
//
//
//

using System;
using System.Globalization;
using System.Text;

using Logger = EnterpriseDT.Util.Debug.Logger;
	
namespace EnterpriseDT.Net.Ftp
{
	/// <summary>  
	/// Factory for creating FTPFile objects
	/// </summary>
	/// <author>       Bruce Blackshaw
	/// </author>
	/// <version>      $Revision: 1.10 $
	/// </version>
	public class FTPFileFactory
	{
        /// <summary>
		/// Get/set the culture info for parsing
		/// </summary>
		public CultureInfo ParsingCulture
		{
			get
			{
				return parserCulture;
			}
			set
			{
				this.parserCulture = value;
                windows.ParsingCulture = value;
                unix.ParsingCulture = value;
                vms.ParsingCulture = value;
                parser.ParsingCulture = value;
			}			
		}        
        
		/// <summary> Logging object</summary>
		private Logger log;
		
		/// <summary> Windows server comparison string</summary>
		internal const string WINDOWS_STR = "WINDOWS";
		
		/// <summary> UNIX server comparison string</summary>
		internal const string UNIX_STR = "UNIX";
        
		/// <summary> VMS server comparison string</summary>
		internal const string VMS_STR = "VMS";
		
		/// <summary> SYST string</summary>
		private string system;
		
		/// <summary> Cached windows parser</summary>
		private FTPFileParser windows = new WindowsFileParser();
		
		/// <summary> Cached unix parser</summary>
		private FTPFileParser unix = new UnixFileParser();

		/// <summary> Cached unix parser</summary>
		private FTPFileParser vms = new VMSFileParser();
		
		/// <summary> Does the parsing work</summary>
		private FTPFileParser parser = null;
		
		/// <summary> Rotate parsers when a ParseException is thrown?</summary>
		private bool rotateParsersOnFail = true;
    
        /// <summary>True if using VMS parser</summary>
        private bool usingVMS = false;
        
        /// <summary>Culture used for parsing file details</summary>
        private CultureInfo parserCulture = CultureInfo.InvariantCulture;        
        
		/// <summary> Constructor
		/// 
		/// </summary>
		/// <param name="system">   SYST string
		/// </param>
		public FTPFileFactory(string system)
		{
			log = Logger.GetLogger(typeof(FTPFileFactory));
			SetParser(system);
		}
		
		/// <summary> Constructor. User supplied parser. Note that parser
		/// rotation (in case of a ParseException) is disabled if
		/// a parser is explicitly supplied
		/// 
		/// </summary>
		/// <param name="parser">  the parser to use
		/// </param>
		public FTPFileFactory(FTPFileParser parser)
		{
			log = Logger.GetLogger(typeof(FTPFileFactory));
			this.parser = parser;
			rotateParsersOnFail = false;
		}
		
		
		/// <summary> Set the remote server type
		/// 
		/// </summary>
		/// <param name="system">   SYST string
		/// </param>
		public void SetParser(string system)
		{
			this.system = system;
			if (system.ToUpper().StartsWith(WINDOWS_STR))
				parser = windows;
			else if (system.ToUpper().StartsWith(UNIX_STR))
				parser = unix;
			else if (system.ToUpper().StartsWith(VMS_STR)) 
            {
				parser = vms;
                usingVMS = true;
            }
			else 
            {
                parser = unix;
			    log.Warn("Unknown SYST '" + system + "' - defaulting to Unix parsing");
            }
		}
		
		
		/// <summary> Parse an array of raw file information returned from the
		/// FTP server
		/// 
		/// </summary>
		/// <param name="files">    array of strings
		/// </param>
		/// <returns> array of FTPFile objects
		/// </returns>
		public virtual FTPFile[] Parse(string[] files)
		{			
			FTPFile[] temp = new FTPFile[files.Length];
			
			// quick check if no files returned
			if (files.Length == 0)
				return temp;
			
			int count = 0;
            bool checkedUnix = false;
            bool reparse = false;
            int reparseCount = 1;
			for (int i = 0; i < files.Length; i++)
			{
                if (reparse) 
                { // rotated parsers, try this line again
                    i -= reparseCount;
                    reparse = false;
                    reparseCount = 1;
                }

                if (files[i] == null || files[i].Trim().Length == 0)
                    continue;
                
                // swap to Unix if looks like Unix listing - the default
                if (!checkedUnix && parser != unix && UnixFileParser.IsUnix(files[i])) {
                    parser = unix;
                    checkedUnix = true;
                    log.Info("Swapping Windows parser to Unix");
                }                
                
                try
				{               
                    FTPFile file = null;
                    if (usingVMS) 
                    {
                        // vms uses more than 1 line for some file listings. We must keep going
                        // thru till we've got everything
                        reparseCount = 1;
                        StringBuilder filename = new StringBuilder(files[i]);
                        while (i+1 < files.Length && files[i+1].IndexOf(';') < 0) 
                        {
                            filename.Append(" ").Append(files[i+1]);
                            i++;
                            reparseCount++;
                        }
                        file = parser.Parse(filename.ToString());
                    }
                    else 
                    {
                        file = parser.Parse(files[i]);
                    }                
				
					// we skip null returns - these are duff lines we know about and don't
					// really want to throw an exception
					if (file != null)
					{
						temp[count++] = file;
					}
				}
				catch (FormatException ex)
				{
                    log.Debug(ex.Message);
					if (rotateParsersOnFail && count == 0)
					{
						// first error, let's try swapping parsers
						RotateParsers();
						FTPFile file = parser.Parse(files[i]);
						if (file != null)
							temp[count++] = file;
					}
					// rethrow
					else
						throw;
				}
			}
			FTPFile[] result = new FTPFile[count];
			Array.Copy(temp, 0, result, 0, count);
			return result;
		}
		
		/// <summary> Swap from one parser to the other. We can just check
		/// object references
		/// </summary>
		private void RotateParsers()
		{
			if (parser == unix)
			{
				parser = windows;
				log.Info("Rotated parser to Windows");
			}
			else if (parser == windows)
			{
				parser = unix;
				log.Info("Rotated parser to Unix");
			}
		}
		
		/// <summary> 
		/// Get the SYST string
		/// </summary>
		/// <returns> the system string.
		/// </returns>
		public virtual string GetSystem()
		{
			return system;
		}
	}
}
