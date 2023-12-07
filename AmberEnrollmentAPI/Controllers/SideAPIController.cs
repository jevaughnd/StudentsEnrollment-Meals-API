using AmberEnrollmentAPI.Data;
using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmberEnrollmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SideAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _cxt;

        public SideAPIController(ApplicationDbContext cxt)
        {
            this._cxt = cxt;
        }

        //SIDE
        [HttpGet("SideOption")]
        public IActionResult GetSideOptions()
        {
            var sItems = _cxt.SideOptions.ToList();
            if (sItems == null)
            {
                return BadRequest();
            }
            return Ok(sItems);
        }




        [HttpGet("{id}")]
        public IActionResult GetSideById(int id)
        {
            var sItem = _cxt.SideOptions.FirstOrDefault(x => x.Id == id);

            if (sItem == null)
            {
                return NotFound();
            }
            return Ok(sItem);
        }



        [HttpPost("SideOptionPost")]
        public IActionResult CreateSideOption([FromBody] SideOption values)
        {
            _cxt.SideOptions.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetSideById), new { id = values.Id },  values);
        }




        [HttpPut("SideOptionPut")]
        public IActionResult UpdateSide([FromBody] SideOption values)
        {
            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.SideOptions.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetSideById), new { id = values.Id } ,values );
        }




        [HttpDelete("{id}")]
        public IActionResult DeleteSideOption(int id)
        {
            var side = _cxt.SideOptions.FirstOrDefault(x => x.Id == id);
            if (side == null)
            {
                return NotFound();
            }
            _cxt.SideOptions.Remove(side);
            _cxt.SaveChanges();
            return Ok(side);

        }




    }
}
