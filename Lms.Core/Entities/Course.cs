using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Models.Entities
{
    /// <summary>
    /// Course entity
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Course id. Primary key
        /// </summary>
        [Key]
        public int CourseId { get; set; }

        /// <summary>
        /// Course title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Course start datum
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Course modules
        /// </summary>
        public ICollection<Module> Modules { get; set; }
    }
}