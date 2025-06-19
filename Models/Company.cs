using System.ComponentModel;

namespace Vigilante.Models
{
    public class Company
    {
        //primary key
        public int Id { get; set; }

        [DisplayName("Company Name")]
        public string Name { get; set; }

        [DisplayName("Company Description")]
        public string Description { get; set; }


        //Navigation Properties
        //for multi tenancy
        public virtual ICollection<VGUser> Members { get; set; }

        public virtual  ICollection<Project> Projects { get; set; }

        //create a relationship to the Invite table
        public virtual ICollection<Invite> Invites { get; set; }
    }
}
