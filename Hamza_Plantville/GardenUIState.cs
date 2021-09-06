using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Hamza_Plantville
{
    public static class GardenUIState
    {
        /// <summary>
        /// populate garden UI list
        /// </summary>
        /// <param name="mw">Our window in which all our interactive UI elements reside</param>
        public static void generateGardenList(MainWindow mw)
        {
            mw.GardenList.Items.Clear();

            if (Player.gardenPlots.Count == (int)Limits.MinLandPlot)
            {
                mw.GardenList.Items.Add("No plants in garden.");
            }
            else
            {
                int landPlot = (int)Limits.MinLandPlot + 1;
                foreach (Garden garden in Player.gardenPlots)
                {
                    mw.GardenList.Items.Add($"{landPlot}) {garden.SeedPlanted.Name} ({garden.getTimeSpanForHarvest(garden)})");
                    landPlot++;
                }
            }

        }

        /// <summary>
        /// Check if we can harvest, if so we update our garden and inventory
        /// </summary>
        /// <param name="mw">Our window</param>
        /// <param name="landPlotNum">Retrieve the landPlot that we currently want to harvest and get the garden from it</param>
        /// <returns>true or false of whether harvesting was completed</returns>
        public static bool canGardenBeHarvested(MainWindow mw, int landPlotNum)
        {
            Garden garden = Player.gardenPlots[landPlotNum];

            if (garden.ReadyToHarvest && !garden.IsSpoiled)
            {
                Player.harvestPlant(garden);
                mw.updateGardenAndInventory();
                MessageBox.Show($"{garden.SeedPlanted.Name} harvested.");
                return true;
            }
            else if (garden.IsSpoiled)
            {
                Player.disposeOfSpoiledPlant(garden);
                GardenUIState.generateGardenList(mw);
                StatusBarUIState.InitializeStatusBarLabels(mw);
                MessageBox.Show($"This {garden.SeedPlanted.Name} plant is spoiled. Disposing from Garden.");
                return false;
            }
            else if (!garden.ReadyToHarvest)
            {
                MessageBox.Show($"This {garden.SeedPlanted.Name} plant is not ready to be harvested.");
                GardenUIState.generateGardenList(mw);
                return false;
            }

            return false;
        }

        /// <summary>
        /// Harvest all action. We display a message box based on how much was harvested/spoiled/not ready and then we update our UI accordingly
        /// </summary>
        /// <param name="mw">Our window</param>
        public static void handleBulkHarvest(MainWindow mw)
        {
            BulkHarvestState bulkHarvestState = Player.HarvestAllAndRemoveAnySpoiledPlants(out int totalHarvested, out int totalSpoiled, out int totalNotReady);

            if (bulkHarvestState == BulkHarvestState.Full)
            {
                MessageBox.Show($"All the plants in the garden have been harvested.");
            }
            else if (bulkHarvestState == BulkHarvestState.PartialSpoiled)
            {
                MessageBox.Show($"{totalHarvested} Plants have been harvested. The remaining {totalSpoiled} were spoiled so they have been disposed of.");
            }
            else if (bulkHarvestState == BulkHarvestState.PartialNotReady)
            {
                MessageBox.Show($"{totalHarvested} Plants have been harvested . The remaining {totalNotReady} are not ready yet.");
            }
            else if (bulkHarvestState == BulkHarvestState.Partial)
            {
                MessageBox.Show($"{totalHarvested} Plants have been harvested. {totalNotReady} are not ready yet, and the remaining {totalSpoiled} are spoiled.");
            }
            else if (bulkHarvestState == BulkHarvestState.None)
            {
                MessageBox.Show($"None of the plants can be harvested at the moment. {totalNotReady} are not ready to be harvested and the remaining {totalSpoiled} are spoiled.");
            }
            else if (bulkHarvestState == BulkHarvestState.NoneReady)
            {
                MessageBox.Show($"None of the plants are ready yet.");
            }
            else if (bulkHarvestState == BulkHarvestState.AllSpoiled)
            {
                MessageBox.Show($"Looks like all the plants have been spoiled. :(");
            }
            else
            {
                MessageBox.Show($"There is nothing to harvest.");
            }

            mw.updateGardenAndInventory();
        }

        /// <summary>
        /// Toggle our garden UI view
        /// </summary>
        /// <param name="mw">Our window</param>
        public static void toggleGardenUI(MainWindow mw)
        {
            GardenUIState.generateGardenList(mw);
            mw.gardenView.Visibility = Visibility.Visible;
            mw.chatView.Visibility = Visibility.Hidden;
            mw.inventoryView.Visibility = Visibility.Hidden;
            mw.seedEmporiumView.Visibility = Visibility.Hidden;
            mw.tradeMarketPlaceView.Visibility = Visibility.Hidden;
            mw.proposeTradeView.Visibility = Visibility.Hidden;

        }
    }
}
