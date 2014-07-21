using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace JustEat_Interview.Logic
{
    /// <summary>
    /// Stores response from server as a list of Restaurant entries
    /// </summary>
    public class ApiResponseProcessor
    {        
        HttpClient _client;
        RestaurantList _restaurantList;

        public ApiResponseProcessor()
        {
             _client = new HttpClient
            {
                BaseAddress = new Uri("http://api-interview.just-eat.com/"),
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                                                                       "VGVjaFRlc3RBUEk6dXNlcjI=");
            _client.DefaultRequestHeaders.Add("Accept-Tenant", "uk");
            _client.DefaultRequestHeaders.Add("Accept-Language", "en-GB");
            _client.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");
            _client.DefaultRequestHeaders.Add("Host", "api-interview.just-eat.com");


        }

        /// <summary>
        /// Returns API response as JSON string, 
        /// </summary>
        /// <param name="path">restaurants or menus</param>
        /// <param name="query"></param>
        /// <returns>JSON formated reponse</returns>
        public string GetApiResponse(string path, string query = "")
        {        
            string result = "";
            _client.GetStringAsync(path + query)
                  .ContinueWith(t => result = t.Result)
                  .Wait();

            return result;
        }

        /// <summary>
        /// Fills Restaurant entries in list from server request. 
        /// </summary>
        public RestaurantList GetRestaurantList(string postCode)
        {
            string respose = GetApiResponse("/restaurants/", "?q=" + postCode);

            _restaurantList = JsonConvert.DeserializeObject<RestaurantList>(respose);
            
            // sort list of restaurants by average rating
            _restaurantList.Restaurants = _restaurantList.Restaurants.OrderByDescending(r => r.RatingStars).ToList();

            return _restaurantList ;
        }

        class Menu
        {
            public string Id { get; set; }
            public object Products { get; set; }
        }

        class MenuList
        {
            public List<Menu> Menus { get; set; }
        }

        class Category
        {
            public string Id { get; set; }
        }

        class CategoryList
        {
            public List<Category> Categories { get; set; }
        }
      
        public class ProductList
        {
            public List<Product> Products { get; set; }

            public ProductList()
            {
                Products = new List<Product>();
            }
        }

        /// <summary>
        /// Returns list of products for given restaurantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Restaurant GetRestaurantProducts(string restaurantId)
        {
            var rest = _restaurantList.Restaurants.Find(r => r.Id == restaurantId);

            if (rest != null)
            {
                GetRestaurantProducts(rest);
            }

            return rest;
        }

        /// <summary>
        /// Retrieves products of resturant using API responses
        /// </summary>
        /// <param name="restaurant"></param>
        /// <returns>List of products of resturant</returns>
        public ProductList GetRestaurantProducts(Restaurant restaurant)
        {
            string response = GetApiResponse("/restaurants/" + restaurant.Id + "/menus");
            MenuList menus = JsonConvert.DeserializeObject<MenuList>(response);
            ProductList productsResult = new ProductList();

            foreach (var menu in menus.Menus)
            {
                try
                {
                    response = GetApiResponse("/menus/" + menu.Id + "/productcategories");
                   
                    var categories = JsonConvert.DeserializeObject<CategoryList>(response);

                    foreach (var category in categories.Categories)
                    {
                        var products = GetProductsByCategory(menu.Id, category.Id);

                        productsResult.Products.AddRange(products.Products);
                    }
                }
                catch
                {
                    // handle NotFound pages for menus
                }
            }

            restaurant.ListOfProductsToSell = productsResult.Products;      // finally set the list of products 
            return productsResult;
        }


        // Sends API request like "menus/57443/productcategories/5/products",
        // returns list of products
        private ProductList GetProductsByCategory(string menuId, string categoryId)
        {
            string response = GetApiResponse("/menus/" + menuId + "/productcategories/" + categoryId + "/products");

            var products = JsonConvert.DeserializeObject<ProductList>(response);

            return products;
        }
    }
}
