﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vigilante.Models
{
    public class VGUser : IdentityUser
    {
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name ="Full Name")]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile AvatarFormFile { get; set; }

        [DisplayName("Avatar")]
        public string? AvatarFileName { get; set; }

        public byte[]? AvatarFileData { get; set; }

        [DisplayName("File Extension")]
        public string? AvatarContentType { get; set; }

        //null company Id - temporary
        public int CompanyId { get; set; }


        //Navigation Properties
        public virtual Company Company { get; set; }
        public virtual  ICollection<Project> Projects { get; set; }

    }
}
