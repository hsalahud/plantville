using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    public class Inventory <T>
    {
        /// <summary>
        /// The inventory holds a list of goods which can be either seeds (for the emporium) or plants (for the player's inventory)
        /// </summary>
        public List<T> goods = new List<T>();
    }
}
