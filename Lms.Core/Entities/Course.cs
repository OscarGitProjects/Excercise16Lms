using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Models.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public ICollection<Module> Modules { get; set; }
    }
}