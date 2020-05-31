using System;
using System.Collections.Generic;

namespace DotNetCoreCsharpProject.Entities
{
    public partial class Levels
    {
        public Levels()
        {
            Students = new HashSet<Students>();
        }

        public int IdNiveau { get; set; }
        public string NomNiveau { get; set; }

        public virtual ICollection<Students> Students { get; set; }
    }
}
