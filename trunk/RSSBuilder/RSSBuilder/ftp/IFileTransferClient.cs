// edtFTPnet
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
// $Log$
// Revision 1.2  2006/06/19 12:59:48  bruceb
// removed explicit namespaces
//
//
//

using System;

namespace EnterpriseDT.Net.Ftp
{
    /// <summary/>
    public interface IFileTransferClient
    {
        #region Properties
        /// <summary/>
        bool CloseStreamsAfterTransfer { get; set; }
        /// <summary/>
        int ControlPort { get; set; }
        /// <summary/>
        bool DeleteOnFailure { get; set; }
        /// <summary/>
        bool IsConnected { get; }
        /// <summary/>
        string RemoteHost { get; set; }
        /// <summary/>
        int Timeout { get; set; }
        /// <summary/>
        int TransferBufferSize { get; set; }
        /// <summary/>
        long TransferNotifyInterval { get; set; }
        /// <summary/>
        FTPTransferType TransferType { get; set; }
        #endregion

        #region Events
        /// <summary/>
        event BytesTransferredHandler BytesTransferred;
        /// <summary/>
        event TransferHandler TransferCompleteEx;
        /// <summary/>
        event TransferHandler TransferStartedEx;
        #endregion

        #region Methods

        #region Connection Methods
        /// <summary/>
        void Connect();
        /// <summary/>
        void Quit();
        #endregion

        #region Authentication Methods
        /// <summary/>
        void Login(string user, string password);
        #endregion

        #region Get Methods
        /// <summary/>
        void Get(System.IO.Stream destStream, string remoteFile);
        /// <summary/>
        void Get(string localPath, string remoteFile);
        /// <summary/>
        byte[] Get(string remoteFile);
        #endregion

        #region Put Methods
        /// <summary/>
        void Put(byte[] bytes, string remoteFile);
        /// <summary/>
        void Put(byte[] bytes, string remoteFile, bool append);
        /// <summary/>
        void Put(string localPath, string remoteFile);
        /// <summary/>
        void Put(string localPath, string remoteFile, bool append);
        /// <summary/>
        void Put(System.IO.Stream srcStream, string remoteFile);
        /// <summary/>
        void Put(System.IO.Stream srcStream, string remoteFile, bool append);
        #endregion

        #region Directory Methods
        /// <summary/>
        void CdUp();
        /// <summary/>
        void ChDir(string dir);
        /// <summary/>
        string[] Dir();
        /// <summary/>
        string[] Dir(string dirname, bool full);
        /// <summary/>
        string[] Dir(string dirname);
        /// <summary/>
        EnterpriseDT.Net.Ftp.FTPFile[] DirDetails(string dirname);
        /// <summary/>
        EnterpriseDT.Net.Ftp.FTPFile[] DirDetails();
        /// <summary/>
        void MkDir(string dir);
        /// <summary/>
        string Pwd();
        /// <summary/>
        void RmDir(string dir);
        #endregion

        #region File Status/Control Methods
        /// <summary/>
        void Delete(string remoteFile);
        /// <summary/>
        DateTime ModTime(string remoteFile);
        /// <summary/>
        void Rename(string from, string to);
        /// <summary/>
        long Size(string remoteFile);
        #endregion

        #region Transfer Control Methods
        /// <summary/>
        void CancelResume();
        /// <summary/>
        void CancelTransfer();
        /// <summary/>
        void Restart(long size);
        /// <summary/>
        void Resume();
        #endregion

        #endregion
    }

    #region FileTransferProtocol

    /// <summary>
    /// Specifies types of File Transfer Protocols.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///		<term>Member name</term>
    ///		<description>Description</description>
    /// </listheader>
    /// <item>
    ///		<term>FTP</term>
    ///		<description>Standard FTP over <b>unencrypted</b> TCP/IP connections.</description>
    /// </item>
    /// <item>
    ///		<term>FTPSExplicit</term>
    ///		<description>Explicit FTPS: Standard FTP-over-SSL as defined in RFC4217.</description>
    /// </item>
    /// <item>
    ///		<term>FTPSImplicit</term>
    ///		<description>Implicit FTPS: Nonstandard, legacy version of FTP-over-SSL.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public enum FileTransferProtocol
    {
        /// <summary>
        /// Standard FTP over <b>unencrypted</b> TCP/IP connections.
        /// </summary>
        FTP,

        /// <summary>
        /// Explicit FTPS: Standard FTP-over-SSL as defined in RFC4217.
        /// </summary>
        FTPSExplicit,

        /// <summary>
        /// Implicit FTPS: Nonstandard, legacy version of FTP-over-SSL.
        /// </summary>
        FTPSImplicit,

        /// <summary>
        /// SFTP - SSH File Transfer Protocol.
        /// </summary>
        SFTP
    }

    #endregion
}
