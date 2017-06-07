using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    class Watcher
    {
        public static void Send(string directory)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = directory;
            watcher.NotifyFilter = NotifyFilters.DirectoryName;

            watcher.Filter = "*.avi";

            watcher.Created += new FileSystemEventHandler(onCreated);

            watcher.EnableRaisingEvents = true;

        }

        private static void onCreated(object source, FileSystemEventArgs e)
        {
            //Code de l'envoi à faire la semaine prochaine 
        }
    }
}
