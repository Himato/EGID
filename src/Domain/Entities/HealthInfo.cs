﻿using System.Collections.Generic;
using EGID.Domain.Enums;

namespace EGID.Domain.Entities
{
    public class HealthInfo
    {
        public HealthInfo()
        {
            HealthRecords = new HashSet<HealthRecord>();
            ExitHospitalRecords = new List<ExitHospitalRecord>();
        }

        public string Id { get; set; }

        public BloodType BloodType { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }

        public string CitizenId { get; set; }
        public CitizenDetails Citizen { get; set; }

        public virtual ICollection<HealthRecord> HealthRecords { get; }
        public virtual ICollection<ExitHospitalRecord> ExitHospitalRecords { get; }
    }
}
