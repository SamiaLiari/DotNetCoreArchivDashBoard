using System;
using System.Collections.Generic;

namespace DotNetCoreCsharpProject.Entities
{
    public partial class Societes
    {
        public Societes()
        {
            Groupes = new HashSet<Groupes>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }
        public string Tele { get; set; }
        public string Ville { get; set; }
        public string NomEncadrant { get; set; }
        public string MailEncadrant { get; set; }
        public string TeleEncadrant { get; set; }

        public virtual ICollection<Groupes> Groupes { get; set; }
    }
}
