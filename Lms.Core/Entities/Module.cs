using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lms.Core.Models.Entities
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
    }
}
