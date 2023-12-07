using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Enrollment_FrontEnd.Controllers
{
    public class SideController : Controller
    {
        const string BASE_URL = "https://localhost:7293/api/SideAPI";
        public IActionResult Index()
        {
            var sidelist = new List<SideOption>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/SideOption").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    sidelist = JsonConvert.DeserializeObject<List<SideOption>>(data);
                }
            }
            return View(sidelist);
        }






        //CREATE: GET
        [HttpGet]
        public IActionResult Create()
        {
            SideOption sideoption = new SideOption();

            using (HttpClient client= new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
            return View(sideoption);
        }


        //CREATE:POST
        [HttpPost]
        public IActionResult Create(SideOption sideOption, int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var side = new SideOption
            {
                Id = id,
                SideName = sideOption.SideName,
            };

            var json = JsonConvert.SerializeObject(side);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage sideResponse = client.PostAsync($"{BASE_URL}/SideOptionPost", data).Result;
            if (sideResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Create Side Option");
                return View(side);
            }
        }





        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            SideOption sideoption = new SideOption();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //side by {id}
                HttpResponseMessage sideResponse = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (sideResponse.IsSuccessStatusCode)
                {
                    var sideData = sideResponse.Content.ReadAsStringAsync().Result;
                    sideoption = JsonConvert.DeserializeObject<SideOption>(sideData)!;
                }

                var side = new SideOption
                {
                    Id = sideoption.Id,
                    SideName = sideoption.SideName,
                };
            
           
            
            }


            return View(sideoption);
        }



        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(SideOption sideOption)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var side = new SideOption
            {
                Id = sideOption.Id,
                SideName = sideOption.SideName,
            };

            var json = JsonConvert.SerializeObject(side);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            HttpResponseMessage sideResponse = client.PutAsync($"{BASE_URL}/SideOptionPut", data).Result;

            if (sideResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Update Side Option");
                return View(side);
            }
        }


        //DETAIL
        public IActionResult Detail(int id)
        {
            SideOption sideOption = new SideOption();

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
                    sideOption = JsonConvert.DeserializeObject<SideOption>(data);
                }
            }
            return View(sideOption);
        }








        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            SideOption sideOption = new SideOption();

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
                    sideOption = JsonConvert.DeserializeObject<SideOption>(data);
                }
            }
            return View(sideOption);
        }


        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, SideOption sideOption)
        {
            //SideOption sideOption = new SideOption();

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
                    sideOption = JsonConvert.DeserializeObject<SideOption>(data);
                }
            }
            return RedirectToAction("Index");
        }



    }
}
