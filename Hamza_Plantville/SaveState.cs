using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Hamza_Plantville
{
    public static class SaveState
    {
        /// <summary>
        /// Method to save game. Now each file generated is of the same name as the username.
        /// </summary>
        public static void SaveGame()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("gardenPlots", Player.gardenPlots);
            dict.Add("inventory", Player.Inventory.goods);
            dict.Add("money", Player.Money);
            dict.Add("numOfLandPlots", Player.NumOfLandPlots);
            dict.Add("proposedTradeIds", Player.proposedTradeIds);

            string serializedJson = JsonConvert.SerializeObject(dict);

            using (StreamWriter sw = new StreamWriter($@"{Player.userName}.txt"))
            {
                sw.WriteLine(serializedJson);
            }
        }

        /// <summary>
        /// method to load saved game. It attempts to find the file name which matches the username. Otherwise a new game is loaded.
        /// </summary>
        /// <param name="savedFileLocation"></param>
        public static void LoadSavedGame(string savedFileLocation)
        {
            using (var reader = new StreamReader(savedFileLocation))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    Dictionary<string, object> deserializedObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(line);
                    List<Garden> savedGardenPlots = JsonConvert.DeserializeObject<List<Garden>>(deserializedObj["gardenPlots"].ToString());
                    List<Plant> savedPlantInventory = JsonConvert.DeserializeObject<List<Plant>>(deserializedObj["inventory"].ToString());
                    List<int>proposedTradeIds = JsonConvert.DeserializeObject<List<int>>(deserializedObj["proposedTradeIds"].ToString());
                    int savedMoney = JsonConvert.DeserializeObject<int>(deserializedObj["money"].ToString());
                    int savedNumOfLandPlots = JsonConvert.DeserializeObject<int>(deserializedObj["numOfLandPlots"].ToString());

                    Player.gardenPlots.AddRange(savedGardenPlots);
                    Player.Inventory = new Inventory<Plant>();
                    Player.Inventory.goods.AddRange(savedPlantInventory);
                    Player.proposedTradeIds.AddRange(proposedTradeIds);
                    Player.Money = savedMoney;
                    Player.NumOfLandPlots = savedNumOfLandPlots;

                }
            }
        }
    }
}
