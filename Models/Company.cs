using System.ComponentModel;

namespace Vigilante.Models
{
    public class Company
    {
        public int Id { get; set; }

        [DisplayName("Company Name")]
        public string Name { get; set; }

        [DisplayName("Company Description")]
        public string Description { get; set; }


        //Navigation Properties
        //for multi tenancy
        public virtual ICollection<VGUser> Members { get; set; }

        public virtual  ICollection<Project> Projects { get; set; }
    }
}
