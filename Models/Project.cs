using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;
using Microsoft.CodeAnalysis.Operations;

namespace Vigilante.Models
{
    public class Project
    {
        //Primary key
        public int Id { get; set; }

        [DisplayName("Company")]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Project Name")]
        public string? Name { get; set; }

        [DisplayName("Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset CreatedDate { get; set; }

        [DisplayName("Start Date")]
        public DateTimeOffset StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTimeOffset EndDate { get; set; }

        [DisplayName("Priority")]
        public int? ProjectPriorityId { get; set; }

        //attachment properties
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFormFile { get; set; }

        [DisplayName("File Name")]
        public string? ImageFileName { get; set; }

        public byte[]? ImageFileData { get; set; }

        [DisplayName("File Extension")]
        public string? ImageContentType { get; set; }

        [DisplayName("Archived")]
        public bool Archived { get; set; }

        //Navigation Properties
        public virtual Company? Company { get; set; }

        public virtual ProjectPriority? ProjectPriority { get; set; }

        public virtual ICollection<VGUser> Members { get; set; } = new HashSet<VGUser>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
