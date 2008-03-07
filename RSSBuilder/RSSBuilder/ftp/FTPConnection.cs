// 
// Copyright (C) 2004 Enterprise Distributed Technologies Ltd
// 
// www.enterprisedt.com

#region Change Log

// Change Log:
// 
// $Log: FTPConnection.cs,v $
// Revision 1.17  2006/06/16 12:11:20  bruceb
// fix autologin/transfer mode bug
//
// Revision 1.16  2006/06/14 10:36:31  hans
// Introduced reference to IFileTransferClient to be used for all operations relating to all file-transfer types.
// Added Protocol property
// Changed property categories
// Changed all <p> tags to <para>
// .NET 2.0 compatibility
// Added FilePathEncoding property which control encoding of control channel
// Added CheckConnection method
//
// Revision 1.15  2006/05/25 05:42:30  hans
// Fixed BytesTransferred event.
//
// Revision 1.14  2006/04/18 07:20:29  hans
// - Changed ActiveIPAddress to PublicIPAddress and its type to string so that it can be set in the form designer.
// - Got rid of duplicate copy of ActivePortRange
//
// Revision 1.13  2006/03/16 22:37:02  hans
// Added PublicIPAddress, ActivePortRange and CloseStreamsAfterTransfer properties.
// Added LogFile, LogToConsole, LogToTrace and LogLevel properties.
// Fixed bug which caused byte-array uploads to be abandoned if they were not cancelled in the Uploading event-handler (!?).
// Fixed bug which caused too many operations to be performed on the GUI thread.
// Fixed bug which caused file-size to be reported as zero in the Downloaded event, if the Downloading event was not being handled also.
//
// Revision 1.12  2006/03/16 20:48:10  bruceb
// added ActivePortRange and ActiveIPAddress
//
// Revision 1.11  2006/02/16 22:09:39  hans
// Added comments
//
// Revision 1.10  2006/02/09 10:34:51  hans
// Improved delegate invocation implementation and fixed up a lot of comments.
//
// Revision 1.9  2005/12/13 19:54:37  hans
// Added DefaultValue attributes for properties and added Renaming and Renamed events
//
// Revision 1.8  2005/10/13 20:50:07  hans
// Fixed up download and upload events.
// Added ReplyReceived event.
//
// Revision 1.7  2005/10/11 22:06:41  hans
// Fixed DownloadFile event bug and improved OnDownloading methods.
// Also removed detect thingie which was a hangover from Express.
//
// Revision 1.6  2005/09/30 17:24:48  bruceb
// remove multiple method stuff
//
// Revision 1.5  2005/09/30 06:25:23  hans
// Moved from Express directory.
//
// Revision 1.2  2005/09/13 21:56:54  hans
// test version
//
// Revision 1.3  2005/08/23 21:24:09  hans
// Beta 2 of Express
//
// Revision 1.2  2005/08/05 07:02:14  hans
// Asynchronous transfers partially developed.
//
// Revision 1.1  2005/07/22 10:44:21  hans
// First version
//

#endregion

#region Using Declarations

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using EnterpriseDT.Util.Debug;

#endregion

namespace EnterpriseDT.Net.Ftp
{
	/// <summary>Provides FTP client functionality.</summary>
	/// <remarks>
	/// <para>
	/// <c>FTPConnection</c> provides FTP client functionality.  It is a .NET Component which
	/// may be used in visual designers, or it may be used as a conventional class.
	/// </para>
	/// <para>
	/// <b>Constructing and connecting: </b>
	/// There are two approaches to constructing an <c>FTPClient</c>.  In one, the domain-name
	/// of the FTP server is provided in the constructor and the FTP client
	/// connects automatically.  In the other, no arguments are provided and the FTP client does
	/// not connect until the <see cref="Connect()"/> method is called.
	/// </para>
	/// <para>
	///	<b>Logging in: </b>
	/// Logging in may be done in two ways: 
	/// (1) by setting the properties <see cref="UserName"/> and <see cref="Password"/>, 
	/// and then calling the parameterless <see cref="Login()"/> method;
	/// (2) by first calling <see cref="SendUserName(string)"/> and then calling <see cref="SendPassword(string)"/>.
	/// </para>
	/// <para>
	///	<b>Directory listings: </b>
	/// Directory listings may be obtained in two basic forms: 
	/// (1) The method <see cref="GetFiles(string)"/> and its siblings return string containing the file-name 
	/// only or, if the <c>full</c> flag of some of the methods is set, the raw listing string, which
	/// differs from one server to the other.
	/// (2) The method <see cref="GetFileInfos(string)"/> return <see cref="FTPFile"/> objects which
	/// contain information about the file including name, size, and date.
	/// </para>
	/// <para>
	///	<b>Changing working directory: </b>
	/// The server maintains a "working directory" for each session.  The path of the current
	/// working directory may be set and retrieved using the <see cref="GetWorkingDirectory()"/> method.
	/// Changing directory to a subdirectory of the current working directory may be done using
	/// the <see cref="ChangeWorkingDirectory(string)"/> method.  Changing up to a parent directory is done
	/// using the <see cref="ChangeWorkingDirectoryUp()"/> method.
	/// </para>
	/// <para>
	///	<b>Getting and putting files: </b>
	/// There are many different methods for getting (downloading) files from the server 
	/// and putting (uploading) them to the server.  Data may be source from or saved to:
	/// (1) files (<see cref="DownloadFile(string,string)"/> and <see cref="UploadFile(string,string)"/>; 
	/// (2) streams (<see cref="DownloadStream(Stream,string)"/> and <see cref="UploadStream(Stream,string)"/>); and
	/// (3) byte-arrays (<see cref="DownloadByteArray(string)"/> and <see cref="UploadByteArray(byte[],string)"/>.
	/// </para>
	/// <para>
	/// <b>Overview of the FTP protocol: </b> FTP is defined in the Request For Comments 959 document (RFC 959), 
	/// which can be obtained from the Internet Engineering Task Force.
	/// </para>
	/// <para>
	/// FTP requires a client program (FTP client) and a server program (FTP server). The FTP client 
	/// can fetch files and file details from the server, and also upload files to the server. 
	/// The server is generally loginPassword protected.
	/// </para>
	/// <para>
	/// FTP commands are initiated by the FTP client, which opens a TCP connection called the control 
	/// connection to the server. This control connection is used for the entire duration of a 
	/// session between the FTP client and server. A session typically begins when the FTP client logs in, 
	/// and ends when the quit command is sent to the server. The control connection is used 
	/// exclusively for sending FTP commands and reading server replies - it is never used to 
	/// transfer files.
	/// </para>
	/// <para>
	/// Transient TCP connections called data connections are set up whenever data (normally a 
	/// file's contents) is to be transferred. For example, the FTP client issues a command to 
	/// retrieve a file from the server via the control channel. A data connection is then 
	/// established, and the file's contents transferred to the FTP client across it. Once the 
	/// transfer is complete, the data connection is closed. Meanwhile, the control connection 
	/// is maintained.
	/// </para>
	/// <para>
	/// <b>Compliance: </b> <see cref="FTPClient"/> implements
	/// FTP as defined by RFC959.  It attempts to match the standard as closely as possible, 
	/// but due to variation in the level of compliance of the numerous FTP servers available,
	/// it sometime allows servers some tolerance.  If the property <see cref="StrictReturnCodes"/>
	/// is set to <c>false</c> then <see cref="FTPClient"/> is more tolerant of non-compliant servers.
	/// </para>
	/// </remarks>
	/// <author>Hans Andersen</author>
	/// <version>$Revision: 1.17 $</version>
	public class FTPConnection : System.ComponentModel.Component
	{
		#region Fields

		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
        
		/// <summary> Logging object</summary>
		private Logger log = Logger.GetLogger(typeof(FTPConnection));

        /// <summary>Reference to <c>IFileTransferClient</c>.</summary>
        private IFileTransferClient activeClient;

		/// <summary>Instance of <c>FTPClient</c>.</summary>
        protected FTPClient ftpClient;

		/// <summary>User-name to log in with.</summary>
		protected string loginUserName;

		/// <summary>Password to log in with.</summary>
		protected string loginPassword;
                        
		/// <summary>Record of the transfer type - make the default ASCII.</summary>
		protected FTPTransferType fileTransferType;

		/// <summary>Determines if the components will automatically log in upon connection.</summary>
		protected bool useAutoLogin = true;

		/// <summary>Determines if events will be fired.</summary>
		protected bool areEventsEnabled = true;

		/// <summary>Determines if events will be fired.</summary>
		protected bool isTransferringData = false;

		/// <summary>Reference to the main window.</summary>
		/// <remarks>
		/// This reference is used for invoking delegates such that they can perform GUI-related actions.
		/// </remarks>
		protected Control guiControl = null;

		/// <summary>Flag used to remember whether or not we've tried to find the main window yet.</summary>
		protected bool haveQueriedForControl = false;

		/// <summary>Size of file currently being transferred.</summary>
		protected long currentFileSize = -1;

		/// <summary>Flag indicating whether or not we're doing a multiple file transfer.</summary>
		protected bool isTransferringMultiple = false;

		/// <summary>
		/// Flag indicating whether or not event-handlers will run on the GUI thread if one is
		/// available.
		/// </summary>
		protected bool useGuiThread = true;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of FTPConnection within the given container.
		/// </summary>
		/// <param name="container">Container to place the new instance in.</param>
		public FTPConnection(System.ComponentModel.IContainer container)
			: this()
		{
			container.Add(this);
		}

		/// <summary>
		/// Default constructor for FTPConnection.
		/// </summary>
		public FTPConnection()
			: this(new FTPClient())
		{
			components = new System.ComponentModel.Container();
		}

		/// <summary>
		/// Create an FTPConnection using the given FTP client.
		/// </summary>
        /// <param name="ftpClient"><see cref="FTPClient"/>-instance to use.</param>
		protected internal FTPConnection(FTPClient ftpClient)
		{
            this.activeClient = ftpClient;
            this.ftpClient = ftpClient;
            this.ftpClient.AutoPassiveIPSubstitution = true;
            this.ftpClient.TransferStartedEx += new TransferHandler(ftpClient_TransferStartedEx);
            this.ftpClient.TransferCompleteEx += new TransferHandler(ftpClient_TransferCompleteEx);
            this.ftpClient.BytesTransferred += new BytesTransferredHandler(ftpClient_BytesTransferred);
            fileTransferType = FTPTransferType.BINARY;
        }

		#endregion

		#region Finalization

		/// <summary>Disconnect from the server (if connected).</summary>
		protected override void Dispose( bool disposing )
		{
			if (disposing)
			{
				if (components!=null)
					components.Dispose();
				if (IsConnected)
					Close(true);
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Properties

		internal Control ParentControl
		{
			get
			{
				return guiControl;
			}
			set
			{
				guiControl = value;
			}
		}
        /// <summary>
        /// Type of secure FTP to use.
        /// </summary>
        /// <remarks>
        /// edtFTPnet/PRO offers three types of FTP:
        /// <list type="bullet">
        /// <listheader><term>Type</term></listheader>
        /// <item>
        /// <term>FTP</term>
        /// <description>Traditional unencrypted FTP.</description>
        /// </item>
        /// <item>
        /// <term>FTPSExplicit</term>
        /// <description>FTP-over-SSL which allows switching between secure and unsecure transfers.</description>
        /// </item>
        /// <item>
        /// <term>FTPSImplicit</term>
        /// <description>FTP-over-SSL which simply performs FTP over pure SSL sockets.</description>
        /// </item>
        /// <item>
        /// <term>SFTP</term>
        /// <description>SSH File Transfer Protocol.</description>
        /// </item>
        /// </list>
        /// </remarks>
        [Category("Connection")]
        [Description("File transfer protocol to use.")]
        [DefaultValue(FileTransferProtocol.FTP)]
        public virtual FileTransferProtocol Protocol
        {
            get
            {
                return FileTransferProtocol.FTP;
            }
            set
            {
                CheckConnection(false);

                if (value != FileTransferProtocol.FTP)
                    throw new FTPException("FTPConnection only supports standard FTP.  "
                        + value + " is supported in SecureFTPConnection.\n"
                        + "SecureFTPConnection is available in edtFTPnet/PRO (www.enterprisedt.com/products/edtftpnetpro).");
            }
        }

		/// <summary>The version of edtFTPj.</summary>
		/// <value>An <c>int</c> array of <c>{major,middle,minor}</c> version numbers.</value>
		[Category("Version")]
		[Description("The edtFTPj build timestamp.")]
		public string Version
		{
			get
			{
				int[] v = FTPClient.Version;
				return v[0] + "." + v[1] + "." + v[2];
			}
            
		}
		/// <summary>The edtFTPj build timestamp.</summary>
		/// <value>
		/// Timestamp of when edtFTPj was build in the format <c>d-MMM-yyyy HH:mm:ss z</c>.
		/// </value>
        [Category("Version")]
		[Description("The edtFTPj build timestamp.")]
		public string BuildTimestamp
		{
			get
			{
				return FTPClient.BuildTimestamp;
			}            
		}

		/// <summary>Controls whether or not checking of return codes is strict.</summary>
		/// <remarks>
		/// <para>
		/// Some servers return non-standard reply-codes.  When this property is <c>false</c>
		/// only the first digit of the reply-code is checked, thus decreasing the sensitivity
		/// of edtFTPj to non-standard reply-codes.  The default is <c>true</c> meaning that
		/// reply-codes must match exactly.
		/// </para>
		/// </remarks>
		/// <value>  
		/// <c>true</c> if strict return code checking, <c>false</c> if non-strict.
		/// </value>
		[Category("FTP/FTPS Settings")]
		[Description("Controls whether or not checking of return codes is strict.")]
		[DefaultValue(true)]
		public bool StrictReturnCodes
		{
			get
			{
                return ftpClient.StrictReturnCodes;
			}
            
			set
			{
                ftpClient.StrictReturnCodes = value;
			}
            
		}
		        
		/// <summary>
		/// IP address of the client as the server sees it.
		/// </summary>
		/// <remarks>
		/// This property is necessary when using active mode in situations where the
		/// FTP client is behind a firewall.
		/// </remarks>
        [Category("FTP/FTPS Settings")]
		[Description("IP address of the client as the server sees it.")]
		[DefaultValue(null)]
		public string PublicIPAddress
		{
			get
			{
                return ftpClient.ActiveIPAddress != null ? ftpClient.ActiveIPAddress.ToString() : "";
			}
			set
			{
				try
				{
                    ftpClient.ActiveIPAddress = IPAddress.Parse(value);
				}
				catch (FormatException)
				{
                    ftpClient.ActiveIPAddress = HostNameResolver.GetAddress(value);
				}
			}
		}
                
		/// <summary>
		/// Specifies the range of ports to be used for data-channels in active mode.
		/// </summary>
		/// <remarks>
		/// <para>By default, the operating system selects the ports to be used for
		/// active-mode data-channels.  When ActivePortRange is defined,
		/// a port within this range will be selected.</para>
		/// <para>This settings is not used in passive mode.</para>
		/// <para>This can be particularly useful in scenarios where it is necessary to 
		/// configure a NAT router to statically route a certain range of ports to the
		/// machine on which the FTP client is running.</para>	
		/// </remarks>
        [Category("FTP/FTPS Settings")]
		[Description("Specifies the range of ports to be used for data-channels in active mode.")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public virtual PortRange ActivePortRange
		{
			get
			{
                return ftpClient.ActivePortRange;
			}
            
			set
			{
                ftpClient.ActivePortRange = value;
			}
		}

		/// <summary> 
		/// TCP timeout (in milliseconds) of the underlying sockets (0 means none).
		/// </summary>
		[Category("Transfer")]
		[Description("TCP timeout (in milliseconds) on the underlying sockets (0 means none).")]
		[DefaultValue(0)]
        public virtual int Timeout
		{
			get
			{
				return ftpClient.Timeout;
			}
			set
			{          
				ftpClient.Timeout = value;
			}        
		}
        
		/// <summary>
		/// The connection-mode (passive or active) of data-channels.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When the connection-mode is active, the server will initiate connections
		/// to the FTP client, meaning that the FTP client must open a socket and wait for the
		/// server to connect to it.  This often causes problems if the FTP client is behind
		/// a firewall.
		/// </para>
		/// <para>
		/// When the connection-mode is passive, the FTP client will initiates connections
		/// to the server, meaning that the FTP client will connect to a particular socket
		/// on the server.  This is generally used if the FTP client is behind a firewall.
		/// </para>
		/// </remarks>
        [Category("FTP/FTPS Settings")]
		[Description("The connection-mode of data-channels.  Usually passive when FTP client is behind a firewall.")]
		[DefaultValue(FTPConnectMode.PASV)]
		public FTPConnectMode ConnectMode
		{
			set
			{
                ftpClient.ConnectMode = value;
			}
			get
			{
                return ftpClient.ConnectMode;
			}
		}

		/// <summary>
		/// Indicates whether the FTP client is currently connect with the server.
		/// </summary>
		[Browsable(false)]
		public bool IsConnected
		{
			get
			{
				return activeClient.IsConnected;
			}
		}
        

		/// <summary>
		/// Indicates whether the FTP client is currently transferring data.
		/// </summary>
		[Browsable(false)]
		public virtual bool IsTransferring
		{
			get
			{
				return isTransferringData;
			}
		}

		/// <summary>
		/// The number of bytes transferred between each notification of the
		/// <see cref="BytesTransferred"/> event.
		/// </summary>
		/// <remarks>
		/// Reduce this value to receive more frequent notifications of transfer progress.
		/// </remarks>
		[Category("Transfer")]
		[Description("The number of bytes transferred between each notification of the BytesTransferred event.")]
		[DefaultValue(4096)]
        public virtual long TransferNotifyInterval
		{
			get
			{
				return ftpClient.TransferNotifyInterval;
			}
			set
			{
				ftpClient.TransferNotifyInterval = value;
			}
		}

		/// <summary>
		/// The size of the buffers used in writing to and reading from the data-sockets.
		/// </summary>
		[Category("Transfer")]
		[Description("The size of the buffers used in writing to and reading from the data sockets.")]
		[DefaultValue(4096)]
        public virtual int TransferBufferSize
		{
			get
			{
				return ftpClient.TransferBufferSize;
			}
            
			set
			{
				ftpClient.TransferBufferSize = value;
			}
		}
     
		/// <summary>
		/// Determines if transfer-methods taking <see cref="Stream"/>s as arguments should
		/// close the stream once the transfer is completed.
		/// </summary>
		/// <remarks>
		/// If <c>CloseStreamsAfterTransfer</c> is <c>true</c> (the default) then streams are closed after 
		/// a transfer has completed, otherwise they are left open.
		/// </remarks>
		[Category("Transfer")]
		[Description("Determines if stream-based transfer-methods should close the stream once the transfer is completed.")]
		[DefaultValue(true)]
		public virtual bool CloseStreamsAfterTransfer
		{
			get
			{
				return ftpClient.CloseStreamsAfterTransfer;
			}
			set
			{
				ftpClient.CloseStreamsAfterTransfer = value;
			}
		}
        
		/// <summary>
		/// The domain-name or IP address of the FTP server.
		/// </summary>
		/// <remarks>
		/// <para>This property may only be set if not currently connected.</para>
		/// </remarks>
		[Category("Connection")]
		[Description("The domain-name or IP address of the FTP server.")]
		[DefaultValue(null)]
		public virtual string ServerAddress
		{
			get
			{
				return ftpClient.RemoteHost;
			}
			set
			{
				ftpClient.RemoteHost = value;
			}
		}    
                
		/// <summary>
		/// The port on the server to which to connect the control-channel. 
		/// </summary>
		/// <remarks>
		/// <para>Most FTP servers use port 21 (the default)</para>
		/// <para>This property may only be set if not currently connected.</para>
		/// </remarks>
		[Category("Connection")]
		[Description("Port on the server to which to connect the control-channel.")]
		[DefaultValue(21)]
        public virtual int ServerPort
		{
			get
			{
				return ftpClient.ControlPort;
			}
			set
			{
				ftpClient.ControlPort = value;
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
		/// downloaded file remains on the FTP client machine - and the download
		/// may be resumed, if it is a binary transfer.
		/// </para>
		/// <para>
		/// By default this flag is set to <c>true</c>.
		/// </para>
		/// </remarks>
		[Category("Transfer")]
		[Description("Controls whether or not a file is deleted when a failure occurs while it is transferred.")]
		[DefaultValue(true)]
        public virtual bool DeleteOnFailure
		{
			get
			{
				return ftpClient.DeleteOnFailure;
			}
			set
			{
				ftpClient.DeleteOnFailure = value;
			}
		}

		/// <summary>
		/// The character-encoding to use when dealing with file- and directory-paths.
		/// </summary>
		/// <remarks>
		/// The default is <c>ASCII</c>, but should be changed when communicating with FTP servers
		/// that have file-names containing non-ASCII characters
		/// </remarks>
		[Category("FTP/FTPS Settings")]
		[Description("The character-encoding to use when dealing with file- and directory-paths.")]
		public Encoding FilePathEncoding
		{
			get
			{
				return ftpClient.ControlEncoding;
			}
			set
			{
				ftpClient.ControlEncoding = value;
			}
		}
        
		/// <summary>The culture for parsing file listings.</summary>
		/// <remarks>
		/// <para>
		/// The <see cref="GetFileInfos(string)"/> method parses the file listings returned.  The names of the file
		/// can contain a wide variety of characters, so it is sometimes necessary to set this
		/// property to match the character-set used on the server.
		/// </para>
		/// <para>
		/// The default is <c>Invariant Language (Invariant Country)</c>.
		/// </para>
		/// </remarks>
        [Category("FTP/FTPS Settings")]
		[Description("The culture for parsing file listings.")]
		[DefaultValue(null)]
		public CultureInfo ParsingCulture
		{
			get
			{
                return ftpClient.ParsingCulture;
			}
			set
			{
                ftpClient.ParsingCulture = value;
			}            
		}
                
		/// <summary>
		/// Override the chosen file factory with a user created one - meaning
		/// that a specific parser has been selected
		/// </summary>
		[Browsable(false)]
		public FTPFileFactory FileInfoParser
		{
			set
			{
                ftpClient.FTPFileFactory = value;
			}            
		}
        
		/// <summary>The latest valid reply from the server.</summary>
		/// <value>
		/// Reply object encapsulating last valid server response.
		/// </value>
		[Browsable(false)]
		public FTPReply LastValidReply
		{
			get
			{
                return ftpClient.LastValidReply;
			}
		}

		/// <summary>The current file transfer type (BINARY or ASCII).</summary>
		/// <value>Transfer-type to be used for uploads and downloads.</value>
		/// <remarks>When the transfer-type is set to <c>BINARY</c> then files
		/// are transferred byte-for-byte such that the transferred file will
		/// be identical to the original.
		/// When the transfer-type is set to <c>BINARY</c> then end-of-line
		/// characters will be translated where necessary between Windows and
		/// UNIX formats.</remarks>
		[Category("Transfer")]
		[Description("The type of file transfer to use, i.e. BINARY or ASCII.")]
		[DefaultValue(FTPTransferType.BINARY)]
        public virtual FTPTransferType TransferType
		{
			get
			{
				return fileTransferType;
			}
			set
			{
				fileTransferType = value;
				if (IsConnected)
					activeClient.TransferType = value;
			}            
		}

		/// <summary>User-name of account on the server.</summary>
		/// <value>The user-name of the account the FTP server that will be logged into upon connection.</value>
		/// <remarks>This property must be set before a connection with the server is made.</remarks>
		[Category("Connection")]
		[Description("User-name of account on the server.")]
		[DefaultValue(null)]
		public string UserName
		{
			get
			{
				return loginUserName;
			}
			set
			{
				CheckConnection(false);
				loginUserName = value;
			}
		}

		/// <summary>Password of account on the server.</summary>
		/// <value>The password of the account the FTP server that will be logged into upon connection.</value>
		/// <remarks>This property must be set before a connection with the server is made.</remarks>
		[Category("Connection")]
		[Description("Password of account on the server.")]
		[DefaultValue(null)]
		public string Password
		{
			get
			{
				return loginPassword;
			}
			set
			{
				CheckConnection(false);
				loginPassword = value;
			}
		}
        
		/// <summary>Determines if the component will automatically log in upon connection.</summary>
		/// <remarks>
		/// <para>
		/// If this flag if <c>true</c> (the default) then the component will automatically attempt 
		/// to log in when the <see cref="Connect"/> method is called.  The <see cref="UserName"/> and 
		/// <see cref="Password"/> (if required) properties should be set previously.
		/// </para>
		/// <para>
		/// If the flag is <c>false</c> then the component will not log in until the <see cref="Login"/>
		/// method is called.
		/// </para>
		/// </remarks>
        [Category("FTP/FTPS Settings")]
		[Description("Determines if the component will automatically log in upon connection.")]
		[DefaultValue(true)]
		public bool AutoLogin
		{
			get
			{
				return useAutoLogin;
			}
			set
			{
				useAutoLogin = value;
			}
		}

		/// <summary>Determines whether or not events are currently enabled.</summary>
		/// <value>The <c>EventsEnabled</c> flag determines whether or not events are currently enabled.
		/// If the flag is <c>true</c> (the default) then events will fire as appropriate.
		/// If the flag is <c>false</c> then no events will be fired by this object.</value>
		[Category("Events")]
		[Description("Determines whether or not events are currently enabled.")]
		[DefaultValue(true)]
		public bool EventsEnabled
		{
			get
			{
				return areEventsEnabled;
			}
			set
			{
				areEventsEnabled = value;
			}
		}

		/// <summary>Determines whether or not event-handlers will be run on the GUI thread if one is available.</summary>
		/// <value>The <c>UseGuiThreadIfAvailable</c> flag determines whether or not event-handlers will be run 
		/// on the GUI thread if one is available.
		/// If the flag is <c>true</c> (the default) then they will be run on the GUI thread if one is available 
		/// (only for Windows Forms applications).
		/// If the flag is <c>false</c> then they will be run on a worker-thread.</value>
		/// <remarks>
		/// It is important to note that if event-handlers are run on a worker-thread then Windows Forms
		/// related operations will usually fail.  Since such operations are commonly used in event-handlers,
		/// the default is <c>true</c>.
		/// </remarks>
		[Category("Events")]
		[Description("Run event-handlers on GUI thread if one is available.")]
		[DefaultValue(true)]
		public bool UseGuiThreadIfAvailable
		{
			get
			{
				return useGuiThread;
			}
			set
			{
				useGuiThread = value;
			}
		}

		/// <summary>
		/// Determines the level of logs written.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Note that no logs will be written unless <see cref="LogToConsole"/> is
		/// <c>true</c> or <see cref="LogFile"/> is set.
		/// </para>
		/// <para>
		/// This method wraps <see cref="Logger.CurrentLevel"/> so setting either
		/// is equivalent to setting the other.
		/// </para>
		/// </remarks>
		[Category("Logging")]
		[Description("Level of logging to be written '")]
		[DefaultValue(LogLevel.Information)]
		public static LogLevel LogLevel
		{
			get
			{
				return Logger.CurrentLevel.GetLevel();
			}
			set
			{
				Logger.CurrentLevel = Level.GetLevel(value);
			}
		}

		/// <summary>
		/// Name of file to which logs will be written.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method wraps <see cref="Logger.PrimaryLogFile"/> so setting either
		/// is equivalent to setting the other.
		/// </para>
		/// </remarks>
		[Category("Logging")]
		[Description("Name of file to which logs will be written.")]
		[DefaultValue(null)]
		public static string LogFile
		{
			get
			{
				return Logger.PrimaryLogFile;
			}
			set
			{
				Logger.PrimaryLogFile = value;
			}
		}

		/// <summary>
		/// Determines whether or not logs will be written to the console.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method wraps <see cref="Logger.LogToConsole"/> so setting either
		/// is equivalent to setting the other.
		/// </para>
		/// </remarks>
		[Category("Logging")]
		[Description("Determines whether or not logs will be written to the console.")]
		[DefaultValue(false)]
		public static bool LogToConsole
		{
			get
			{
				return Logger.LogToConsole;
			}
			set
			{
				Logger.LogToConsole = value;
			}
		}

		/// <summary>
		/// Determines whether or not logs will be written using <see cref="System.Diagnostics.Trace"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method wraps <see cref="Logger.LogToTrace"/> so setting either
		/// is equivalent to setting the other.
		/// </para>
		/// </remarks>
		[Category("Logging")]
		[Description("Determines whether or not logs will be written using .NET's trace.")]
		[DefaultValue(false)]
		public static bool LogToTrace
		{
			get
			{
				return Logger.LogToTrace;
			}
			set
			{
				Logger.LogToTrace = value;
			}
		}

		#endregion

		#region Events
        
		/// <summary>Occurs when the component is connecting to the server.</summary> 
		[Category("Connection")]
		[Description("Occurs when the component is connecting to the server.")]
		public virtual event FTPConnectionEventHandler Connecting;

		/// <summary>Occurs when the component has connected to the server.</summary> 
		[Category("Connection")]
		[Description("Occurs when the component has connected to the server.")]
		public virtual event FTPConnectionEventHandler Connected;

		/// <summary>Occurs when the component is about to log in.</summary> 
		[Category("Connection")]
		[Description("Occurs when the component is about to log in.")]
		public virtual event FTPLogInEventHandler LoggingIn;

		/// <summary>Occurs when the component has logged in.</summary> 
		[Category("Connection")]
		[Description("Occurs when the component has logged in.")]
		public virtual event FTPLogInEventHandler LoggedIn;

		/// <summary>Occurs when the component is about to close its connection to the server.</summary> 
		[Category("Connection")]
		[Description("Occurs when the component is about to close its connection to the server.")]
		public virtual event FTPConnectionEventHandler Closing;

		/// <summary>Occurs when the component has closed its connection to the server.</summary> 
		[Category("Connection")]
		[Description("Occurs when the component has closed its connection to the server.")]
		public virtual event FTPConnectionEventHandler Closed;

		/// <summary>Occurs when a file is about to be uploaded to the server.</summary>
		/// <remarks>The <see cref="FTPFileTransferEventArgs"/> argument passed to
		/// handlers has a <see cref="FTPFileTransferEventArgs.Cancel"/> property,
		/// that, if set to <c>true</c> will result in the transfer being cancelled.</remarks>
		[Category("File")]
		[Description("Occurs when a file is about to be uploaded to the server.")]
		public virtual event FTPFileTransferEventHandler Uploading;

		/// <summary>Occurs when a file has been uploaded to the server.</summary> 
		/// <remarks>The <see cref="FTPFileTransferEventArgs"/> argument passed to
		/// handlers has a <see cref="FTPFileTransferEventArgs.Cancel"/> property,
		/// that indicates whether or not the transfer was cancelled.</remarks>
		[Category("File")]
		[Description("Occurs when a file has been uploaded to the server.")]
		public virtual event FTPFileTransferEventHandler Uploaded;

		/// <summary>Occurs when a file is about to be downloaded from the server.</summary> 
		/// <remarks>The <see cref="FTPFileTransferEventArgs"/> argument passed to
		/// handlers has a <see cref="FTPFileTransferEventArgs.Cancel"/> property,
		/// that, if set to <c>true</c> will result in the transfer being cancelled.</remarks>
		[Category("File")]
		[Description("Occurs when a file is about to be downloaded from the server.")]
		public virtual event FTPFileTransferEventHandler Downloading;

		/// <summary>Occurs when a file has been downloaded from the server.</summary> 
		/// <remarks>The <see cref="FTPFileTransferEventArgs"/> argument passed to
		/// handlers has a <see cref="FTPFileTransferEventArgs.Cancel"/> property,
		/// that indicates whether or not the transfer was cancelled.</remarks>
		[Category("File")]
		[Description("Occurs when a file has been downloaded from the server.")]
		public virtual event FTPFileTransferEventHandler Downloaded;
            
		/// <summary>Occurs every time a specified number of bytes of data have been transferred.</summary>
		/// <remarks>The property, <see cref="FTPConnection.TransferNotifyInterval"/>, determines
		/// the number of bytes sent between notifications.</remarks>
        [Category("Transfer")]
        [Description("Occurs every time 'TransferNotifyInterval' bytes have been transferred.")]
        public virtual event BytesTransferredHandler BytesTransferred;

		/// <summary>Occurs when a remote file is about to be renamed.</summary> 
		[Category("File")]
		[Description("Occurs when a remote file is about to be renamed.")]
		public virtual event FTPFileRenameEventHandler RenamingFile;

		/// <summary>Occurs when a remote file has been renamed.</summary> 
		[Category("File")]
		[Description("Occurs when a remote file has been renamed.")]
		public virtual event FTPFileRenameEventHandler RenamedFile;

		/// <summary>Occurs when a file is about to be deleted from the server.</summary> 
		/// <remarks>The <see cref="FTPFileTransferEventArgs"/> argument passed to
		/// handlers has a <see cref="FTPFileTransferEventArgs.Cancel"/> property,
		/// that, if set to <c>true</c> will result in the deletion being cancelled.</remarks>
		[Category("File")]
		[Description("Occurs when a file is about to be deleted from the server.")]
		public virtual event FTPFileTransferEventHandler Deleting;

		/// <summary>Occurs when a file has been deleted from the server.</summary> 
		/// <remarks>The <see cref="FTPFileTransferEventArgs"/> argument passed to
		/// handlers has a <see cref="FTPFileTransferEventArgs.Cancel"/> property,
		/// that indicates whether or not the deletion was cancelled.</remarks>
		[Category("File")]
		[Description("Occurs when a file has been deleted from the server.")]
		public virtual event FTPFileTransferEventHandler Deleted;

		/// <summary>Occurs when the working directory on the server is about to be changed.</summary> 
		[Category("Directory")]
		[Description("Occurs when the working directory on the server is about to be changed.")]
		public virtual event FTPDirectoryEventHandler DirectoryChanging;

		/// <summary>Occurs when the working directory on the server has been changed.</summary> 
		[Category("Directory")]
		[Description("Occurs when the working directory on the server is changed.")]
		public virtual event FTPDirectoryEventHandler DirectoryChanged;

		/// <summary>Occurs when a command is sent to the server.</summary> 
		[Category("Commands")]
		[Description("Occurs when a command is sent to the server.")]
		public virtual event FTPMessageHandler CommandSent
		{
			add
			{
                ftpClient.CommandSent += value;
			}
			remove
			{
                ftpClient.CommandSent -= value;
			}
		}

		/// <summary>Occurs when a reply is received from the server.</summary> 
		[Category("Commands")]
		[Description("Occurs when a reply is received from the server.")]
		public virtual event FTPMessageHandler ReplyReceived
		{
			add
			{
                ftpClient.ReplyReceived += value;
			}
			remove
			{
                ftpClient.ReplyReceived -= value;
			}
		}

		#endregion

		#region Connect Operations (Synchronous)

		/// <summary>Connect to the FTP server and (if <see cref="AutoLogin"/> is set) log into the server.</summary>
		/// <remarks>
		/// <para>The <see cref="ServerAddress"/> property must be set prior to calling this method.</para>
		/// <para>If <see cref="AutoLogin"/> is <c>true</c> then the component will attempt to
		/// log in immediately after successfully connecting.</para>
		/// <para>This method will throw an <c>FTPException</c> if the component is already connected to the server.</para>
		/// </remarks>
		public virtual void Connect()
		{
			lock (activeClient)
			{
				try
				{
					OnConnecting();
                    activeClient.Connect();
					OnConnected(true);
				}
				catch
				{
					OnConnected(false);
					throw;
				}
				if (PerformAutoLogin())
                    activeClient.TransferType = fileTransferType;
			}
		}

		/// <summary>Attempt to log into the server if <see cref="AutoLogin"/> is on.</summary>
		/// <remarks>A login attempt will take place only if the <see cref="UserName"/> property
		/// and (optionally) the <see cref="Password"/> property have been set.</remarks>
		protected bool PerformAutoLogin()
		{
            bool loggedIn = false;
			if (useAutoLogin && loginUserName!=null)
			{
				try
				{
					OnLoggingIn(loginUserName, loginPassword, false);
                    ftpClient.User(loginUserName);

                    if (loginPassword != null && ftpClient.LastValidReply.ReplyCode != "230")
                        ftpClient.Password(loginPassword);
					loggedIn = true;
				}
				finally
				{
					OnLoggedIn(loginUserName, loginPassword, loggedIn);
				}
			}
            return loggedIn;
		}

		/// <summary>Quit the FTP session.</summary> 
		/// <remarks>The session will be closed by sending a <c>QUIT</c> command before closing the socket.</remarks>
		public void Close()
		{
			Close(false);
		}
           
		/// <summary>Close the FTP connection.</summary> 
		/// <remarks>If <c>abruptClose</c> is <c>true</c> then the session will be closed immediately 
		/// by closing the control socket without sending the <c>QUIT</c> command, otherwise the
		/// session will be closed by sending a <c>QUIT</c> command before closing the socket.</remarks>
		/// <param name="abruptClose">Closes session abruptly (see comments).</param>
		public virtual void Close(bool abruptClose)
		{
			try
			{
				OnClosing();
				if (abruptClose)
				{
					lock (activeClient)
					{
						activeClient.CancelTransfer();
                        ftpClient.QuitImmediately();
					}
				}
				else
				{
					lock (activeClient)
					{
						activeClient.Quit();
					}
				}
			}
			finally
			{
				OnClosed();
			}
		}
 
		#endregion
     
		#region Login Operations (Synchronous)
		
		/// <summary>Log into an account on the FTP server using <see cref="UserName"/> and <see cref="Password"/>.</summary>
		/// <remarks>This is only necessary if <see cref="AutoLogin"/> is <c>false</c>.</remarks>
		public virtual void Login()
		{
            CheckFTPType(true);
            OnLoggingIn(loginUserName, loginPassword, false);
			bool hasLoggedIn = false;
			lock (activeClient)
			{
				try
				{
					activeClient.Login(loginUserName, loginPassword);
					hasLoggedIn = true;
				}
				finally
				{
					OnLoggedIn(loginUserName, loginPassword, hasLoggedIn);
				}
			}
		}
   
		/// <summary>
		/// Supply the user-name to log into an account on the FTP server. 
		/// Must be followed by the <see cref="SendPassword(string)"/> method.
		/// </summary>
		/// <remarks>This is only necessary if <see cref="AutoLogin"/> is <c>false</c>.</remarks>
		/// <param name="user">User-name of the client's account on the server.</param>
		public virtual void SendUserName(string user)
		{
            CheckFTPType(true);
            lock (activeClient)
			{
                ftpClient.User(user);
			}
		}
 
		/// <summary>
		/// Supply the password for the previously supplied
		/// user-name to log into the FTP server. Must be
		/// preceeded by the <see cref="SendUserName(string)"/> method
		/// </summary>
		/// <remarks>This is only necessary if <see cref="AutoLogin"/> is <c>false</c>.</remarks>
		/// <param name="loginPassword">Password of the client's account on the server.</param>
		public virtual void SendPassword(string loginPassword)
		{
            CheckFTPType(true);
            lock (activeClient)
			{
                ftpClient.Password(loginPassword);
			}
		}
     
		#endregion		

		#region Transfer Control Operations (Synchronous)
		
		/// <summary>Cancels the current transfer.</summary>
		/// <remarks>This method is generally called from a separate
		/// thread. Note that this may leave partially written files on the
		/// server or on local disk, and should not be used unless absolutely
		/// necessary. The server is not notified.</remarks>
		public virtual void CancelTransfer()
		{
			activeClient.CancelTransfer();
		}        

		/// <summary>Make the next file transfer (upload or download) resume.</summary>
		/// <remarks>
		/// <para>
		/// For uploads, the
		/// bytes already transferred are skipped over, while for downloads, if 
		/// writing to a file, it is opened in append mode, and only the bytes
		/// required are transferred.
		/// </para>
		/// <para>
		/// Currently resume is only supported for BINARY transfers (which is
		/// generally what it is most useful for). 
		/// </para>
		/// </remarks>
		public virtual void ResumeTransfer()
		{
			activeClient.Resume();
		}

		/// <summary>Cancel the resume.</summary>
		/// <remarks>
		/// Use this method if something goes wrong
		/// and the server is left in an inconsistent state.
		/// </remarks>
		public virtual void CancelResume()
		{
			activeClient.CancelResume();
		}

		#endregion

		#region Upload Operations (Synchronous)
		
		/// <summary>
		/// Upload a local file to the FTP server in the current working directory.
		/// </summary>
		/// <param name="localPath">Path of the local file.</param>
		/// <param name="remoteFile">Name of remote file in current working directory.</param>
		public virtual void UploadFile(string localPath, string remoteFile)
		{
			UploadFile(localPath, remoteFile, false);
		}

		/// <summary>
		/// Upload a stream of data to the FTP server in the current working directory.
		/// </summary>
		/// <remarks>
		/// The stream is closed after the transfer is complete if
		/// <see cref="CloseStreamsAfterTransfer"/> is <c>true</c> (the default) and are left
		/// open otherwise.
		/// </remarks>
		/// <param name="srcStream">Input stream of data to put.</param>
		/// <param name="remoteFile">Name of remote file in current working directory.</param>
		public virtual void UploadStream(Stream srcStream, string remoteFile)
		{
			UploadStream(srcStream, remoteFile, false);
		}

		/// <summary>
		/// Upload an array of bytes to the FTP server in the current working directory.
		/// </summary>
		/// <param name="bytes">Array of bytes to put.</param>
		/// <param name="remoteFile">Name of remote file in current working directory.</param>
		public virtual void UploadByteArray(byte[] bytes, string remoteFile)
		{
			UploadByteArray(bytes, remoteFile, false);
		}
        
		/// <summary>
		/// Upload a local file to the FTP server in the current working directory. Allows appending
		/// if current file exists.
		/// </summary>
		/// <param name="localPath">Path of the local file.</param>
		/// <param name="remoteFile">Name of remote file in current working directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise.</param>
		public virtual void UploadFile(string localPath, string remoteFile, bool append)
		{
			lock (activeClient)
			{
				bool transferCompleted = false;
				try
				{
					if (OnUploading(localPath, remoteFile, append))
						try
						{
							isTransferringData = true;
							activeClient.Put(localPath, remoteFile, append);
							transferCompleted = true;
						}
						finally
						{
							isTransferringData = false;
						}
				}
				finally
				{
					OnUploaded(localPath, remoteFile, append, !transferCompleted);
				}
			}
		}

		/// <summary>
		/// Upload a stream of data to the FTP server in the current working directory.  Allows appending
		/// if current file exists.
		/// </summary>
		/// <remarks>
		/// The stream is closed after the transfer is complete if
		/// <see cref="CloseStreamsAfterTransfer"/> is <c>true</c> (the default) and are left
		/// open otherwise.
		/// </remarks>
		/// <param name="srcStream">Input stream of data to put.</param>
		/// <param name="remoteFile">Name of remote file in current working directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise.</param>
		public virtual void UploadStream(Stream srcStream, string remoteFile, bool append)
		{
			lock (activeClient)
			{
				bool transferCompleted = false;
				try
				{
					if (OnUploading(srcStream, remoteFile, append))
						try
						{
							isTransferringData = true;
							activeClient.Put(srcStream, remoteFile, append);
							transferCompleted = true;
						}
						finally
						{
							isTransferringData = false;
						}
				}
				finally
				{
					OnUploaded(srcStream, remoteFile, append, !transferCompleted);
				}
			}
		}        

		/// <summary>
		/// Upload data to the FTP server in the current working directory. Allows
		/// appending if current file exists.
		/// </summary>
		/// <param name="bytes">Array of bytes to put.</param>
		/// <param name="remoteFile">Name of remote file in current working directory.</param>
		/// <param name="append"><c>true</c> if appending, <c>false</c> otherwise.</param>
		public virtual void UploadByteArray(byte[] bytes, string remoteFile, bool append)
		{            
			lock (activeClient)
			{
				bool transferCompleted = false;
				try
				{
					if (OnUploading(bytes, remoteFile, append))
						try
						{
							isTransferringData = true;
							activeClient.Put(bytes, remoteFile, append);
							transferCompleted = true;
						}
						finally
						{
							isTransferringData = false;
						}
				}
				finally
				{
					OnUploaded(bytes, remoteFile, append, !transferCompleted);
				}
			}
		}

    
		#endregion

		#region Download Operations (Synchronous)
        
		/// <summary>Download a file from the FTP server and save it locally.</summary>
		/// <remarks>Transfers in the current <see cref="TransferType"/>. </remarks>
		/// <param name="localPath">Local file to put data in.</param>
		/// <param name="remoteFile">Name of remote file in current working directory.</param>
		public virtual void DownloadFile(string localPath, string remoteFile)
		{            
			lock (activeClient)
			{
				bool transferCompleted = false;
				try
				{
					if (isTransferringMultiple || OnDownloading(localPath, remoteFile))
						try
						{
							isTransferringData = true;
							activeClient.Get(localPath, remoteFile);
							transferCompleted = true;
						}
						finally
						{
							isTransferringData = false;
						}
				}
				finally
				{
					if (!isTransferringMultiple)
						OnDownloaded(localPath, remoteFile, !transferCompleted);
				}
			}
		}

		/// <summary>Download a file from the FTP server and write it to the given stream.</summary>
		/// <remarks>
		/// Transfers in the current <see cref="TransferType"/>.
		/// The stream is closed after the transfer is complete if
		/// <see cref="CloseStreamsAfterTransfer"/> is <c>true</c> (the default) and are left
		/// open otherwise.
		/// </remarks>
		/// <param name="destStream">Data stream to write data to.</param>
		/// <param name="remoteFile">Name of remote file in current working directory.</param>
		public virtual void DownloadStream(Stream destStream, string remoteFile)
		{
			lock (activeClient)
			{
				bool transferCompleted = false;
				try
				{
					if (isTransferringMultiple || OnDownloading(destStream, remoteFile))
						try
						{
							isTransferringData = true;
							activeClient.Get(destStream, remoteFile);
							transferCompleted = true;
						}
						finally
						{
							isTransferringData = false;
						}
				}
				finally
				{
					if (!isTransferringMultiple)
						OnDownloaded(destStream, remoteFile, !transferCompleted);
				}
			}
		}        
 
		/// <summary>Download data from the FTP server and return it as a byte-array.</summary>
		/// <remarks>
		/// Transfers in the current <see cref="TransferType"/>. Note
		/// that we may experience memory limitations as the
		/// entire file must be held in memory at one time.
		/// </remarks>
		/// <param name="remoteFile">Name of remote file in current working directory.</param>
		/// <returns>Returns a byte-array containing the file-data.</returns>
		public virtual byte[] DownloadByteArray(string remoteFile)
		{       
			lock (activeClient)
			{
				bool transferCompleted = false;
				byte[] bytes = null;
				try
				{
					if (isTransferringMultiple || OnDownloading(remoteFile))
						try
						{
							isTransferringData = true;
							bytes = activeClient.Get(remoteFile);
							transferCompleted = true;
						}
						finally
						{
							isTransferringData = false;
						}
				}
				finally
				{
					if (!isTransferringMultiple)
						OnDownloaded(bytes, remoteFile, !transferCompleted);
				}
				return bytes;
			}
		}
    
		#endregion
		
		#region Generic Operations (Synchronous)

		/// <summary>
		/// Invokes the given site command on the server.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Site-specific commands are special commands that may be defined by a server.  
		/// Such commands are defined on a server-by-server basis.
		/// </para>
		/// <para>
		/// For example, a specific FTP server might define a <c>PROCESS</c> site-command which 
		/// results in another piece of software on the server being directed to perform some
		/// sort of processing on a particular file.  The command required might be:
		/// </para>
		/// <code>
		///		SITE PROCESS file-path
		/// </code>
		/// <para>
		/// In this case, the site-command would be invoked as follows:
		/// </para>
		/// <code>
		///		ftpConnection.InvokeSiteCommand("PROCESS", filePath);
		/// </code>
		/// </remarks>
		/// <param name="command">Site-specific command to be invoked.</param>
		/// <param name="arguments">Arguments of the command to be invoked.</param>
		/// <returns>The reply returned by the server.</returns>
		public virtual FTPReply InvokeSiteCommand(string command, params string[] arguments)
		{        
			StringBuilder commandString = new StringBuilder(command);
			foreach (string argument in arguments)
			{
				commandString.Append(" ");
				commandString.Append(argument);
			}
			lock (activeClient)
			{
                ftpClient.Site(commandString.ToString());
				return LastValidReply;
			}
		}

		/// <summary>
		/// Invokes the given literal FTP command on the server.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If a particular FTP command is not supported by <see cref="FTPConnection"/>, this
		/// method may sometimes be used to invoke the command.  This will only work for 
		/// simple commands that don't require special processing.
		/// </para>
		/// <para>
		/// An example of an FTP command that could be invoked using this method is the 
		/// <c>FEAT</c> command (which is actually behind the <see cref="GetFeatures()"/> method.
		/// This would be done as follows:
		/// </para>
		/// <code>
		///		string features = ftpConnection.InvokeFTPCommand("FEAT", "211");
		/// </code>
		/// The returned <c>string</c> could then be parsed to obtain the supported features
		/// of the server.
		/// </remarks>
		/// <param name="command">Command to be sent.</param>
		/// <param name="validCodes">Valid return-codes (used for validating reply).</param>
		/// <returns>The reply returned by the server.</returns>
		public virtual FTPReply InvokeFTPCommand(string command, params string[] validCodes)
		{        
			lock (activeClient)
			{
                ftpClient.Quote(command, validCodes);
				return LastValidReply;
			}
		}
    
		/// <summary>Get the server supplied features.</summary>
		/// <returns>
		/// <c>string</c>-array containing server features, or <c>null</c> if no features or not supported.
		/// </returns>
		public virtual string[] GetFeatures()
		{
            CheckFTPType(true);
            lock (activeClient)
			{
                return ftpClient.Features();
			}
		}

		/// <summary>Get the type of the operating system at the server.</summary>
		/// <returns>The type of server operating system.</returns>
		public virtual string GetSystemType()
		{
            CheckFTPType(true);
            lock (activeClient)
			{
                return ftpClient.GetSystem();
			}
		}

		/// <summary>Get the help text for the specified FTP command.</summary>
		/// <param name="command">Name of the FTP command to get help for.</param>
		/// <returns>Help text from the server for the supplied command.</returns>
		public virtual string GetCommandHelp(string command)
		{
            CheckFTPType(true);
            lock (activeClient)
			{
                return ftpClient.Help(command);
			}
		}

		#endregion

		#region Directory Operations (Synchronous)
        
		/// <summary>
		/// Returns the working directory's contents as an array of <see cref="FTPFile"/> objects.
		/// </summary>
		/// <remarks>
		/// This method works for Windows and most Unix FTP servers.  Please inform EDT
		/// about unusual formats (<a href="support@enterprisedt.com">support@enterprisedt.com</a>).
		/// </remarks>
		/// <returns>An array of <see cref="FTPFile"/> objects.</returns>
		public virtual FTPFile[] GetFileInfos()
		{
			lock (activeClient)
			{
				return activeClient.DirDetails(".");
			}
		}
        
		/// <summary>
		/// Returns the given directory's contents as an array of <see cref="FTPFile"/> objects.
		/// </summary>
		/// <remarks>
		/// This method works for Windows and most Unix FTP servers.  Please inform EDT
		/// about unusual formats (<a href="support@enterprisedt.com">support@enterprisedt.com</a>).
		/// </remarks>
		/// <param name="directory">Name of directory AND/OR filemask.</param>
		/// <returns>An array of <see cref="FTPFile"/> objects.</returns>
		public virtual FTPFile[] GetFileInfos(string directory)
		{
			lock (activeClient)
			{
				return activeClient.DirDetails(directory);
			}
		}

		/// <summary>
		/// Lists current working directory's contents as an array of strings of file-names.
		/// </summary>
		/// <returns>An array of current working directory listing strings.</returns>
		public virtual string[] GetFiles()
		{            
			return GetFiles(".");
		} 

		/// <summary>
		/// List the given directory's contents as an array of strings of file-names.
		/// </summary>
		/// <param name="directory">Name of directory</param>
        /// <remarks>
        /// The directory name can sometimes be a file mask depending on the FTP server.
        /// </remarks>
		/// <returns>An array of directory listing strings.</returns>
		public virtual string[] GetFiles(string directory)
		{
			lock (activeClient)
			{
				string[] files = activeClient.Dir(directory, false);
				if (files.Length==0 && LastValidReply.ReplyText.ToLower().IndexOf("permission")>=0)
				{
					FTPFile[] ftpFiles = activeClient.DirDetails(directory);
					files = new string[ftpFiles.Length];
					for (int i=0; i<ftpFiles.Length; i++)
						files[i] = ftpFiles[i].Name;
				}
				return files;
			}
		}
	
		/// <summary>Delete the specified remote directory.</summary>
		/// <remarks>
		/// This method does not recursively delete files.
		/// </remarks>
		/// <param name="directory">Name of remote directory to delete.</param>
		public virtual void DeleteDirectory(string directory)
		{            
			lock (activeClient)
			{
				activeClient.RmDir(directory);
			}
		}

		/// <summary>Create the specified remote directory.</summary>
		/// <param name="directory">Name of remote directory to create.</param>
		public virtual void CreateDirectory(string directory)
		{            
			lock (activeClient)
			{
				activeClient.MkDir(directory);
			}
		}

		/// <summary>
		/// Returns the working directory on the server.
		/// </summary>
		/// <returns>The working directory on the server.</returns>
		public string GetWorkingDirectory()
		{
			lock (activeClient)
			{
				return activeClient.Pwd();
			}
		}

		/// <summary>
		/// Changes the working directory.
		/// </summary>
		/// <param name="directory">Directory to change to (may be relative or absolute).</param>
		public void ChangeWorkingDirectory(string directory)
		{
			lock (activeClient)
			{
				string oldDirectory = null;
				if (areEventsEnabled && (DirectoryChanging!=null || DirectoryChanged!=null))
					oldDirectory = GetWorkingDirectory();
				OnChangingDirectory(oldDirectory, directory);
					activeClient.ChDir(directory);
				OnChangedDirectory(oldDirectory, directory);
			}
		}

		/// <summary>
		/// Changes to the parent of the current working directory on the server.
		/// </summary>
		public void ChangeWorkingDirectoryUp()
		{
			lock (activeClient)
			{
				string oldDirectory = null;
				if (areEventsEnabled && (DirectoryChanging!=null || DirectoryChanged!=null))
					oldDirectory = GetWorkingDirectory();
				OnChangingDirectory(oldDirectory, "..");
					activeClient.CdUp();
				if (DirectoryChanged!=null)
					OnChangedDirectory(oldDirectory, GetWorkingDirectory());
			}
		}

		#endregion

		#region File Status/Control Operations (Synchronous)

		/// <summary>Delete the specified remote file.</summary>
		/// <param name="remoteFile">Name of remote file to delete.</param>
		/// <returns><c>true</c> if file was deleted successfully.</returns>
		public virtual bool DeleteFile(string remoteFile)
		{        
			lock (activeClient)
			{
				bool deletionCompleted = false;
				try
				{
					if (OnDeleting(remoteFile))
						activeClient.Delete(remoteFile);
					deletionCompleted = true;
					return deletionCompleted;
				}
				finally
				{
					OnDeleted(remoteFile, deletionCompleted);
				}
			}
		}
         
		/// <summary>Rename a file or directory.</summary>
		/// <param name="from">Name of file or directory to rename.</param>
		/// <param name="to">New file-name.</param>
		public virtual void RenameFile(string from, string to)
		{            
			lock (activeClient)
			{
				bool renameCompleted = false;
				try
				{
					OnRenaming(from, to);
					activeClient.Rename(from, to);
					renameCompleted = true;
				}
				finally
				{
					OnRenamed(from, to, renameCompleted);
				}
			}
		}

		/// <summary>
		/// Get the size of a remote file. 
		/// </summary>
		/// <remarks>
		/// This is not a standard FTP command, it is defined in "Extensions to FTP", a draft RFC 
		/// (draft-ietf-ftpext-mlst-16.txt).
		/// </remarks>
		/// <param name="remoteFile">Name or path of remote file in current working directory.</param>
		/// <returns>Size of file in bytes.</returns>
		public virtual long GetSize(string remoteFile)
		{            
			lock (activeClient)
			{
				return activeClient.Size(remoteFile);
			}
		}

		/// <summary>Get modification time for a remote file.</summary>
		/// <param name="remoteFile">Name of remote file.</param>
		/// <returns>Modification time of file as a <c>DateTime</c>.</returns>
		public virtual DateTime GetLastWriteTime(string remoteFile)
		{            
			lock (activeClient)
			{
				return activeClient.ModTime(remoteFile);
			}
		}

		#endregion

		#region Notification Methods

		/// <summary>
		/// Invokes the given event-handler.
		/// </summary>
		/// <param name="eventHandler">Event-handler to invoke.</param>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Event arguments.</param>
		protected void InvokeEventHandler(Delegate eventHandler, object sender, EventArgs e)
		{
			InvokeDelegate(true, eventHandler, new object[] {sender, e});
		}

		/// <summary>
		/// Invokes the given delegate.
		/// </summary>
		/// <param name="preferAsync">If <c>true</c> then an attempt will be made to 
		/// invoke the delegate asynchronously</param>
		/// <param name="del">Delegate to invoke.</param>
		/// <param name="args">Arguments with which to invoke the delegate.</param>
		/// <returns>Return value of delegate (if any).</returns>
		protected object InvokeDelegate(bool preferAsync, Delegate del, params object[] args)
		{
			log.Debug("Invoking delegate " + del.GetType().FullName);
			if (preferAsync && guiControl==null && !haveQueriedForControl)
			{
				try
				{
					if (Container is Control)
					{
						log.Debug("Container is a control, so using its thread");
						guiControl = (Control)Container;
					}
					else
					{
						log.Debug("Finding MainWindowHandle");
						IntPtr mainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
                            if (mainWindowHandle != IntPtr.Zero)
                            {
                                log.Debug("MainWindowHandle found");
                                guiControl = Form.FromHandle(mainWindowHandle);
                            }
						else
							log.Debug("No MainWindowHandle found");
					}
				}
				catch (Exception ex)
				{
					log.Error("Error while getting GUI control", ex);
				}
				finally
				{
					haveQueriedForControl = true;
				}
			}
			log.Debug(guiControl!=null ? "Have GUI control" : "No GUI control");
			if (preferAsync && guiControl!=null)
				log.Debug(guiControl.InvokeRequired ? "GUI control invocation required" : "GUI control invocation not required");
			if (preferAsync && useGuiThread && guiControl!=null && guiControl.InvokeRequired)
			{
				log.Debug("Beginning delegate invocation");
				IAsyncResult result = guiControl.BeginInvoke(del, args);
				log.Debug("Delegate invocation begun - waiting for completion");
				while (!result.IsCompleted)
					Thread.Sleep(100);
				log.Debug("Ending invocation");
				object returnValue = guiControl.EndInvoke(result);
				log.Debug("Returning result");
				return returnValue;
			}
			else
			{
				log.Debug("Invoking delegate dynamically");
				object returnValue = del.DynamicInvoke(args);
				log.Debug("Dynamic delegate invocation complete");
				return returnValue;
			}
		}

		/// <summary>
		/// Called when a connection-attempt is being made.
		/// </summary>
		protected virtual void OnConnecting()
		{
			if (areEventsEnabled && Connecting!=null)
				InvokeEventHandler(Connecting, this, new FTPConnectionEventArgs(ServerAddress, ServerPort, false));
		}

		/// <summary>
		/// Called when a connection-attempt has completed.
		/// </summary>
		/// <param name="hasConnected"><c>true</c> if the connection-attempt succeeded.</param>
		protected virtual void OnConnected(bool hasConnected)
		{
			if (areEventsEnabled && Connected!=null)
				InvokeEventHandler(Connected, this, new FTPConnectionEventArgs(ServerAddress, ServerPort, hasConnected));
		}

		/// <summary>
		/// Called when the client is about to log in.
		/// </summary>
		protected virtual void OnLoggingIn(string userName, string password, bool hasLoggedIn)
		{
			if (areEventsEnabled && LoggingIn!=null)
				InvokeEventHandler(LoggingIn, this, new FTPLogInEventArgs(userName, password, hasLoggedIn));
		}

		/// <summary>
		/// Called when the client has logged in.
		/// </summary>
		/// <param name="userName">User-name of account.</param>
		/// <param name="password">Password of account.</param>
		/// <param name="hasLoggedIn"><c>true</c> if the client logged in successfully.</param>
		protected virtual void OnLoggedIn(string userName, string password, bool hasLoggedIn)
		{
			if (areEventsEnabled && LoggedIn!=null)
				InvokeEventHandler(LoggedIn, this, new FTPLogInEventArgs(userName, password, hasLoggedIn));
		}

		/// <summary>
		/// Called when a connection is about to close.
		/// </summary>
		protected internal void OnClosing()
		{
			if (areEventsEnabled && Closing!=null)
				InvokeEventHandler(Closing, this, new FTPConnectionEventArgs(ServerAddress, ServerPort, true));
		}

		/// <summary>
		/// Called when a connection has closed.
		/// </summary>
		protected internal void OnClosed()
		{
			if (areEventsEnabled && Closed!=null)
				InvokeEventHandler(Closed, this, new FTPConnectionEventArgs(ServerAddress, ServerPort, false));
		}

		/// <summary>
		/// Called when a file is about to be uploaded.
		/// </summary>
		/// <param name="localPath">Path of local file.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <param name="append">Flag indicating whether or not the remote file is being appended to.</param>
		/// <returns><c>true</c> if the operation is to continue.</returns>
		protected bool OnUploading(string localPath, string remoteFile, bool append)
		{
			if (areEventsEnabled && Uploading!=null)
			{
				long fileSize = new FileInfo(localPath).Length;
				FTPFileTransferEventArgs e = new FTPFileTransferEventArgs(localPath, remoteFile, fileSize, append, false);
				InvokeEventHandler(Uploading, this, e);
				return !e.Cancel;
			}
			else
				return true;
		}

		/// <summary>
		/// Called when a file uploading operation has completed (though it may have been cancelled).
		/// </summary>
		/// <param name="localPath">Path of local file.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <param name="append">Flag indicating whether or not the remote file was being appended to.</param>
		/// <param name="cancelled">Flag indicating whether or not the upload attempt was cancelled by an event-handler.</param>
		protected virtual void OnUploaded(string localPath, string remoteFile, bool append, bool cancelled)
		{
			if (areEventsEnabled && Uploaded!=null)
			{
				long fileSize = new FileInfo(localPath).Length;
				InvokeEventHandler(Uploaded, this, new FTPFileTransferEventArgs(localPath, remoteFile, fileSize, append, cancelled));
			}
		}

		/// <summary>
		/// Called when a stream is about to be uploaded.
		/// </summary>
		/// <param name="srcStream">Stream to upload.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <param name="append">Flag indicating whether or not the remote file is being appended to.</param>
		/// <returns><c>true</c> if the operation is to continue.</returns>
		protected bool OnUploading(Stream srcStream, string remoteFile, bool append)
		{
			if (areEventsEnabled && Uploading!=null)
			{
				FTPFileTransferEventArgs e = new FTPFileTransferEventArgs(srcStream, remoteFile, srcStream.Length, append, false);
				InvokeEventHandler(Uploading, this, e);
				return !e.Cancel;
			}
			else
				return true;
		}

		/// <summary>
		/// Called when a file uploading operation has completed (though it may have been cancelled).
		/// </summary>
		/// <param name="srcStream">Stream to upload.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <param name="append">Flag indicating whether or not the remote file was being appended to.</param>
		/// <param name="cancelled">Flag indicating whether or not the upload attempt was cancelled by an event-handler.</param>
		protected virtual void OnUploaded(Stream srcStream, string remoteFile, bool append, bool cancelled)
		{
			if (areEventsEnabled && Uploaded!=null)
				InvokeEventHandler(Uploaded, this, new FTPFileTransferEventArgs(srcStream, remoteFile, srcStream.Length, append, cancelled));
		}

		/// <summary>
		/// Called when a byte-array is about to be uploaded.
		/// </summary>
		/// <param name="bytes">Byte-array to upload.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <param name="append">Flag indicating whether or not the remote file is being appended to.</param>
		/// <returns><c>true</c> if the operation is to continue.</returns>
		protected bool OnUploading(byte[] bytes, string remoteFile, bool append)
		{
			if (areEventsEnabled && Uploading!=null)
			{
				FTPFileTransferEventArgs e = new FTPFileTransferEventArgs(remoteFile, bytes.Length, append, false);
				InvokeEventHandler(Uploading, this, e);
				return !e.Cancel;
			}
			else
				return true;
		}

		/// <summary>
		/// Called when a file uploading operation has completed (though it may have been cancelled).
		/// </summary>
		/// <param name="bytes">Byte-array to upload.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <param name="append">Flag indicating whether or not the remote file was being appended to.</param>
		/// <param name="cancelled">Flag indicating whether or not the upload attempt was cancelled by an event-handler.</param>
		protected virtual void OnUploaded(byte[] bytes, string remoteFile, bool append, bool cancelled)
		{
			if (areEventsEnabled && Uploaded!=null)
				InvokeEventHandler(Uploaded, this, new FTPFileTransferEventArgs(bytes, remoteFile, bytes.Length, append, cancelled));
		}

		/// <summary>
		/// Called when a file is about to be downloaded.
		/// </summary>
		/// <param name="localPath">Path of local file.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <returns><c>true</c> if the operation is to continue.</returns>
		protected bool OnDownloading(string localPath, string remoteFile)
		{
			if (areEventsEnabled && (Downloading!=null || Downloaded!=null))
				currentFileSize = GetSize(remoteFile);

			if (areEventsEnabled && Downloading!=null)
			{
				FTPFileTransferEventArgs e = new FTPFileTransferEventArgs(localPath, remoteFile, currentFileSize, false, false);
				InvokeEventHandler(Downloading, this, e);
				return !e.Cancel;
			}
			else
				return true;
		}

		/// <summary>
		/// Called when a file downloading operation has completed (though it may have been cancelled).
		/// </summary>
		/// <param name="localPath">Path of local file.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <param name="cancelled">Flag indicating whether or not the download attempt was cancelled by an event-handler.</param>
		protected virtual void OnDownloaded(string localPath, string remoteFile, bool cancelled)
		{
			if (areEventsEnabled && Downloaded!=null)
				InvokeEventHandler(Downloaded, this, new FTPFileTransferEventArgs(localPath, remoteFile, currentFileSize, false, cancelled));
		}

		/// <summary>
		/// Called when a file is about to be downloaded.
		/// </summary>
		/// <param name="destStream">Stream to which data will be written.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <returns><c>true</c> if the operation is to continue.</returns>
		protected bool OnDownloading(Stream destStream, string remoteFile)
		{
			if (areEventsEnabled && (Downloading!=null || Downloaded!=null))
				currentFileSize = GetSize(remoteFile);

			if (areEventsEnabled && Downloading!=null)
			{
				FTPFileTransferEventArgs e = new FTPFileTransferEventArgs(destStream, remoteFile, currentFileSize, false, false);
				InvokeEventHandler(Downloading, this, e);
				return !e.Cancel;
			}
			else
				return true;
		}

		/// <summary>
		/// Called when a file downloading operation has completed (though it may have been cancelled).
		/// </summary>
		/// <param name="destStream">Stream to which data will be written.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <param name="cancelled">Flag indicating whether or not the download attempt was cancelled by an event-handler.</param>
		protected virtual void OnDownloaded(Stream destStream, string remoteFile, bool cancelled)
		{
			if (areEventsEnabled && Downloaded!=null)
				InvokeEventHandler(Downloaded, this, new FTPFileTransferEventArgs(destStream, remoteFile, currentFileSize, false, cancelled));
		}

		/// <summary>
		/// Called when a file is about to be downloaded.
		/// </summary>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <returns><c>true</c> if the operation is to continue.</returns>
		protected bool OnDownloading(string remoteFile)
		{
			if (areEventsEnabled && (Downloading!=null || Downloaded!=null))
				currentFileSize = GetSize(remoteFile);

			if (areEventsEnabled && Downloading!=null)
			{
				FTPFileTransferEventArgs e = new FTPFileTransferEventArgs(remoteFile, currentFileSize, false, false);
				InvokeEventHandler(Downloading, this, e);
				return !e.Cancel;
			}
			else
				return true;
		}

		/// <summary>
		/// Called when a file downloading operation has completed (though it may have been cancelled).
		/// </summary>
		/// <param name="bytes">Byte-array containing downloaded data.</param>
		/// <param name="remoteFile">Path of remote file.</param>
		/// <param name="cancelled">Flag indicating whether or not the download attempt was cancelled by an event-handler.</param>
		protected virtual void OnDownloaded(byte[] bytes, string remoteFile, bool cancelled)
		{
			if (areEventsEnabled && Downloaded!=null)
				InvokeEventHandler(Downloaded, this, new FTPFileTransferEventArgs(bytes, remoteFile, currentFileSize, false, cancelled));
		}

        /// <summary>
        /// Called every time a specified number of bytes of data have been transferred.
        /// </summary>
        /// <param name="remoteFile">The name of the file being transferred.</param>
        /// <param name="byteCount">The current count of bytes transferred.</param>
        protected virtual void OnBytesTransferred(string remoteFile, long byteCount)
        {
            if (areEventsEnabled && BytesTransferred!=null)
                InvokeEventHandler(BytesTransferred, this, new BytesTransferredEventArgs(remoteFile, byteCount));
        }

		/// <summary>
		/// Called when the working directory is about to be changed.
		/// </summary>
        /// <remarks>The property, <see cref="FTPConnection.TransferNotifyInterval"/>, determines
        /// the number of bytes sent between notifications.</remarks>
        /// <param name="oldDirectory">Current directory.</param>
		/// <param name="newDirectory">New directory</param>
		protected virtual void OnChangingDirectory(string oldDirectory, string newDirectory)
		{
			if (areEventsEnabled && DirectoryChanging!=null)
				InvokeEventHandler(DirectoryChanging, this, new FTPDirectoryEventArgs(oldDirectory, newDirectory));
		}

		/// <summary>
		/// Called when the working directory has been changed.
		/// </summary>
		/// <param name="oldDirectory">Previous directory.</param>
		/// <param name="newDirectory">New directory</param>
		protected virtual void OnChangedDirectory(string oldDirectory, string newDirectory)
		{
			if (areEventsEnabled && DirectoryChanged!=null)
				InvokeEventHandler(DirectoryChanged, this, new FTPDirectoryEventArgs(oldDirectory, newDirectory));
		}

		/// <summary>
		/// Called when a file is about to be deleted.
		/// </summary>
		/// <param name="remoteFile">File to delete.</param>
		/// <returns><c>false</c> if the deletion should proceed; <c>true</c> otherwise.</returns>
		protected bool OnDeleting(string remoteFile)
		{
			if (areEventsEnabled && Deleting!=null)
			{
				FTPFileTransferEventArgs e = new FTPFileTransferEventArgs(remoteFile, -1, false, false);
				InvokeEventHandler(Deleting, this, e);
				return !e.Cancel;
			}
			else
				return true;
		}

		/// <summary>
		/// Called when a file deletion operation has completed (though it may have been cancelled).
		/// </summary>
		/// <param name="remoteFile">File deleted.</param>
		/// <param name="cancelled"><c>true</c> if the operation was cancelled (and the file was not deleted).</param>
		protected virtual void OnDeleted(string remoteFile, bool cancelled)
		{
			if (areEventsEnabled && Deleted!=null)
				InvokeEventHandler(Deleted, this, new FTPFileTransferEventArgs(remoteFile, -1, false, cancelled));
		}

		/// <summary>
		/// Called when a file is about to be renamed.
		/// </summary>
		/// <param name="from">Current name.</param>
		/// <param name="to">New name.</param>
		protected virtual void OnRenaming(string from, string to)
		{            
			if (areEventsEnabled && RenamingFile!=null)
				InvokeEventHandler(RenamingFile, this, new FTPFileRenameEventArgs(from, to, false));
		}

		/// <summary>
		/// Called when a file has been renamed.
		/// </summary>
		/// <param name="from">Previous name.</param>
		/// <param name="to">New name.</param>
		/// <param name="renameCompleted">Indicates whether or not the rename operation was completed.</param>
		protected virtual void OnRenamed(string from, string to, bool renameCompleted)
		{            
			if (areEventsEnabled && RenamedFile!=null)
				InvokeEventHandler(RenamedFile, this, new FTPFileRenameEventArgs(from, to, renameCompleted));
		}

		#endregion

		#region Transfer Event Handlers

        /// <summary>
        /// Event-handler for <see cref="IFileTransferClient.TransferStartedEx"/> events received from <see cref="IFileTransferClient"/>s.
        /// </summary>
        /// <remarks>This method simply passes <see cref="IFileTransferClient.TransferStartedEx"/> events onto
        /// <see cref="Uploading"/>/<see cref="Downloading"/> handlers.</remarks>
        /// <param name="sender">Sender of events.</param>
        /// <param name="e">Event arguments.</param>
        protected internal void ftpClient_TransferStartedEx(object sender, TransferEventArgs e)
		{
			if (isTransferringMultiple)
			{
				switch (e.Direction)
				{
					case TransferDirection.UPLOAD:
						OnUploading(e.LocalFilePath, e.RemoteFilename, false);
						break;
					case TransferDirection.DOWNLOAD:
						OnDownloading(e.LocalFilePath, e.RemoteFilename);
						break;
				}
			}
		}

        /// <summary>
        /// Event-handler for <see cref="IFileTransferClient.TransferCompleteEx"/> events received from <see cref="IFileTransferClient"/>s.
        /// </summary>
        /// <remarks>This method simply passes <see cref="IFileTransferClient.TransferCompleteEx"/> events onto
        /// <see cref="Uploading"/>/<see cref="Downloading"/> handlers.</remarks>
        /// <param name="sender">Sender of events.</param>
        /// <param name="e">Event arguments.</param>
        protected internal void ftpClient_TransferCompleteEx(object sender, TransferEventArgs e)
		{
			if (isTransferringMultiple)
			{
				switch (e.Direction)
				{
					case TransferDirection.UPLOAD:
						OnUploaded(e.LocalFilePath, e.RemoteFilename, false, false);
						break;
					case TransferDirection.DOWNLOAD:
						OnDownloaded(e.LocalFilePath, e.RemoteFilename, false);
						break;
				}
			}
		}

        /// <summary>
        /// Event-handler for <see cref="IFileTransferClient.BytesTransferred"/> events received from <see cref="IFileTransferClient"/>s.
        /// </summary>
        /// <remarks>This method simply passes <see cref="IFileTransferClient.BytesTransferred"/> events onto
        /// <see cref="BytesTransferred"/> handlers.</remarks>
        /// <param name="sender">Sender of events.</param>
        /// <param name="e">Event arguments.</param>
        protected internal void ftpClient_BytesTransferred(object sender, BytesTransferredEventArgs e)
        {
            OnBytesTransferred(e.RemoteFile, e.ByteCount);
        }

		#endregion

        #region Helper Methods

        /// <summary> 
        /// Checks if the client has connected to the server and throws an exception if it hasn't.
        /// This is only intended to be used by subclasses
        /// </summary>
        /// <throws>FTPException Thrown if the client has not connected to the server. </throws>
        protected void CheckConnection(bool shouldBeConnected)
        {
            if (shouldBeConnected && !activeClient.IsConnected)
                throw new FTPException("The FTP client has not yet connected to the server.  " +
                "The requested action cannot be performed until after a connection has been established.");
            else if (!shouldBeConnected && activeClient.IsConnected)
                throw new FTPException("The FTP client has already been connected to the server.  " +
                "The requested action must be performed before a connection is established.");
        }
 
        /// <summary>
        /// Checks the FTP type and throws and exception if it's incorrect.
        /// </summary>
        /// <param name="ftpOnly"><c>true</c> if the type must be FTP.</param>
        protected virtual void CheckFTPType(bool ftpOnly)
        {
        }

        #endregion
    }

    #region FTPDirectoryEvent Args and Delegate

    /// <summary>
	/// Provides data for the <see cref="FTPConnection.DirectoryChanging"/> and
	/// <see cref="FTPConnection.DirectoryChanged"/> events.
	/// </summary>
	public class FTPDirectoryEventArgs : EventArgs
	{
		private string oldDirectory;
		private string newDirectory;

		internal FTPDirectoryEventArgs(string oldDirectory, string newDirectory)
		{
			this.oldDirectory = oldDirectory;
			this.newDirectory = newDirectory;
		}

		/// <summary>
		/// Working directory prior to change.
		/// </summary>
		public string OldDirectory
		{
			get
			{
				return oldDirectory;
			}
		}

		/// <summary>
		/// Working directory after change.
		/// </summary>
		public string NewDirectory
		{
			get
			{
				return newDirectory;
			}
		}
	}

	/// <summary>
	/// Represents the method that will handle the <see cref="FTPConnection.DirectoryChanging"/>
	/// and <see cref="FTPConnection.DirectoryChanged"/> events.
	/// </summary>
	public delegate void FTPDirectoryEventHandler(object sender, FTPDirectoryEventArgs e);

	#endregion

	#region FTPFileTransferEvent Args and Delegate

	/// <summary>
	/// Provides data for the <see cref="FTPConnection.Uploading"/>, <see cref="FTPConnection.Uploaded"/>,
	/// <see cref="FTPConnection.Downloading"/>, and <see cref="FTPConnection.Downloaded"/> events.
	/// </summary>
	public class FTPFileTransferEventArgs : EventArgs
	{
		/// <summary>
		/// Type of data source or destination.
		/// </summary>
		public enum DataType 
		{ 
			/// <summary>File data source/destination.</summary>
			File,
			/// <summary>Stream data source/destination.</summary>
			Stream, 
			/// <summary>Byte-array data source/destination.</summary>
			ByteArray 
		};

		private DataType localDataType;
		private string localFilePath;
		private Stream dataStream;
		private byte[] byteArray;
		private bool append;
		private string remoteFileName;
		private long fileSize;
		private bool cancelTransfer;

		internal FTPFileTransferEventArgs(string localFilePath, string remoteFileName, long fileSize, bool append, bool cancelled)
		{
			this.localDataType = DataType.File;
			this.localFilePath = localFilePath;
			this.remoteFileName = remoteFileName;
			this.fileSize = fileSize;
			this.append = append;
			this.cancelTransfer = cancelled;
		}

		internal FTPFileTransferEventArgs(Stream dataStream, string remoteFileName, long fileSize, bool append, bool cancelled)
		{
			this.localDataType = DataType.Stream;
			this.dataStream = dataStream;
			this.remoteFileName = remoteFileName;
			this.fileSize = fileSize;
			this.append = append;
			this.cancelTransfer = cancelled;
		}

		internal FTPFileTransferEventArgs(byte[] bytes, string remoteFileName, long fileSize, bool append, bool cancelled)
		{
			this.localDataType = DataType.ByteArray;
			this.byteArray = bytes;
			this.remoteFileName = remoteFileName;
			this.fileSize = fileSize;
			this.append = append;
			this.cancelTransfer = cancelled;
		}

		internal FTPFileTransferEventArgs(string remoteFileName, long fileSize, bool append, bool cancelled)
		{
			this.localDataType = DataType.ByteArray;
			this.byteArray = null;
			this.remoteFileName = remoteFileName;
			this.fileSize = fileSize;
			this.append = append;
			this.cancelTransfer = cancelled;
		}

		/// <summary>
		/// Type of local data source/destination.
		/// </summary>
		public DataType LocalDataType
		{
			get
			{
				return localDataType;
			}
		}

		/// <summary>
		/// Path of local file if <see cref="LocalDataType"/> is <c>File</c>.
		/// </summary>
		public string LocalPath
		{
			get
			{
				return localFilePath;
			}
		}

		/// <summary>
		/// Reference to <see cref="Stream"/> if <see cref="LocalDataType"/> is <c>Stream</c>.
		/// </summary>
		public Stream Stream
		{
			get
			{
				return dataStream;
			}
		}

		/// <summary>
		/// Reference to byte-array if <see cref="LocalDataType"/> is <c>ByteArray</c>.
		/// </summary>
		public byte[] Bytes
		{
			get
			{
				return byteArray;
			}
		}

		/// <summary>
		/// Indicates whether or not data was appended to the remote file.
		/// </summary>
		public bool Appended
		{
			get
			{
				return append;
			}
		}

		/// <summary>
		/// Name of remote file.
		/// </summary>
		public string RemoteFile
		{
			get
			{
				return remoteFileName;
			}
		}

		/// <summary>
		/// Size of remote file.
		/// </summary>
		public long FileSize
		{
			get
			{
				return fileSize;
			}
		}

		/// <summary>
		/// Cancel transfer.
		/// </summary>
		/// <remarks>
		/// <para>
		/// For <see cref="FTPConnection.Uploading"/> and <see cref="FTPConnection.Downloading"/>
		/// this flag may be set to <c>false</c> if the operation is to be aborted.
		/// For <see cref="FTPConnection.Uploaded"/> and <see cref="FTPConnection.Downloaded"/>
		/// this flag indicates if the operation was aborted.
		/// </para>
		/// <para>
		/// Note that multiple file transfers cannot be cancelled.
		/// </para>
		/// </remarks>
		public bool Cancel
		{
			get
			{
				return this.cancelTransfer;
			}
			set
			{
				this.cancelTransfer = value;
			}
		}
	}

	/// <summary>
	/// Represents the method that will handle the <see cref="FTPConnection.Uploading"/>, 
	/// <see cref="FTPConnection.Uploaded"/>, <see cref="FTPConnection.Downloading"/>, 
	/// and <see cref="FTPConnection.Downloaded"/> events.
	/// </summary>
	public delegate void FTPFileTransferEventHandler(object sender, FTPFileTransferEventArgs e);

	#endregion

	#region FTPFileRenameEvent Args and Handler

	/// <summary>
	/// Provides data for the <see cref="FTPConnection.RenamingFile"/>
	/// and <see cref="FTPConnection.RenamedFile"/> events.
	/// </summary>
	public class FTPFileRenameEventArgs : EventArgs
	{
		private string oldFileName;
		private string newFileName;
		private bool renameCompleted;

		internal FTPFileRenameEventArgs(string oldFileName, string newFileName, bool renameCompleted)
		{
			this.oldFileName = oldFileName;
			this.newFileName = newFileName;
			this.renameCompleted = renameCompleted;
		}

		/// <summary>
		/// Name of file before the renaming takes place.
		/// </summary>
		public string OldFileName
		{
			get
			{
				return oldFileName;
			}
		}

		/// <summary>
		/// Name of file after the renaming takes place.
		/// </summary>
		public string NewFileName
		{
			get
			{
				return newFileName;
			}
		}

		/// <summary>
		/// Indicates whether or not the renaming operation has been completed successfully.
		/// </summary>
		public bool RenameCompleted
		{
			get
			{
				return renameCompleted;
			}
		}
	}

	/// <summary>
	/// Represents the method that will handle the <see cref="FTPConnection.RenamingFile"/>
	/// and <see cref="FTPConnection.RenamedFile"/> events.
	/// </summary>
	public delegate void FTPFileRenameEventHandler(object sender, FTPFileRenameEventArgs e);

	#endregion

	#region FTPLogInEvent Args and Handler

	/// <summary>
	/// Provides data for the <see cref="FTPConnection.LoggingIn"/>
	/// and <see cref="FTPConnection.LoggedIn"/> events.
	/// </summary>
	public class FTPLogInEventArgs : EventArgs
	{
		private string userName;
		private string password;
		private bool hasLoggedIn;

		internal FTPLogInEventArgs(string userName, string password, bool hasLoggedIn)
		{
			this.userName = userName;
			this.password = password;
			this.hasLoggedIn = hasLoggedIn;
		}

		/// <summary>
		/// User-name of account on server.
		/// </summary>
		public string UserName
		{
			get
			{
				return userName;
			}
		}

		/// <summary>
		/// Password of account on server.
		/// </summary>
		public string Password
		{
			get
			{
				return password;
			}
		}

		/// <summary>
		/// Indicates whether or not the client has logged in.
		/// </summary>
		public bool HasLoggedIn
		{
			get
			{
				return hasLoggedIn;
			}
		}
	}

	/// <summary>
	/// Represents the method that will handle the <see cref="FTPConnection.LoggingIn"/>
	/// and <see cref="FTPConnection.LoggedIn"/> events.
	/// </summary>
	public delegate void FTPLogInEventHandler(object sender, FTPLogInEventArgs e);

	#endregion

	#region FTPConnectionEvent Args and Handler

	/// <summary>
	/// Provides data for the <see cref="FTPConnection.Connecting"/>
	/// and <see cref="FTPConnection.Connected"/> events.
	/// </summary>
	public class FTPConnectionEventArgs : EventArgs
	{
		private string serverAddress;
		private int serverPort;
		private bool connected;

		internal FTPConnectionEventArgs(string serverAddress, int serverPort, bool connected)
		{
			this.serverAddress = serverAddress;
			this.serverPort = serverPort;
			this.connected = connected;
		}

		/// <summary>
		/// Address of server.
		/// </summary>
		public string ServerAddress
		{
			get
			{
				return serverAddress;
			}
		}

		/// <summary>
		/// FTP port on server.
		/// </summary>
		public int ServerPort
		{
			get
			{
				return this.serverPort;
			}
		}

		/// <summary>
		/// Indicates whether or not the client is now connected to the server.
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return this.connected;
			}
		}
	}

	/// <summary>
	/// Represents the method that will handle the <see cref="FTPConnection.Connecting"/>
	/// and <see cref="FTPConnection.Connected"/> events.
	/// </summary>
	public delegate void FTPConnectionEventHandler(object sender, FTPConnectionEventArgs e);

	#endregion
}
