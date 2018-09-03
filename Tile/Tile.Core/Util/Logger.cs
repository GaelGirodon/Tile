using System;
using System.IO;
using System.Text;

namespace Tile.Core.Util
{
    /// <summary>
    /// Basic logger
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// File containing log
        /// </summary>
        public string LogFile
        {
            get { return _logFile; }
            set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("The log file can't be empty");
                _logFile = value;
            }
        }
        private string _logFile;

        /// <summary>
        /// Initialize the logger
        /// </summary>
        /// <param name="file">File containing log</param>
        public Logger(string file)
        {
            LogFile = file;
        }

        /// <summary>
        /// Initialize the log file
        /// </summary>
        public void Init()
        {
            try
            {
                Log("======================= " + DateTime.Now + " =======================");
            }
            catch (Exception e)
            {
                throw new IOException($"Unable to write in the log file: {LogFile}", e);
            }
        }

        #region Logging

        /// <summary>
        /// Log a message only in the log file
        /// </summary>
        /// <param name="message">The message to log</param>
        private void Log(string message)
        {
            File.AppendAllText(LogFile, message + Environment.NewLine);
        }

        /// <summary>
        /// Write a success message
        /// </summary>
        /// <param name="message">The message to write</param>
        public void Success(string message)
        {
            Console.WriteLine("SUCCESS\t  " + message);
            Log("SUCCESS\t  " + message);
        }

        /// <summary>
        /// Write an information message
        /// </summary>
        /// <param name="message">The message to write</param>
        public void Info(string message)
        {
            Console.WriteLine("INFO\t  " + message);
            Log("INFO\t  " + message);
        }

        /// <summary>
        /// Write a warning message
        /// </summary>
        /// <param name="message">The message to write</param>
        public void Warning(string message)
        {
            Console.WriteLine("WARNING\t  " + message);
            Log("WARNING\t  " + message);
        }

        /// <summary>
        /// Write an error message
        /// </summary>
        /// <param name="message">The message to write</param>
        public void Error(string message)
        {
            Console.Error.WriteLine("ERROR\t  " + message);
            Log("ERROR\t  " + message);
        }

        #endregion
    }
}
