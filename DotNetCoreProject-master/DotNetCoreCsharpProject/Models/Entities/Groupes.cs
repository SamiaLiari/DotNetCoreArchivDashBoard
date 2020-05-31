using System;
using System.Collections.Generic;

namespace DotNetCoreCsharpProject.Entities
{
    public partial class Groupes
    {
        public Groupes()
        {
            Reports = new HashSet<Reports>();
            Students = new HashSet<Students>();
        }

        public int Id { get; set; }
        public int? Idprof { get; set; }
        public int? IdSociete { get; set; }
        public double? Note { get; set; }
        public string Sujet { get; set; }
        public string DesciptionSujet { get; set; }
        public string DesciptionFile { get; set; }
        public DateTime? DateSoutenance { get; set; }
        public DateTime? DateDepotRapport1 { get; set; }
        public DateTime? DateDepotRapport2 { get; set; }
        public DateTime? DateDepotRapport3 { get; set; }
        public DateTime? DateDepotRapport4 { get; set; }
        public string RapportAvt1 { get; set; }
        public string RapportAvt2 { get; set; }
        public string RapportAvt3 { get; set; }
        public string RapportAvt4 { get; set; }

        public virtual Societes IdSocieteNavigation { get; set; }
        public virtual Professors IdprofNavigation { get; set; }
        public virtual ICollection<Reports> Reports { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}
