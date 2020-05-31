using System;
using System.Collections.Generic;

namespace DotNetCoreCsharpProject.Entities
{
    public partial class PlanningDates
    {
        public int Id { get; set; }
        public string PlanningSoutenanceFile { get; set; }
        public DateTime? DateLimitRapportAvt1 { get; set; }
        public DateTime? DateLimitRapportAvt2 { get; set; }
        public DateTime? DateLimitRapportAvt3 { get; set; }
        public DateTime? DateLimitRapportAvt4 { get; set; }
        public DateTime? DateLimitRapportFinal { get; set; }
        public DateTime? DateOuvertureEnrGrp { get; set; }
        public DateTime? DateFinEnrGrp { get; set; }
        public DateTime? DateAttributionEnc { get; set; }
        public DateTime? DateLimitInsSujet { get; set; }
    }
}
