using Microsoft.Build.Evaluation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vigilante.Models
{
    public class Ticket
    {
        //Primary Key
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Title")]
        public string? Title { get; set; }

        [Required]
        [DisplayName("Description")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Date Created")]
        public DateTimeOffset Created { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Updated")]
        public DateTimeOffset? Updated { get; set; }

        [DisplayName("Archived")]
        public bool Archived { get; set; }

        [DisplayName("Project")]
        public int ProjectId { get; set; }


        //referencing the look up tables (type, priority, status)
        [DisplayName("Ticket Type")]
        public int TicketTypeId { get; set; }

        [DisplayName("Ticket Priority")]
        public int TicketPriorityId { get; set; }

        [DisplayName("Ticket Status")]
        public int TicketStatusId { get; set; }


        //these are not int because they are our identity users 
        [DisplayName("Ticket Owner")]
        public string? OwnerUserId { get; set; }

        [DisplayName("Ticket Developer")]
        public string?   DeveloperUserId { get; set; }


        //Navigation Properties

        //foreign keys with IDs
        public virtual Project? Project { get; set; }
        public virtual TicketType? TicketType { get; set; }

        public virtual TicketPriority? TicketPriority { get; set; }

        public virtual TicketStatus? TicketStatus { get; set; }

        public virtual VGUser? OwnerUser { get; set; }

        public virtual VGUser? DeveloperUser { get; set; }


        //multi relationships
        public virtual ICollection<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();

        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();

        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();

        public virtual ICollection<TicketHistory> History { get; set; } = new HashSet<TicketHistory>();



    }
}
