using System.ComponentModel.DataAnnotations;

namespace School.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName{ get; set; }
    }
}
