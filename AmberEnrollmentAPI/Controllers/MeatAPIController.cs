using AmberEnrollmentAPI.Data;
using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmberEnrollmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeatAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _cxt;

        public MeatAPIController(ApplicationDbContext cxt)
        {
            this._cxt = cxt;
        }


        //MEAT END POINTS
        [HttpGet("MeatOption")]
        public IActionResult GetMeats()
        {
            var mItems = _cxt.MeatOptions.ToList();

            if (mItems == null)
            {
                return BadRequest();
            }
            return Ok(mItems);
        }



        [HttpGet("{id}")]
        public IActionResult GetMeatById(int id)
        {
            var mItem = _cxt.MeatOptions.FirstOrDefault(x => x.Id == id);

            if (mItem == null)
            {
                return NotFound();
            }
            return Ok(mItem);
        }




        [HttpPost("MeatOptionPost")]
        public IActionResult CreateMeatOption([FromBody] MeatOption values)
        {
            _cxt.MeatOptions.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetMeatById), new { id = values.Id }, values);
        }






        [HttpPut("MeatOptionPut")]
        public IActionResult UpdateMeat([FromBody] MeatOption values)
        {
            if(values.Id == null)
            {
                return NotFound();
            }
            _cxt.MeatOptions.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetMeatById), new { id = values.Id }, values);
        }




        [HttpDelete("{id}")]
        public IActionResult DeleteMeat(int id)
        {
            var meat = _cxt.MeatOptions.FirstOrDefault(m => m.Id == id);
            if (meat == null)
            {
                return NotFound();  
                
            }
            _cxt.MeatOptions.Remove(meat);
            _cxt.SaveChanges();
            return Ok(meat);
        }





    }
}
