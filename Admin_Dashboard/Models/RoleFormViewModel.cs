﻿using System.ComponentModel.DataAnnotations;

namespace Admin_Dashboard.Models
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage = "Name IS Required")]
        [StringLength(256)]
        public string Name { get; set; }
    }
}
