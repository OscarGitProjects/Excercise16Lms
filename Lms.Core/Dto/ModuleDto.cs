using System;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Dto
{
    /// <summary>
    /// Information om module data
    /// </summary>
    public class ModuleDto
    {
        /// <summary>
        /// Module id
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Module title
        /// </summary>
        [Required(ErrorMessage = "Måste ange titel")]
        [StringLength(256, ErrorMessage = "Max 256 bokstäver")]
        public string Title { get; set; }

        /// <summary>
        /// Module start datum
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Module slut datum dvs start datum plus 1 månad
        /// </summary>
        public DateTime EndDate { 
            get {
                return StartDate.AddMonths(1);
            }
        }
    }
}
