using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace Vigilante.Models
{   
    public class TicketHistory
    {
        //this model is different because it will be updated dynamically unlike 
        //ticket type, status and proirity which will be seeded properties
        public int Id { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("Updated Item")]
        public string Property { get; set; }

        [DisplayName("Previous")]
        public string OldValue { get; set; }

        [DisplayName("Current")]
        public string NewValue { get; set; }

        [DisplayName("Date Modified")]
        public DateTimeOffset Created { get; set; }

        [DisplayName("Description of Changes Made")]
        public string Description { get; set; }

        [DisplayName("Team Member")]
        public string UserId { get; set; }

        //Navigation Properties 
        //Create relationships
        public virtual Ticket Ticket { get; set; }

        public virtual VGUser User { get; set; }

    }
}


