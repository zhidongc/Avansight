using System;
using System.Collections.Generic;
using System.Text;

namespace Avansight.Domain
{
    public class TreatmentReading
    {
        public int TreatmentReadingId { get; set; }
        public string VisitWeek { get; set; }
        public double Reading { get; set; }
        public int PatientId { get; set; }
    }
}
