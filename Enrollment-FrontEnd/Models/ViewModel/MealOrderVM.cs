using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Enrollment_FrontEnd.Models.ViewModel
{
    public class MealOrderVM
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }


        //Relation
        [Display(Name = "Student")]
        public int? StudentId { get; set; }


        //Relation
        [Display(Name = "Program")]
        public int ProgrammeId { get; set; }


        //Relation
        [Display(Name = "Meal Type")]
        public int MealTypeId { get; set; }
       


        //Relation
        [Display(Name = "Meat Option")]
        public int MeatOptionId { get; set; }


        //Relation
        [Display(Name = "Starch Option")]
        public int StarchOptionId { get; set; }


        //Relation
        [Display(Name = "Side Option")]
        public int SideOptionId { get; set; }

        //Relation
        [Display(Name = "Beverage")]
        public int BeverageOptionId { get; set; }





        //---------------------------------------------------------------

        //DDList Values
        public List<SelectListItem>? StudentSelectList { get; set; }
        public List<SelectListItem>? ProgrammeSelectList { get; set; }

        //
        public List<SelectListItem>? MealItemsList { get; set; }

        //
        public List<SelectListItem>? MeatItemsList { get; set; }
        public List<SelectListItem>? StarchItemsList  { get; set; }
        public List<SelectListItem>? SideItemsList { get; set; }
        public List<SelectListItem>? BeverageItemsList { get; set; }


        //Selected Values
        public int SelectedStudentId { get; set; }
        public int SelectedProgrammeId { get; set; }
        //
        public int SelectedMealItemId { get; set; }


        public int SelectedMeatItemId { get; set; }
        public int SelectedStarchItemId { get; set; }
        public int SelectedSideItemId { get; set; }
        public int SelectedBeverageItemId { get; set; }


      

    }
}
