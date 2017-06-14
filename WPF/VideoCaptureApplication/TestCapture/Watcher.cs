using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Net.Http.Formatting;

namespace VideoCaptureApplication.TestCapture
{
    class Watcher
    {
        public static void Send(string directory)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = directory;
            watcher.NotifyFilter = NotifyFilters.DirectoryName;

            //Only watch avi files
            watcher.Filter = "*.avi";

            watcher.Created += new FileSystemEventHandler(onCreated);

            //Begin watching
            watcher.EnableRaisingEvents = true;

        }
        //Define the event handlers
        private static void onCreated(object source, FileSystemEventArgs e)
        {

            //Création du client http
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("Http://localhost:63315");

            //Header du client
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/bson"));

            string videoFilename = e.FullPath;
            Byte[] content = File.ReadAllBytes(videoFilename);
            //Pour le current blob, recupérer le dernier caractere du nom --> le mettre en double et boom

            Video obj = new Video(content, "StreamVideo", false, 1,".avi");





            /*
            //Sauvegarde du content dans une variable
            string videoFilename = Path.GetFileNameWithoutExtension(fileName) + chunkNumber;
            Byte[] content = File.ReadAllBytes(videoFilename);

            //Création de l'objet video avec le filename sans incrémentation
            Video fichier = new Video(content, Path.GetFileNameWithoutExtension(fileName), false, chunkNumber);

            //Envoi de la video
            MediaTypeFormatter bsonFormatter = new BsonMediaTypeFormatter();
            //var response = await client.PostAsync("/api/video/uploadStream", fichier, bsonFormatter);
            */
        }
    }
}

