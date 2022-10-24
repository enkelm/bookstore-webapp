﻿using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Cover Type")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
