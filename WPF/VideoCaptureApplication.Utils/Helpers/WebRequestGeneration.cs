using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using VideoCaptureApplication.Utils.Constants;

namespace VideoCaptureApplication.Utils.Helpers
{
    public class WebRequestGeneration
    {
        /// <summary>
        /// Creates the web request.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="webMethod">The web method.</param>
        /// <returns></returns>
        public static HttpWebRequest CreateWebRequest(string service, string methodName, string webMethod)
        {
            var uri = new Uri((service + methodName));
            //var uri = new Uri(string.Format(ConfigurationManager.AppSettings[AppConstants.ServiceUri], service, methodName));
            HttpWebRequest webRequest = WebRequest.CreateHttp(uri);

            string username = string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName);
            string password = string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName);

            //webRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password))));
            //webRequest.Headers.Add("Accept-Encoding", "gzip");
            //webRequest.Headers.Add("HostName", Dns.GetHostName());


            //webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (!string.IsNullOrEmpty(webMethod)) webRequest.Method = webMethod;

            webRequest.ContentType = "application/json; charset=utf-8";
            webRequest.Timeout = 300000;

            return webRequest;
        }

        /// <summary>
        /// Request the web service, without parameter
        /// </summary>
        /// <param name="webRequest">the httpRequest</param>
        /// <param name="successAction">Action called to return the result</param>
        /// <param name="resultType">Type sent by the webservice to deserialize and resent to successAction</param>
        public static string DoRequestWithoutParameterAndWithReturn(HttpWebRequest webRequest, Action<object> successAction, Type resultType)
        {
            string resultError = string.Empty;
            HttpWebResponse response = null;
            try
            {
                response = webRequest.GetResponse() as HttpWebResponse;

                if (response != null)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    }

                    Stream stream = response.GetResponseStream();
                    try
                    {
                        if (stream != null)
                        {
                            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(resultType);
                            object returnObject = deserializer.ReadObject(stream);
                            successAction.Invoke(returnObject);
                        }
                    }
                    catch (WebException exception)
                    {
                       // resultError = exception.GetInnerMostException().Message;
                        response = (HttpWebResponse)exception.Response;
                    }
                    catch (Exception exception)
                    {
                        //resultError = exception.GetInnerMostException().Message;
                    }
                    finally
                    {
                        if (stream != null)
                        {
                            stream.Close();
                            stream.Dispose();
                        }
                    }
                }
            }
            catch (WebException exception)
            {
                Stream responseStream = null;

                if (exception.Response != null)
                {
                    responseStream = exception.Response.GetResponseStream();
                }

                if (responseStream != null)
                {
                    try
                    {
                        //resultError = exception.GetInnerMostException().Message;

                       // DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(RequestResult));
                        //RequestResult returnObject = deserializer.ReadObject(responseStream) as RequestResult;

                        //if (returnObject != null)
                        //{
                        //    resultError += Environment.NewLine + returnObject.ErrorDetails;
                        //}
                    }
                    catch (Exception ex)
                    {
                        //resultError += Environment.NewLine + ex.GetInnerMostException().Message;
                    }
                }
                else
                {
                   // resultError = exception.GetInnerMostException().Message;
                }

                response = (HttpWebResponse)exception.Response;
            }
            catch (Exception exception)
            {
                //resultError = exception.GetInnerMostException().Message;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
            }

            return resultError;
        }

        public static string DoPutGetRequestWithReturn(HttpWebRequest webRequest, object datas, Action<object> successAction, Type resultType)
        {
            string resultError = string.Empty;
            HttpWebResponse response = null;
            Stream stream = null;

            try
            {
                stream = webRequest.GetRequestStream();

                if (datas != null)
                {
                    try
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(datas.GetType(),
                            new DataContractJsonSerializerSettings
                            {
                                DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ss'.000Z'")
                            });
                        serializer.WriteObject(stream, datas);
                    }
                    catch (WebException exception)
                    {
                        //resultError = exception.GetInnerMostException().Message;
                    }
                    catch (Exception exception)
                    {
                        //resultError = exception.GetInnerMostException().Message;
                    }
                    finally
                    {
                        stream.Close();
                        stream.Dispose();
                    }

                    response = webRequest.GetResponse() as HttpWebResponse;
                    if (response != null)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                        }

                        Stream streamResponse = response.GetResponseStream();
                        try
                        {
                            if (streamResponse != null)
                            {
                                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(resultType, new DataContractJsonSerializerSettings()
                                {
                                    UseSimpleDictionaryFormat = true
                                });

                                object returnObject = deserializer.ReadObject(streamResponse);
                                successAction.Invoke(returnObject);
                            }
                        }
                        catch (WebException exception)
                        {
                            //resultError += Environment.NewLine + exception.GetInnerMostException().Message;
                            response = (HttpWebResponse)exception.Response;
                        }
                        catch (Exception exception)
                        {
                           // resultError += Environment.NewLine + exception.GetInnerMostException().Message;
                        }
                        finally
                        {
                            if (streamResponse != null)
                            {
                                streamResponse.Close();
                                streamResponse.Dispose();
                            }
                        }
                    }
                }
            }
            catch (WebException exception)
            {
                Stream responseStream = null;

                if (exception.Response != null)
                {
                    responseStream = exception.Response.GetResponseStream();
                }

                if (responseStream != null)
                {
                    try
                    {
                        //DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(RequestResult));
                        //RequestResult returnObject = deserializer.ReadObject(responseStream) as RequestResult;

                        //if (returnObject != null)
                        //{
                        //    resultError = returnObject.ErrorDetails;
                        //}
                    }
                    catch (Exception ex)
                    {
                        //resultError = exception.GetInnerMostException().Message + Environment.NewLine + ex.GetInnerMostException().Message;
                    }
                }
                else
                {
                    //resultError = exception.GetInnerMostException().Message;
                }

                // error service 
                response = (HttpWebResponse)exception.Response;
            }
            catch (Exception exception)
            {
                //resultError = exception.GetInnerMostException().Message;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
            }

            return resultError;
        }

        public static string DoPutRequestWithReturn(HttpWebRequest webRequest, object datas, Action<object> successAction, Type resultType)
        {
            string resultError = string.Empty;
            Exception serverException = null;

            HttpWebResponse response = null;
            Stream stream = null;

            try
            {
                stream = webRequest.GetRequestStream();

                if (datas != null)
                {
                    try
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(datas.GetType(),
                            new DataContractJsonSerializerSettings
                            {
                                DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ss'.000Z'")
                            });
                        serializer.WriteObject(stream, datas);
                    }
                    catch (WebException exception)
                    {
                        resultError = exception.Message;
                    }
                    catch (Exception exception)
                    {
                        //resultError = exception.GetInnerMostException().Message;
                    }
                    finally
                    {
                        stream.Close();
                        stream.Dispose();
                    }

                    response = webRequest.GetResponse() as HttpWebResponse;
                    if (response != null)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                        }

                        Stream responseStream = response.GetResponseStream();

                        try
                        {
                            if (responseStream != null)
                            {
                               // DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(RequestResult));
                                //object returnObject = deserializer.ReadObject(responseStream);
                               // successAction.Invoke(returnObject);
                            }
                        }
                        catch (WebException exception)
                        {
                            //resultError += Environment.NewLine + exception.GetInnerMostException().Message;
                            serverException = exception;
                        }
                        catch (Exception exception)
                        {
                            //resultError += Environment.NewLine + exception.GetInnerMostException().Message;
                            serverException = exception;
                        }
                        finally
                        {
                            if (responseStream != null)
                            {
                                stream.Close();
                                stream.Dispose();
                            }
                        }
                    }
                }
            }
            catch (WebException exception)
            {
                Stream responseStream = null;

                if (exception.Response != null)
                {
                    responseStream = exception.Response.GetResponseStream();
                }

                if (responseStream != null)
                {
                    try
                    {
                        //DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(RequestResult));
                        //RequestResult returnObject = deserializer.ReadObject(responseStream) as RequestResult;

                        //if (returnObject != null)
                        //{
                        //    resultError = returnObject.ErrorDetails;
                        //}
                    }
                    catch (Exception ex)
                    {
                       // resultError = exception.GetInnerMostException().Message + Environment.NewLine + ex.GetInnerMostException().Message;
                    }
                }
                else
                {
                    //resultError = exception.GetInnerMostException().Message;
                }

                // error service 
                response = (HttpWebResponse)exception.Response;
                serverException = exception;
            }
            catch (Exception exception)
            {
                //resultError = exception.GetInnerMostException().Message;
                serverException = exception;
            }
            finally
            {
                if (!string.IsNullOrEmpty(resultError))
                {
                    //RequestResult errResult = new RequestResult { Isvalid = false, ErrorDetails = resultError, ServerException = serverException };
                    //successAction.Invoke(errResult);
                }

                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
            }

            return resultError;
        }
    }
}
