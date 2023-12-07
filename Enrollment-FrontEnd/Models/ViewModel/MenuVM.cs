using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Enrollment_FrontEnd.Models.ViewModel
{
    public class MenuVM
    {
        public int Id { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        //relation
        [Display(Name = "Category")]
        public int ItemCategoryId { get; set; }

        //relation
        [Display(Name = "Meal Type")]
        public int MealTypeId { get; set; }


        //DDlist values
        public List<SelectListItem>? MealTypeList { get; set; }
        public List<SelectListItem>? ItemCatList { get; set; }
        
        //Selected id values
        public int SelectedMealTypeId { get; set; }
        public int SelectedItemCatId { get; set; }
        
        public IFormFile? MenuIdImageFile { get; set; }
    }
}
