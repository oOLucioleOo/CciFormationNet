
using System;
using System.IO;
using Newtonsoft.Json;
using VideoCaptureApplication.Utils.Constants;

namespace VideoCaptureApplication.Utils.Helpers
{
    public class JsonUtils
    {
        public static object ReadJsonFile(string dataPath, Type resultType)
        {
            object result;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText($@"{dataPath}\{AppConstants.FileName}"))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize(file, resultType);
            }

            return result;
        }
    }
}
