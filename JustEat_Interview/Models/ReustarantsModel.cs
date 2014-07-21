using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat_Interview.Logic;

namespace JustEat_Interview.Models
{
    public class ReustarantsModel
    {
        RestaurantList _restaurantList = new RestaurantList();
        ApiResponseProcessor _apiResonseProcessor = new ApiResponseProcessor();

        public RestaurantList GetAll()
        {
            _restaurantList = _apiResonseProcessor.GetRestaurantList(postCode: "se24");
            return _restaurantList;
        }
    }
}