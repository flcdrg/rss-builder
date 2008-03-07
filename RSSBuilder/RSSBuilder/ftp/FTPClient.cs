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

#region Change Log

// Change Log:
// 
// $Log: FTPClient.cs,v $
// Revision 1.43  2006/06/16 12:12:02  bruceb
// moved out some common types into another file, server wakeup
//
// Revision 1.42  2006/06/14 10:29:15  hans
// Made FTPClient implement IFileTransferClient
// Added ControlEncoding property
// Changed all <p> tags to <para>
//
// Revision 1.41  2006/06/14 10:08:41  bruceb
// moved types into separate file
//
// Revision 1.40  2006/05/27 10:23:38  bruceb
// change default port range to 1024->5000
//
// Revision 1.39  2006/05/25 05:43:02  hans
// Flush output-stream in stream-based puts.
//
// Revision 1.38  2006/04/18 07:16:53  hans
// - Changed PortRange so that its properties can be set after construction.
// - FTPClient.ActivePortRange now has a default object instead of null.  This was mainly done so that its properties could be viewed in FTPConnection.
//
// Revision 1.37  2006/03/16 22:27:46  hans
// Added stream and byte-array fields to TransferEventArgs.
// Improved comments.
// Added CloseStreamsAfterTransfer property.
// Moved TransferStarted event firing to before any transfer operations have taken place (prevents hangs in event-handlers with FTP operations in them).
//
// Revision 1.36  2006/02/09 10:30:27  hans
// Fixed bug in TransferEventArgs constructor
//
// Revision 1.35  2005/12/13 19:52:36  hans
// Added AutoPassiveIPSubstitution
//
// Revision 1.34  2005/11/28 21:20:28  hans
// Set culture to Invariant if value is null.
//
// Revision 1.32  2005/10/13 17:22:47  bruceb
// fixed TransferComplete events so they occur after 226 ack
//
// Revision 1.31  2005/09/30 18:04:22  bruceb
// fix re 226 being returned if no files
//
// Revision 1.30  2005/09/30 09:24:47  hans
// Added local file-name to TransferEventArgs
//
// Revision 1.29  2005/09/30 06:33:44  bruceb
// permit 350 return from STOR
//
// Revision 1.28  2005/09/20 11:12:01  bruceb
// data set compile fix
//
// Revision 1.27  2005/09/20 10:25:02  bruceb
// Restart() public, don't use abort in cancel, dir listing addition for empty dir
//
// Revision 1.26  2005/09/02 07:01:41  hans
// Check for remoteHost before connect
//
// Revision 1.25  2005/08/23 21:23:04  hans
// Added remoteFile to ByteTransferEventArgs.
//
// Revision 1.24  2005/08/05 13:45:51  bruceb
// active mode port/ip address setting
//
// Revision 1.23  2005/08/04 21:58:41  bruceb
// 550/450 changes
//
// Revision 1.22  2005/07/22 10:39:30  hans
// Added comments
//

#endregion

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Globalization;
using System.ComponentModel;

using EnterpriseDT.Net;
using EnterpriseDT.Util.Debug;

namespace EnterpriseDT.Net.Ftp
{
    #region Types
      
    /// <summary>
    /// Specifies a TCP port range defining the lower and upper limits for
    /// data-channels.
    /// </summary>
    /// <remarks>
    /// The default is to let the operating system select
    /// the port number within the range 1024-5000.  If the range is set to
    /// anything other than the default then ports will be selected sequentially,
    /// increasing by one until the higher limit is reached and then wrapping around
    /// to the lower limit.
    /// </remarks>
    public class PortRange 
    {
        /// <summary>
        /// Lowest port number permitted.  This is also the default value for 
        /// <see cref="LowPort"/>.
        /// </summary>
        private const int LOW_PORT = 1024;
        
        /// <summary>
        /// Default value for <see cref="HighPort"/>.
        /// </summary>
        private const int DEFAULT_HIGH_PORT = 5000;
        
        /// <summary>
        /// Highest port number permitted.
        /// </summary>
        private const int HIGH_PORT = 65535;
        
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public PortRange()
		{
			this.low = LOW_PORT;
			this.high = DEFAULT_HIGH_PORT;
		}
        
		/// <summary>
		/// Constructor setting the lower and higher limits of the range.
		/// </summary>
		/// <param name="low">Lower limit of the port-range.</param>
        /// <param name="high">Higher limit of the port-range.</param>
		public PortRange(int low, int high)
		{
			if (low >= high || low < LOW_PORT || high > HIGH_PORT)
				throw new ArgumentException("Ports must be in range [" + LOW_PORT + "," + HIGH_PORT + "]");
			this.low = low;
			this.high = high;
		}
        
        /// <summary>
        /// Lowest port number in range.
        /// </summary>
        /// <remarks>
        /// The default value is 1024.  If it is left at this value and <see cref="HighPort"/>
        /// is left at 5000 then the OS will select the port.  If it is set to
        /// anything other than 1024 then ports will be selected sequentially,
        /// increasing by one until the higher limit is reached and then wrapping around
        /// to the lower limit.
        /// </remarks>
		[Description("Lowest port number in range.")]
		[DefaultValue(LOW_PORT)]
		public int LowPort
        {
            get 
            {
                return low;
            }
			set
			{
				if (value >= high || value < LOW_PORT)
					throw new ArgumentException("Ports must be in range [" + LOW_PORT + "," + HIGH_PORT + "]");
				low = value;
			}
        }
        
        /// <summary>
        /// Highest port number in range.
        /// </summary>
        /// <remarks>
        /// The default value is 5000.  If it is left at this value and <see cref="LowPort"/>
        /// is left at 1024 then the OS will select the port.  If it is set to
        /// anything other than 5000 then ports will be selected sequentially,
        /// increasing by one until the higher limit is reached and then wrapping around
        /// to the lower limit.
        /// </remarks>
        [Description("Highest port number in range.")]
		[DefaultValue(HIGH_PORT)]
		public int HighPort
        {
            get 
            {
                return high;
            }
			set
			{
				if (low >= value || value > HIGH_PORT)
					throw new ArgumentException("Ports must be in range [" + LOW_PORT + "," + HIGH_PORT + "]");
				high = value;
			}
        }

        /// <summary>
        /// Determines if the operating system should select the ports within the range 1024-5000.
        /// </summary>
        /// <remarks>
        /// If <c>UseOSAssignment</c> is set to <c>true</c> then the OS will select data-channel
        /// ports within the range 1024-5000.  Otherwise ports will be selected sequentially,
        /// increasing by one until the higher limit is reached and then wrapping around
        /// to the lower limit.  Setting this flag will cause <see cref="LowPort"/> and
        /// <see cref="HighPort"/> to be set to 1024 and 5000, respectively.
        /// </remarks>
        [Description("Determines if the operating system should select the ports within the range 1024-5000.")]
        [DefaultValue(true)]
        public bool UseOSAssignment
        {
            get
            {
                return low == LOW_PORT && high == DEFAULT_HIGH_PORT;
            }
            set
            {
                low = LOW_PORT;
                high = DEFAULT_HIGH_PORT;
            }
        }

		public override string ToString()
		{
			return string.Format("{0} -> {1}", low, high);;
		}

        
        /// <summary>
        /// Low port number in range
        /// </summary>
        private int low;
        
        /// <summary>
        /// High port number in range
        /// </summary>
        private int high;        
    }
           
    /// <summary>
    /// Enumerates the connect modes that are possible, active and passive
    /// </summary>
    public enum FTPConnectMode 
    {
        /// <member>   
        /// Represents active - PORT - connect mode
        /// </member>
        ACTIVE = 1,

        /// <member>   
        /// Represents passive - PASV - connect mode
        /// </member>
        PASV = 2
    }
    
    #endregion
    
    /// <summary>  
    /// Supports client-side FTP. Most common
    /// FTP operations are present in this class.
    /// </summary>
    /// <author>Bruce Blackshaw</author>
    /// <version>$Revision: 1.43 $</version>
    public class FTPClient : IFileTransferClient
    {
		/// <summary>The version of edtFTPj.</summary>
		/// <value>An <c>int</c> array of <c>{major,middle,minor}</c> version numbers.</value>
		public static int[] Version
        {
            get
            {
                return version;
            }
            
        }

		/// <summary>The edtFTPj build timestamp.</summary>
		/// <value>
		/// Timestamp of when edtFTPj was build in the format <c>d-MMM-yyyy HH:mm:ss z</c>.
		/// </value>
		public static string BuildTimestamp
        {
            get
            {
                return buildTimestamp;
            }            
        }

		/// <summary>Controls whether or not checking of return codes is strict.</summary>
		/// <remarks>
		/// <para>
		/// Some servers return non-standard reply-codes.  Setting this property to <c>false</c>
		/// only the first digit of the reply-code is checked, thus decreasing the sensitivity
		/// of edtFTPj to non-standard reply-codes.  The default is <c>true</c> meaning that
		/// reply-codes must match exactly.
		/// </para>
		/// </remarks>
		/// <value>  
		/// <c>true</c> if strict return code checking, <c>false</c> if non-strict.
		/// </value>
		public bool StrictReturnCodes
        {
            get
            {
                return strictReturnCodes;
            }
            
            set
            {
                this.strictReturnCodes = value;
                if (control != null)
                    control.StrictReturnCodes = value;
            }
            
        }
		/// <summary> 
		/// TCP timeout on the underlying sockets.
		/// </summary>
		virtual public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {                
                this.timeout = value;
                if (control != null)
                    control.Timeout = value;
            }        
        }

        /// <summary>  
        /// Is the client currently connected?
        /// </summary>
        public bool Connected
        {
            get
            {
                return control != null;
            }
        }
      
		/// <summary>
		/// The connection-mode (passive or active) of data-channels.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When the connection-mode is active, the server will initiate connections
		/// to the client, meaning that the client must open a socket and wait for the
		/// server to connect to it.  This often causes problems if the client is behind
		/// a firewall.
		/// </para>
		/// <para>
		/// When the connection-mode is passive, the client will initiates connections
		/// to the server, meaning that the client will connect to a particular socket
		/// on the server.  This is generally used if the client is behind a firewall.
		/// </para>
		/// </remarks>
		public FTPConnectMode ConnectMode
        {
            set
            {
                connectMode = value;
            }
            get
            {
                return connectMode;
            }
        }
        
		/// <summary>
		/// Indicates whether the client is currently connect with the server.
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return control!=null;
			}
		}

		/// <summary>
		/// The number of bytes transferred between each notification of the
		/// <see cref="BytesTransferred"/> event.
		/// </summary>
		/// <remarks>
		/// Reduce this value to receive more frequent notifications of transfer progress.
		/// </remarks>
		public long TransferNotifyInterval
        {
            get
            {
                return monitorInterval;
            }
            set
            {
                monitorInterval = value;
            }
        }

		/// <summary>
		/// The size of the buffers (in bytes) used in writing to and reading from the data-sockets.
		/// </summary>
		public int TransferBufferSize
        {
            get
            {
                return transferBufferSize;
            }
            
            set
            {
				if (value<=0)
					throw new ArgumentException("TransferBufferSize must be greater than 0.");
                transferBufferSize = value;
            }
        }
             
		/// <summary>
		/// The domain-name or IP address of the FTP server.
		/// </summary>
		/// <remarks>
		/// <para>This property may only be set if not currently connected.</para>.
		/// </remarks>
		public virtual string RemoteHost
        {
            get
            {
                return remoteHost;
            }
            set
            {
                CheckConnection(false);
                remoteHost = value;
            }
        }    
        
		/// <summary>
		/// Controls whether or not a file is deleted when a failure occurs.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If <c>true</c>, a partially downloaded file is deleted if there
		/// is a failure during the download.  For example, the connection
		/// to the FTP server might have failed. If <c>false</c>, the partially
		/// downloaded file remains on the client machine - and the download
		/// may be resumed, if it is a binary transfer.
		/// </para>
		/// <para>
		/// By default this flag is set to <c>true</c>.
		/// </para>
		/// </remarks>
		public bool DeleteOnFailure
        {
            get
            {
                return deleteOnFailure;
            }
            set
            {
                deleteOnFailure = value;
            }
        }    
        
		/// <summary>
		/// The port on the server to which to connect the control-channel. 
		/// </summary>
		/// <remarks>
		/// <para>Most FTP servers use port 21 (the default)</para>
		/// <para>This property may only be set if not currently connected.</para>
		/// </remarks>
		public int ControlPort
        {
            get
            {
                return controlPort;
            }
            set
            {
                CheckConnection(false);
                controlPort = value;
            }
        }    
        
		/// <summary>The culture for parsing file listings.</summary>
		/// <remarks>
		/// The <see cref="DirDetails(string)"/> method parses the file listings returned.  The names of the file
		/// can contain a wide variety of characters, so it is sometimes necessary to set this
		/// property to match the character-set used on the server.
		/// </remarks>
		public CultureInfo ParsingCulture
        {
            get
            {
                return parserCulture;
            }
            set
            {
				if (value==null)
					value = CultureInfo.InvariantCulture;
                this.parserCulture = value;
                if (fileFactory != null)
                    fileFactory.ParsingCulture = value;
            }            
        }

		/// <summary>
		/// The encoding to use when dealing with file and directory paths.
		/// </summary>
		public Encoding ControlEncoding
		{
			get
			{
				return controlEncoding;
			}
			set
			{
				controlEncoding = value;
			}
		}
                
		/// <summary>
		/// Override the chosen file factory with a user created one - meaning
		/// that a specific parser has been selected.
		/// </summary>
		public FTPFileFactory FTPFileFactory
        {
            set
            {
                this.fileFactory = value;
            }            
        }
        
		/// <summary>The latest valid reply from the server.</summary>
		/// <value>
		/// Reply object encapsulating last valid server response.
		/// </value>
		public FTPReply LastValidReply
        {
            get
            {
                return lastValidReply;
            }
        }

		/// <summary>The current file transfer type (BINARY or ASCII).</summary>
		/// <remarks>When the transfer-type is set to <c>BINARY</c> then files
		/// are transferred byte-for-byte such that the transferred file will
		/// be identical to the original.
		/// When the transfer-type is set to <c>BINARY</c> then end-of-line
		/// characters will be translated where necessary between Windows and
		/// UNIX formats.</remarks>
		public FTPTransferType TransferType
        {
            get
            {
                return transferType;
            }
            set
            {                
                CheckConnection(true);
                
                // determine the character to send
                string typeStr = ASCII_CHAR;
                if (value.Equals(FTPTransferType.BINARY))
                    typeStr = BINARY_CHAR;
                
                // send the command
                FTPReply reply = control.SendCommand("TYPE " + typeStr);
                lastValidReply = control.ValidateReply(reply, "200");
                
                // record the type
                transferType = value;
            }            
        }
        
        /// <summary>
        /// Port range for active mode, used only if it is
        /// necessary to limit the ports to a narrow range specified
        /// in a firewall
        /// </summary>
        public PortRange ActivePortRange
        {
            get
            {
                return activePortRange;
            }
            
            set
            {                
                activePortRange = value;
                if (control != null)
                    control.SetActivePortRange(value);
            }
        }
        
        /// <summary>
        /// Force the PORT command to send a fixed IP address, used only for
        /// certain firewalls
        /// </summary>
        public IPAddress ActiveIPAddress
        {
            get
            {
                return activeIPAddress;
            }
            set
            {
                activeIPAddress = value;
                if (control != null)
                    control.SetActiveIPAddress(value);
            }
        }
        
		/// <summary>
		/// Use <c>AutoPassiveIPSubstitution</c> to ensure that 
		/// data-socket connections are made to the same IP address
		/// that the control socket is connected to.
		/// </summary>
		/// <remarks>
		/// <para>
		/// <c>AutoPassiveIPSubstitution</c> is useful in passive mode when the 
		/// FTP server is supplying an incorrect IP address to the client for 
		/// use in creating data connections (directory listings and file 
		/// transfers), e.g. an internal IP address that is not accessible from 
		/// the client. Instead, the client will use the IP address obtained 
		/// from the FTP server's hostname.
		/// </para>
		/// <para>
		/// This usually happens when an FTP server is behind
		/// a NAT router and has not been configured to reflect the fact that
		/// its internal (LAN) IP address is different from the address that
		/// external (Internet) machines connect to.
		/// </para>
		/// </remarks>
		public bool AutoPassiveIPSubstitution
		{
			get
			{
				return this.autoPassiveIPSubstitution;
			}
			set
			{
				this.autoPassiveIPSubstitution = value;
				if (control!=null)
					control.AutoPassiveIPSubstitution = value;
			}
		}

		/// <summary>
		/// If <c>true</c> then streams are closed after a transfer has completed.
		/// </summary>
		/// <remarks>The default is <c>true</c>.</remarks>
		public bool CloseStreamsAfterTransfer
		{
			get
			{
				return closeStreamsAfterTransfer;
			}
			set
			{
				closeStreamsAfterTransfer = value;
			}
		}

        /// <summary>
        /// The interval in seconds that the server is sent a wakeup message during
        /// large transfers.
        /// </summary>
        /// <remarks>During very large transfers some servers timeout, meaning
        /// that the transfer is not correctly completed. If this value is
        /// set to 0, no wakeup messages are sent. Note that many servers can't
        /// cope with a NOOP sent during a transfer, so only set this property if
        /// you are having timeout problems with very large transfers. It can result
        /// in receiving replies to NOOP with subsequent commands, so use with
        /// caution and check your log files.</remarks>
        public int ServerWakeupInterval
        {
            get 
            {
                return noOperationInterval;
            }
            set 
            {
                CheckConnection(false);
                noOperationInterval = value;
            }
        }

		/// <summary>
		/// Notifies of the start of a transfer.
		/// </summary>
		[Obsolete("Use TransferStartedEx")]
		public virtual event EventHandler TransferStarted;
        
		/// <summary>
		/// Notifies of the start of a transfer, and supplies more details than <see cref="TransferStarted"/>
		/// </summary>
		public virtual event TransferHandler TransferStartedEx;
        
		/// <summary>
		/// Notifies of the completion of a transfer.
		/// </summary> 
		[Obsolete("Use TransferCompleteEx")]
		public virtual event EventHandler TransferComplete;
        
		/// <summary>
		/// Notifies of the completion of a transfer, and supplies more details than <see cref="TransferComplete"/>
		/// </summary> 
		public virtual event TransferHandler TransferCompleteEx;
            
		/// <summary>
		/// Event triggered every time <see cref="TransferNotifyInterval"/> bytes transferred.
		/// </summary> 
		public virtual event BytesTransferredHandler BytesTransferred;
        
		/// <summary>
		/// Triggered every time a command is sent to the server.
		/// </summary> 
		public virtual event FTPMessageHandler CommandSent;
        
		/// <summary>
		/// Triggered every time a reply is received from the server.
		/// </summary> 
		public virtual event FTPMessageHandler ReplyReceived;
        
        /// <summary> Default byte interval for transfer monitor</summary>
        private const int DEFAULT_MONITOR_INTERVAL = 4096;
        
        /// <summary> Default transfer buffer size</summary>
        private const int DEFAULT_BUFFER_SIZE = 4096;
        
        /// <summary> Major version (substituted by ant)</summary>
        private static string majorVersion = "1";
        
        /// <summary> Middle version (substituted by ant)</summary>
        private static string middleVersion = "2";
        
        /// <summary> Middle version (substituted by ant)</summary>
        private static string minorVersion = "3";
        
        /// <summary> Full version</summary>
        private static int[] version;
        
        /// <summary> Timestamp of build</summary>
        private static string buildTimestamp = "19-Jun-2006 15:32:23 BST";
        
        /// <summary>  
        /// The char sent to the server to set BINARY
        /// </summary>
        private static string BINARY_CHAR = "I";

        /// <summary>  
        /// The char sent to the server to set ASCII
        /// </summary>
        private static string ASCII_CHAR = "A";
        
        /// <summary>  
        /// Server string indicating no files found
        /// </summary>
        private static string NO_FILES = "NO FILES";
        
        /// <summary>  
        /// Server string indicating no files found
        /// </summary>
        private static string EMPTY_DIR = "EMPTY";
        
        /// <summary>
        /// Server string for OS/390 indicating no files found
        /// </summary>
        private static string NO_DATA_SETS_FOUND = "NO DATA SETS FOUND";

        /// <summary>
        /// Server string indicating no files found (wu-ftpd)
        /// </summary>
        private static string NO_SUCH_FILE_OR_DIR = "NO SUCH FILE OR DIRECTORY";

        /// <summary>
        /// Array of empty directory messages
        /// </summary>
        private static string[] EMPTY_DIR_MSGS = {NO_FILES, NO_SUCH_FILE_OR_DIR, EMPTY_DIR, NO_DATA_SETS_FOUND};

        /// <summary>
        /// Sometimes returned with 226 in NLST if no files found
        /// </summary>
        private static string TRANSFER_COMPLETE = "TRANSFER COMPLETE";

        /// <summary>Date format</summary>
        private static readonly string tsFormat = "yyyyMMddHHmmss";
        
        /// <summary> Logging object</summary>
        private Logger log;
        
        /// <summary>  Socket responsible for controlling
        /// the connection
        /// </summary>
        internal FTPControlSocket control = null;
        
        /// <summary>  Socket responsible for transferring
        /// the data
        /// </summary>
        internal FTPDataSocket data = null;
        
        /// <summary>  Socket timeout for both data and control. In
        /// milliseconds
        /// </summary>
        internal int timeout = 0;

        /// <summary>
        /// Interval for NOOP calls during large transfers in seconds
        /// </summary>
        private int noOperationInterval = 0;
        
        /// <summary> Use strict return codes if true</summary>
        private bool strictReturnCodes = true;
        
        /// <summary>  Can be used to cancel a transfer</summary>
        private bool cancelTransfer = false;
        
        /// <summary> If true, a file transfer is being resumed</summary>
        private bool resume = false;
        
        /// <summary>If a download to a file fails, delete the partial file</summary>
        private bool deleteOnFailure = true;        
        
        /// <summary> Resume byte marker point</summary>
        private long resumeMarker = 0;
        
        /// <summary> Bytes transferred in between monitor callbacks</summary>
        private long monitorInterval = DEFAULT_MONITOR_INTERVAL;
        
        /// <summary> Size of transfer buffers</summary>
        private int transferBufferSize = DEFAULT_BUFFER_SIZE;
        
        /// <summary>Culture used for parsing file details</summary>
        private CultureInfo parserCulture = CultureInfo.InvariantCulture;
        
        /// <summary> Parses LIST output</summary>
        private FTPFileFactory fileFactory = null;
                        
        /// <summary>  Record of the transfer type - make the default ASCII</summary>
        private FTPTransferType transferType = FTPTransferType.ASCII;
        
        /// <summary>  Record of the connect mode - make the default PASV (as this was
        /// the original mode supported)
        /// </summary>
        private FTPConnectMode connectMode = FTPConnectMode.PASV;
        
        /// <summary>
        /// Port range for active mode
        /// </summary>
        private PortRange activePortRange = new PortRange();
        
        /// <summary>
        /// IP address to send with active mode
        /// </summary>
        private IPAddress activeIPAddress = null;
        
        /// <summary>
        /// Holds the last valid reply from the server on the control socket
        /// </summary>
        internal FTPReply lastValidReply;
        
        /// <summary>
        /// Port on which we connect to the FTP server and messages are passed
        /// </summary>
        internal int controlPort = -1;        
        
        /// <summary>
        /// Remote host we are connecting to
        /// </summary>
        internal string remoteHost = null;
        
		/// <summary>
		/// If true, uses the original host IP if an internal IP address
		/// is returned by the server in PASV mode.
		/// </summary>
		private bool autoPassiveIPSubstitution = false;

		/// <summary>
		/// If <c>true</c> then streams are closed after a transfer has completed.
		/// </summary>
		/// <remarks>
		/// The default is <c>true</c>.
		/// </remarks>
		private bool closeStreamsAfterTransfer = true;

		/// <summary>
		/// The encoding to use when dealing with file and directory paths.
		/// </summary>
		private Encoding controlEncoding = Encoding.ASCII;

        #region Constructors
        
		/// <summary>Constructs an <c>FTPClient</c> instance and connects to the FTP server.</summary>
		/// <param name="remoteHost">The domain-name or IP address of the FTP server.</param>
		[Obsolete("This constructor is obsolete; use the default constructor and properties instead")]
		public FTPClient(string remoteHost):
            this(remoteHost, FTPControlSocket.CONTROL_PORT, 0)
        {
        }
        
		/// <summary>Constructs an <c>FTPClient</c> instance and connects to the FTP server.</summary>
		/// <param name="remoteHost">The domain-name or IP address of the FTP server.</param>
		/// <param name="controlPort">The port for control stream (-1 for default port).</param>
		[Obsolete("This constructor is obsolete; use the default constructor and properties instead")]
		public FTPClient(string remoteHost, int controlPort):
            this(remoteHost, controlPort, 0)
        {
        }
                
		/// <summary>Constructs an <c>FTPClient</c> instance and connects to the FTP server.</summary>
		/// <param name="remoteHost">The domain-name or IP address of the FTP server.</param>
		/// <param name="controlPort">The port for control stream (-1 for default port).</param>
		/// <param name="timeout">The length of the timeout in milliseconds (pass in 0 for no timeout)</param>
		[Obsolete("This constructor is obsolete; use the default constructor and properties instead")]
		public FTPClient(string remoteHost, int controlPort, int timeout):
            this(HostNameResolver.GetAddress(remoteHost), controlPort, timeout)
        {
            this.remoteHost = remoteHost;
        }
        
        
		/// <summary>Constructs an <c>FTPClient</c> instance and connects to the FTP server.</summary>
		/// <param name="remoteAddr">The address of the FTP server.</param>
		[Obsolete("This constructor is obsolete; use the default constructor and properties instead")]
		public FTPClient(IPAddress remoteAddr):
            this(remoteAddr, FTPControlSocket.CONTROL_PORT, 0)
        {
        }
        
        
		/// <summary>Constructs an <c>FTPClient</c> instance and connects to the FTP server.</summary>
		/// <param name="remoteAddr">The address of the FTP server.</param>
		/// <param name="controlPort">The port for control stream (-1 for default port).</param>
		[Obsolete("This constructor is obsolete; use the default constructor and properties instead")]
		public FTPClient(IPAddress remoteAddr, int controlPort):
            this(remoteAddr, controlPort, 0)
        {
        }
        
		/// <summary>Constructs an <c>FTPClient</c> instance and connects to the FTP server.</summary>
		/// <param name="remoteAddr">The address of the FTP server.</param>
		/// <param name="controlPort">The port for control stream (-1 for default port).</param>
		/// <param name="timeout">The length of the timeout in milliseconds (pass in 0 for no timeout)</param>
		[Obsolete("This constructor is obsolete; use the default constructor and properties instead")]
		public FTPClient(IPAddress remoteAddr, int controlPort, int timeout)
        {
            InitBlock();
            remoteHost = remoteAddr.ToString();
            Connect(remoteAddr, controlPort, timeout);
        }
        
		/// <summary>Constructs an <c>FTPClient</c>.</summary>
		/// <remarks>
		/// The <c>FTPClient</c> will not connect to the FTP server until <see cref="Connect()"/> is called.
		/// </remarks>
		public FTPClient()
        {
            InitBlock();
        }
        
        #endregion
        
        /// <summary>  
        /// Instance initializer. Sets formatter to GMT.
        /// </summary>
        private void InitBlock()
        {
            log = Logger.GetLogger(typeof(FTPClient));
            transferType = FTPTransferType.ASCII;
            connectMode = FTPConnectMode.PASV;
            controlPort = FTPControlSocket.CONTROL_PORT;
        }
        
		/// <summary>Connect to the FTP server.</summary>
		/// <remarks>
		/// <para>
		/// The <see cref="RemoteHost"/> property must be set prior to calling this method.
		/// This method must be called before <see cref="Login(string,string)"/> or <see cref="User(string)"/>
		/// is called.
		/// </para>
		/// <para>
		/// This method will throw an <c>FTPException</c> if the client is already connected to the server.
		/// </para>
		/// </remarks>
		public virtual void Connect() 
        {
            CheckConnection(false);
			if (remoteHost==null)
				throw new FTPException("RemoteHost is not set.");
            Connect(HostNameResolver.GetAddress(remoteHost), controlPort, timeout);
        }
        
        internal virtual void Connect(IPAddress remoteAddr, int controlPort, int timeout) 
        {
            if (controlPort < 0) 
            {
                log.Warn("Invalid control port supplied: " + controlPort + " Using default: " +
                    FTPControlSocket.CONTROL_PORT);
                controlPort = FTPControlSocket.CONTROL_PORT;
            }    
            this.controlPort = controlPort;
            if (log.DebugEnabled)
                log.Debug("Connecting to " + remoteAddr.ToString() + ":" + controlPort);
            Initialize(new FTPControlSocket(remoteAddr, controlPort, timeout, controlEncoding));            
        }
        
        /// <summary>Set the control socket explicitly.</summary>
        /// <param name="control">Control socket reference.</param>
        internal void Initialize(FTPControlSocket control)
        {
            this.control = control;
			this.control.AutoPassiveIPSubstitution = autoPassiveIPSubstitution;
            
            // set up the event handlers so they call back to this object - and can
            // then be passed on if required
            control.CommandSent += new FTPMessageHandler(CommandSentControl);
            control.ReplyReceived += new FTPMessageHandler(ReplyReceivedControl);
            if (activePortRange != null)
                control.SetActivePortRange(activePortRange);
            if (activeIPAddress != null)
                control.SetActiveIPAddress(activeIPAddress);
        }

        
        /// <summary> 
        /// Checks if the client has connected to the server and throws an exception if it hasn't.
        /// This is only intended to be used by subclasses
        /// </summary>
        /// <throws>FTPException Thrown if the client has not connected to the server. </throws>
        internal void CheckConnection(bool shouldBeConnected)
        {
            if (shouldBeConnected && control == null)
                throw new FTPException("The FTP client has not yet connected to the server.  " + 
                "The requested action cannot be performed until after a connection has been established.");
            else if (!shouldBeConnected && control != null)
                throw new FTPException("The FTP client has already been connected to the server.  " + 
                "The requested action must be performed before a connection is established.");
        }
        
        
        internal void CommandSentControl(object client, FTPMessageEventArgs message) 
        {
            if (CommandSent != null)
                CommandSent(this, message);            
        }
        
        
        internal void ReplyReceivedControl(object client, FTPMessageEventArgs message) 
        {
            if (ReplyReceived != null)
                ReplyReceived(this, message);
        }
        
        
		/// <summary>Switch debug of responses on or off</summary>
		/// <param name="on"><c>true</c> if you wish to have responses to
		/// the log stream, <c>false</c> otherwise.</param>
		/// <deprecated>
		/// Use the <see cref="EnterpriseDT.Util.Debug.Logger"/> class to 
		/// switch debugging on and off.
		/// </deprecated>
		public void DebugResponses(bool on)
        {
            if (on)
                Logger.CurrentLevel = Level.DEBUG;
            else
                Logger.CurrentLevel = Level.OFF;
        }
        
        
		/// <summary>Cancels the current transfer.</summary>
		/// <remarks>This method is generally called from a separate
		/// thread. Note that this may leave partially written files on the
		/// server or on local disk, and should not be used unless absolutely
		/// necessary. The server is not notified.</remarks>
		public virtual void CancelTransfer()
        {
            cancelTransfer = true;
        }
        
		/// <summary>Login into an account on the FTP server using the user-name and password provided.</summary>
		/// <remarks>This
		/// call completes the entire login process. Note that
		/// <see cref="Connect()"/> must be called first.</remarks>
		/// <param name="user">User-name.</param>
		/// <param name="password">Password.</param>
		public virtual void Login(string user, string password)
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("USER " + user);
            
            // we allow for a site with no password - 230 response
            string[] validCodes = new string[]{"230", "331"};
            lastValidReply = control.ValidateReply(reply, validCodes);
            if (lastValidReply.ReplyCode.Equals("230"))
                return ;
            else
            {
                Password(password);
            }
        }
        
		/// <summary>
		/// Supply the user-name to log into an account on the FTP server. 
		/// Must be followed by the <see cref="Password(string)"/> method.
		/// Note that <see cref="Connect()"/> must be called first. 
		/// </summary>
		/// <param name="user">User-name.</param>
		public virtual void User(string user)
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("USER " + user);
            
            // we allow for a site with no password - 230 response
            string[] validCodes = new string[]{"230", "331"};
            lastValidReply = control.ValidateReply(reply, validCodes);
        }
        
        
		/// <summary>
		/// Supplies the password for a previously supplied
		/// user-name to log into the FTP server. Must be
		/// preceeded by the <see cref="User(string)"/> method
		/// </summary>
		/// <param name="password">Password.</param>
		public virtual void Password(string password)
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("PASS " + password);
            
            // we allow for a site with no passwords (202)
            string[] validCodes = new string[]{"230", "202"};
            lastValidReply = control.ValidateReply(reply, validCodes);
        }
        

		/// <summary>Issue arbitrary ftp commands to the FTP server.</summary>
		/// <param name="command">FTP command to be sent to server.</param>
		/// <param name="validCodes">Valid return codes for this command.</param>
		/// <returns>The text returned by the FTP server.</returns>
		public virtual string Quote(string command, string[] validCodes)
        {        
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand(command);
            
            // allow for no validation to be supplied
            if (validCodes != null)
            {
                lastValidReply = control.ValidateReply(reply, validCodes);
                
            }
            else // not doing any validation
            {
                lastValidReply = reply;
            }
            return lastValidReply.ReplyText;
        }
        
        
		/// <summary>
		/// Get the size of a remote file. 
		/// </summary>
		/// <remarks>
		/// This is not a standard FTP command, it is defined in "Extensions to FTP", a draft RFC 
		/// (draft-ietf-ftpext-mlst-16.txt).
		/// </remarks>
		/// <param name="remoteFile">Name or path of remote file in current directory.</param>
		/// <returns>Size of file in bytes.</returns>
		public virtual long Size(string remoteFile)
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("SIZE " + remoteFile);
            lastValidReply = control.ValidateReply(reply, "213");
            
            // parse the reply string .
            string replyText = lastValidReply.ReplyText;
            
            // trim off any trailing characters after a space, e.g. webstar
            // responds to SIZE with 213 55564 bytes
            int spacePos = replyText.IndexOf((System.Char) ' ');
            if (spacePos >= 0)
                replyText = replyText.Substring(0, (spacePos) - (0));
            
            // parse the reply
            try
            {
                return Int64.Parse(replyText);
            }
            catch (FormatException)
            {
                throw new FTPException("Failed to parse reply: " + replyText);
            }
        }
        
		/// <summary>Make the next file transfer (put or get) resume.</summary>
		/// <remarks>
		/// <para>
		/// For puts, the
		/// bytes already transferred are skipped over, while for gets, if 
		/// writing to a file, it is opened in append mode, and only the bytes
		/// required are transferred.
		/// </para>
		/// <para>
		/// Currently resume is only supported for BINARY transfers (which is
		/// generally what it is most useful for). 
		/// </para>
		/// </remarks>
		/// <throws>FTPException</throws>
		public virtual void Resume()
        {
            if (transferType.Equals(FTPTransferType.ASCII))
                throw new FTPException("Resume only supported for BINARY transfers");
            resume = true;
        }
        
        /// <summary> 
        /// Cancel the resume. Use this method if something goes wrong
        /// and the server is left in an inconsistent state
        /// </summary>
        /// <throws>  SystemException </throws>
        /// <throws>  FTPException </throws>
        public virtual void CancelResume()
        {
            Restart(0);
            resume = false;
        }
        
		/// <summary>Set the REST marker so that the next transfer doesn't start at the beginning of the remote file</summary>
		/// <remarks>
		/// Issue the RESTart command to the remote server. This indicates the byte
        /// position that REST is performed at. For put, bytes start at this point, while
        /// for get, bytes are fetched from this point.
		/// </remarks>
		/// <param name="size">the REST param, the mark at which the restart is performed on the remote file. 
		/// For STOR, this is retrieved by SIZE</param>
		/// <throws>SystemException </throws>
		/// <throws>FTPException </throws>
		public void Restart(long size)
        {
            FTPReply reply = control.SendCommand("REST " + size);
            lastValidReply = control.ValidateReply(reply, "350");
        }
        
		/// <summary>
		/// Put a local file onto the FTP server in the current directory.
		/// </summary>
		/// <param name="localPath">Path of the local file.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		public virtual void Put(string localPath, string remoteFile)
        {            
            Put(localPath, remoteFile, false);
        }
        
		/// <summary>
		/// Put a stream of data onto the FTP server in the current directory.
		/// </summary>
		/// <remarks>
		/// The stream is closed after the transfer is complete if
		/// <see cref="CloseStreamsAfterTransfer"/> is <c>true</c> (the default) and are left
		/// open otherwise.
		/// </remarks>
		/// <param name="srcStream">Input stream of data to put.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		public virtual void Put(Stream srcStream, string remoteFile)
        {            
            Put(srcStream, remoteFile, false);
        }
        
        
		/// <summary>
		/// Put a local file onto the FTP server in the current directory. Allows appending
		/// if current file exists.
		/// </summary>
		/// <param name="localPath">Path of the local file.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise</param>
		public virtual void Put(string localPath, string remoteFile, bool append)
        {            
			// events called before get in case event-handlers want to
			// do any FTP operations
			if (TransferStarted != null)
				TransferStarted(this, new EventArgs());
			if (TransferStartedEx != null)
				TransferStartedEx(this, new TransferEventArgs(localPath, remoteFile, TransferDirection.UPLOAD, transferType));

			// get according to set type
            if (transferType == FTPTransferType.ASCII)
            {
                PutASCII(localPath, remoteFile, append);
            }
            else
            {
                PutBinary(localPath, remoteFile, append);
            }
            ValidateTransfer();
		
			if (TransferComplete != null)
				TransferComplete(this, new EventArgs());
			if (TransferCompleteEx != null)
				TransferCompleteEx(this, new TransferEventArgs(localPath, remoteFile, TransferDirection.UPLOAD, transferType));
		}
        
		/// <summary>
		/// Put a stream of data onto the FTP server in the current directory.  Allows appending
		/// if current file exists
		/// </summary>
		/// <remarks>
		/// The stream is closed after the transfer is complete if
		/// <see cref="CloseStreamsAfterTransfer"/> is <c>true</c> (the default) and are left
		/// open otherwise.
		/// </remarks>
		/// <param name="srcStream">Input stream of data to put.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise.</param>
		public virtual void Put(Stream srcStream, string remoteFile, bool append)
        {            
			// events called before get in case event-handlers want to
			// do any FTP operations
			if (TransferStarted != null)
				TransferStarted(this, new EventArgs());
			if (TransferStartedEx != null)
				TransferStartedEx(this, new TransferEventArgs(srcStream, remoteFile, TransferDirection.UPLOAD, transferType));

			// get according to set type
            if (transferType == FTPTransferType.ASCII)
            {
                PutASCII(srcStream, remoteFile, append);
            }
            else
            {
                PutBinary(srcStream, remoteFile, append);
            }
            ValidateTransfer();

			if (TransferComplete != null)
				TransferComplete(this, new EventArgs());
			if (TransferCompleteEx != null)
				TransferCompleteEx(this, new TransferEventArgs(srcStream, remoteFile, TransferDirection.UPLOAD, transferType));
		}
        
		/// <summary>Validate that the Put() or get() was successful.</summary>
		/// <remarks>This method is not for general use.</remarks>
		public void ValidateTransfer()
        {            
            CheckConnection(true);
            
            // check the control response
            string[] validCodes = new string[]{"225", "226", "250", "426", "450"};
            FTPReply reply = control.ReadReply();
            
            // permit 426/450 error if we cancelled the transfer, otherwise
            // throw an exception
            string code = reply.ReplyCode;
            if ((code.Equals("426") || code.Equals("450")) && !cancelTransfer)
                throw new FTPException(reply);
            
            lastValidReply = control.ValidateReply(reply, validCodes);
        }
        
		/// <summary>Close the data socket</summary>
		private void CloseDataSocket()
        {   
            if (data != null)
            {
                try
                {
                    data.Close();
                    data = null;
                }
                catch (SystemException ex)
                {
                    log.Warn("Caught exception closing data socket", ex);
                }
            }
        }

        /// <summary>
        /// If required, send a server wakeup message
        /// </summary>
        /// <remarks>A NOOP message is sent to the server</remarks>
        /// <param name="start">time the interval started</param>
        /// <returns>if a wakeup was sent, a new interval start time, 
        /// otherwise the one passed in</returns>
        private DateTime SendServerWakeup(DateTime start)
        {
            if (noOperationInterval == 0)
                return start;

            int elapsed = (int)((TimeSpan)(DateTime.Now - start)).TotalSeconds;
            if (elapsed >= noOperationInterval)
            {
                log.Info("Sending server wakeup message");
                control.WriteCommand("NOOP");
                return DateTime.Now;
            }
            return start;
        }

		/// <summary>Request the server to set up the put.</summary>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise.</param>
		private void InitPut(string remoteFile, bool append)
        {    
            CheckConnection(true);
            
            // reset the cancel flag
            cancelTransfer = false;
            
            bool close = false;
            data = null;
            try
            {
                // set up data channel
                data = control.CreateDataSocket(connectMode);
                data.Timeout = timeout;
                
                // if resume is requested, we must obtain the size of the
                // remote file and issue REST
                if (resume)
                {
                    if (transferType.Equals(FTPTransferType.ASCII))
                        throw new FTPException("Resume only supported for BINARY transfers");
                    resumeMarker = Size(remoteFile);
                    Restart(resumeMarker);
                }
                
                // send the command to store
                string cmd = append?"APPE ":"STOR ";
                FTPReply reply = control.SendCommand(cmd + remoteFile);
                
                // Can get a 125 or a 150, also allow 350 (for Global eXchange Services server)
                string[] validCodes = new string[]{"125", "150", "350"};
                lastValidReply = control.ValidateReply(reply, validCodes);
            }
            catch (SystemException)
            {
                close = true;
                throw;
            }
            catch (FTPException)
            {
                close = true;
                throw;
            }
            finally
            {
                if (close)
                {
                    resume = false;
                    CloseDataSocket();
                }
            }
        }
        
        
		/// <summary>
		/// Put as ASCII, i.e. read a line at a time and write
		/// inserting the correct FTP separator.
		/// </summary>
		/// <param name="localPath">Path of the local file.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise</param>
		private void PutASCII(string localPath, string remoteFile, bool append)
        {            
            // create an inputstream & pass to common method
            Stream srcStream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
            PutASCII(srcStream, remoteFile, append);
        }
        
		/// <summary>
		/// Put as ASCII, i.e. read a line at a time and write
		/// inserting the correct FTP separator.
		/// </summary>
		/// <remarks>
		/// The stream is closed after the transfer is complete if
		/// <see cref="CloseStreamsAfterTransfer"/> is <c>true</c> (the default) and are left
		/// open otherwise.
		/// </remarks>
		/// <param name="srcStream">Unput stream of data to put.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise</param>
		private void PutASCII(Stream srcStream, string remoteFile, bool append)
        {
            // need to read line by line ...
            StreamReader input = null;
            StreamWriter output = null;
            SystemException storedEx = null;
            long size = 0;
            try
            {
				input = new StreamReader(srcStream);
                                     
                InitPut(remoteFile, append);
                
                // get an character output stream to write to ... AFTER we
                // have the ok to go ahead AND AFTER we've successfully opened a
                // stream for the local file
                output = new StreamWriter(data.DataStream);
                
                // write \r\n as required by RFC959 after each line
                long monitorCount = 0;
                string line = null;
                DateTime start = DateTime.Now;
                while ((line = input.ReadLine()) != null && !cancelTransfer)
                {
                    size += line.Length;
                    monitorCount += line.Length;
                    output.Write(line);
                    output.Write(FTPControlSocket.EOL);

                    if (BytesTransferred != null && monitorCount > monitorInterval)
                    {
                        BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
                        monitorCount = 0;
                    }
                    start = SendServerWakeup(start);
                }
            }
            catch (SystemException ex)
            {
                storedEx = ex;              
            }
            finally
            {
                try
                {
                    if (closeStreamsAfterTransfer && input != null)
                        input.Close();
                }
                catch (SystemException ex)
                {
                    log.Warn("Caught exception closing stream", ex);
                }
                
                try {
                    if (output != null)
                        output.Close();
                }
                catch (SystemException ex)
                {
                    log.Warn("Caught exception closing data socket", ex);
                }
                CloseDataSocket();
            
                // if we did get an exception bail out now
                if (storedEx != null) {
                    log.Error("Caught exception", storedEx);
                    throw storedEx;
                }
                
                // notify the final transfer size
                if (BytesTransferred != null)
                    BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
            }
        }
        
		/// <summary>Put as binary, i.e. read and write raw bytes.</summary>
		/// <param name="localPath">Path of the local file.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise</param>
		private void PutBinary(string localPath, string remoteFile, bool append)
        {
            
            // open input stream to read source file ... do this
            // BEFORE opening output stream to server, so if file not
            // found, an exception is thrown
            Stream srcStream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
            PutBinary(srcStream, remoteFile, append);
        }
        
		/// <summary>Put as binary, i.e. read and write raw bytes.</summary>
		/// <remarks>
		/// The stream is closed after the transfer is complete if
		/// <see cref="CloseStreamsAfterTransfer"/> is <c>true</c> (the default) and are left
		/// open otherwise.
		/// </remarks>
		/// <param name="srcStream">Input stream of data to put.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise</param>
		private void PutBinary(Stream srcStream, string remoteFile, bool append)
        {    
            BufferedStream input = null;
            BinaryWriter output = null;
            SystemException storedEx = null;
            long size = 0;
            try
            {
				input = new BufferedStream(srcStream);
                
                InitPut(remoteFile, append);
                
                // get an output stream
                output = new BinaryWriter(data.DataStream);
                
                // if resuming, we skip over the unwanted bytes
                if (resume)
                {
                    input.Seek(resumeMarker, SeekOrigin.Current);
                }
                
                byte[] buf = new byte[transferBufferSize];

                // read a chunk at a time and write to the data socket            
                long monitorCount = 0;
                int count = 0;
                DateTime start = DateTime.Now;
                while ((count = input.Read(buf, 0, buf.Length)) > 0 && !cancelTransfer)
                {
                    output.Write(buf, 0, count);
                    size += count;
                    monitorCount += count;
                    if (BytesTransferred != null && monitorCount > monitorInterval)
                    {
                        BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
                        monitorCount = 0;
                    }
                    start = SendServerWakeup(start);
                }
            }
            catch (SystemException ex)
            {
                storedEx = ex;              
            }
            finally
            {
                resume = false;
                try
                {
                    if (closeStreamsAfterTransfer && input != null)
                        input.Close();
                }
                catch (SystemException ex)
                {
                    log.Warn("Caught exception closing stream", ex);
                }
                
                try {
                    if (output != null)
                        output.Close();
                }
                catch (SystemException ex)
                {
                    log.Warn("Caught exception closing data socket", ex);
                }
                CloseDataSocket();
                
                // if we did get an exception bail out now
                if (storedEx != null) {
                    log.Error("Caught exception", storedEx);
                    throw storedEx;
                }
                                
                // notify the final transfer size
                if (BytesTransferred != null) 
                    BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));            

                // log bytes transferred
                log.Debug("Transferred " + size + " bytes to remote host");
            }
        }
        
        
		/// <summary>
		/// Put data onto the FTP server in the current directory.
		/// </summary>
		/// <param name="bytes">Array of bytes to put.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		public virtual void Put(byte[] bytes, string remoteFile)
        {            
            Put(bytes, remoteFile, false);
        }
        
		/// <summary>
		/// Put data onto the FTP server in the current directory. Allows
		/// appending if current file exists.
		/// </summary>
		/// <param name="bytes">Array of bytes to put.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise.</param>
		public virtual void Put(byte[] bytes, string remoteFile, bool append)
        {            
            MemoryStream srcStream = new MemoryStream(bytes);
            Put(srcStream, remoteFile, append);
        }
        
        
		/// <summary>
		/// Get data from the FTP server using the currently
		/// set transfer mode.
		/// </summary>
		/// <param name="localPath">Local file to put data in.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		public virtual void Get(string localPath, string remoteFile)
        {
			// events called before get in case event-handlers want to
			// do any FTP operations
			if (TransferStarted != null)
				TransferStarted(this, new EventArgs());
			if (TransferStartedEx != null)
				TransferStartedEx(this, new TransferEventArgs(localPath, remoteFile, TransferDirection.DOWNLOAD, transferType));

			// get according to set type
            if (transferType == FTPTransferType.ASCII)
            {
                GetASCII(localPath, remoteFile);
            }
            else
            {
                GetBinary(localPath, remoteFile);
            }
            ValidateTransfer();

			if (TransferComplete != null)
				TransferComplete(this, new EventArgs());
			if (TransferCompleteEx != null)
				TransferCompleteEx(this, new TransferEventArgs(localPath, remoteFile, TransferDirection.DOWNLOAD, transferType));
		}
        
		/// <summary>
		/// Get data from the FTP server, using the currently
		/// set transfer mode.
		/// </summary>
		/// <remarks>
		/// The stream is closed after the transfer is complete if
		/// <see cref="CloseStreamsAfterTransfer"/> is <c>true</c> (the default) and are left
		/// open otherwise.
		/// </remarks>
		/// <param name="destStream">Data stream to write data to.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		public virtual void Get(Stream destStream, string remoteFile)
        {            
			// events called before get in case event-handlers want to
			// do any FTP operations
			if (TransferStarted != null)
				TransferStarted(this, new EventArgs());
			if (TransferStartedEx != null)
				TransferStartedEx(this, new TransferEventArgs(destStream, remoteFile, TransferDirection.DOWNLOAD, transferType));

			// get according to set type
            if (transferType == FTPTransferType.ASCII)
            {
                GetASCII(destStream, remoteFile);
            }
            else
            {
                GetBinary(destStream, remoteFile);
            }
            ValidateTransfer();

			if (TransferComplete != null)
				TransferComplete(this, new EventArgs());
			if (TransferCompleteEx != null)
				TransferCompleteEx(this, new TransferEventArgs(destStream, remoteFile, TransferDirection.DOWNLOAD, transferType));
		}
        
		/// <summary>Request to the server that the get is set up.</summary>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		private void InitGet(string remoteFile)
        {
            CheckConnection(true);
            
            // reset the cancel flag
            cancelTransfer = false;
            
            bool close = false;
            data = null;
            try
            {
                // set up data channel
                data = control.CreateDataSocket(connectMode);
                data.Timeout = timeout;
                
                // if resume is requested, we must issue REST
                if (resume)
                {
                    if (transferType.Equals(FTPTransferType.ASCII))
                        throw new FTPException("Resume only supported for BINARY transfers");
                    Restart(resumeMarker);
                }
                
                // send the retrieve command
                FTPReply reply = control.SendCommand("RETR " + remoteFile);
                
                // Can get a 125 or a 150
                string[] validCodes1 = new string[]{"125", "150"};
                lastValidReply = control.ValidateReply(reply, validCodes1);
            }
            catch (SystemException)
            {
                close = true;
                throw;
            }
            catch (FTPException)
            {
                close = true;
                throw;
            }
            finally
            {
                if (close)
                {
                    resume = false;
                    CloseDataSocket();
                }
            }
        }
        
        
		/// <summary>
		/// Get as ASCII, i.e. read a line at a time and write
		/// using the correct newline separator for the OS.
		/// </summary>
		/// <param name="localPath">Local file to put data in.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		private void GetASCII(string localPath, string remoteFile)
        {               
            
            // Need to store the local file name so the file can be
            // deleted if necessary.
            FileInfo localFile = new FileInfo(localPath);

            // check it is writable
            if (localFile.Exists)
            {
                if ((File.GetAttributes(localPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    throw new FTPException(localPath + " is readonly - cannot write");
            }

            // Call InitGet() before creating the FileOutputStream.
            // This will prevent being left with an empty file if a FTPException
            // is thrown by InitGet().
            InitGet(remoteFile);
            
            SystemException storedEx = null;
            long size = 0;

            
            // create the buffered stream for writing
            StreamWriter output = new StreamWriter(localPath);
            
            // get an character input stream to read data from ... AFTER we
            // have the ok to go ahead AND AFTER we've successfully opened a
            // stream for the local file
            StreamReader input = null;
            try 
            {
                input = new StreamReader(data.DataStream);
                
                // If we are in active mode we have to set the timeout of the passive
                // socket. We can achieve this by setting Timeout again.
                // If we are in passive mode then we are merely setting the value twice
                // which does no harm anyway. Doing this simplifies any logic changes.
                data.Timeout = timeout;
          
                // output a new line after each received newline
                long monitorCount = 0;
                string line = null;
                DateTime start = DateTime.Now;
                while ((line = ReadLine(input)) != null && !cancelTransfer)
                {
                    size += line.Length;
                    monitorCount += line.Length;
                    output.WriteLine(line);

                    if (BytesTransferred != null && monitorCount > monitorInterval)
                    {
                        BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));                
                        monitorCount = 0;
                    }
                    start = SendServerWakeup(start);
                }
                // if asked to transfer, abort
                //if (cancelTransfer)
                //    Abort();
            }
            catch (SystemException ex)
            {
                storedEx = ex;              
            }
            
            try 
            {
                output.Close();
            }
            catch (SystemException ex)
            {
                log.Warn("Caught exception closing output stream", ex);
            }
            
            try {
                if (input != null)
                    input.Close();
            }
            catch (SystemException ex)
            {
                log.Warn("Caught exception closing data socket", ex);
            }
            CloseDataSocket();
            
            // if we failed to write the file, rethrow the exception
            if (storedEx != null) 
            {
                // delete the partial file if failure occurred
                if (deleteOnFailure)
                    localFile.Delete();
               
                log.Error("Caught exception", storedEx);
                throw storedEx;
            }
            
            if (BytesTransferred != null)
                BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
        }
        
		/// <summary>
		/// Get as ASCII, i.e. read a line at a time and write
		/// using the correct newline separator for the OS.
		/// </summary>
		/// <remarks>
		/// The stream is closed after the transfer is complete if
		/// <see cref="CloseStreamsAfterTransfer"/> is <c>true</c> (the default) and are left
		/// open otherwise.
		/// </remarks>
		/// <param name="destStream">Data stream to write data to.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		private void GetASCII(Stream destStream, string remoteFile)
        {            
			InitGet(remoteFile);
            
            // create the buffered stream for writing
            StreamWriter output = new StreamWriter(destStream);
                                                   
            // get an character input stream to read data from ... AFTER we
            // have the ok to go ahead
            StreamReader input = null;
            SystemException storedEx = null;
            long size = 0;
            try 
            {
                input = new StreamReader(data.DataStream);
                
                // B. McKeown:
                // If we are in active mode we have to set the timeout of the passive
                // socket. We can achieve this by setting Timeout again.
                // If we are in passive mode then we are merely setting the value twice
                // which does no harm anyway. Doing this simplifies any logic changes.
                data.Timeout = timeout;
                                
                // output a new line after each received newline
                long monitorCount = 0;
                string line = null;
                DateTime start = DateTime.Now;
                while ((line = ReadLine(input)) != null && !cancelTransfer)
                {
                    size += line.Length;
                    monitorCount += line.Length;
                    output.WriteLine(line);

                    if (BytesTransferred != null && monitorCount > monitorInterval)
                    {
                        BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
                        monitorCount = 0;
                    }
                    start = SendServerWakeup(start);
                }
				output.Flush();
                // if asked to transfer, abort
                //if (cancelTransfer)
                   // Abort();
            }
            catch (SystemException ex)
            {
                storedEx = ex;
            }
            
            try {
				if (closeStreamsAfterTransfer)
	                output.Close();
            }
            catch (SystemException ex)
            {
                log.Warn("Caught exception closing output stream", ex);
            }
            
            try {
                if (input != null)
                    input.Close();
            }
            catch (SystemException ex)
            {
                log.Warn("Caught exception closing data socket", ex);
            }
            CloseDataSocket();          
            
            // if we failed to write the file, rethrow the exception
            if (storedEx != null) {
                log.Error("Caught exception", storedEx);
                throw storedEx;
            }
                
            if (BytesTransferred != null)
                BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
        }
        
		/// <summary>Get as binary file, i.e. straight transfer of data.</summary>
		/// <param name="localPath">Local file to put data in.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		private void GetBinary(string localPath, string remoteFile)
        {            
            // B. McKeown: Need to store the local file name so the file can be
            // deleted if necessary.
            FileInfo localFile = new FileInfo(localPath);
            
            // if resuming, we must find the marker
            if (localFile.Exists)
            {
                if ((File.GetAttributes(localPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    throw new FTPException(localPath + " is readonly - cannot write");
                if (resume)
                    resumeMarker = localFile.Length;
            }
   
            // B.McKeown:
            // Call InitGet() before creating the FileOutputStream.
            // This will prevent being left with an empty file if a FTPException
            // is thrown by InitGet().
            InitGet(remoteFile);
            
            // create the output stream for writing the file
            FileMode mode = resume ? FileMode.Append : FileMode.Create;
            BinaryWriter output = new BinaryWriter(new FileStream(localPath, mode));
            
            // get an input stream to read data from ... AFTER we have
            // the ok to go ahead AND AFTER we've successfully opened a
            // stream for the local file
            BinaryReader input = null;
            long size = 0;
            SystemException storedEx = null;
            try 
            {
                input = new BinaryReader(data.DataStream);
                
                // B. McKeown:
                // If we are in active mode we have to set the timeout of the passive
                // socket. We can achieve this by calling setTimeout() again.
                // If we are in passive mode then we are merely setting the value twice
                // which does no harm anyway. Doing this simplifies any logic changes.
                data.Timeout = timeout;
                
                // do the retrieving
                long monitorCount = 0;
                byte[] chunk = new byte[transferBufferSize];
                int count;
                DateTime start = DateTime.Now;
                
                // read from socket & write to file in chunks
                while ((count = ReadChunk(input, chunk, transferBufferSize)) > 0 && !cancelTransfer)
                {
                    output.Write(chunk, 0, count);
                    size += count;
                    monitorCount += count;
                    
                    if (BytesTransferred != null && monitorCount > monitorInterval)
                    {
                        BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
                        monitorCount = 0;
                    }
                    start = SendServerWakeup(start);
                }
                // if asked to transfer, abort
                //if (cancelTransfer)
                   // Abort();               
            }
            catch (SystemException ex)
            {
                storedEx = ex;
            }

            resume = false;
            
            try {
                output.Close();
            }
            catch (SystemException ex)
            {
                log.Warn("Caught exception closing output stream", ex);
            }            
            
            try {
                if (input != null)
                    input.Close();
            }
            catch (SystemException ex)
            {
                log.Warn("Caught exception closing data socket", ex);
            }
            CloseDataSocket();          
            
            // if we failed to write the file, rethrow the exception
            if (storedEx != null) 
            {
                // delete the partial file if failure occurred
                if (deleteOnFailure)
                   localFile.Delete();
                log.Error("Caught exception", storedEx);
                throw storedEx;
            }
            
            if (BytesTransferred != null)
                BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));

            // log bytes transferred
            log.Debug("Transferred " + size + " bytes from remote host");
        }
        
		/// <summary>Get as binary file, i.e. straight transfer of data.</summary>
		/// <remarks>
		/// The stream is closed after the transfer is complete if
		/// <see cref="CloseStreamsAfterTransfer"/> is <c>true</c> (the default) and are left
		/// open otherwise.
		/// </remarks>
		/// <param name="destStream">Stream to write to.</param>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		private void GetBinary(Stream destStream, string remoteFile)
        {            
			InitGet(remoteFile);
            
            // create the buffered output stream for writing the file
            BufferedStream output = new BufferedStream(destStream);
            
            // get an input stream to read data from ... AFTER we have
            // the ok to go ahead AND AFTER we've successfully opened a
            // stream for the local file
            BinaryReader input = null;
            long size = 0;
            SystemException storedEx = null;
            try 
            {
                input = new BinaryReader(data.DataStream);
    
                // B. McKeown:
                // If we are in active mode we have to set the timeout of the passive
                // socket. We can achieve this by calling setTimeout() again.
                // If we are in passive mode then we are merely setting the value twice
                // which does no harm anyway. Doing this simplifies any logic changes.
                data.Timeout = timeout;
                
                // do the retrieving
                long monitorCount = 0;
                byte[] chunk = new byte[transferBufferSize];
                int count;
                DateTime start = DateTime.Now;
                
                // read from socket & write to file in chunks
                while ((count = ReadChunk(input, chunk, transferBufferSize)) > 0 && !cancelTransfer)
                {
                    output.Write(chunk, 0, count);
                    size += count;
                    monitorCount += count;
                    
                    if (BytesTransferred != null && monitorCount > monitorInterval)
                    {
                        BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
                        monitorCount = 0;
                    }
                    start = SendServerWakeup(start);
                }
                output.Flush();
                // if asked to transfer, abort
                //if (cancelTransfer)
                //    Abort();
            }
            catch (SystemException ex)
            {
                storedEx = ex;
            }
            
            try {
				if (closeStreamsAfterTransfer)
	                output.Close();
            }
            catch (SystemException ex)
            {
                log.Warn("Caught exception closing output stream", ex);
            }
            
            try {
                if (input != null)
                    input.Close();
            }
            catch (SystemException ex)
            {
                log.Warn("Caught exception closing data socket", ex);
            }
            CloseDataSocket();          
            
            // if we failed to write to the stream, rethrow the exception
            if (storedEx != null) {
                log.Error("Caught exception", storedEx);
                throw storedEx;
            }
             
            if (BytesTransferred != null)
                BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
            
            // log bytes transferred
            log.Debug("Transferred " + size + " bytes from remote host");
        }
        
		/// <summary>Get data from the FTP server.</summary>
		/// <remarks>
		/// Transfers in whatever mode we are in. Retrieve as a byte array. Note
		/// that we may experience memory limitations as the
		/// entire file must be held in memory at one time.
		/// </remarks>
		/// <param name="remoteFile">Name of remote file in current directory.</param>
		public virtual byte[] Get(string remoteFile)
        {            
			// events called before get in case event-handlers want to
			// do any FTP operations
			if (TransferStarted != null)
				TransferStarted(this, new EventArgs());  
			if (TransferStartedEx != null)
				TransferStartedEx(this, new TransferEventArgs(new byte[0], remoteFile, TransferDirection.DOWNLOAD, transferType));

			InitGet(remoteFile);
            
            // get an input stream to read data from
            BinaryReader input = null;
            long size = 0;
            SystemException storedEx = null;
            MemoryStream memStr = null;
            try
            {
                input = new BinaryReader(data.DataStream);
                
                // B. McKeown:
                // If we are in active mode we have to set the timeout of the passive
                // socket. We can achieve this by calling setTimeout() again.
                // If we are in passive mode then we are merely setting the value twice
                // which does no harm anyway. Doing this simplifies any logic changes.
                data.Timeout = timeout;
                
                // do the retrieving
                long monitorCount = 0;
                byte[] chunk = new byte[transferBufferSize]; // read chunks into
                memStr = new MemoryStream(transferBufferSize); // temp swap buffer
                int count; // size of chunk read
                DateTime start = DateTime.Now;
                
                // read from socket & write to file
                while ((count = ReadChunk(input, chunk, transferBufferSize)) > 0 && !cancelTransfer)
                {
                    memStr.Write(chunk, 0, count);
                    size += count;
                    monitorCount += count;
                    
                    if (BytesTransferred != null && monitorCount > monitorInterval)
                    {
                        BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
                        monitorCount = 0;
                    }
                    start = SendServerWakeup(start);    
                }
                // if asked to transfer, abort
                //if (cancelTransfer)
                   // Abort();
            }
            catch (SystemException ex)
            {
                storedEx = ex;
            }
        
            try 
            {
                if (memStr != null)
                    memStr.Close();
            }
            catch (SystemException ex)
            {
                log.Warn("Caught exception closing stream", ex);
            }            
            
            try {
                if (input != null)
                    input.Close();
            }
            catch (SystemException ex)
            {
                log.Warn("Caught exception closing data socket", ex);
            }
            CloseDataSocket(); 
            
             // if we failed to write to the stream, rethrow the exception
            if (storedEx != null) {
                log.Error("Caught exception", storedEx);
                throw storedEx;
            }
            
            // notify final transfer size
            if (BytesTransferred != null)
                BytesTransferred(this, new BytesTransferredEventArgs(remoteFile, size));
            
            ValidateTransfer();
		
			byte[] buffer = memStr == null ? null : memStr.ToArray();

			if (TransferComplete != null)
				TransferComplete(this, new EventArgs());
			if (TransferCompleteEx != null)
				TransferCompleteEx(this, new TransferEventArgs(buffer, remoteFile, TransferDirection.UPLOAD, transferType));
            
            return buffer;
        }
        
        
		/// <summary>Run a site-specific command on the server.</summary>
		/// <remarks>
		/// Support for commands is dependent on the server.
		/// </remarks>
		/// <param name="command">The site command to run</param>
		/// <returns><c>true</c> if command ok, <c>false</c> if command not implemented.</returns>
		public virtual bool Site(string command)
        {            
            CheckConnection(true);
            
            // send the retrieve command
            FTPReply reply = control.SendCommand("SITE " + command);
            
            // Can get a 200 (ok) or 202 (not impl). Some
            // FTP servers return 502 (not impl)
            string[] validCodes = new string[]{"200", "202", "502"};
            lastValidReply = control.ValidateReply(reply, validCodes);
            
            // return true or false? 200 is ok, 202/502 not
            // implemented
            if (reply.ReplyCode.Equals("200"))
                return true;
            else
                return false;
        }
        
		/// <summary>
		/// List the current directory's contents as an array of FTPFile objects.
		/// </summary>
		/// <remarks>
		/// This works for Windows and most Unix FTP servers.  Please inform EDT
		/// about unusual formats (support@enterprisedt.com).
		/// </remarks>
		/// <returns>An array of <see cref="FTPFile"/> objects.</returns>
		public virtual FTPFile[] DirDetails()
        {
            return DirDetails(null);
        }        
        
		/// <summary>
		/// List a directory's contents as an array of FTPFile objects.
		/// </summary>
		/// <remarks>
		/// This works for Windows and most Unix FTP servers.  Please inform EDT
		/// about unusual formats (support@enterprisedt.com). Note that for some
		/// servers, this will not work from the parent directory of dirname. You
		/// need to ChDir() into dirname and use DirDetails() (with no arguments).
		/// </remarks>
		/// <param name="dirname">Name of directory OR filemask (if supported by the server).</param>
		/// <returns>An array of <see cref="FTPFile"/> objects.</returns>
		public virtual FTPFile[] DirDetails(string dirname)
        {
            // create the factory
            if (fileFactory == null)
                fileFactory = new FTPFileFactory(GetSystem());
            
            // get the details and parse
            return fileFactory.Parse(Dir(dirname, true));
        }
        
		/// <summary>
		/// List current directory's contents as an array of strings of
		/// filenames.
		/// </summary>
		/// <returns>An array of current directory listing strings.</returns>
		public virtual string[] Dir()
        {            
            return Dir(null, false);
        } 
        
		/// <summary>
		/// List a directory's contents as an array of strings of filenames.
		/// </summary>
		/// <param name="dirname">Name of directory OR filemask.</param>
		/// <returns>An array of directory listing strings.</returns>
		public virtual string[] Dir(string dirname)
        {            
            return Dir(dirname, false);
        }

		/// <summary>
		/// List a directory's contents as an array of strings.
		/// </summary>
		/// <remarks>
		/// If <c>full</c> is <c>true</c> then a detailed
		/// listing if returned (if available), otherwise just filenames are provided.
		/// The detailed listing varies in details depending on OS and
		/// FTP server. Note that a full listing can be used on a file
		/// name to obtain information about a file
		/// </remarks> 
		/// <param name="dirname">Name of directory OR filemask.</param>
		/// <param name="full"><c>true</c> if detailed listing required, <c>false</c> otherwise.</param>
		/// <returns>An array of directory listing strings.</returns>
		public virtual string[] Dir(string dirname, bool full)
        {
            CheckConnection(true);
            
            try
            {
                // set up data channel
                data = control.CreateDataSocket(connectMode);
                data.Timeout = timeout;
                
                // send the retrieve command
                string command = full?"LIST ":"NLST ";
                if (dirname != null)
                    command += dirname;
                
                // some FTP servers bomb out if NLST has whitespace appended
                command = command.Trim();
                FTPReply reply = control.SendCommand(command);
                
                // check the control response. wu-ftp returns 550 if the
                // directory is empty, so we handle 550 appropriately. Similarly
                // proFTPD returns 450. If dir is empty, some servers return 226 Transfer complete
                string[] validCodes1 = new string[]{"125", "226", "150", "450", "550"};
                lastValidReply = control.ValidateReply(reply, validCodes1);
                
                // an empty array of files for 450/550
                string[] result = new string[0];
                
                // a normal reply ... extract the file list
                string replyCode = lastValidReply.ReplyCode;
                if (!replyCode.Equals("450") && !replyCode.Equals("550") && !replyCode.Equals("226"))
                {
                    // get a character input stream to read data from .
                    StreamReader input = new StreamReader(data.DataStream);
                    
                    // read a line at a time
                    ArrayList lines = new ArrayList(10);
                    string line = null;
                    try
                    {
                        while ((line = ReadLine(input)) != null)
                        {
                            lines.Add(line);
                            log.Debug("-->" + line);
                        }
                    }
                    finally
                    {
                        try {
                            input.Close();
                        }
                        catch (SystemException ex)
                        {
                            log.Warn("Caught exception closing data socket", ex);
                        }
                        CloseDataSocket(); // need to close here
                    }
                    
                    // check the control response
                    string[] validCodes2 = new string[]{"226", "250"};
                    reply = control.ReadReply();
                    lastValidReply = control.ValidateReply(reply, validCodes2);
                    
                    // empty array is default
                    if (!(lines.Count == 0))
                    {
                        log.Debug("Found " + lines.Count + " listing lines");
                        result = new string[lines.Count];
                        lines.CopyTo(result);
                    }
                    else
                        log.Debug("No listing data found");
                }
                else { // throw exception if not a "no files" message or transfer complete
					string replyText = lastValidReply.ReplyText.ToUpper();
                    if (!NoFilesMessage(replyText) && 
						replyText.IndexOf(TRANSFER_COMPLETE) < 0)
                        throw new FTPException(reply);
                }
                return result;
            }
            finally
            {
                CloseDataSocket();
            }
        }

        /// <summary>
        /// Check to see if this message indicates no files found in listing
        /// </summary>
        /// <param name="msg">FTP error message to check</param>
        /// <returns>true if a no files message, false otherwise</returns>
        private bool NoFilesMessage(string msg) 
        {
            for (int i = 0; i < EMPTY_DIR_MSGS.Length; i++)
            {
                if (msg.ToUpper().IndexOf(EMPTY_DIR_MSGS[i]) >= 0)
                    return true;
            }
            return false;
        }
        
		/// <summary>
		/// Attempts to read a specified number of bytes from the given 
		/// <code>BufferedStream</code> and place it in the given byte-array.
		/// </summary>
		/// <remarks>
		/// The purpose of this method is to permit subclasses to execute
		/// any additional code necessary when performing this operation. 
		/// </remarks>
		/// <param name="input">The <code>BinaryReader</code> to read from.</param>
		/// <param name="chunk">The byte-array to place read bytes in.</param>
		/// <param name="chunksize">Number of bytes to read.</param>
		/// <returns>Number of bytes actually read.</returns>
		/// <throws>SystemException Thrown if there was an error while reading. </throws>
		internal virtual int ReadChunk(BinaryReader input, byte[] chunk, int chunksize)
        {            
            return input.Read(chunk, 0, chunksize);
        }
        
		/// <summary>Attempts to read a single character from the given <code>StreamReader</code>.</summary>
		/// <remarks>
		/// The purpose of this method is to permit subclasses to execute
		/// any additional code necessary when performing this operation. 
		/// </remarks>
		/// <param name="input">The <code>StreamReader</code> to read from.</param>
		/// <returns>The character read.</returns>
		/// <throws>SystemException Thrown if there was an error while reading. </throws>
		internal virtual int ReadChar(StreamReader input)
        {
            return input.Read();
        }
        
		/// <summary>
		/// Attempts to read a single line from the given <code>StreamReader</code>. 
		/// </summary>
		/// <remarks>
		/// The purpose of this method is to permit subclasses to execute
		/// any additional code necessary when performing this operation. 
		/// </remarks>
		/// <param name="input">The <code>StreamReader</code> to read from.</param>
		/// <returns>The string read.</returns>
		/// <throws>SystemException Thrown if there was an error while reading. </throws>
		internal virtual string ReadLine(StreamReader input)
        {
            return input.ReadLine();
        }
                
		/// <summary>Delete the specified remote file.</summary>
		/// <param name="remoteFile">Name of remote file to delete.</param>
		public virtual void Delete(string remoteFile)
        {            
            CheckConnection(true);
            string[] validCodes = new string[]{"200", "250"};
            FTPReply reply = control.SendCommand("DELE " + remoteFile);
            lastValidReply = control.ValidateReply(reply, validCodes);
        }
        
        
		/// <summary>Rename a file or directory.</summary>
		/// <param name="from">Name of file or directory to rename.</param>
		/// <param name="to">Intended name.</param>
		public virtual void Rename(string from, string to)
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("RNFR " + from);
            lastValidReply = control.ValidateReply(reply, "350");
            
            reply = control.SendCommand("RNTO " + to);
            lastValidReply = control.ValidateReply(reply, "250");
        }
        
        
		/// <summary>Delete the specified remote working directory.</summary>
		/// <param name="dir">Name of remote directory to delete.</param>
		public virtual void RmDir(string dir)
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("RMD " + dir);
            
            // some servers return 200,257, technically incorrect but
            // we cater for it ...
            string[] validCodes = new string[]{"200", "250", "257"};
            lastValidReply = control.ValidateReply(reply, validCodes);
        }
        
        
		/// <summary>Create the specified remote working directory.</summary>
		/// <param name="dir">Name of remote directory to create.</param>
		public virtual void MkDir(string dir)
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("MKD " + dir);
            
            // some servers return 200,257, technically incorrect but
            // we cater for it ...
            string[] validCodes = new string[]{"200", "250", "257"};
            lastValidReply = control.ValidateReply(reply, validCodes);
        }
        
        
		/// <summary>Change the remote working directory to that supplied.</summary>
		/// <param name="dir">Name of remote directory to change to.</param>
		public virtual void ChDir(string dir)
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("CWD " + dir);
            lastValidReply = control.ValidateReply(reply, "250");
        }

        
		/// <summary>Change the remote working directory to the parent directory.</summary>
		public virtual void CdUp()
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("CDUP");
            string[] validCodes = new string[]{"200", "250"};
            lastValidReply = control.ValidateReply(reply, validCodes);
        }
        
		/// <summary>Get modification time for a remote file.</summary>
		/// <param name="remoteFile">Name of remote file.</param>
		/// <returns>Modification time of file as a <c>DateTime</c>.</returns>
		public virtual DateTime ModTime(string remoteFile)
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("MDTM " + remoteFile);
            lastValidReply = control.ValidateReply(reply, "213");
            
            // parse the reply string, which returns UTC
            DateTime ts = 
                DateTime.ParseExact(lastValidReply.ReplyText, tsFormat, null);
            
            // return the equivalent in local time
            return TimeZone.CurrentTimeZone.ToLocalTime(ts);
        }
        
		/// <summary>Get the current remote working directory.</summary>
		/// <returns>The current working directory.</returns>
		public virtual string Pwd()
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("PWD");
            lastValidReply = control.ValidateReply(reply, "257");
            
            // get the reply text and extract the dir
            // listed in quotes, if we can find it. Otherwise
            // just return the whole reply string
            string text = lastValidReply.ReplyText;
            int start = text.IndexOf((System.Char) '"');
            int end = text.LastIndexOf((System.Char) '"');
            if (start >= 0 && end > start)
                return text.Substring(start + 1, (end) - (start + 1));
            else
                return text;
        }
        
        
		/// <summary>Get the server supplied features.</summary>
		/// <returns>
		/// <c>string</c>-array containing server features, or <c>null</c> if no features or not supported.
		/// </returns>
		public virtual string[] Features()
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("FEAT");
            string[] validCodes = new string[]{"211", "500", "502"};
            lastValidReply = control.ValidateReply(reply, validCodes);
            if (lastValidReply.ReplyCode.Equals("211"))
                return lastValidReply.ReplyData;
            else
                throw new FTPException(reply);
        }
        
		/// <summary>Get the type of the OS at the server.</summary>
		/// <returns>The type of server OS.</returns>
		public virtual string GetSystem()
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("SYST");
            string[] validCodes = new string[]{"200", "213", "215"};
            lastValidReply = control.ValidateReply(reply, validCodes);
            return lastValidReply.ReplyText;
        }

        /// <summary>  
        /// Send a "no operation" message that does nothing, which can
        /// be called periodically to prevent the connection timing out.
        /// </summary>
        public void NoOperation()
        {
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("NOOP");
            lastValidReply = control.ValidateReply(reply, "200");
        }
          
        /// <summary>  Get the help text for the specified command
        /// 
        /// </summary>
        /// <param name="command"> name of the command to get help on
        /// </param>
        /// <returns> help text from the server for the supplied command
        /// </returns>
        public virtual string Help(string command)
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("HELP " + command);
            string[] validCodes = new string[]{"211", "214"};
            lastValidReply = control.ValidateReply(reply, validCodes);
            return lastValidReply.ReplyText;
        }
        
		/// <summary>Abort the current action.</summary>
		/// <remarks>
		/// This does not close the FTP session.
		/// </remarks>
		protected virtual void Abort()
        {            
            CheckConnection(true);
            
            FTPReply reply = control.SendCommand("ABOR");
            string[] validCodes = new string[]{"426", "226"};
            lastValidReply = control.ValidateReply(reply, validCodes);
        }
        
		/// <summary>Quit the FTP session by sending a <c>QUIT</c> command before closing the socket.</summary>
		public virtual void Quit()
        {            
            CheckConnection(true);
            
            fileFactory = null;
            try
            {
                FTPReply reply = control.SendCommand("QUIT");
                string[] validCodes = new string[]{"221", "226"};
                lastValidReply = control.ValidateReply(reply, validCodes);
            }
            finally
            {
                // ensure we clean up the connection
                control.Logout();
                control = null;
            }
        }
        
		/// <summary>
		/// Quit the FTP session immediately by closing the control socket
		/// without sending the <c>QUIT</c> command.
		/// </summary>
		public virtual void QuitImmediately() 
        {
            CheckConnection(true);
            
            fileFactory = null;
            
            control.Logout();
            control = null;
        }
        
        
        /// <summary>Work out the version array.</summary>
        static FTPClient()
        {
            try
            {
                version = new int[3];
                version[0] = Int32.Parse(majorVersion);
                version[1] = Int32.Parse(middleVersion);
                version[2] = Int32.Parse(minorVersion);
            }
            catch (FormatException ex)
            {
                System.Console.Error.WriteLine("Failed to calculate version: " + ex.Message);
            }
        }
    }
}
