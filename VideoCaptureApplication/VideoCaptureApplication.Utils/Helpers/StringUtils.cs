
using System;
using System.IO;
using System.Reflection;

namespace VideoCaptureApplication.Utils.Helpers
{
    public static class StringUtils
    {
        /// <summary>
        ///   Gets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        ///   Gets the user directory path.
        /// </summary>
        /// <returns>The user directory path</returns>
        public static string GetUserDirectoryPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.None), "VideoCaptureApplication");
            }
        }

        /// <summary>
        /// Gets the app root directory.
        /// </summary>
        /// <returns>The application root directory</returns>
        public static string GetAppRootDirectory
        {
            get
            {
                var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                string path = string.Empty;
                if (directoryName != null)
                {
                    path = directoryName.Substring(6);
                }

                return path;
            }
        }
    }
}
