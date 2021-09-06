using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    public static class StatusBarUIState
    {
        /// <summary>
        /// Intializing and updating status bar values
        /// </summary>
        /// <param name="mw">Our window</param>
        public static void InitializeStatusBarLabels(MainWindow mw)
        {
            mw.moneyLabel.Content = $"Money: ${Player.Money}";
            mw.landPlotsLabel.Content = $"Land: {Player.NumOfLandPlots}";
        }
    }
}
