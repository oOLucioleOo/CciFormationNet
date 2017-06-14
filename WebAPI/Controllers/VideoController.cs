using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;
using Entity;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class VideoController : ApiController
    {

        private readonly double _latency = 3;

        [HttpGet]
        public HttpResponseMessage Get(string filename, string ext)
        {
            var video = new VideoStream(filename, ext);
            var mediaType = new MediaTypeHeaderValue("video/" + ext);

            var response = Request.CreateResponse();
            response.Content = new PushStreamContent(video.WriteToStream, mediaType);

            return response;
        }

        [HttpGet]
        public void Notify()
        {
            var hubContext = HubManager.Instance;

            hubContext.StartStream();
        }

        [HttpPost]
        public void uploadStream(Video video)
        {
            Console.WriteLine(Request.Content.ToString());
            try
            {
                //Sauvegarde du fichier reçu dans le StorageTemp
                File.WriteAllBytes(string.Format(HostingEnvironment.ApplicationPhysicalPath + "StorageTemp\\" + video.name + video.currentBlob + video.extension), video.content);

                if ((video.isLast) || (video.currentBlob % _latency == 0))
                {
                    concatVideo(video);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }  
        }

        private void concatVideo(Video video)
        {
            FileStream fsAll = null;
            try
            {
                fsAll = new FileStream(video.name + Guid.NewGuid() + video.extension, FileMode.Create);
            
                double count = 0;

                if ((video.currentBlob % _latency == 0) || ((video.currentBlob % _latency == 0) && (video.isLast)))
                {
                    count = video.currentBlob - _latency;
                }
                else if (video.isLast)
                {
                    count = video.currentBlob - (video.currentBlob % _latency);
                }

                for (double i = count; i < video.currentBlob; i++)
                {
                    FileStream fs = new FileStream(
                        string.Format(HostingEnvironment.ApplicationPhysicalPath + "StorageTemp\\" + video.name + i
                                      + video.extension), FileMode.Open);
                    fs.CopyTo(fsAll);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                fsAll?.Close();
            }
        }
    }
}