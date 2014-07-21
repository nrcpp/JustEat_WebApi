using System.Net.Http;
using System.Net.Http.Headers;

namespace JustEat_Interview.Logic
{
    using System;
    
    class Program
    {
        static void Main(string[] args)
        {
            ApiResponseProcessor proc = new ApiResponseProcessor();
            
            var rests = proc.GetRestaurantList("se24");
            proc.GetRestaurantProducts(rests.Restaurants[10]);
        }
    }
}
