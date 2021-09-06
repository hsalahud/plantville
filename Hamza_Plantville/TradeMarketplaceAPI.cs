using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    static public class TradeMarketplaceAPI
    {
        /// <summary>
        /// trade api properties
        /// </summary>
        public static List<TradeMetaData> TradeData = new List<TradeMetaData>();

        /// <summary>
        /// getting all trade data and storing it in the above property. This method also returns the trade data
        /// </summary>
        /// <returns></returns>
        public static async Task<List<TradeMetaData>> getTradeData()
        {
            // used by Visual Studio to create socket connections
            HttpClient client = new HttpClient();
            string request = @"https://plantville.herokuapp.com/trades";
            //Console.WriteLine(request);
            // Makes request to cat facts
            HttpResponseMessage response = await client.GetAsync(request);
            // Not required, but checks if status code is successful. Other status codes are errors or page not found.
            if (response.IsSuccessStatusCode)
            {
                TradeData.Clear();
                // print JSON response
                string data = await response.Content.ReadAsStringAsync();

                List<TradeMetaData> deserializedObj = JsonConvert.DeserializeObject<List<TradeMetaData>>(data);

                TradeData = deserializedObj;
                return deserializedObj;
                //foreach (TradeMetaData trade in deserializedObj){
                //    Console.WriteLine(trade.pk);
                //}

                //deserializing the array like this will also deserialize the team object for each player
                //List<Player> players = JsonConvert.DeserializeObject<List<Player>>(deserializedObj["data"].ToString());

                //PlayerData.AddRange(players);

            }
            return null;

        }

        /// <summary>
        /// updating trade from open to closed once it is accepted
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="accepted_by"></param>
        /// <returns></returns>
        public static async Task<bool> updateTrade(string pk, string accepted_by)
        {
            var acceptedTrade = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("trade_id", pk),
                new KeyValuePair<string, string>("accepted_by", accepted_by)
            });

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.PostAsync(@"http://plantville.herokuapp.com/accept_trade", acceptedTrade);

            return response.IsSuccessStatusCode;

        }

        /// <summary>
        /// posting a new trade after the proposal has been accepted. the id of the newly created trade is then stored in the Player.ProposedTradeIds property.
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="author"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static async Task<bool> postTrade(string plant, string author, string price, string quantity)
        {
            var proposedTrade = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("plant", plant),
                new KeyValuePair<string, string>("author", author),
                 new KeyValuePair<string, string>("price", price),
                new KeyValuePair<string, string>("quantity", quantity)
            });

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.PostAsync(@"http://plantville.herokuapp.com/trades", proposedTrade);
            
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                int deserializedObj = JsonConvert.DeserializeObject<int>(data);
                Player.proposedTradeIds.Add(deserializedObj);
            }
            return response.IsSuccessStatusCode;
        }
    }
}
