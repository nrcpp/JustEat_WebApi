using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustEat_Interview.Logic
{
    /// <summary>
    /// JSON -> .NET. Uses for fast conversion and storing API reponse
    /// </summary>
    public class RestaurantList
    {
        /// <summary>
        /// Like "SE19"
        /// </summary>
        public string ShortResultText { get; set; }

        /// <summary>
        /// List of restaurant entries
        /// </summary>
        public List<Restaurant> Restaurants { get; set; }
    }
}
