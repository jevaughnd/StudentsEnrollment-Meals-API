using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Enrollment_FrontEnd.Models
{
    public class Menu
    {
        public int Id { get; set; }


        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        //Relation
        public int ItemCategoryId { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }


        //Relation
        public int MealTypeId { get; set; }
        public virtual MealType MealType { get; set; }

        



        //Image
        public string? MenuIdImageFilePath { get; set; }
    }
}
