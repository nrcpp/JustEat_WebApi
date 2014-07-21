using JustEat_Interview.Logic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustEat_Interview.Logic
{
    /// <summary>
    /// Represents restaurant entry in JustEat API response
    /// </summary>
    public class Restaurant
    {
        public string Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Average rating
        /// </summary>
        public double RatingStars { get; set; }

        public class LogoFields
        {
            public string StandardResolutionURL { get; private set; }
            public object Deals { get; set; }
            public double NumberOfRatings { get; set; }
        };

        /// <summary>
        /// Restaurant logo (array of values) 
        /// </summary>
        public object Logo { get; set; }

        /// <summary>
        /// Url on JustEat site
        /// </summary>
        public string Url { get; set; }

        public string Postcode { get; set; }

        public List<Product> ListOfProductsToSell { get; set; }

        public string GetLogoUrl()
        {
            if (Logo == null)
                return "";

            JObject inner = ((JArray)Logo)[0].Value<JObject>();
            List<string> values = inner.Properties().Select(p => (string)p.Value).ToList();
            string logoUrl = values.FirstOrDefault();
           
            return logoUrl;
        }
    }
}