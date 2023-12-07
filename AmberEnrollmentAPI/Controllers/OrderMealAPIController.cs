using AmberEnrollmentAPI.Data;
using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmberEnrollmentAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderMealAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _cxt;

        public OrderMealAPIController(ApplicationDbContext cxt)
        {
            this._cxt = cxt;
        }

        //MEAL_ORDER END POINTS
        [HttpGet("MealOrder")]
        public IActionResult GetOrders()
        {
            var oItems = _cxt.MealOrders.Include(b => b.Student)
                                        .Include(b => b.Programme)
                                        .Include(b => b.MealType)
                                        .Include(b => b.MeatOption)
                                        .Include(b => b.StarchOption)
                                        .Include(b => b.SideOption)
                                        .Include(b => b.BeverageOption)
                                        .ToList();
            if (oItems == null)
            {
                return BadRequest();
            }
            return Ok(oItems);

        }

        //Finds Order Record
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var oItem = _cxt.MealOrders.Include(b => b.Student)
                                       .Include(b => b.Programme)
                                       .Include(b => b.MealType)
                                       .Include(b => b.MeatOption)
                                       .Include(b => b.StarchOption)
                                       .Include(b => b.SideOption)
                                       .Include(b => b.BeverageOption)
                                       .FirstOrDefault(x => x.Id == id);
            if (oItem == null)
            {
                return NotFound();
            }
            return Ok(oItem);

        }

        //To create Order Record
        [HttpPost]
        [Route("MealOrder-Post")]
        public IActionResult CreateMealOrder([FromBody] MealOrder values)
        {
            _cxt.MealOrders.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetOrderById), new { id = values.Id }, values);
       
        }


        //-----------------------
        //Editing A Order Record
        [HttpPut("MealOrder-Put")]
        public IActionResult UpdateOrder( [FromBody] MealOrder values)
        {
            //if (values.Id != null)
            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.MealOrders.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetOrderById), new { id = values.Id }, values);
        }


        //-------------------
        //Delete Order Record
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _cxt.MealOrders.Include(b => b.Student)
                                       .Include(b => b.Programme)
                                       .Include(b => b.MealType)
                                       .Include(b => b.MeatOption)
                                       .Include(b => b.StarchOption)
                                       .Include(b => b.SideOption)
                                       .Include(b => b.BeverageOption)
                                       .FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            _cxt.MealOrders.Remove(order);
            _cxt.SaveChanges();
            return Ok(order);
        }








        //=========================================================================================================================//
        //STUDENT END POINTS
        [HttpGet("Student")]
        public IActionResult GetStudents()
        {
            var pupil = _cxt.Students.ToList();
            if (pupil == null)
            {
                return BadRequest();
            }


            return Ok(pupil);
        }


        //----------------------------
        //Finds Record where id is = to the result of the firstOrDefault query
        [HttpGet("Student/{Id}")]
        public IActionResult GetStudentById(int id)
        {
            var pupil = _cxt.Students.FirstOrDefault(x => x.Id == id); //gets individual student by id

            if (pupil == null)
            {
                return NotFound();
            }
            return Ok(pupil);
        }


        //To Create Student Record
        //----------------------
        [HttpPost("Student-Post")]
        public IActionResult CreateStudent([FromBody] Student values)
        {
            _cxt.Students.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetStudentById), new { id = values.Id }, values);
        }



        //---------------------
        //Editing Student Record
        [HttpPut("Student-Put")]
        public IActionResult UpdateStudent(int id, [FromBody] Student values)
        {
            var pupil = _cxt.Students.FirstOrDefault(x => x.Id == id);
            if (pupil == null)
            {
                return NotFound();
            }
            _cxt.Students.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetStudentById), new { id = values.Id }, values);
        }











        //=========================================================================================================================//
        //PROGRAMME END POINTS
        //-----------------------
        [HttpGet("Programme")]
        public IActionResult GetProgrammes()
        {
            var progItem = _cxt.Programmes.ToList();
            if (progItem == null)
            {
                return BadRequest();
            }
            return Ok(progItem);
        }

        //-----------------------
        //Finds Record where id is = to the result of the firstOrDefault query
        [HttpGet("Programme/{Id}")]
        public IActionResult GetProgrammeById(int id)
        {
            var progItem = _cxt.Programmes.FirstOrDefault(x => x.Id == id); //gets individual programme by Id

            if (progItem == null)
            {
                return NotFound();
            }
            return Ok(progItem);
        }



        //-----------------------
        [HttpPost("Programme-Post")]
        public IActionResult CreateProgramme([FromBody] Programme values)
        {
            _cxt.Programmes.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetProgrammeById), new { id = values.Id }, values);
        }


        //-----------------------
        [HttpPut("Programme-Put")]
        public IActionResult UpdateProgramme(int id, [FromBody] Programme values)
        {
            var progItem = _cxt.Programmes.FirstOrDefault(x => x.Id == id);
            if (progItem == null)
            {
                return NotFound();
            }
            _cxt.Programmes.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetProgrammeById), new { id = values.Id }, values);
        }




        //================================================================================================

        //MEAL TYPE END POINTS
        [HttpGet("MealType")]
        public IActionResult GetMealTypes()
        {
            var mealType = _cxt.MealTypes.ToList();
            if (mealType == null)
            {
                return BadRequest();
            }
            return Ok(mealType);
        }


        [HttpGet("MealType/{Id}")]
        public IActionResult GetMealTypeById(int id)
        {
            var mealType = _cxt.MealTypes.FirstOrDefault(c => c.Id == id); //gets individual mealtype record

            if (mealType == null)
            {
                return NotFound();
            }
            return Ok(mealType);
        }



        [HttpPost("MealTypePost")]
        public IActionResult CreateMealType([FromBody] MealType values)
        {
            _cxt.MealTypes.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetMealTypeById), new { id = values.Id }, values);
        }



        [HttpPut("MealTypePut")]
        public IActionResult UpdateMealType(int id, [FromBody] MealType values)
        {
            var mealType = _cxt.MealTypes.FirstOrDefault(c => c.Id == id);
            if (mealType == null)
            {
                return NotFound();
            }
            _cxt.MealTypes.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetMealTypeById), new { id = values.Id }, values);
        }




        //================================================================================================
        //MEAT END POINTS


        [HttpGet("MeatOption")]
        public IActionResult GetMeats()
        {
            var meatItem = _cxt.MeatOptions.ToList();

            if (meatItem == null) 
            { 
                return BadRequest(); 
            }
            return Ok(meatItem);
        }



        [HttpGet("MeatOption/{Id}")]
        public IActionResult GetMeatById(int id)
        {
            var meatItem = _cxt.MeatOptions.FirstOrDefaultAsync(c => c.Id == id);

            if (meatItem == null)
            {
                return NotFound();
            }
            return Ok(meatItem);
        }



        [HttpPost("MeatOptionPost")]
        public IActionResult CreateMeat([FromBody] MeatOption values)
        {
            _cxt.MeatOptions.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetMeatById), new {id = values.Id}, values);
        }



        [HttpPut("MeatOptionPut")]
        public IActionResult UpdateMeat(int id, [FromBody] MeatOption values)
        {
            var meatItem = _cxt.MeatOptions.FirstOrDefault(c => c.Id == id);
            if(meatItem == null)
            {
                return NotFound();
            }
            _cxt.MeatOptions.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetMeatById), new {id = values.Id}, values);
        }











        //================================================================================================
        //STARCH END POINTS


        [HttpGet("StarchOption")]
        public IActionResult GetStarches()
        {
            var starchItem = _cxt.StarchOptions.ToList();

            if (starchItem == null)
            {
                return BadRequest();
            }
            return Ok(starchItem);
        }



        [HttpGet("StarchOption/{Id}")]
        public IActionResult GetStarchById(int id)
        {
            var starchItem = _cxt.StarchOptions.FirstOrDefaultAsync(s => s.Id == id);

            if (starchItem == null)
            {
                return NotFound();
            }
            return Ok(starchItem);
        }



        [HttpPost("StarchOptionPost")]
        public IActionResult CreateStarch([FromBody] StarchOption values)
        {
            _cxt.StarchOptions.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetStarchById), new { id = values.Id }, values);
        }



        [HttpPut("StarchOptionPut")]
        public IActionResult UpdateStarch(int id, [FromBody] StarchOption values)
        {
            var starchItem = _cxt.StarchOptions.FirstOrDefault(s => s.Id == id);
            if (starchItem == null)
            {
                return NotFound();
            }
            _cxt.StarchOptions.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetStarchById), new { id = values.Id }, values);
        }










        //================================================================================================
        //SIDE OPTION END POINTS
        [HttpGet("SideOption")]
        public IActionResult GetSideOption()
        {
            var sideItem = _cxt.SideOptions.ToList();

            if (sideItem == null)
            {
                return BadRequest();
            }
            return Ok(sideItem);
        }



        [HttpGet("SideOption/{Id}")]
        public IActionResult GetSideById(int id)
        {
            var sideItem = _cxt.SideOptions.FirstOrDefaultAsync(s => s.Id == id);

            if (sideItem == null)
            {
                return NotFound();
            }
            return Ok(sideItem);
        }



        [HttpPost("SideOptionPost")]
        public IActionResult CreateSide([FromBody] SideOption values)
        {
            _cxt.SideOptions.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetSideById), new { id = values.Id }, values);
        }



        [HttpPut("SideOptionPut")]
        public IActionResult UpdateSide(int id, [FromBody] SideOption values)
        {
            var sideItem = _cxt.SideOptions.FirstOrDefault(s => s.Id == id);
            if (sideItem == null)
            {
                return NotFound();
            }
            _cxt.SideOptions.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetSideById), new { id = values.Id }, values);

        }







        //================================================================================================
        //BEVERAGE END POINTS

        [HttpGet("BeverageOption")]
        public IActionResult GetBeverageOption()
        {
            var beverageItem = _cxt.BeverageOptions.ToList();

            if (beverageItem == null)
            {
                return BadRequest();
            }
            return Ok(beverageItem);
        }



        [HttpGet("BeverageOption/{Id}")]
        public IActionResult GetBeverageById(int id)
        {
            var beverageItem = _cxt.BeverageOptions.FirstOrDefaultAsync(b => b.Id == id);

            if (beverageItem == null)
            {
                return NotFound();
            }
            return Ok(beverageItem);
        }



        [HttpPost("BeverageOptionPost")]
        public IActionResult CreateBeverage([FromBody] BeverageOption values)
        {
            _cxt.BeverageOptions.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetBeverageById), new { id = values.Id }, values);
        }



        [HttpPut("BeverageOptionPut")]
        public IActionResult UpdateBeverage(int id, [FromBody] BeverageOption values)
        {
            var beverageItem = _cxt.BeverageOptions.FirstOrDefault(b => b.Id == id);
            if (beverageItem == null)
            {
                return NotFound();
            }
            _cxt.BeverageOptions.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetBeverageById), new { id = values.Id }, values);

        }

















    }
}
