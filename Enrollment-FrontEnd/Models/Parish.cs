using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Enrollment_FrontEnd.Models
{
    public class Parish
    {
        public int Id { get; set; }


        [Display(Name = "Parish")]
        public string ParishName { get; set; }
    }
}
