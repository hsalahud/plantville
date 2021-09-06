using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Hamza_Plantville
{
    public static class SeedEmporiumUIState
    {
        /// <summary>
        /// retrieve data from the seed emporium class and populate the seed emporium UI list
        /// </summary>
        /// <param name="mw">We pass the window in since the UI elements reside in the window</param>
        public static void generateSeedEmporiumList(MainWindow mw)
        {
            SeedEmporium.getSeeds();
            foreach (Seed seed in SeedEmporium.inventory.goods)
            {
                mw.SeedsEmporiumList.Items.Add($"{seed.Name} ${seed.Price}");
            }
        }

        /// <summary>
        /// When we attempt to make a purchase from the seed emporium, we call this method to see if we can make the purchase, if we can, we update the player and UI state
        /// </summary>
        /// <param name="mw">The window in which our UI elements reside</param>
        /// <param name="seed">The seed we want to purchase</param>
        /// <returns></returns>
        public static bool handlePurchaseState(MainWindow mw, Seed seed)
        {

            if (Player.Money >= seed.Price && Player.NumOfLandPlots > (int)Limits.MinLandPlot)
            {
                Player.makePurchase(seed);
                GardenUIState.generateGardenList(mw);
                return true;
            }
            else if (Player.Money < seed.Price && Player.NumOfLandPlots <= (int)Limits.MinLandPlot)
            {
                MessageBox.Show($"You neither have enough enough money nor enough land to purchase {seed} seeds.");
                return false;
            }
            else if (Player.Money < seed.Price)
            {
                MessageBox.Show($"You do not have enough money to purchase {seed} seeds.");
                return false;
            }
            else if (Player.NumOfLandPlots == 0)
            {
                MessageBox.Show($"You have no land to plant seeds.");
                return false;
            }

            return false;

        }

        /// <summary>
        /// Display UI for seed emporium
        /// </summary>
        /// <param name="mw"></param>
        public static void toggleSeedEmporiumUI(MainWindow mw)
        {
            mw.seedEmporiumView.Visibility = Visibility.Visible;
            mw.inventoryView.Visibility = Visibility.Hidden;
            mw.gardenView.Visibility = Visibility.Hidden;
            mw.chatView.Visibility = Visibility.Hidden;
            mw.tradeMarketPlaceView.Visibility = Visibility.Hidden;
            mw.proposeTradeView.Visibility = Visibility.Hidden;

        }
    }
}
