using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hamza_Plantville
{
    public class MessageMetaData : ListBoxItem
    {
        /// <summary>
        /// since the api for getMessage has extra properties in addition to the fields property which is our message,
        /// I created this metadata class to represent each object received from the api. This makes serializing a little more easy
        /// and seperates the definition of message from the additional information that is retrieved from the server.
        /// </summary>
        public string model { get; set; }
        public int pk { get; set; }
        public Message fields { get; set; }

    }
}
