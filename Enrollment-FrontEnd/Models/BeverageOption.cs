using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AmberEnrollmentAPI.Models
{
    public class BeverageOption
    {
        public int Id { get; set; }




        [Display(Name = "Beverage Option Name")]
        public string BeverageName { get; set; }
    }
}