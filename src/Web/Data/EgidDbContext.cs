﻿using EGID.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace EGID.Web.Data
{
    public class EgidDbContext : DbContext
    {
        public EgidDbContext(DbContextOptions<EgidDbContext> options) : base(options) {}
         
        public DbSet<HealthInfo> HealthInformation { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<Citizen> Citizens { get; set; }

        public DbSet<CitizenUpdateRequest> CitizenUpdateRequests { get; set; }
    }
}
