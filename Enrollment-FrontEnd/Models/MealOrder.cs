using Enrollment_FrontEnd.Controllers;
using Enrollment_FrontEnd.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;


namespace AmberEnrollmentAPI.Models
{
    public class MealOrder
    {

        public int Id { get; set; }


        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}, {0:hh:mm tt}")]
        public DateTime Date { get; set; }




        //Relation
        [Display(Name = "Student")]
        public int StudentId { get; set; } //took off ?, off of int
        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }



        //Relation
        [Display(Name = "Programme")]
        public int ProgrammeId { get; set; }
        [ForeignKey("ProgrammeId")]
        public virtual Programme? Programme { get; set; }




        //relation
        [Display(Name = "Meal Type")]
        public int MealTypeId { get; set; }
        [ForeignKey("MealTypeId")]
        public virtual MealType? MealType { get; set; }




        //Relation
        [Display(Name = "Meat")]
        public int MeatOptionId { get; set; }
        [ForeignKey("MeatOptionId")]
        //public virtual MeatOption? MeatOption { get; set; }
        public virtual MeatOption? MeatOption { get; set; }


        //Relation
        [Display(Name = "Starch")]
        public int StarchOptionId { get; set; }
        [ForeignKey("StarchOptionId")]
        public virtual StarchOption? StarchOption { get; set; }



        //Relation
        [Display(Name = "Side")]
        public int SideOptionId { get; set; }
        [ForeignKey("SideOptionId")]
        public virtual SideOption? SideOption { get; set; }



        //Relation
        [Display(Name = "Beverage")]
        public int BeverageOptionId { get; set; }
        [ForeignKey("BeverageOptionId")]
        public virtual BeverageOption? BeverageOption { get; set; }






    }
}
