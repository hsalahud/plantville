using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    public static class Player
    {
        /// <summary>
        /// Player properties - this class is used to update only player properties
        /// </summary>
        /// 
        public static string userName { get; set; }
        public static int Money { get; set; }
        //might have to change this to Seed or Plant rather than int
        public static int NumOfLandPlots { get; set; }
        public static Inventory<Plant> Inventory = new Inventory<Plant>();
        public static List<Garden> gardenPlots = new List<Garden>();
        //Store proposed trade IDs
        public static List<int> proposedTradeIds = new List<int>();

        /// <summary>
        /// Handling initializing player data
        /// </summary>
        public static void initializePlayer()
        {
            if (File.Exists($@"{Player.userName}.txt"))
            {
                SaveState.LoadSavedGame($@"{Player.userName}.txt");
            }
            else
            {
                Money = (int)Limits.StartingMoney;
                NumOfLandPlots = (int)Limits.MaxLandPlot;
                //Inventory = new Inventory<Plant>();
            }
        }

        /// <summary>
        /// Method to make a purchase - updating player data
        /// </summary>
        /// <param name="seed"></param>
        public static void makePurchase(Seed seed)
        {
            Money -= seed.Price;
            NumOfLandPlots -= 1;
            gardenPlots.Add(new Garden()
            {
                SeedPlanted = seed,
                HarvestTime = DateTime.Now.Add(seed.HarvestDuration),
                IsSpoiled = false,
                ReadyToHarvest = false
            });
        }

        /// <summary>
        /// Method to handle updating player properties when the user attempts to harvest plant
        /// </summary>
        /// <param name="garden">The garden we want to harvest</param>
        public static void harvestPlant(Garden garden)
        {
            Plant plant = Plant.sproutPlant(garden.SeedPlanted);
            Plant inventoryPlant = Inventory.goods.Find(invPlant => invPlant.SeedOfPlant.Name.Equals(plant.SeedOfPlant.Name));
            if ( inventoryPlant!= null)
            {
                inventoryPlant.Quantity++;
            }
            else
            {
                Inventory.goods.Add(plant);
            }
            
            gardenPlots.Remove(garden);
            NumOfLandPlots++;
        }

        /// <summary>
        /// Method to handle updating player properties when the user attempts to harvest all plants - helper function to bulkHarvestState
        /// </summary>
        /// <param name="garden">List of all gardens to attempt to harvest</param>
        public static void harvestAllPlants(List<Garden> garden)
        {
            foreach (Garden gardenPlot in garden)
            {
                harvestPlant(gardenPlot);
            }
        }

        /// <summary>
        /// handling player properties when the user disposes of a spoiled plant
        /// </summary>
        /// <param name="garden"></param>
        public static void disposeOfSpoiledPlant(Garden garden)
        {
            gardenPlots.Remove(garden);
            NumOfLandPlots++;
        }

        /// <summary>
        /// handling player properties when the user dispposes of all spoiled plants - helper function to bulk harvest state
        /// </summary>
        /// <param name="gardenPlots"></param>
        public static void disposeOfAllSpoiledPlants(List<Garden> gardenPlots)
        {
            int totalRemoved = gardenPlots.RemoveAll(garden => garden.IsSpoiled);
            NumOfLandPlots += totalRemoved;
        }

        /// <summary>
        /// Handling the user properties when we attempt to harvest all the land plots. This method also determines spoilage and plants that are not ready yet
        /// </summary>
        /// <param name="totalHarvested"> passing argument as reference so that we can return them so that the UI can handle it acordingly </param>
        /// <param name="totalSpoiled">passing argument as reference so that we can return them so that the UI can handle it acordingly</param>
        /// <param name="totalNotReady">passing argument as reference so that we can return them so that the UI can handle it acordingly</param>
        /// <returns>returns an enum value of BulkHarvestState. The state determines how the UI handles the result of harvesting all land plots/plants</returns>
        public static BulkHarvestState HarvestAllAndRemoveAnySpoiledPlants(out int totalHarvested, out int totalSpoiled, out int totalNotReady)
        {

            List<Garden> toHarvest = gardenPlots.FindAll(gardenPlot => gardenPlot.ReadyToHarvest && !gardenPlot.IsSpoiled);
            List<Garden> toDispose = gardenPlots.FindAll(gardenPlot => gardenPlot.IsSpoiled);
            List<Garden> notReady =  gardenPlots.FindAll(gardenPlot => !gardenPlot.ReadyToHarvest && !gardenPlot.IsSpoiled);
            totalHarvested = toHarvest.Count;
            totalSpoiled = toDispose.Count;
            totalNotReady = notReady.Count;

            if (totalHarvested == 0)
            {
                if (totalSpoiled > 0 && totalSpoiled == gardenPlots.Count)
                {
                    disposeOfAllSpoiledPlants(gardenPlots);
                    return BulkHarvestState.AllSpoiled;
                }
                else if (totalNotReady > 0 && totalNotReady == gardenPlots.Count)
                {
                    return BulkHarvestState.NoneReady;
                }
                else if (totalSpoiled > 0 && totalNotReady > 0)
                {
                    disposeOfAllSpoiledPlants(gardenPlots);
                    return BulkHarvestState.None;
                }
                else
                {
                    return BulkHarvestState.Empty;
                }
                
            }

            harvestAllPlants(toHarvest);

            if (toDispose.Count > 0 && notReady.Count== 0)
            {
                disposeOfAllSpoiledPlants(gardenPlots);
                return BulkHarvestState.PartialSpoiled;
            }
            else if (toDispose.Count == 0 && notReady.Count > 0)
            {
                return BulkHarvestState.PartialNotReady;
            }
            else if (toDispose.Count > 0 && notReady.Count > 0)
            {
                disposeOfAllSpoiledPlants(gardenPlots);
                return BulkHarvestState.Partial;
            }
            else
            {
                return BulkHarvestState.Full;
            }
        }

        /// <summary>
        /// Method to sell all plants
        /// </summary>
        /// <param name="profit">passing argument as a reference so that it can be returned</param>
        public static void SellAllPlants(out int profit)
        {
            int costs = 10;
            int revenue = 0;

            foreach (Plant plant in Inventory.goods)
            {
                revenue += plant.SeedOfPlant.HarvestPrice * plant.Quantity;
            }

            profit = revenue - costs;
            Money += profit;

            Inventory.goods.Clear();
        }

        /// <summary>
        /// This method accepts a trade. It checks if you have enough plants and processes the trade.
        /// </summary>
        /// <param name="trade">Trade that we are attempting to accept</param>
        /// <returns>true if trade is successfully complete, otherwise false</returns>
        public static bool tradePlants(TradeMetaData trade)
        {

            Plant plantToTrade = Inventory.goods.Find(plant => plant.SeedOfPlant.Name.ToLower().Equals(trade.fields.plant.ToLower()));

            if (plantToTrade!= null)
            {
                if (plantToTrade.Quantity >= trade.fields.quantity)
                {
                    plantToTrade.Quantity -= trade.fields.quantity;

                    //If quantity is 0, we remove the plant from our inventory
                    if(plantToTrade.Quantity == 0)
                    {
                        Inventory.goods.Remove(plantToTrade);
                    }

                    Money += trade.fields.price;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
           
        }

        /// <summary>
        /// When someone accepts your trade, we invoke this method to receive your trade
        /// </summary>
        /// <param name="trade">Trade that you proposed</param>
        /// <returns>true if you received the trade, otherwise false</returns>
        public static bool receiveProposedTrade(TradeMetaData trade)
        {
            int match = proposedTradeIds.Find(id => id == trade.pk);
           
            if (match!=0)
            {
                Money -= trade.fields.price;
                Plant plant = Inventory.goods.Find(foundPlant => foundPlant.SeedOfPlant.Name.ToLower().Equals(trade.fields.plant));
                if (plant != null)
                {
                    plant.Quantity += trade.fields.quantity;
                }
                else
                {
                    Inventory.goods.Add(new Plant()
                    {
                        SeedOfPlant = Seed.generateSeed(trade.fields.plant),
                        Quantity = trade.fields.quantity
                    });
                }
                
                proposedTradeIds.Remove(trade.pk);
                return true;
            }
            return false;
        }
    }
}
