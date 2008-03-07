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
// $Log: VMSFileParser.cs,v $
// Revision 1.2  2005/06/03 21:23:22  bruceb
// comment tweak
//
// Revision 1.1  2005/06/03 11:32:47  bruceb
// vms changes
//
//

using System;
using System.Globalization;
using System.Text;
using Logger = EnterpriseDT.Util.Debug.Logger;

namespace EnterpriseDT.Net.Ftp
{    
    /// <summary>  
    /// Represents a remote OpenVMS file parser
    /// </summary>
    /// <author>Bruce Blackshaw
    /// </author>
    /// <version>$Revision: 1.2 $</version>
    /// <remarks>Hacked and modified from some helpful source provided by Jason Schultz</remarks>
    public class VMSFileParser:FTPFileParser
    {            
        /// <summary>Directory field</summary>
        private static readonly string DIR = ".DIR";
    
        /// <summary>Directory field</summary>
        private static readonly string HDR = "Directory";
    
         /// <summary>Total field</summary>
        private static readonly string TOTAL = "Total";
    
         /// <summary> Number of expected fields</summary>
        private static readonly int BLOCKSIZE = 512*1024;
        
        /// <summary> Number of expected fields</summary>
        private static readonly int MIN_EXPECTED_FIELD_COUNT = 4;
        
        /// <summary> Parse server supplied string</summary>
        /// <param name="raw">raw string to parse</param>
        /// <returns>FTPFile object representing the raw string</returns>
        /// <remarks>Listing look like the below:
        ///  OUTPUT: 
        ///    
        ///    Directory dirname
        ///     
        ///    filename
        ///            used/allocated    dd-MMM-yyyy HH:mm:ss [group,owner]        (PERMS)
        ///    filename
        ///            used/allocated    dd-MMM-yyyy HH:mm:ss [group,owner]        (PERMS)
        ///    ...
        ///    
        ///    Total of n files, n/m blocks
        /// </remarks>
        public override FTPFile Parse(string raw) 
        {
            string[] fields = Split(raw);      
            
            // skip blank lines
            if(fields.Length <= 0)
                return null;
            // skip line which lists Directory
            if (fields.Length >= 2 && fields[0].Equals(HDR))
                return null;
            // skip line which lists Total
            if (fields.Length > 0 && fields[0].Equals(TOTAL))
                return null;
            if (fields.Length < MIN_EXPECTED_FIELD_COUNT)
                return null;
            
            // first field is name
            string name = fields[0];
            
            // make sure it is the name (ends with ';<INT>')
            int semiPos = name.LastIndexOf(';');
            // check for ;
            if(semiPos <= 0) 
            {
                throw new FormatException("File version number not found in name '" + name + "'");
            }
            name = name.Substring(0, semiPos);
            
            // check for version after ;
            string afterSemi = fields[0].Substring(semiPos + 1);
            try
            {
                Int64.Parse(afterSemi);
                // didn't throw exception yet, must be number
                // we don't use it currently but we might in future
            }
            catch(FormatException) 
            {
                // don't worry about version number
            }

            // test is dir
            bool isDir = false;
            if (semiPos < 0) 
            {
                semiPos = fields[0].Length;
            }
            if( semiPos <= 4) 
            {
                // string to small to have a .DIR
            }
            else 
            {
                // look for .DIR
                string tstExtnsn = fields[0].Substring(semiPos - DIR.Length, DIR.Length);
                if(tstExtnsn.Equals(DIR))
                    isDir = true;
            }
            
            // 2nd field is size USED/ALLOCATED format, or perhaps just USED
            int slashPos = fields[1].IndexOf('/');
            string sizeUsed = fields[1];
            if (slashPos > 0)
                sizeUsed = fields[1].Substring(0, slashPos);
            long size = Int64.Parse(sizeUsed) * BLOCKSIZE;
            
            // 3 & 4 fields are date time
            string lastModifiedStr = TweakDateString(fields);
            DateTime lastModified = DateTime.Parse(lastModifiedStr.ToString(),ParsingCulture.DateTimeFormat);
            
            // 5th field is [group,owner]
            string group = null;
            string owner = null;
            if (fields.Length >= 5)
            {
                if (fields[4][0] == '[' && fields[4][fields[4].Length-1] == ']') 
                {
                    int commaPos = fields[4].IndexOf(',');
                    if (commaPos < 0)
                    {
                        throw new FormatException("Unable to parse [group,owner] field '" + fields[4] + "'");
                    }
                    group = fields[4].Substring(1, commaPos-1);
                    owner = fields[4].Substring(commaPos+1, fields[4].Length-commaPos-2);
                }
            }
            
            // 6th field is permissions e.g. (RWED,RWED,RE,)
            string permissions = null;
            if (fields.Length >= 6)
            {
                if (fields[5][0] == '(' && fields[5][fields[5].Length-1] == ')') 
                {
                    permissions = fields[5].Substring(1, fields[5].Length-2);
                }
            }
            
            FTPFile file = new FTPFile(FTPFile.VMS, raw, name, size, isDir, ref lastModified); 
            file.Group = group;
            file.Owner = owner;
            file.Permissions = permissions;
            return file;
        }
        
        /// <summary> Tweak the date string to make the month camel case</summary>
        /// <param name="fields">array of fields</param>
        private string TweakDateString(string[] fields) 
        {
            // convert the last 2 chars of month to lower case
            StringBuilder lastModifiedStr = new StringBuilder();
            bool monthFound = false;
            for (int i = 0; i < fields[2].Length; i++)
            {
                if (!Char.IsLetter(fields[2][i])) 
                {
                    lastModifiedStr.Append(fields[2][i]);
                }
                else
                {
                    if (!monthFound)
                    {
                        lastModifiedStr.Append(fields[2][i]);
                        monthFound = true;
                    }
                    else
                    {
                        lastModifiedStr.Append(Char.ToLower(fields[2][i]));
                    }
                }
            }  
            lastModifiedStr.Append(" ").Append(fields[3]);
            return lastModifiedStr.ToString();
        }
    }
}
