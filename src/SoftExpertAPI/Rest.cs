using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


class Rest
{


    public static dynamic request(string method, string urlBase, string recurso, Dictionary<string, string> headers, string json) {
        return internalRequest(method, urlBase, recurso, headers, json);
    }

    public static dynamic request(string method, string urlBase, string recurso, Dictionary<string, string> headers, Dictionary<string, string> x_www_form_urlencoded)
    {
        return internalRequest(method, urlBase, recurso, headers, null, x_www_form_urlencoded);
    }

    public static dynamic request(string method, string urlBase, string recurso, Dictionary<string, string> headers)
    {
        return internalRequest(method, urlBase, recurso, headers);
    }



    private static dynamic internalRequest(string method, string urlBase, string recurso, Dictionary<string, string>? headers = null, string? body = null, Dictionary<string, string>? x_www_form_urlencoded = null) {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{urlBase}{recurso}");
        request.Method = method;
        request.Timeout = 1000*60*5;

        if (headers != null)
        {
            foreach (var item in headers)
            {
                request.Headers.Add(item.Key, item.Value);
            }
        }

        StreamWriter requestWriter = null;
        if (body != null)
        {
            try
            {
                JSchema schema = JSchema.Parse(body);
                request.ContentType = "application/json";
            }
            catch (Exception)
            {
                request.ContentType = "application/xml";
            }
            
            request.ContentLength = body.Length;
            requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(body);
            requestWriter.Close();
        }

        if (headers.ContainsKey("ContentType")) {
            request.ContentType = headers["ContentType"];
        }
        


        if (x_www_form_urlencoded != null)
        {
            //convert dictionary to url encoded string
            string dictionaryString = "";
            foreach (KeyValuePair<string, string> keyValues in x_www_form_urlencoded)
            {
                dictionaryString += keyValues.Key + "=" + keyValues.Value + "&";
            }
            dictionaryString = dictionaryString.Substring(0, dictionaryString.Length - 1);
            // Convert string to bytes
            byte[] byteArray = Encoding.UTF8.GetBytes(dictionaryString);
            // Set the content type of the data being posted.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the content length of the string being posted.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
        }
        


        
        

        try
        {
            WebResponse webResponse = request.GetResponse();
            Stream webStream = webResponse.GetResponseStream();
            StreamReader responseReader = new StreamReader(webStream);
            string response = responseReader.ReadToEnd();
            responseReader.Close();

            try
            {
                return JArray.Parse($"[{response}]")[0];
            }
            catch (Exception e)
            {

            }

            return response;
        }
        catch (System.Net.WebException responseException)
        {

            using (WebResponse responseError = responseException.Response)
            {
                var httpResponse = (HttpWebResponse)responseError;
                using (Stream data = responseError.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(data);
                    throw new Exception(sr.ReadToEnd(), responseException);
                }
            }
        }
        catch (Exception e)
        {
            throw e;
        }

    }
}

