using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustEat_Interview.Logic;

namespace JustEat_Interview.Controllers
{
    public class HomeController : Controller
    {
        RestaurantList _restaurantList = new RestaurantList();
        static ApiResponseProcessor _apiProcessor = new ApiResponseProcessor();

        //
        // GET: /Home/

        public ActionResult Index()
        {         
            return View(model:null );
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            try
            {
                string postCode = form["postCode"];
                
                this._restaurantList = _apiProcessor.GetRestaurantList(postCode);
                return View(_restaurantList);
            }
            catch (Exception ex)
            {
                return View(model:null);
            }
        }

        public ActionResult ShowProducts(string id)
        {
            Restaurant rest =  _apiProcessor.GetRestaurantProducts(id);
            return View("ShowProducts", model: rest);
        }
    }
}
