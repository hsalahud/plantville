using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    public class Trade
    {
        /// <summary>
        /// Trade properties as stored in the server
        /// </summary>
        public string accepted_by { get; set; }
        public string author { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string plant { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public string state { get; set; }

        
    }
}
