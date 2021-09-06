using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Hamza_Plantville
{
    static public class ProposeTradeUIState
    {
        /// <summary>
        /// Method to toggle propose trade view
        /// </summary>
        /// <param name="mw">main window</param>
        public static void togglProposeTradeUI(MainWindow mw)
        {
            initializeProposeTradeCB(mw);
            mw.proposeTradeView.Visibility = Visibility.Visible;
            mw.chatView.Visibility = Visibility.Hidden;
            mw.gardenView.Visibility = Visibility.Hidden;
            mw.inventoryView.Visibility = Visibility.Hidden;
            mw.seedEmporiumView.Visibility = Visibility.Hidden;
            mw.tradeMarketPlaceView.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// method that dyanmically populates the plant combobox with plant names from seed emporium
        /// Default select is already in items, so if it is equal to one then we add all the items otherwise we do nothing.
        /// </summary>
        /// <param name="mw"></param>
        public static void initializeProposeTradeCB(MainWindow mw)
        {
            if(mw.plantCB.Items.Count == 1)
            {
                foreach (Seed seed in SeedEmporium.inventory.goods)
                {
                    mw.plantCB.Items.Add(seed.Name);
                }
            }

            
        }

        /// <summary>
        /// method that does verification checks on the proposed trade and posts it if it is correct. It checks if we have selected a plant, if we have enough money
        /// </summary>
        /// <param name="mw"></param>
        public static async void proposeTrade(MainWindow mw)
        {
            string plant = mw.plantCB.Text;
            string quantity = mw.quantityInput.Text;
            string price = mw.priceInput.Text;

            if (!plant.ToLower().Equals("select")){
                if (quantity.Length > 0)
                {
                    if(price.Length > 0)
                    {
                        if (Int32.TryParse(price, out int result))
                        {
                            if (result <= Player.Money)
                            {
                                if (await TradeMarketplaceAPI.postTrade(plant, Player.userName, price, quantity))
                                {
                                    MessageBox.Show($"You have successfully proposed to buy {quantity} of {plant} for ${price}");
                                    mw.priceInput.Text = "";
                                    mw.quantityInput.Text = "";
                                }

                            }
                            else
                            {
                                MessageBox.Show("You do not have enough money to propose this trade.");
                                mw.priceInput.Text = "";
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Include a price to the proposal.");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Include a quantity to the proposal.");
                }
                
                
            }
            else
            {
                MessageBox.Show("Select a plant");
            }
            
        }
    }
}
