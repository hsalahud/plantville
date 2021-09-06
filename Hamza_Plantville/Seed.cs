using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    public class Seed
    {
        /// <summary>
        /// Seed properties
        /// </summary>
        public string Name { get; set; }
        public int Price { get; set; }
        public int HarvestPrice { get; set; }
        public TimeSpan HarvestDuration { get; set; }


        private Seed ()
        {

        }

        /// <summary>
        /// We have two constructors for the seed class. The one above helps create default seeds and the one below helps us create custom seeds for testing scalability
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="harvestPrice"></param>
        /// <param name="harvestDuration"></param>
        private Seed(string name, int price, int harvestPrice, TimeSpan harvestDuration)
        {
            Name = name;
            Price = price;
            HarvestPrice = harvestPrice;
            HarvestDuration = harvestDuration;
        }

        /// <summary>
        /// Since I have made the seed constructors private, I created this class to generate a seed with optional parameters. Added it in a try/catch block in case there is an issue with the number of parameters used
        /// </summary>
        /// <param name="parameters">If the parameter has a length of 1 and that length is the name of the default seeds, we go ahead add create instances of those seeds. Other wise we create a
        /// custom seed by feeding in all the properties that a seed needs</param>
        /// <returns>the desired seed</returns>
        public static Seed generateSeed(params object[] parameters)
        {
            try
            {
                if (parameters.Length==1 && (string)parameters[0]=="pear" || (string)parameters[0] == "strawberry" || (string)parameters[0] == "spinach")
                {
                    string name = (string)parameters[0];
                    switch (name.ToLower())
                    {
                        case "pear":
                            return new Seed()
                            {
                                Name = name,
                                Price = 2,
                                HarvestPrice = 20,
                                HarvestDuration = new TimeSpan(0, 2, 0)
                            };
                        case "strawberry":
                            return new Seed()
                            {
                                Name = name,
                                Price = 2,
                                HarvestPrice = 8,
                                HarvestDuration = new TimeSpan(0, 0, 30)

                            };
                        case "spinach":
                            return new Seed()
                            {
                                Name = name,
                                Price = 5,
                                HarvestPrice = 21,
                                HarvestDuration = new TimeSpan(0, 1, 0)

                            };
                        default:
                            return null;
                    }
                }
                else
                {
                    string name = (string)parameters[0];
                    int price = (int)parameters[1];
                    int harvestPrice = (int)parameters[2];
                    TimeSpan harvestDuration = (TimeSpan)parameters[3];
                    return new Seed(name, price, harvestPrice, harvestDuration);
                }
                
               
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString() + " potentially used incorrect number of parameters.");
                return null;
            }
            
        }

    }
}
