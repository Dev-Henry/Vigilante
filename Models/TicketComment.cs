using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Vigilante.Models
{
    public class TicketComment
    {
        public int Id { get; set; }

        [DisplayName("Member Comment")]
        public string Comment { get; set; }

        [DisplayName("Date Created")]
        public DateTimeOffset Created { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("Team Member")]
        [BindNever]
        public string? UserId { get; set; }



        //Navigation properties
        [BindNever]
        public virtual Ticket? Ticket { get; set; }

        [BindNever]
        public virtual VGUser? User { get; set; }
    }
}

