﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Dto
{
    public class CourseDto
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Måste ange titel")]
        [StringLength(256, ErrorMessage = "Max 256 bokstäver")]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { 
            get {
                return StartDate.AddMonths(3);                    
            }
        }
    }
}