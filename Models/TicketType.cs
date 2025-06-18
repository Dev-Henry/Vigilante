using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vigilante.Models
{
    public class TicketType
    {
        public int Id { get; set; }

        [DisplayName("Type Name")]
        public string Name { get; set; }
    }
}
