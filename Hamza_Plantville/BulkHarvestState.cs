using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    /// <summary>
    /// Constants for better readability of code
    /// </summary>
    public enum BulkHarvestState
    {
        Full,
        PartialSpoiled,
        PartialNotReady,
        Partial,
        None,
        NoneReady,
        AllSpoiled,
        Empty
    }
}
