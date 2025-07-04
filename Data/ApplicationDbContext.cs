﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vigilante.Models;

namespace Vigilante.Data
{
    public class ApplicationDbContext : IdentityDbContext<VGUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Invite> Invites { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<NotificationType> NotificationTypes { get; set; }


        public DbSet<ProjectPriority> ProjectPriorities { get; set; }

        public DbSet<TicketAttachment> TicketAttachments { get; set; }

        public DbSet<TicketComment> TicketComments { get; set; }

        public DbSet<TicketHistory> TicketHistories { get; set; }

        public DbSet<TicketPriority> TicketPriorities { get; set; }

        public DbSet<TicketStatus> TicketStatuses { get; set; }

        public DbSet<TicketType> TicketTypes { get; set; }

    }
}
