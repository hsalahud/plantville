using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    public class Garden
    {
        /// <summary>
        /// Garden properties
        /// </summary>
        public Seed SeedPlanted { get; set; }
        public DateTime HarvestTime { get; set; }
        public bool IsSpoiled { get; set; }
        public bool ReadyToHarvest { get; set; }

        public Garden()
        {

        }

        /// <summary>
        /// Get time span to determine whether we are ready to harvest, not ready, or if garden/plant is spoiled
        /// </summary>
        /// <param name="garden">garden which we want to assess its status</param>
        /// <returns>string which will bethen displayed in the UI for the user to see</returns>
        public string getTimeSpanForHarvest(Garden garden)
        {
            TimeSpan ts = garden.HarvestTime - DateTime.Now;

            if (ts.TotalSeconds <= -120)
            {
                garden.IsSpoiled = true;
                return ("spoiled");
            }
            else if (ts.TotalSeconds <= 1)
            {
                garden.ReadyToHarvest = true;
                return "harvest";
            }
            else if (ts.TotalSeconds <= 60)
            {
                return $"{Math.Round(ts.TotalSeconds)} seconds left";
            }
            else
            {
                return $"{ts.Minutes} minutes and {ts.Seconds} seconds left";
            }

        }
    }
}
