﻿using System;
using System.Collections.Generic;

namespace RRPatients.Models
{
    public partial class DispensingUnit
    {
        public DispensingUnit()
        {
            Medications = new HashSet<Medication>();
        }

        public string DispensingCode { get; set; } = null!;

        public virtual ICollection<Medication> Medications { get; set; }
    }
}
