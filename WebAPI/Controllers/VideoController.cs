
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

    private void concatenateVideo()
    {
        try
        {
            FileStream fsAll = new FileStream("video.mp4", FileMode.Create);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}