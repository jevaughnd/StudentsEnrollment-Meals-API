using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Enrollment_FrontEnd.Models
{
    public class ItemCategory
    {
        public int Id { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }
    }
}
