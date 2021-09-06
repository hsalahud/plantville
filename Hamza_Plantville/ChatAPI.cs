using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    static public class ChatAPI
    {
        //ChatAPI properties
        public static List<MessageMetaData> ChatData = new List<MessageMetaData>();
        public static bool PostSuccess { get; set; }
        /// <summary>
        /// API call to get all chat data and deserializing json
        /// </summary>
        /// <returns></returns>
        public static async Task getChatData()
        {
            // used by Visual Studio to create socket connections
            HttpClient client = new HttpClient();
            string request = @"https://plantville.herokuapp.com/";
            //Console.WriteLine(request);
            // Makes request to cat facts
            HttpResponseMessage response = await client.GetAsync(request);
            // Not required, but checks if status code is successful. Other status codes are errors or page not found.
            if (response.IsSuccessStatusCode)
            {
                ChatData.Clear();
                // print JSON response
                string data = await response.Content.ReadAsStringAsync();

                List<MessageMetaData> deserializedObj = JsonConvert.DeserializeObject<List<MessageMetaData>>(data);

                ChatData = deserializedObj;

            }
        }

        /// <summary>
        /// API call to post a message
        /// </summary>
        /// <param name="username"></param>
        /// <param name="msgText"></param>
        /// <returns>true if successful call to API, else false</returns>
        public static async Task<bool> postMessage(string username, string msgText )
        {
            var msg = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("message", msgText)
            });

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.PostAsync(@"http://plantville.herokuapp.com/", msg);

            
            if (response.IsSuccessStatusCode)
            {
                PostSuccess = true;
            }
            else
            {
                PostSuccess = false;
            }

            return PostSuccess;

        }
    }
}
