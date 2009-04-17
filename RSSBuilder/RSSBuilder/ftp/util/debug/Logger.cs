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
// $Log: Logger.cs,v $
// Revision 1.15  2006/06/14 10:13:36  hans
// Fixed comments and added .NET 2.0 compatibility
//
// Revision 1.14  2006/05/03 06:38:30  bruceb
// log levels now can be enums or INFO etc
//
// Revision 1.13  2006/04/20 05:40:26  hans
// Fixed comment error.
//
// Revision 1.12  2006/03/16 22:19:24  hans
// Added simple properties to control logging.  These are used by FTPConnection, but also provide a really simple interface for the user.
//
// Revision 1.11  2006/02/16 20:19:41  hans
// Added RemoveAppender feature.
//
// Revision 1.10  2006/01/08 19:05:49  bruceb
// change msg about param not found to INFO
//
// Revision 1.9  2005/12/13 20:19:11  hans
// Added properties for controlling logging output format.
//
// Revision 1.8  2005/02/07 17:22:39  bruceb
// make sure Exception message included in log
//
// Revision 1.7  2004/12/22 22:58:17  bruceb
// fixed bad level problem
//
// Revision 1.6  2004/11/13 18:20:52  bruceb
// clear appenders/loggers in shutdown
//
// Revision 1.5  2004/11/06 11:15:24  bruceb
// namespace tidying up
//
// Revision 1.4  2004/10/29 09:42:30  bruceb
// removed /// from file headers
//
//
//

using System;
using System.Globalization;
using System.Collections;
using System.Configuration;

namespace EnterpriseDT.Util.Debug
{  
	/// <summary>  
	/// Logger class that mimics log4net Logger class
	/// </summary>
	/// <author>       Bruce Blackshaw
	/// </author>
	/// <version>      $Revision: 1.15 $
	/// </version>
	public class Logger
	{
		/// <summary> 
		/// Set all loggers to this level
		/// </summary>
		public static Level CurrentLevel
		{
			set
			{
				globalLevel = value;
			}	
            get
            {
                return globalLevel;
            }
		}
        		
		/// <summary>If true then class-names will be shown in log.</summary>
		public static bool ShowClassNames
		{
			get
			{
				return showClassNames;
			}
			set
			{
				showClassNames = value;
			}
		}

		/// <summary>If true then timestamps will be shown in log.</summary>
		public static bool ShowTimestamp
		{
			get
			{
				return showTimestamp;
			}
			set
			{
				showTimestamp = value;
			}
		}

		/// <summary> 
		/// Is debug logging enabled?
		/// </summary>
		/// <returns> true if enabled
		/// </returns>
		virtual public bool DebugEnabled
		{
			get
			{
				return IsEnabledFor(Level.DEBUG);
			}
			
		}
		/// <summary> Is info logging enabled for the supplied level?
		/// 
		/// </summary>
		/// <returns> true if enabled
		/// </returns>
		virtual public bool InfoEnabled
		{
			get
			{
				return IsEnabledFor(Level.INFO);
			}
			
		}

		/// <summary>
		/// The primary log file is simply the first file appender
		/// that has been added to the logger.
		/// </summary>
		public static string PrimaryLogFile
		{
			get
			{
				return mainFileAppender!=null ? mainFileAppender.FileName : null;
			}
			set
			{
				if (mainFileAppender==null)
					AddAppender(new FileAppender(value));
				else if (mainFileAppender.FileName!=value)
				{
					RemoveAppender(mainFileAppender);
					AddAppender(new FileAppender(value));
				}
			}
		}

		/// <summary>
		/// If this property is <c>true</c> then logs will be written to the
		/// console.
		/// </summary>
		public static bool LogToConsole
		{
			get
			{
				return mainConsoleAppender!=null;
			}
			set
			{
				if (value==true)
				{
					if (mainConsoleAppender==null)
						AddAppender(new StandardOutputAppender());
				}
				else
				{
					if (mainConsoleAppender!=null)
						RemoveAppender(mainConsoleAppender);
				}
			}
		}

		/// <summary>
		/// If this property is <c>true</c> then logs will be written using
		/// <see cref="System.Diagnostics.Trace"/>.
		/// </summary>
		public static bool LogToTrace
		{
			get
			{
				return mainTraceAppender!=null;
			}
			set
			{
				if (value==true)
				{
					if (mainTraceAppender==null)
						AddAppender(new TraceAppender());
				}
				else
				{
					if (mainTraceAppender!=null)
						RemoveAppender(mainTraceAppender);
				}
			}
		}
		
		/// <summary> Level of all loggers</summary>
		private static Level globalLevel;
		
		/// <summary>Date format</summary>
		private static readonly string format = "d MMM yyyy HH:mm:ss.fff";
        
        private static readonly string LEVEL_PARAM = "edtftp.log.level";
		
		/// <summary> Hash of all loggers that exist</summary>
		private static Hashtable loggers = Hashtable.Synchronized(new Hashtable(10));
		
		/// <summary> Vector of all appenders</summary>
		private static ArrayList appenders = ArrayList.Synchronized(new ArrayList(2));
				
		/// <summary> Timestamp</summary>
		private DateTime ts;
		
		/// <summary> Class name for this logger</summary>
		private string clazz;

		/// <summary>If true then class-names will be shown in log.</summary>
		private static bool showClassNames = true;

		/// <summary>If true then timestamps will be shown in log.</summary>
		private static bool showTimestamp = true;

		/// <summary>Main file appender</summary>
		private static FileAppender mainFileAppender = null;

		/// <summary>Main file appender</summary>
		private static StandardOutputAppender mainConsoleAppender = null;

		/// <summary>Main file appender</summary>
		private static TraceAppender mainTraceAppender = null;

		/// <summary> 
		/// Constructor
		/// </summary>
		/// <param name="clazz">    
		/// class this logger is for
		/// </param>
		private Logger(string clazz)
		{
			this.clazz = clazz;
		}
		
		
		/// <summary> Get a logger for the supplied class
		/// 
		/// </summary>
		/// <param name="clazz">   full class name
		/// </param>
		/// <returns>  logger for class
		/// </returns>
		public static Logger GetLogger(System.Type clazz)
		{
			return GetLogger(clazz.FullName);
		}
		
		/// <summary> 
		/// Get a logger for the supplied class
		/// </summary>
		/// <param name="clazz">   full class name
		/// </param>
		/// <returns>  logger for class
		/// </returns>
		public static Logger GetLogger(string clazz)
		{
			Logger logger = (Logger) loggers[clazz];
			if (logger == null)
			{
				logger = new Logger(clazz);
				loggers[clazz] = logger;
			}
			return logger;
		}
		
		/// <summary> 
		/// Add an appender to our list
		/// </summary>
		/// <param name="newAppender">
		/// new appender to add
		/// </param>
		public static void AddAppender(Appender newAppender)
		{
			appenders.Add(newAppender);
			if (newAppender is FileAppender && mainFileAppender==null)
				mainFileAppender = (FileAppender)newAppender;
			if (newAppender is StandardOutputAppender && mainConsoleAppender==null)
				mainConsoleAppender = (StandardOutputAppender)newAppender;
			if (newAppender is TraceAppender && mainTraceAppender==null)
				mainTraceAppender = (TraceAppender)newAppender;
		}

		/// <summary> 
		/// Remove an appender from our list
		/// </summary>
		/// <param name="appender">appender to remove</param>
		public static void RemoveAppender(Appender appender)
		{
			appenders.Remove(appender);
			if (appender==mainFileAppender)
				mainFileAppender = null;
			if (appender==mainConsoleAppender)
				mainConsoleAppender = null;
			if (appender==mainTraceAppender)
				mainTraceAppender = null;
		}

		/// <summary> Close and remove all appenders and loggers</summary>
		public static void Shutdown()
		{
			for (int i = 0; i < appenders.Count; i++)
			{
				Appender a = (Appender) appenders[i];
				a.Close();
			}
			loggers.Clear();
			appenders.Clear();
		}
		
		/// <summary> Log a message 
		/// 
		/// </summary>
		/// <param name="level">    log level
		/// </param>
		/// <param name="message">  message to log
		/// </param>
		/// <param name="t">        throwable object
		/// </param>
		public virtual void Log(Level level, string message, Exception t)
		{
			if (globalLevel.IsGreaterOrEqual(level))
				OurLog(level, message, t);
		}
		
		/// <summary> 
		/// Log a message to our logging system
		/// </summary>
		/// <param name="level">    log level
		/// </param>
		/// <param name="message">  message to log
		/// </param>
		/// <param name="t">        throwable object
		/// </param>
		private void OurLog(Level level, string message, Exception t)
		{
            ts = DateTime.Now;
			string stamp = ts.ToString(format, CultureInfo.CurrentCulture.DateTimeFormat);
			System.Text.StringBuilder buf = new System.Text.StringBuilder(level.ToString());
			if (showClassNames)
			{
				buf.Append(" [");
				buf.Append(clazz);
				buf.Append("]");
			}
			if (showTimestamp)
			{
				buf.Append(" ");
				buf.Append(stamp);
			}
			buf.Append(" : ");
			buf.Append(message);
			if (t != null)
                buf.Append(" : ").Append(t.Message);
			if (appenders.Count == 0)
			{
				// by default to stdout
				System.Console.Out.WriteLine(buf.ToString());
				if (t != null)
				{
                    System.Console.Out.WriteLine(t.StackTrace);
					if (t.InnerException!=null)
					{
						System.Console.Out.WriteLine(
							string.Format("CAUSED BY - {0}: {1}\r\n{2}", 
							t.InnerException.GetType().FullName, 
							t.InnerException.Message, 
							t.StackTrace));
					}
				}
			}
			else
			{
				for (int i = 0; i < appenders.Count; i++)
				{
					Appender a = (Appender) appenders[i];
					a.Log(buf.ToString());
					if (t != null)
					{
						a.Log(t);
						if (t.InnerException!=null)
						{
							a.Log("CAUSED BY:");
							a.Log(t.InnerException);
						}
					}
				}
			}
		}
		
		/// <summary> Log an info level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
		public virtual void Info(string message)
		{
			Log(Level.INFO, message, null);
		}
		
		/// <summary> Log an info level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
		/// <param name="t">        throwable object
		/// </param>
		public virtual void Info(string message, Exception t)
		{
			Log(Level.INFO, message, t);
		}
				
		/// <summary> Log an info level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
        /// <param name="args">arguments references in the message.
        /// </param>
        public virtual void Info(string message, params object[] args)
		{
			if (IsEnabledFor(Level.INFO))
				Log(Level.INFO, string.Format(message, args), null);
		}

		/// <summary> Log a warning level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
		public virtual void  Warn(string message)
		{
			Log(Level.WARN, message, null);
		}
		
		/// <summary> Log a warning level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
		/// <param name="t">        throwable object
		/// </param>
		public virtual void  Warn(string message, Exception t)
		{
			Log(Level.WARN, message, t);
		}
		
		/// <summary> Log an error level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
		public virtual void Error(string message)
		{
			Log(Level.ERROR, message, null);
		}
		
		/// <summary> Log an error level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
		/// <param name="t">        throwable object
		/// </param>
		public virtual void Error(string message, Exception t)
		{
			Log(Level.ERROR, message, t);
		}
		
		/// <summary> Log a fatal level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
		public virtual void Fatal(string message)
		{
			Log(Level.FATAL, message, null);
		}
		
		/// <summary> Log a fatal level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
		/// <param name="t">        throwable object
		/// </param>
		public virtual void Fatal(string message, Exception t)
		{
			Log(Level.FATAL, message, t);
		}
		
		/// <summary> Log a debug level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
		public virtual void Debug(string message)
		{
			Log(Level.DEBUG, message, null);
		}
		
		/// <summary> Log a debug level message
		/// 
		/// </summary>
		/// <param name="message">message to log
        /// </param>
        /// <param name="args">arguments references in the message.
        /// </param>
		public virtual void Debug(string message, params object[] args)
		{
			if (IsEnabledFor(Level.DEBUG))
				Log(Level.DEBUG, string.Format(message, args), null);
		}
		
		
		/// <summary> Log a debug level message
		/// 
		/// </summary>
		/// <param name="message">  message to log
		/// </param>
		/// <param name="t">        throwable object
		/// </param>
		public virtual void Debug(string message, Exception t)
		{
			Log(Level.DEBUG, message, t);
		}
		
		/// <summary> Is logging enabled for the supplied level?
		/// 
		/// </summary>
		/// <param name="level">  level to test for
		/// </param>
		/// <returns> true   if enabled
		/// </returns>
		public virtual bool IsEnabledFor(Level level)
		{
			if (globalLevel.IsGreaterOrEqual(level))
				return true;
			return false;
		}
        
		/// <summary> Determine the logging level</summary>
		static Logger()
		{
			{
                globalLevel = null;
//#if NET20
                //string level = ConfigurationManager.AppSettings[LEVEL_PARAM];
//#else
				string level = ConfigurationSettings.AppSettings[LEVEL_PARAM];
//#endif
                if (level != null)
                {
                    // first try with the strings INFO etc
				    globalLevel = Level.GetLevel(level);
                    if (globalLevel == null)
                    {
                        try
                        {
                            // now try from the enum
                            LogLevel logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), level, true);
                            globalLevel = Level.GetLevel(logLevel);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                
                // if no level set, switch off
                if (globalLevel == null)
                {
                    globalLevel = Level.OFF;
                    if (level != null) 
                    {
                        System.Console.Out.WriteLine("WARN: '" + LEVEL_PARAM + "' configuration property invalid. Unable to parse '" + level + "' - logging switched off");
                    }
                }
			}
		}
	}
}
