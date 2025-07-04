﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Vigilante.Extensions;

namespace Vigilante.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("File Date")]
        public DateTimeOffset Created { get; set; }

        [DisplayName("Team Member")]
        public string? UserId { get; set; }


        //attachment properties
        [DisplayName("File Description")]
        public string? Description { get; set; }

        [NotMapped]
        [DisplayName("Select a file")]
        [DataType(DataType.Upload)]
        [MaxFileSize(1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".ppt", ".pptx", ".html" })]
        public IFormFile? FormFile { get; set; }

        [DisplayName("File Name")]
        public string? FileName { get; set; }

        //I might take this out
        public byte[]? FileData { get; set; }

        [DisplayName("File Extension")]
        public string? FileContentType { get; set; }


        //Navigation Properties
        [BindNever]
        public virtual Ticket? Ticket { get; set; }
        [BindNever]
        public virtual VGUser? User { get; set; }

    }
}
