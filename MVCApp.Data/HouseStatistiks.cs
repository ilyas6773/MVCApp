using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCAppData.SecondaryModels;

namespace MVCAppData
{
    public class HouseStatistiks
    {
        public int CountHouses { get; set; }
        public int NumberOfActiveHouses { get; set; }
        public int NumberOfInactiveHouses { get; set; }
        public AdAgeModel AdAgeStats { get; set; }
        public PriceModel PriceStats { get; set; }
    }
}
