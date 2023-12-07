using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Enrollment_FrontEnd.Controllers
{
    public class BeverageController : Controller
    {
        const string BASE_URL = "https://localhost:7293/api/BeverageAPI";
        public IActionResult Index()
        {
            var beveragelist = new List<BeverageOption>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/BeverageOption").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    beveragelist = JsonConvert.DeserializeObject<List<BeverageOption>>(data);
                }
            }
            return View(beveragelist);
        }






        //CREATE: GET
        [HttpGet]
        public IActionResult Create()
        {
            BeverageOption beverageOption = new BeverageOption();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
            return View(beverageOption);

        }



        //CREATE:POST
        [HttpPost]
        public IActionResult Create (BeverageOption beverageOption, int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var beverage = new BeverageOption
            {
                Id = id,
                BeverageName = beverageOption.BeverageName,
            };

            var json = JsonConvert.SerializeObject(beverage);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage bevResponse = client.PostAsync($"{BASE_URL}/BeverageOptionPost", data).Result;
            if (bevResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Create Beverage Option");
                return View(beverage);
            }
        }



        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            BeverageOption beverageOption = new BeverageOption();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //beverage by {id}
                HttpResponseMessage bevResponse = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (bevResponse.IsSuccessStatusCode)
                {
                    var bevData = bevResponse.Content.ReadAsStringAsync().Result;
                    beverageOption = JsonConvert.DeserializeObject<BeverageOption>(bevData)!;
                }


                var beverage = new BeverageOption
                {
                    Id = beverageOption.Id,
                    BeverageName = beverageOption.BeverageName,
                };


            }

            return View(beverageOption);

        }


        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(BeverageOption beverageOption)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var beverage = new BeverageOption
            {
                Id = beverageOption.Id,
                BeverageName = beverageOption.BeverageName,
            };

            var json = JsonConvert.SerializeObject(beverage);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage bevResponse = client.PutAsync($"{BASE_URL}/BeverageOptionPut", data).Result;
            if (bevResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Update Beverage Option");
                return View(beverage);
            }
        }




        //DETAIL
        public IActionResult Detail(int id)
        {
            BeverageOption beverageOption = new BeverageOption();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    beverageOption = JsonConvert.DeserializeObject<BeverageOption>(data);
                }
            }
            return View(beverageOption);
        }

        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            BeverageOption beverageOption = new BeverageOption();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    beverageOption = JsonConvert.DeserializeObject<BeverageOption>(data);
                }
            }
            return View(beverageOption);
        }


        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, BeverageOption beverageOption)
        {
            //BeverageOption beverageOption = new BeverageOption();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.DeleteAsync($"{BASE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    beverageOption = JsonConvert.DeserializeObject<BeverageOption>(data);
                }
            }
            return RedirectToAction("Index");
        }



    }
}
