using System;
using System.Collections.Generic;

namespace DotNetCoreCsharpProject.Entities
{
    public partial class Professors
    {
        public Professors()
        {
            Groupes = new HashSet<Groupes>();
        }

        public int Id { get; set; }
        public string IdUser { get; set; }
        public int IdFil { get; set; }

        public virtual Filieres IdFilNavigation { get; set; }
        public virtual AspNetUsers IdUserNavigation { get; set; }
        public virtual ICollection<Groupes> Groupes { get; set; }
    }
}
