using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmberEnrollmentAPI.Models
{
    public class MealOrder
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }


        //Relation
        public int? StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }


        //Relation
        public int ProgrammeId { get; set; }
        [ForeignKey("ProgrammeId")]
        public virtual Programme? Programme { get; set; }


        ////Relation
        //public int MenuId { get; set; }
        //[ForeignKey("MenuId")]
        //public virtual Menu? Menu { get; set; }


        ////relation
        public int MealTypeId { get; set; }
        [ForeignKey("MealTypeId")]
        public virtual MealType? MealType { get; set; }





        //Relation
        public int MeatOptionId { get; set; }
        [ForeignKey("MeatOptionId")]
        public virtual MeatOption? MeatOption { get; set; }


        //Relation
        public int StarchOptionId { get; set; }
        [ForeignKey("StarchOptionId")]
        public virtual StarchOption? StarchOption { get; set; }



        //Relation
        public int SideOptionId { get; set; }
        [ForeignKey("SideOptionId")]
        public virtual SideOption? SideOption { get; set; }



        //Relation
        public int BeverageOptionId { get; set; }
        [ForeignKey("BeverageOptionId")]
        public virtual BeverageOption? BeverageOption { get; set; }






    }
}
