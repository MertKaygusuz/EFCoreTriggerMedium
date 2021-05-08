using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEFTriggerProject.Models
{
    public class UserDto
    {
        public int UserId { get; set; }

        [MaxLength(30, ErrorMessage = "En fazla 30 karakter girilebilir.")]
        [Required]
        public string Name { get; set; }

        [MaxLength(30, ErrorMessage = "En fazla 30 karakter girilebilir.")]
        [Required]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-mail adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public string Password { get; set; }
    }
}
