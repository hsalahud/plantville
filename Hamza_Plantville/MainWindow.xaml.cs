using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Hamza_Plantville
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<ComboBoxItem> cbItems { get; set; }
        public static ComboBoxItem SelectedcbItem { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method to initialize player data and sending the data to different parts of the UI
        /// </summary>
        private void initializePlantVille()
        {
            ChatUIState.generateChatData(this);
            Player.initializePlayer();
            SeedEmporiumUIState.generateSeedEmporiumList(this);
            GardenUIState.generateGardenList(this);
            InventoryUIState.generateInventoryList(this);
            StatusBarUIState.InitializeStatusBarLabels(this);
        }

        /// <summary>
        /// Toggle different UI elements based on whether we click on seed emporioum, garden, or inventory
        /// </summary>
        /// <param name="buttonText"> the text of the button we click on</param>
        private void toggleListBoxVisibility(string buttonText)
        {
            if (buttonText.ToLower().Equals("garden"))
            {
                GardenUIState.toggleGardenUI(this);
            }
            else if (buttonText.ToLower().Equals("inventory"))
            {
                InventoryUIState.toggleInventoryUI(this);
            }
            else if (buttonText.ToLower().Equals("seeds emporium"))
            {
                SeedEmporiumUIState.toggleSeedEmporiumUI(this);
            }
            else if (buttonText.ToLower().Equals("chat"))
            {
                ChatUIState.toggleChatUI(this);
            }
            else if (buttonText.ToLower().Equals("trade marketplace"))
            {
                TradeMarketplaceUIState.toggleMarketplaceUI(this);
            }
            else if (buttonText.ToLower().Equals("propose trade"))
            {
                ProposeTradeUIState.togglProposeTradeUI(this);
            }
        }


        /// <summary>
        /// Adding ability to simultaneously update all lists
        /// </summary>
        public void updateGardenAndInventory()
        {
            StatusBarUIState.InitializeStatusBarLabels(this);
            GardenUIState.generateGardenList(this);
            InventoryUIState.generateInventoryList(this);
        }

        /// <summary>
        /// event listener to click on garden/inventory/seed emporium buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            toggleListBoxVisibility(button.Content.ToString());
        }

        /// <summary>
        /// ///event listener to handle purchasing seeds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handlePurchaseSeed(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(SeedsEmporiumList, e.OriginalSource as DependencyObject) as ListBoxItem;

            if (item != null)
            {
                string[] seedText = item.Content.ToString().Split(' ');
                string seedName = seedText[0];
                Seed seedPurchase = SeedEmporium.inventory.goods.Find(seedBought => seedBought.Name.Equals(seedName));
                //int price = Int32.Parse(seedText[1].Substring(1));
                if (SeedEmporiumUIState.handlePurchaseState(this, seedPurchase))
                {
                    MessageBox.Show($"You purchased {seedPurchase.Name} for ${seedPurchase.Price}");
                    StatusBarUIState.InitializeStatusBarLabels(this);
                }
            }

        }

        /// <summary>
        /// event listener to harvest one plant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleHarvestGarden(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(GardenList, e.OriginalSource as DependencyObject) as ListBoxItem;

            if (item != null)
            {
                string[] gardenText = item.Content.ToString().Split(')');
                int landPlotNum = Int32.Parse(gardenText[0]) - 1;

                GardenUIState.canGardenBeHarvested(this, landPlotNum);
            }

        }

        /// <summary>
        /// event listener for selling all plants or harvesting all plants
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bulkActions_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (button.Name.ToString().Equals("harvestAllBtn"))
            {
                GardenUIState.handleBulkHarvest(this);
            }
            else if (button.Name.ToString().Equals("sellAllBtn"))
            {
                InventoryUIState.handleSellAll(this);
            }
        }

        /// <summary>
        /// event listener to save game upon closing window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveGame(object sender, CancelEventArgs e)
        {
            SaveState.SaveGame();
        }

        /// <summary>
        /// event listener for sign in button. Once signed in, we toggle the game view with all the settings the player interacts with
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signIn(object sender, RoutedEventArgs e)
        {
            if (userIdInput.Text.Length > 0)
            {
                Player.userName = userIdInput.Text;
                initializePlantVille();
                labelTitle.Content = $"Plantville, {Player.userName}";
                game.Visibility = Visibility.Visible;
                login.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Please enter a username.");
            }

        }

        /// <summary>
        /// keyboard event listener so we can sign in upon clicking enter
        /// Includes verification of whether a username was entered, otherwise a message box is triggered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signInViaKeyBoard(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (userIdInput.Text.Length > 0)
                {
                    
                    Player.userName = userIdInput.Text;
                    initializePlantVille();
                    labelTitle.Content = $"Plantville, {Player.userName}";
                    game.Visibility = Visibility.Visible;
                    login.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Please enter a username.");
                }

            }
        }

        /// <summary>
        /// event handler when clicking send message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleSendMsg(object sender, RoutedEventArgs e)
        {
            ChatUIState.postMessageDisplay(this);
            chatInput.Text = "";
        }

        /// <summary>
        /// event handler when sending message by pressing enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleSendMsgViaKeyboard(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ChatUIState.postMessageDisplay(this);
                chatInput.Text = "";
            }
        }

        /// <summary>
        /// event listener for clicking accept trade. If we have a trade selected then we go through with the process, otherwise he trigger a message telling user to select a trade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleAcceptTrade_Click(object sender, RoutedEventArgs e)
        {

            var trade = MarketList.SelectedValue as TradeMetaData;

            if (trade != null)
            {
                TradeMarketplaceUIState.acceptTrade(trade, this);
            }
            else
            {
                MessageBox.Show("Select a trade before clicking Accept Trade.");
            }
        }

        /// <summary>
        /// Event handler for proposing trade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleProposeTrade(object sender, RoutedEventArgs e)
        {
            ProposeTradeUIState.proposeTrade(this);
        }

        /// <summary>
        /// validation helper method for proposing trade. We cannot add letters, only numbers for price and quantity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
