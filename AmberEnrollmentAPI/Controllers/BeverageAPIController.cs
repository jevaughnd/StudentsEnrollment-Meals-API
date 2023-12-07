using AmberEnrollmentAPI.Data;
using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmberEnrollmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeverageAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _cxt;

        public BeverageAPIController(ApplicationDbContext cxt)
        {
            this._cxt = cxt;
        }

        //Beverage

        [HttpGet("BeverageOption")]

        public IActionResult GetBeverages()
        {
            var bItems = _cxt.BeverageOptions.ToList();

            if(bItems == null)
            {
                return BadRequest();
            }
            return Ok(bItems);
        }



        [HttpGet("{id}")]
        public IActionResult GetBeverageById(int id)
        {
            var bItem = _cxt.BeverageOptions.FirstOrDefault(x => x.Id == id);

            if(bItem == null)
            {
                return NotFound();
            }
            return Ok(bItem);
        }


        [HttpPost("BeverageOptionPost")]
        public IActionResult CreateBeverage([FromBody] BeverageOption values )
        {
            _cxt.BeverageOptions.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetBeverageById), new {id = values.Id}, values );
        }



        [HttpPut("BeverageOptionPut")]

        public IActionResult UpdateBeverage([FromBody] BeverageOption values )
        {
            if(values.Id == null)
            {
                return NotFound();
            }
            _cxt.BeverageOptions.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetBeverageById), new {id = values.Id}, values );
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBeverage(int id)
        {
            var beverage = _cxt.BeverageOptions.FirstOrDefault(x => x.Id == id);
            if (beverage == null)
            {
                return NotFound();
            }
            _cxt.BeverageOptions.Remove(beverage);
            _cxt.SaveChanges();
            return Ok(beverage);
        }



    }
}
