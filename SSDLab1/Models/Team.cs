using System;
using System.ComponentModel.DataAnnotations;

namespace SSDLab1.Models
{
    public class Team
    {
        [Key]
        public string Id { get; set; }
        
        [Required, Display(Name = "Team Name")]
        public string TeamName  { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Display(Name = "Established Date")]
        public DateTime EstablishedDate  { get; set; }
    }
}