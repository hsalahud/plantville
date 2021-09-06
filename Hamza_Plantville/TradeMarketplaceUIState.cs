using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Hamza_Plantville
{
    public static class TradeMarketplaceUIState
    {
        /// <summary>
        /// method to toggle marketplace view
        /// </summary>
        /// <param name="mw">main window</param>
        public static void toggleMarketplaceUI(MainWindow mw)
        {
            generateTradeMarketplaceData(mw);
            isProposedTradeAccepted(mw);
            mw.tradeMarketPlaceView.Visibility = Visibility.Visible;
            mw.inventoryView.Visibility = Visibility.Hidden;
            mw.gardenView.Visibility = Visibility.Hidden;
            mw.chatView.Visibility = Visibility.Hidden;
            mw.seedEmporiumView.Visibility = Visibility.Hidden;
            mw.proposeTradeView.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// method to generate marketplace data and store it in the appropriate listbox
        /// </summary>
        /// <param name="mw"></param>
        public static async void generateTradeMarketplaceData(MainWindow mw)
        {
            await TradeMarketplaceAPI.getTradeData();

            mw.MarketList.Items.Clear();

            foreach (TradeMetaData trade in TradeMarketplaceAPI.TradeData)
            {
                mw.MarketList.Items.Add(trade);
            }
        }

        /// <summary>
        /// Method to accept trade. It checks if the trade is open, and if the player successfully trades the plant, we then send a post request to the API and show the result to the user
        /// Validation is handled here as well by showing info in messagebox
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="mw"></param>
        public static async void acceptTrade(TradeMetaData trade, MainWindow mw)
        {
            if (!trade.fields.author.ToLower().Equals(Player.userName.ToLower()))
            {
                //The two conditions below help us bypass other users having programmed the same plant its plural form. So we can recognize the plural and singular forms as belonging to the same plant
                //thus allowing us to successfully complete the trade if we have enough quantities of those plants.
                if (trade.fields.plant.ToLower().Equals("strawberries"))
                {
                    trade.fields.plant = "strawberry";
                }
                    
                if (trade.fields.plant.ToLower().Equals("pears"))
                {
                    trade.fields.plant = "pear";
                }

                if (trade.fields.state.ToLower().Equals("open"))
                {
                    if (Player.tradePlants(trade))
                    {
                        if (await TradeMarketplaceAPI.updateTrade(trade.pk.ToString(), Player.userName))
                        {
                            MessageBox.Show($"Trade accepted! You sold {trade.fields.quantity} {trade.fields.plant} for ${trade.fields.price} to {trade.fields.author}");
                            StatusBarUIState.InitializeStatusBarLabels(mw);
                            generateTradeMarketplaceData(mw);
                        }

                    }
                    else
                    {
                        MessageBox.Show($"You do not have enough {trade.fields.plant} to complete this trade.");
                    }
                }
                else
                {
                    MessageBox.Show("This trade is closed. Only open trades can be accepted.");
                }
            }
            else
            {
                MessageBox.Show("You cannot accept your own trade proposal.");
            }
            

        }

        /// <summary>
        /// When another user accepts the trade you propose, and you visit the marketplace, we retrieve the trade data
        /// match the ids of the trade data with the ones that are in Player.ProposedTradeIds and if they are now closed,
        /// we handle the player receiving the proposed trade, and then notify the player that someone has accepted the trade you proposed.
        /// All appropriate updates are then made.
        /// </summary>
        /// <param name="mw">main window</param>
        public static async void isProposedTradeAccepted(MainWindow mw)
        {
            //Thread.Sleep(500);
            if (Player.proposedTradeIds.Count > 0)
            {
                List<TradeMetaData> trades = await TradeMarketplaceAPI.getTradeData();
                List<TradeMetaData> matchedTrades = trades.FindAll(trade => Player.proposedTradeIds.Contains(trade.pk) && trade.fields.state.ToLower().Equals("closed"));
                if (matchedTrades.Count > 0)
                {
                    foreach (TradeMetaData trade in matchedTrades)
                    {
                        if (Player.receiveProposedTrade(trade))
                        {
                            MessageBox.Show($"Your trade was accepted by {trade.fields.accepted_by}! You received {trade.fields.quantity} of {trade.fields.plant} for ${trade.fields.price}.");
                            StatusBarUIState.InitializeStatusBarLabels(mw);
                        }
                    }
                }
                
            }

        }
    }
}
