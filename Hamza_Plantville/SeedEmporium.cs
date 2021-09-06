using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    public static class SeedEmporium
    {
        /// <summary>
        /// Seed Emporium properties - The emporium has a list of seeds
        /// </summary>
        public static Inventory<Seed> inventory = new Inventory<Seed>();
        public static List<Seed> getSeeds()
        { 
            inventory.goods.Add(Seed.generateSeed("strawberry"));
            inventory.goods.Add(Seed.generateSeed("spinach"));
            inventory.goods.Add(Seed.generateSeed("pear"));
            //REMOVE COMMENT BELOW TO ADD NEW CUSTOM SEED
            //seeds.Add(Seed.generateSeed("mango", 10, 50, new TimeSpan(0, 5, 0)));

            return inventory.goods;
        }
    }
}
