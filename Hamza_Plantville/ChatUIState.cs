using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Hamza_Plantville
{
    static public class ChatUIState
    {
        /// <summary>
        /// toggle chat view
        /// </summary>
        /// <param name="mw">main window</param>
        public static void toggleChatUI(MainWindow mw)
        {
            generateChatData(mw);
            mw.chatView.Visibility = Visibility.Visible;
            mw.gardenView.Visibility = Visibility.Hidden;
            mw.inventoryView.Visibility = Visibility.Hidden;
            mw.seedEmporiumView.Visibility = Visibility.Hidden;
            mw.tradeMarketPlaceView.Visibility = Visibility.Hidden;
            mw.proposeTradeView.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// call api to get all chata data and set it to listbox
        /// </summary>
        /// <param name="mw"></param>
        public static async void generateChatData(MainWindow mw)
        {
            await ChatAPI.getChatData();

            mw.chatList.Items.Clear();

            foreach(MessageMetaData msg in ChatAPI.ChatData)
            {
                mw.chatList.Items.Add(new ListBoxItem
                {
                    Tag = msg.pk,
                    Content = $"{msg.fields.username}: {msg.fields.message}"
                });
 
            }
        }

        //POST
        /// <summary>
        /// handling grabbing message from input and using it to post the message with the api
        /// </summary>
        /// <param name="mw">main window</param>
        public static async void postMessageDisplay(MainWindow mw)
        {
            await ChatAPI.postMessage(Player.userName, mw.chatInput.Text);

            if (ChatAPI.PostSuccess)
            {
                generateChatData(mw);
            }
            
        }
    }
}
