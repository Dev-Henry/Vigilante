using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vigilante.Models
{
    public class Notification
    {
        //primary key
        public int Id { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [Required]
        [DisplayName("Title")]
        public string? Title { get; set; }

        [Required]
        [DisplayName("Title")]
        public string? Message { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Created")]
        public DateTimeOffset Created { get; set; }

        [Required]
        [DisplayName("Recipient")]
        public string? RecipientId { get; set; }

        [Required]
        [DisplayName("Sender")]
        public string? SenderId { get; set; }

        [DisplayName("Has been viewed")] 
        public bool Viewed { get; set; }


        //Navigation Properties
        //relationship to ticket and VGUser
        public virtual Ticket? Ticket { get; set; }

        public virtual VGUser? Recipient { get; set; }

        public virtual VGUser? Sender { get; set; }
    }
}
