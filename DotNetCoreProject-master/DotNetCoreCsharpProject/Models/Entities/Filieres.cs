using System;
using System.Collections.Generic;

namespace DotNetCoreCsharpProject.Entities
{
    public partial class Filieres
    {
        public Filieres()
        {
            Professors = new HashSet<Professors>();
            Students = new HashSet<Students>();
        }

        public int IdFiliere { get; set; }
        public string NomFiliere { get; set; }

        public virtual ICollection<Professors> Professors { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}
