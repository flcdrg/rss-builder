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
// $Log: BaseSocket.cs,v $
// Revision 1.3  2006/06/14 10:11:50  hans
// Added empty comment blocks to get rid of warnings.  Needs comments.
//
// Revision 1.2  2006/04/18 01:36:41  bruceb
// timeout for Accept and close socket if timeout
//
// Revision 1.1  2006/03/17 23:09:52  hans
// Common base for sockets.
//
// Revision 1.2  2004/11/13 19:03:49  bruceb
// GetStream() changed, added comments
//
// Revision 1.1  2004/10/29 14:30:10  bruceb
// first cut
//
//

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace EnterpriseDT.Net
{
	/// <summary>  
	/// Socket abstraction that simplifies socket code
	/// </summary>
	/// <author>   
    /// Hans Andersen    
	/// </author>
	/// <version>      
    /// $Revision: 1.3 $
	/// </version>
	public abstract class BaseSocket
	{
	    /// <summary>
	    /// Creates a new Socket for a newly created connection
	    /// </summary>
        /// <param name="timeout">Accept timeout in milliseconds</param>
        public abstract BaseSocket Accept(int timeout);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public abstract IAsyncResult BeginAccept(AsyncCallback callback, Object state);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public abstract BaseSocket EndAccept(IAsyncResult asyncResult);

	    /// <summary>
	    /// Associates a Socket with a local endpoint.
	    /// </summary>
		public abstract void Bind(EndPoint localEP);

	    /// <summary>
	    /// Closes the Socket connection and releases all associated resources.
	    /// </summary>
		public abstract void Close();
        
	    /// <summary>
	    /// Establishes a connection to a remote endpoint
	    /// </summary>
		public abstract void Connect(EndPoint remoteEP);

	    /// <summary>
	    /// Places socket in a listening state.
	    /// </summary>
		public abstract void Listen(int backlog);

	    /// <summary>
	    /// Get the stream associated with the socket.
	    /// </summary>
	    /// <remarks>
	    /// The stream returned owns the socket, so closing the
	    /// stream will close the socket
	    /// </remarks>
		public abstract Stream GetStream();

	    /// <summary>
	    /// Receives data from a bound Socket.
	    /// </summary>
		public abstract int Receive(byte[] buffer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="socketFlags"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public abstract IAsyncResult BeginReceive(
            byte[] buffer,
            int offset,
            int size,
            SocketFlags socketFlags,
            AsyncCallback callback,
            Object state
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public abstract int EndReceive(IAsyncResult asyncResult);


	    /// <summary>
	    /// Sends data to a connected Socket.
	    /// </summary>
		public abstract int Send(byte[] buffer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="socketFlags"></param>
        /// <returns></returns>
        public abstract int Send(
            byte[] buffer,
            int offset,
            int size,
            SocketFlags socketFlags
        );


	    /// <summary>
	    /// Sets a Socket option.
	    /// </summary>
		public abstract void SetSocketOption(
            SocketOptionLevel optionLevel, 
            SocketOptionName optionName, 
            int optionValue);

	    /// <summary>
	    /// Gets the local endpoint.
	    /// </summary>
		public abstract EndPoint LocalEndPoint {get;}

        /// <summary>
        /// Gets the remote endpoint.
        /// </summary>
        public abstract EndPoint RemoteEndPoint {get;}

	}

	/// <summary>  
	/// Standard implementation of BaseSocket
	/// </summary>
	/// <author>   
    /// Hans Andersen    
	/// </author>
	/// <version>      
    /// $Revision: 1.3 $
	/// </version>
	public class StandardSocket : BaseSocket
	{
	    /// <summary>
	    /// The real socket this class is wrapping
	    /// </summary>
		private Socket socket;

	    /// <summary>
	    /// Initializes a new instance of the StandardSocket class
	    /// </summary>
 		public StandardSocket(
			AddressFamily addressFamily,
			SocketType socketType,
			ProtocolType protocolType
			)
		{
			socket = new Socket(addressFamily, socketType, protocolType);
		}

	    /// <summary>
	    /// Initializes a new instance of the StandardSocket class
	    /// </summary>
		protected StandardSocket(Socket socket)
		{
			this.socket = socket;
		}

	    /// <summary>
	    /// Creates a new Socket for a newly created connection
	    /// </summary>
	    /// <param name="timeout">Accept timeout in milliseconds</param>
		public override BaseSocket Accept(int timeout)
		{
            bool success = socket.Poll(timeout*1000, SelectMode.SelectRead);
            if (!success)
            {
                socket.Close();
                throw new IOException("Failed to accept connection within timeout period (" + timeout + ")");
            }
            return new StandardSocket(socket.Accept());
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            return socket.BeginAccept(callback, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public override BaseSocket EndAccept(IAsyncResult asyncResult)
        {
            return new StandardSocket(socket.EndAccept(asyncResult));
        }

	    /// <summary>
	    /// Associates a Socket with a local endpoint.
	    /// </summary>
		public override void Bind(EndPoint localEP)
		{
			socket.Bind(localEP);
		}

	    /// <summary>
	    /// Closes the Socket connection and releases all associated resources.
	    /// </summary>
		public override void Close()
		{
			socket.Close();
		}

	    /// <summary>
	    /// Establishes a connection to a remote endpoint
	    /// </summary>
		public override void Connect(EndPoint remoteEP)
		{
			socket.Connect(remoteEP);
		}

	    /// <summary>
	    /// Places socket in a listening state.
	    /// </summary>
		public override void Listen(int backlog)
		{
			socket.Listen(backlog);
		}

	    /// <summary>
	    /// Get the stream associated with the socket.
	    /// </summary>
		public override Stream GetStream()
		{
			return new NetworkStream(socket, true);
		}

	    /// <summary>
	    /// Receives data from a bound Socket.
	    /// </summary>
		public override int Receive(byte[] buffer)
		{
			return socket.Receive(buffer);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="socketFlags"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            return socket.BeginReceive(buffer, offset, size, socketFlags, callback, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public override int EndReceive(IAsyncResult asyncResult)
        {
            return socket.EndReceive(asyncResult);
        }

	    /// <summary>
	    /// Sends data to a connected Socket.
	    /// </summary>
		public override int Send(byte[] buffer)
		{
			return socket.Send(buffer);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="socketFlags"></param>
        /// <returns></returns>
        public override int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return socket.Send(buffer, offset, size, socketFlags);
        }

	    /// <summary>
	    /// Sets a Socket option.
	    /// </summary>
		public override void SetSocketOption(
            SocketOptionLevel optionLevel, 
            SocketOptionName optionName, 
            int optionValue)
		{
			socket.SetSocketOption(optionLevel, optionName, optionValue);
		}

	    /// <summary>
	    /// Gets the local endpoint.
	    /// </summary>
		public override EndPoint LocalEndPoint 
		{
			get
			{
				return socket.LocalEndPoint;
			}
		}

        /// <summary>
        /// Gets the remote end-point.
        /// </summary>
        public override EndPoint RemoteEndPoint
        {
            get 
            {
                return socket.RemoteEndPoint;
            }
        }
	}
}