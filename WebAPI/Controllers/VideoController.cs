
using Entity;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;



public class VideoController : ApiController
{
    [HttpGet]
    public HttpResponseMessage Get(string filename, string ext)
    {
        var video = new VideoStream(filename, ext);
        var mediaType = new MediaTypeHeaderValue("video/" + ext);

        var response = Request.CreateResponse();
        response.Content = new PushStreamContent(video.WriteToStream, mediaType);

        return response;
    }

    [HttpPost]
    public void uploadStream(Video video)
    {
        try
        {
            if(video.current == video.count - 1)
            {
                File.WriteAllBytes(string.Format(HostingEnvironment.ApplicationPhysicalPath + "StorageTemp\\" + video.name + ".mp4"), video.size);

                //MemoryStream
                /*if (concatenateVideo(video))
                {
                    Console.WriteLine("Enregistrement de la video");
                }*/

            }
            else
            {
                File.WriteAllBytes(string.Format(HostingEnvironment.ApplicationPhysicalPath + "StorageTemp\\" + video.name + ".mp4"), video.size);
            }

        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }  
    }

    private Boolean concatenateVideo(Video video)
    {
        try
        {
            FileStream fsAll = new FileStream("video.mp4", FileMode.Create);
            for(int i =0; i<video.count-1; i++)
            {
                FileStream fs = new FileStream(string.Format(HostingEnvironment.ApplicationPhysicalPath + "StorageTemp\\Good morning gif"+i+".mp4"), FileMode.Open);
                fs.CopyTo(fsAll);
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}