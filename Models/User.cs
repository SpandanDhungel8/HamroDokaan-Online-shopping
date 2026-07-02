
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
namespace HamroDokaan.Models
{


    public class User
    {
        [Key]
        public int User_Id { get; set; }

        [Required]
        [EmailAddress]
        public string User_Email { get; set; }

        [Required]
        public string User_Password { get; set; }

        public string User_Role { get; set; } = "Customer";

        public bool IsApproved { get; set; }

        public List<Order>? Orders { get; set; } 
    }
}