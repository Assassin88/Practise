using System;
using System.IO;
using System.Reflection;

namespace Sababa.Data.Storage.Classes
{
    public static class StorageSettings
    {
        /// <summary>
        /// Creates a directory for the specified path if it does not exist
        /// </summary>
        /// <param name="path">Directory name for create</param>
        /// <returns>Returns true if directory was not created </returns>
        public static bool CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the full path to the directory
        /// </summary>
        /// <param name="directoryName">Directory name</param>
        /// <returns>Returns full path</returns>
        public static string GetPathDirectory(string directoryName)
        {
            return String.IsNullOrEmpty(directoryName) ? $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\KeyValueStorage\\"
                : $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\{directoryName}\\";
        }

        /// <summary>
        /// Returns new password or default password if parameter is null or empty
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Returns password</returns>
        public static string GetPassword(string password)
        {
            return String.IsNullOrEmpty(password) ? "<P.A.$.$.W.0.R.D>" : password;
        }
    }
}
