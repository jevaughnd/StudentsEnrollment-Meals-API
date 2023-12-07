using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmberEnrollmentAPI.Models
{
    public class Menu
    {
        [Key]


        public int Id { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string ItemName { get; set; }




        //Relation
        public int ItemCategoryId { get; set; }
        [ForeignKey("ItemCategoryId")]
        public virtual ItemCategory? ItemCategory { get; set; }

        //Relation
        public int MealTypeId { get; set; }
        [ForeignKey("MealTypeId")]
        public virtual MealType? MealType { get; set; }

     


        //Image
        [Column(TypeName = "varchar(500)")]
        public string? MenuIdImageFilePath { get; set; } = String.Empty;






    }
}
