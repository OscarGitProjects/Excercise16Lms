using System;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Dto
{
    /// <summary>
    /// Information om course data
    /// </summary>
    public class CourseDto
    {
        /// <summary>
        /// Course id
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// Course title
        /// </summary>
        [Required(ErrorMessage = "Måste ange titel")]
        [StringLength(256, ErrorMessage = "Max 256 bokstäver")]
        public string Title { get; set; }

        /// <summary>
        /// Course start datum
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Course slut datum dvs start datum plus 3 månader
        /// </summary>
        public DateTime EndDate { 
            get {
                return StartDate.AddMonths(3);                    
            }
        }
    }
}
