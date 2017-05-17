
using System.Net.Http;
using System.Net.Http.Headers;
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
}