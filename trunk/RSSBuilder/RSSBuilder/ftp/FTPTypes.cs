// 
// Copyright (C) 2006 Enterprise Distributed Technologies Ltd
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
// $Log: FTPTypes.cs,v $
// Revision 1.1  2006/06/19 13:00:12  bruceb
// extracted types into another file
//
// Revision 1.2  2006/06/16 12:14:48  bruceb
// added FTPMessageEventArgs etc
//
// Revision 1.1  2006/06/14 10:07:38  bruceb
// moved out of FTPClient
//
//

using System;
using System.IO;

namespace EnterpriseDT.Net.Ftp
{
    #region Types

    /// <summary>
    /// Event args for ReplyReceived and CommandSent events
    /// </summary>    
    public class FTPMessageEventArgs : EventArgs 
    {
        /// <summary>
        /// Constructor
        /// <param name="message"> 
        /// The message sent to or from the remote host
        /// </param>
        /// </summary>        
        public FTPMessageEventArgs(string message) 
        {
            this.message = message;
        }
        
        /// <summary>
        /// Gets the message 
        /// </summary>   
        public string Message
        {
            get 
            {
                return message;
            }
        }
        
        private string message;
    }
    
    /// <summary>
    /// Event args for BytesTransferred event
    /// </summary>    
    public class BytesTransferredEventArgs : EventArgs 
    {
        /// <summary>
        /// Constructor
        /// <param name="remoteFile">The name of the file being transferred.</param>
        /// <param name="byteCount">The current count of bytes transferred.</param>
        /// </summary>        
        public BytesTransferredEventArgs(string remoteFile, long byteCount) 
        {
            this.remoteFile = remoteFile;
            this.byteCount = byteCount;
        }
        
        /// <summary>
        /// Constructor
        /// <param name="byteCount">The current count of bytes transferred.</param>
        /// </summary>        
        public BytesTransferredEventArgs(long byteCount) 
        {
            this.byteCount = byteCount;
        }
        
        /// <summary>
        /// Gets the byte count.
        /// </summary>   
        public long ByteCount
        {
            get 
            {
                return byteCount;
            }
        }
       
        /// <summary>
        /// Name of the file.
        /// </summary>   
        public string RemoteFile
        {
            get 
            {
                return remoteFile;
            }
        }
        
        private long byteCount;
        private string remoteFile;
    }    
    
    /// <summary>
    /// Event args for TransferStarted/Complete
    /// </summary>    
    public class TransferEventArgs : EventArgs 
    {
        /// <summary>
        /// Constructor
        /// </summary>        
        /// <param name="localStream"> 
        /// The stream being transferred to/from.
        /// </param>
        /// <param name="remoteFilename"> 
        /// The remote file name to be uploaded or downloaded
        /// </param>
        /// <param name="direction"> 
        /// Upload or download
        /// </param>
        /// <param name="transferType"> 
        /// ASCII or binary
        /// </param>
        public TransferEventArgs(Stream localStream, string remoteFilename, TransferDirection direction, FTPTransferType transferType) 
        {
            this.localStream = localStream;
            this.remoteFilename = remoteFilename;
            this.direction = direction;
            this.transferType = transferType;
        }
        /// <summary>
        /// Constructor
        /// </summary>        
        /// <param name="localByteArray"> 
        /// The byte-array being transferred to/from.
        /// </param>
        /// <param name="remoteFilename"> 
        /// The remote file name to be uploaded or downloaded
        /// </param>
        /// <param name="direction"> 
        /// Upload or download
        /// </param>
        /// <param name="transferType"> 
        /// ASCII or binary
        /// </param>
        public TransferEventArgs(byte[] localByteArray, string remoteFilename, TransferDirection direction, FTPTransferType transferType) 
        {
            this.localByteArray = localByteArray;
            this.remoteFilename = remoteFilename;
            this.direction = direction;
            this.transferType = transferType;
        }
      
        /// <summary>
        /// Constructor
        /// </summary>        
        /// <param name="localFilePath"> 
        /// Path of the local file to be uploaded or downloaded (<c>null</c> for <c>Stream</c> and <c>byte[]</c> transfers)
        /// </param>
        /// <param name="remoteFilename"> 
        /// The remote file name to be uploaded or downloaded
        /// </param>
        /// <param name="direction"> 
        /// Upload or download
        /// </param>
        /// <param name="transferType"> 
        /// ASCII or binary
        /// </param>
        public TransferEventArgs(string localFilePath, string remoteFilename, TransferDirection direction, FTPTransferType transferType) 
        {
            this.localFilePath = localFilePath;
            this.remoteFilename = remoteFilename;
            this.direction = direction;
            this.transferType = transferType;
        }

        /// <summary>
        /// Gets the path of the local file.
        /// </summary>   
        public string LocalFilePath
        {
            get 
            {
                return localFilePath;
            }
        }

        /// <summary>
        /// Gets the stream being transferred to/from.
        /// </summary>   
        public Stream LocalStream
        {
            get 
            {
                return localStream;
            }
        }

        /// <summary>
        /// Gets the byte-array being transferred to/from.
        /// </summary>   
        public byte[] LocalByteArray
        {
            get 
            {
                return localByteArray;
            }
        }
        
        /// <summary>
        /// Gets the remote filename 
        /// </summary>   
        public string RemoteFilename
        {
            get 
            {
                return remoteFilename;
            }
        }
        
        /// <summary>
        /// Gets the transfer direction 
        /// </summary>   
        public TransferDirection Direction
        {
            get 
            {
                return direction;
            }
        }
        
        /// <summary>
        /// Gets the transfer type 
        /// </summary>   
        public FTPTransferType TransferType
        {
            get 
            {
                return transferType;
            }
        }
        
        private Stream localStream;
        private byte[] localByteArray;
        private string localFilePath;
        private string remoteFilename;
        private TransferDirection direction;
        private FTPTransferType transferType;
    }    

    /// <summary>
    /// Delegate used for ReplyReceived and CommandSent events
    /// </summary>
    public delegate void FTPMessageHandler(object sender, FTPMessageEventArgs e);     
    
    /// <summary>
    /// Delegate used for the BytesTransferred event
    /// </summary>
    public delegate void BytesTransferredHandler(object sender, BytesTransferredEventArgs e);
        
    /// <summary>
    /// Delegate used for TransferStarted and TransferComplete events
    /// </summary>
    public delegate void TransferHandler(object sender, TransferEventArgs e);
    
    /// <summary>
    /// Enumerates the possible transfer directions
    /// </summary>
    public enum TransferDirection 
    {
        /// <member>   
        /// Represents uploading a file
        /// </member>
        UPLOAD = 1,

        /// <member>   
        /// Represents downloading a file
        /// </member>
        DOWNLOAD = 2
    }
    
    /// <summary>  
    /// Enumerates the transfer types possible. We support only the two common types, 
    /// ASCII and Image (often called binary).
    /// </summary>
    public enum FTPTransferType 
    {
        /// <member>   
        /// Represents ASCII transfer type
        /// </member>
        ASCII = 1,

        /// <member>   
        /// Represents Image (or binary) transfer type
        /// </member>
        BINARY = 2
    }

    #endregion
}
