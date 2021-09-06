using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    public class Plant
    {
        /// <summary>
        /// Plant property: plant has a seed
        /// </summary>
        public Seed SeedOfPlant { get; set; }

        public int Quantity { get; set; }

        public Plant ()
        {
            Quantity = 1;
        }

        /// <summary>
        /// when the seed is ready to be harvested, we create a plant class out of it
        /// </summary>
        /// <param name="seed">The seed from which the plant came from</param>
        /// <returns>A new plant</returns>
        public static Plant sproutPlant(Seed seed)
        {
            return new Plant()
            {
                SeedOfPlant = seed
            };
        }

        /// <summary>
        /// overriding ToString method so that we can store plant into the listbox as specified in the project. This method is called when the listbox is deciding what text to show in the listbox.
        /// Withough it, we will either need to using binding in xaml. Otherwise the listbox will just show the object being stored in the listbox and not the text we desire.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{SeedOfPlant.Name} [{Quantity}] ${SeedOfPlant.HarvestPrice}";
        }

    }
}
