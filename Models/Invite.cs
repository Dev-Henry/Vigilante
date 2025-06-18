using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vigilante.Models
{
    public class Invite
    {   
        //primary key
        public int Id { get; set; }

        [DisplayName("Date Sent")]
        public DateTimeOffset InviteDate { get; set; }

        [DisplayName("Joined Date")]
        public DateTimeOffset JoinDate { get; set; }

        [DisplayName("Code")]
        public Guid CompanyToken { get; set; }

        //foreign keys
        [DisplayName("Company")]
        public int CompanyId { get; set; }


        [DisplayName("Project")]
        public int ProjectId { get; set; }


        //these are strings because they are User IDs
        [DisplayName("Invitor")]
        public string InvitorId { get; set; }


        [DisplayName("Invitee")]
        public string InviteeId { get; set; }


        //invitee info table
        [DisplayName("Invitee Email")]
        public string InviteeEmail { get; set; }

        [DisplayName("Invitee First Name")]
        public string InviteeFirstName { get; set; }

        [DisplayName("Invitee Last Name")]
        public string InviteeLastName { get; set; }


        //record keeping - determine validity of invite
        public bool IsValid { get; set; }

        //navigation properties
        public virtual  Company Company { get; set; }

        public virtual VGUser Invitor { get; set; }

        public virtual VGUser Invitee { get; set; }

        public virtual Project Project { get; set; }

    }
}
