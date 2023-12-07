using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Enrollment_FrontEnd.Controllers
{
    public class StarchController : Controller
    {
        const string BASE_URL = "https://localhost:7293/api/StarchAPI";
        public IActionResult Index()
        {
            var starchlist = new List<StarchOption>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/StarchOption").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    starchlist = JsonConvert.DeserializeObject<List<StarchOption>>(data);
                }
            }

            return View(starchlist);
        }



        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            StarchOption starchoption = new StarchOption();

            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }

            return View(starchoption);
        }


        //CREATE:POST
        [HttpPost]
        public IActionResult Create (StarchOption starchoption, int id)
        {
            HttpClient client= new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var starch = new StarchOption
            {
                Id = id,
                StarchName = starchoption.StarchName,
            };

            var json = JsonConvert.SerializeObject(starch);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage starchResponse = client.PostAsync($"{BASE_URL}/StarchOptionPost", data).Result;
            if (starchResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Create Starch Option");
                return View(starch);
            }

        }






        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            StarchOption starchoption = new StarchOption();

            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Starch By {id}
                HttpResponseMessage starchResponse = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (starchResponse.IsSuccessStatusCode )
                {
                    var starchData = starchResponse.Content.ReadAsStringAsync().Result;
                    starchoption = JsonConvert.DeserializeObject<StarchOption>(starchData)!;
                }


                var starch = new StarchOption
                {
                    Id = starchoption.Id,
                    StarchName = starchoption.StarchName,
                };
            }
            return View(starchoption);
        }


        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(StarchOption starchoption)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var starch = new StarchOption
            {
                Id = starchoption.Id,
                StarchName = starchoption.StarchName,
            };

            var json = JsonConvert.SerializeObject(starch);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage starchResponse = client.PutAsync($"{BASE_URL}/StarchOptionPut", data).Result;
            if (starchResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Update Starch Option");
                return View(starch);
            }

        }






        //DETAIL
        public IActionResult Detail(int id)
        {
            StarchOption starchOption = new StarchOption();

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
                    starchOption = JsonConvert.DeserializeObject<StarchOption>(data);
                }
            }

            return View(starchOption);
        }


        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            StarchOption starchOption = new StarchOption();

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
                    starchOption = JsonConvert.DeserializeObject<StarchOption>(data);
                }
            }
            return View(starchOption);
        }




        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, StarchOption starchOption)
        {
            //StarchOption starchOption = new StarchOption();

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
                    starchOption = JsonConvert.DeserializeObject<StarchOption>(data);
                }
            }
            return RedirectToAction("Index");
        }



    }
}
