using System.ComponentModel.DataAnnotations;

namespace Vigilante.Models
{
    public class NotificationType
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Notification Type")]
        public string? Name { get; set; }
    }
}

