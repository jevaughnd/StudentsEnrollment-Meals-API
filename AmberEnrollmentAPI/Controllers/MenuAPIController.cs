using AmberEnrollmentAPI.Data;
using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmberEnrollmentAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MenuAPIController : ControllerBase
    {


        private readonly ApplicationDbContext _ctx;

        public MenuAPIController(ApplicationDbContext context)
        {
            this._ctx = context;
        }


        //MENU END POINTS
        [HttpGet("Menu")]
        public IActionResult GetMenus()
        {
            var menuItems = _ctx.Menus.Include(b => b.ItemCategory)
                                      .Include(b => b.MealType)
                                      .ToList();

            if (menuItems == null)
            {
                return BadRequest();
            }
            //Construct the img url for each menu item
            var baseUrl = "https://localhost:7293/images/";

            foreach (var menu in menuItems)
            {
                menu.MenuIdImageFilePath = baseUrl + menu.MenuIdImageFilePath;
            }


            return Ok(menuItems);
        }


        //-----------------------
        //Finds Menu Record by id
        [HttpGet("{id}")]
        public IActionResult GetMenuById(int id)
        {
            var menuItem = _ctx.Menus.Include(b => b.ItemCategory)
                                     .Include(b => b.MealType)
                                     .FirstOrDefault(c => c.Id == id);//gets individual menus by id
            if (menuItem == null)
            {
                return NotFound();
            }
            return Ok(menuItem);
        }




        //---------------------
        //Create a Menu record
        [HttpPost("MenuPost")]
        public IActionResult CreateMenu([FromBody] Menu values)
        {
            _ctx.Menus.Add(values);
            _ctx.SaveChanges();
            return CreatedAtAction(nameof(GetMenuById), new { id = values.Id }, values);

        }

        //------------------------
        //Create Image File
        [HttpPost("FilePost")]
        public async Task<IActionResult> Create([FromForm] MenuCreateDto model)
        {
            if (ModelState.IsValid)
            {
                //take file from Dto
                var menuImageFile = model.MenuIdImageFile;

                if(menuImageFile != null && menuImageFile.Length > 0)
                {
                    //generate a unique file name
                    var uniqueFileName = Guid.NewGuid() + "_" + menuImageFile.FileName;

                    //define the final file path on the API server
                    var apiFilePath = Path.Combine("api", "server", "menuUploads", uniqueFileName);

                    //Save file server
                    using (var stream = new FileStream(apiFilePath, FileMode.Create))
                    {
                        await menuImageFile.CopyToAsync(stream);
                    }

                    //Store the file path in the database along with other menu details
                    var menu = new Menu
                    {
                        MealTypeId= model.MealTypeId,
                        ItemName = model.ItemName,
                        ItemCategoryId = model.ItemCategoryId,
                        MenuIdImageFilePath = apiFilePath != String.Empty ? apiFilePath:"",
                    };





                    //save menu item to database
                    _ctx.Menus.Add(menu);
                    await _ctx.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetMenuById), new {id = menu.Id}, menu);
                }
            }
            return BadRequest(ModelState);
        }


        //------------------------------
        //Retrieve a link to the uploaded file
        [HttpGet("files/{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            // Construct the full path to the file based on the provided 'fileName'
            string filePath = Path.Combine("api", "server", "menuUploads", fileName);


            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Or handle the case where the file doesn't exist
            }

            // Determine the content type based on the file's extension
            string contentType = GetContentType(fileName);


            // Return the image file as a FileStreamResult with the appropriate content type
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, contentType); // Adjust the content type as needed

        }
        private string GetContentType(string fileName)
        {
            // Determine the content type based on the file's extension
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream"; // Default to binary data
            }
        }


        //----------------------------
        //Update Menu Reccord
        [HttpPut("filePut/{id}")]
        public async Task <IActionResult> UpdateMenu(int id, [FromForm] MenuCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var existingMenuItem = await _ctx.Menus.FindAsync(id);
                
                if (existingMenuItem == null)
                {
                    return NotFound();
                }

                // Check if a new menu image is provided
                if (model.MenuIdImageFile != null && model.MenuIdImageFile.Length > 0)
                {
                    // Generate a unique file name for the menu image
                    var uniqueMenuFileName = Guid.NewGuid() + "_" + model.MenuIdImageFile.FileName;

                    // Define the final file path on the server
                    var apiMenuFilePath = Path.Combine("api", "server", "menu-upload-upadted", uniqueMenuFileName);

                    // Save the new menu image to the server, overwriting the existing menu image
                    using (var stream = new FileStream(apiMenuFilePath, FileMode.Create))
                    {
                        await model.MenuIdImageFile.CopyToAsync(stream);
                    }

                    // Update the menu image file path in the database
                    existingMenuItem.MenuIdImageFilePath = apiMenuFilePath;
                }


                //Update other properties of the Menu entity based on the model
                existingMenuItem.MealTypeId = model.MealTypeId;
                existingMenuItem.ItemName = model.ItemName;
                existingMenuItem.ItemCategoryId = model.ItemCategoryId;

                //save all changes
                _ctx.Menus.Update(existingMenuItem);
                _ctx.SaveChanges();
                return CreatedAtAction(nameof(GetMenuById), new {id = existingMenuItem.Id}, existingMenuItem);
            }
            return BadRequest(ModelState);
        }









        //----------------------
        //Delete A Menu Record
        [HttpDelete("{id}")]
        public IActionResult DeleteMenuById(int id)
        {
            var menuItem = _ctx.Menus.Include(b => b.ItemCategory)
                                      .Include(b => b.MealType)
                                      .FirstOrDefault(c => c.Id == id); //Delete by individual id

            if (menuItem == null) { return NotFound(); }
            _ctx.Menus.Remove(menuItem);
            _ctx.SaveChanges();
            return Ok(menuItem);
        }




















        //===============================================================================================================================//
        //ITEM CATEGORY End Points

        [HttpGet("ItemCategory")]
        public IActionResult GetItemCategories()
        {
            var itemCat = _ctx.ItemCategories.ToList();
            if (itemCat == null)
            {
                return BadRequest();
            }
            return Ok(itemCat);
        }


        [HttpGet("ItemCategory/{Id}")]
        public IActionResult GetItemCategoryById(int id)
        {
            var itemCat = _ctx.ItemCategories.FirstOrDefaultAsync(c => c.Id == id); //Gets individual ItemCategroy by Id
            if (itemCat == null) 
            { 
                return NotFound(); 
            } 
            return Ok(itemCat);
        }



        [HttpPost("ItemCategoryPost")]
        public IActionResult CreateItemCategory([FromBody] ItemCategory values)
        {
            _ctx.ItemCategories.Add(values);
            _ctx.SaveChanges();
            return CreatedAtAction(nameof(GetItemCategoryById), new { id = values.Id }, values);

        }




        [HttpPut("ItemCategoryPut")]
        public IActionResult UpdateItemCategory(int id, [FromBody] ItemCategory values)
        {
            var itemCat = _ctx.ItemCategories.FirstOrDefault(c => c.Id == id);
            if (itemCat == null)
            {
                return NotFound();
            }
            _ctx.ItemCategories.Update(values);
            _ctx.SaveChanges();
            return CreatedAtAction(nameof(GetItemCategoryById), new {id = values.Id}, values);
        }









        //===============================================================================================================================//
        //MEAL TYPE End Points


        [HttpGet("MealType")]
        public IActionResult GetMealTypes()
        {
            var mealType = _ctx.MealTypes.ToList();
            if (mealType == null)
            { 
                return BadRequest(); 
            } 
            return Ok(mealType);
        }


        [HttpGet("MealType/{Id}")]
        public IActionResult GetMealTypeById(int id) 
        { 
            var mealType = _ctx.MealTypes.FirstOrDefault(c => c.Id==id); //gets individual mealtype record

            if (mealType == null)
            {
                return NotFound();
            }
            return Ok(mealType);
        }



        [HttpPost("MealTypePost")]
        public IActionResult CreateMealType([FromBody] MealType values)
        {
            _ctx.MealTypes.Add(values);
            _ctx.SaveChanges();
            return CreatedAtAction(nameof(GetMealTypeById), new {id = values.Id}, values);
        }



        [HttpPut("MealTypePut")]
        public IActionResult UpdateMealType(int id, [FromBody] MealType values) 
        { 
            var mealType = _ctx.MealTypes.FirstOrDefault(c => c.Id ==id);
            if(mealType == null)
            {
                return NotFound();
            }
            _ctx.MealTypes.Update(values);
            _ctx.SaveChanges();
            return CreatedAtAction(nameof(GetMealTypeById), new { id = values.Id},values );
        }




    }
}
























