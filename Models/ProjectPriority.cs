using System.ComponentModel;

namespace Vigilante.Models
{
    public class ProjectPriority
    {
        public int id { get; set; }

        [DisplayName("Project Name")]
        public string Name { get; set; }
    }
}
