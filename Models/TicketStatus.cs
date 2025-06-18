using System.ComponentModel;

namespace Vigilante.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }

        [DisplayName("Status Name")]
        public string Name { get; set; }
    }
}
