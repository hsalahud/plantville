using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    public class TradeMetaData
    {
        /// <summary>
        /// Trade metadata is similar to MessageMetaData. Each trade object retrieved from the server includes additional information and the trade itself is stored in the fields property.
        /// Using this MetData class helps with deserialization and helps sepearte additional information from what is stored in the trade class which is directly related to the trade itself
        /// </summary>
        public string model { get; set; }
        public int pk { get; set; }
        public Trade fields { get; set; }

        /// <summary>
        /// Similar to the plant class, we override the ToString method so that when the trade is stored in the listbox, the listbox invokes this method to display the information that this method returns
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (fields.state.ToLower().Equals("closed"))
            {
                return $"[{fields.state}] {fields.author} bought {fields.quantity} {fields.plant} for ${fields.price} from {fields.accepted_by}";
            }
            else
            {
                return $"[{fields.state}] {fields.author} wants to buy {fields.quantity} {fields.plant} for ${fields.price}";
            }
        }
    }
}
