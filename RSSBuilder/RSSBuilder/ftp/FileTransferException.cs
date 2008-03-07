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
// $Log: FileTransferException.cs,v $
// Revision 1.1  2006/06/19 13:00:26  bruceb
// restructured exception hierarchy
//
// Revision 1.1  2006/06/14 10:07:38  bruceb
// moved out of FTPClient
//
//

using System;

namespace EnterpriseDT.Net.Ftp
{
    
	/// <summary>  
	/// Exceptions specific to file transfer protocols
	/// </summary>
	/// <author>      Bruce Blackshaw
	/// </author>
	/// <version>     $Revision: 1.1 $
	/// 
	/// </version>
	public class FileTransferException : ApplicationException
	{
		/// <summary>   
		/// Constructor. Delegates to super.
		/// </summary>
		/// <param name="msg">  Message that the user will be
		/// able to retrieve
		/// </param>
		public FileTransferException(string msg)
            : base(msg)
		{
		}

	}
}