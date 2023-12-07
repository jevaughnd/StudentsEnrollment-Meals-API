using AmberEnrollmentAPI.Data;
using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmberEnrollmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarchAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _cxt;

        public StarchAPIController(ApplicationDbContext cxt)
        {
            this._cxt = cxt;
        }


        //STARCH ENDPOINTS
        [HttpGet("StarchOption")]
        public IActionResult GetStarches()
        {
            var sItems = _cxt.StarchOptions.ToList();

            if (sItems == null)
            {
                return BadRequest();
            }
            return Ok(sItems);
        }



        [HttpGet("{id}")]
        public IActionResult GetStarchById(int id)
        {
            var sItem = _cxt.StarchOptions.FirstOrDefault(x => x.Id == id);

            if (sItem == null)
            {
                return NotFound();
            }
            return Ok(sItem);
        }


        [HttpPost("StarchOptionPost")]
        public IActionResult CreateStarch([FromBody] StarchOption values)
        {
            _cxt.StarchOptions.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetStarchById), new { id = values.Id }, values);
        }




        [HttpPut("StarchOptionPut")]
        public IActionResult UpdateStarch([FromBody] StarchOption values)
        {
            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.StarchOptions.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetStarchById), new { id = values.Id }, values);
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteStarch(int id)
        {
            var starch = _cxt.StarchOptions.FirstOrDefault(x => x.Id == id);

            if (starch == null) { return NotFound(); }
            _cxt.StarchOptions.Remove(starch);
            _cxt.SaveChanges();
            return Ok(starch);
        }






    }

}
