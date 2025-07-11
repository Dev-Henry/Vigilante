﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace Vigilante.Models.ViewModels
{
    public class AddProjectWithPMViewModel
    {
        public Project? Project { get; set; }

        public SelectList? PMList { get; set; }

        public string? PmId { get; set; }

        public SelectList? PriorityList { get; set; }

    }
}
