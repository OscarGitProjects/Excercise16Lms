using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lms.Core.Models.Entities
{
    /// <summary>
    /// Module entity
    /// </summary>
    public class Module
    {
        /// <summary>
        /// Module id. Primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Module title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Module start datum
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Id för den course som module tillhör
        /// </summary>
        [ForeignKey("Course")]
        public int CourseId { get; set; }
    }
}
