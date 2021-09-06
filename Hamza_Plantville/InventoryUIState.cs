using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Hamza_Plantville
{
    public static class InventoryUIState
    {
        /// <summary>
        /// method to populate inventory list by accessing inventory from the player class
        /// </summary>
        /// <param name="mw">Our window</param>
        public static void generateInventoryList(MainWindow mw)
        {
            mw.InventoryList.Items.Clear();
            //int enteredInventoryOrder = 1;
            if (Player.Inventory.goods.Count == 0)
            {
                mw.InventoryList.Items.Add("No fruits or vegetables harvested.");
            }

            foreach (Plant plant in Player.Inventory.goods)
            {
                mw.InventoryList.Items.Add(plant);
            }

        }

        /// <summary>
        /// Handling the UI for selling all items in the inventory
        /// </summary>
        /// <param name="mw">Our window</param>
        public static void handleSellAll(MainWindow mw)
        {
            if (Player.Inventory.goods.Count == 0)
            {
                var msg = MessageBox.Show($"Are you sure you want to go to the farmer's market without any inventory?", "Lose money for no reason?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (msg == MessageBoxResult.Yes)
                {
                    handleSellAllState(mw);
                }
            }
            else
            {
                handleSellAllState(mw);
            }
        }

        /// <summary>
        /// Helper function to handleSellAll. This function mainly computes the profit/loss and handles the info in our status bar and display in the message box
        /// </summary>
        /// <param name="mw">Our window</param>
        public static void handleSellAllState(MainWindow mw)
        {
            Player.SellAllPlants(out int profit);
            if (profit > 0)
            {
                MessageBox.Show($"Cleared Inventory. Made ${profit}");
            }
            else
            {
                MessageBox.Show($"Cleared Inventory. Incurred a loss of ${profit * -1}");
            }

            InventoryUIState.generateInventoryList(mw);
            StatusBarUIState.InitializeStatusBarLabels(mw);
        }

        /// <summary>
        /// Toggling the Inventory view/UI
        /// </summary>
        /// <param name="mw">Our window</param>
        public static void toggleInventoryUI(MainWindow mw)
        {
            generateInventoryList(mw);
            mw.inventoryView.Visibility = Visibility.Visible;
            mw.gardenView.Visibility = Visibility.Hidden;
            mw.chatView.Visibility = Visibility.Hidden;
            mw.seedEmporiumView.Visibility = Visibility.Hidden;
            mw.tradeMarketPlaceView.Visibility = Visibility.Hidden;
            mw.proposeTradeView.Visibility = Visibility.Hidden;

        }
    }
}
