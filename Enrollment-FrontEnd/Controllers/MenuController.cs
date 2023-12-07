using Enrollment_FrontEnd.Models;
using Enrollment_FrontEnd.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Enrollment_FrontEnd.Controllers
{
    public class MenuController : Controller
    {
        const string BASE_URL = "https://localhost:7293/MenuAPI"; //swagger request url

        const string MEALTYPE_ENDPOINT = "MealType";
        const string ITEMCATEGORY_ENDPOINT = "ItemCategory";


        //INDEX
        public IActionResult Index()
        {
            var menuList = new List<Menu>();
            using(HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BASE_URL);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = httpClient.GetAsync($"{BASE_URL}/Menu").Result;
                if(response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    menuList = JsonConvert.DeserializeObject<List<Menu>>(data);
                }
            }
            return View(menuList);
        }


        public IActionResult DisplayIndex()
        {
            var menuList = new List<Menu>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BASE_URL);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = httpClient.GetAsync($"{BASE_URL}/Menu").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    menuList = JsonConvert.DeserializeObject<List<Menu>>(data);
                }
            }
            return View(menuList);
        }




        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            Menu menu = new Menu(); //global variable
            List<MealType>mealList = new List<MealType>();
            List<ItemCategory> itemCaList = new List<ItemCategory>();

            using(HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"{BASE_URL}");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //MealType
                HttpResponseMessage mealResponse = httpClient.GetAsync($"{BASE_URL}/{MEALTYPE_ENDPOINT}").Result;
                {
                    if (mealResponse.IsSuccessStatusCode)
                    {
                        var mealData = mealResponse.Content.ReadAsStringAsync().Result;
                        mealList = JsonConvert.DeserializeObject<List<MealType>>(mealData)!;
                    }
                        
                }

                //ItemCategory
                HttpResponseMessage itemCatResponse = httpClient.GetAsync($"{BASE_URL}/{ITEMCATEGORY_ENDPOINT}").Result;
                {
                    if (itemCatResponse.IsSuccessStatusCode)
                    {
                        var itCatData = itemCatResponse.Content.ReadAsStringAsync().Result;
                        itemCaList = JsonConvert.DeserializeObject<List<ItemCategory>>(itCatData)!;
                    }

                }

                //DDL in VM
                var viewModel = new MenuVM
                {
                    MealTypeList = mealList.Select(meal => new SelectListItem
                    {
                        Text = meal.MealTypeName,
                        Value = meal.Id.ToString(),

                    }).ToList(),


                    ItemCatList = itemCaList.Select(itemCat => new SelectListItem
                    {
                        Text = itemCat.CategoryName,
                        Value = itemCat.Id.ToString(),

                    }).ToList()

                };
                return View(viewModel);

            }
            
        }



        //CREATE:POST
        [HttpPost]
        public async Task <IActionResult> Create (MenuVM menuVm, int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"{BASE_URL}");
            httpClient.DefaultRequestHeaders.Accept.Clear();

            //
            if(!ModelState.IsValid) return View(menuVm);

            var menu = new MenuCreateDto
            {
                MealTypeId = menuVm.SelectedMealTypeId,
                ItemName = menuVm.ItemName,
                ItemCategoryId = menuVm.SelectedItemCatId,
                MenuIdImageFile = menuVm.MenuIdImageFile
            };

            //Call the Method to Send the DTO with the info to the API
            var response = await SendMenuToApi(menu);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Create Menu Item");
                return View(menuVm);
            }//end create method


        }





        //CREATE:FILE POST
        private async Task<HttpResponseMessage> SendMenuToApi(MenuCreateDto menuDto)
        {
            using (var httpClient = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    // Set the Content-Type header to "multipart/form-data"
                    formData.Headers.ContentType.MediaType = "multipart/form-data";

                    // Add menu data to the request
                    formData.Add(new StringContent(menuDto.MealTypeId.ToString()), "MealTypeId");
                    formData.Add(new StringContent(menuDto.ItemName),"ItemName");
                    formData.Add(new StringContent(menuDto.ItemCategoryId.ToString()), "ItemCategoryId");


                    //Add the file to the request
                    if (menuDto.MenuIdImageFile !=null && menuDto.MenuIdImageFile.Length > 0)
                    {
                        formData.Add(new StreamContent(menuDto.MenuIdImageFile.OpenReadStream())
                        {
                            Headers = { ContentLength = menuDto.MenuIdImageFile.Length,
                                ContentType = new MediaTypeHeaderValue(
                                    menuDto.MenuIdImageFile.ContentType)
                            }
                        }, "MenuIdImageFile", menuDto.MenuIdImageFile.FileName);
                    }

                    //Send to API Code
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));


                    // Send the data to the API
                    return await httpClient.PostAsync($"{BASE_URL}/FilePost", formData);

                }
            }
        }








        //----------------------------------------------------------------------
        //EDIT MENU:GET

        [HttpGet]
        public IActionResult EditMenu(int id)
        {
            Menu menu = new Menu(); //global variable
            List<MealType> mealList = new List<MealType>();
            List<ItemCategory> itemCaList = new List<ItemCategory>();

            MenuCreateDto menuCreateDto = new MenuCreateDto();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"{BASE_URL}");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //MealType
                HttpResponseMessage mealResponse = httpClient.GetAsync($"{BASE_URL}/{MEALTYPE_ENDPOINT}").Result;
                {
                    if (mealResponse.IsSuccessStatusCode)
                    {
                        var mealData = mealResponse.Content.ReadAsStringAsync().Result;
                        mealList = JsonConvert.DeserializeObject<List<MealType>>(mealData)!;
                    }

                }

                //ItemCategory
                HttpResponseMessage itemCatResponse = httpClient.GetAsync($"{BASE_URL}/{ITEMCATEGORY_ENDPOINT}").Result;
                {
                    if (itemCatResponse.IsSuccessStatusCode)
                    {
                        var itCatData = itemCatResponse.Content.ReadAsStringAsync().Result;
                        itemCaList = JsonConvert.DeserializeObject<List<ItemCategory>>(itCatData)!;
                    }

                }


                //shows values in form
                HttpResponseMessage menuItemResponse = httpClient.GetAsync($"{BASE_URL}/{id}").Result;
                {
                    if (menuItemResponse.IsSuccessStatusCode)
                    {
                        var menuData = menuItemResponse.Content.ReadAsStringAsync().Result;
                        menu = JsonConvert.DeserializeObject<Menu>(menuData)!;
                    }

                }

                //DDL in VM
                var viewModel = new MenuVM
                {
                    Id = menu.Id,
                    SelectedMealTypeId = menu.MealTypeId,
                    ItemName = menu.ItemName,
                    SelectedItemCatId = menu.ItemCategoryId,
                    MenuIdImageFile = menuCreateDto.MenuIdImageFile,



                    MealTypeList = mealList.Select(meal => new SelectListItem
                    {
                        Text = meal.MealTypeName,
                        Value = meal.Id.ToString(),

                    }).ToList(),


                    ItemCatList = itemCaList.Select(itemCat => new SelectListItem
                    {
                        Text = itemCat.CategoryName,
                        Value = itemCat.Id.ToString(),

                    }).ToList()

                };
                return View(viewModel);

            }

        }




        //EDIT MENU:POST
        [HttpPost]
        public async Task<IActionResult> EditMenu(MenuVM menuVm)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"{BASE_URL}");
            httpClient.DefaultRequestHeaders.Accept.Clear();

            //
            if (!ModelState.IsValid) return View(menuVm);

            var menu = new MenuCreateDto
            {
                MealTypeId = menuVm.SelectedMealTypeId,
                ItemName = menuVm.ItemName,
                ItemCategoryId = menuVm.SelectedItemCatId,
                MenuIdImageFile = menuVm.MenuIdImageFile
            };


            //Call the Method to Send the DTO with the info to the API
            var response = await SendMenuToApi2(menu, menuVm.Id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Update Menu Item");
                return View(menuVm);
            }//end create method


        }





        //EDIT MENU: POST
        private async Task<HttpResponseMessage> SendMenuToApi2(MenuCreateDto menuDto, int id = 0)
        {
            using (var httpClient = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    // Set the Content-Type header to "multipart/form-data"
                    formData.Headers.ContentType.MediaType = "multipart/form-data";

                    // Add menu data to the request
                    formData.Add(new StringContent(menuDto.MealTypeId.ToString()), "MealTypeId");
                    formData.Add(new StringContent(menuDto.ItemName), "ItemName");
                    formData.Add(new StringContent(menuDto.ItemCategoryId.ToString()), "ItemCategoryId");


                    //Add the file to the request
                    if (menuDto.MenuIdImageFile != null && menuDto.MenuIdImageFile.Length > 0)
                    {
                        formData.Add(new StreamContent(menuDto.MenuIdImageFile.OpenReadStream())
                        {
                            Headers = { ContentLength = menuDto.MenuIdImageFile.Length,
                                ContentType = new MediaTypeHeaderValue(
                                    menuDto.MenuIdImageFile.ContentType)
                            }
                        }, "MenuIdImageFile", menuDto.MenuIdImageFile.FileName);
                    }

                    //Send to API Code
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));


                    // Send the data to the API
                    return await httpClient.PutAsync($"{BASE_URL}/filePut/{id}", formData);

                }
            }
        }










        //----------------------
        //DETAIL
        public IActionResult Detail(int id)
        {
            Menu menu = new Menu();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"{BASE_URL}/{id}");
                
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = httpClient.GetAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    menu = JsonConvert.DeserializeObject<Menu>(data);
                }
            }
            return View(menu);
        }








        //-----------------------
        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
             Menu menu = new Menu();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}"); ///string interpulation

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    menu = JsonConvert.DeserializeObject<Menu>(data);
                }
            }
            return View(menu);
        }  ///delete get: exact same as detail get


        //---------------------
        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, Menu menu) ///Similar to detail, but includes menu
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}"); //string interpulation

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    menu = JsonConvert.DeserializeObject<Menu>(data);
                }
            }
            return RedirectToAction("Index");
        }







    }
}
