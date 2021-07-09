using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PersonUT.Models
{

    public class Person
    {
        [Key]
        [Display(Name = "Email address")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Required]
        public bool? Gender { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public int Phone { get; set; }

        [Display(Name = "Birth Place")]
        public string BirthPlace { get; set; }
        public int Age { get; set; }

        [Display(Name = "Gradated")]
        public bool? IsGradated { get; set; }

    }

}