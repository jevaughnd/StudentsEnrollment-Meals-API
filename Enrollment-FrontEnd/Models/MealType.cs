using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Enrollment_FrontEnd.Models
{
    public class MealType
    {
        public int Id { get; set; }


        [Display(Name = "Meal Type")]
        public string MealTypeName { get; set; }
    }
}
