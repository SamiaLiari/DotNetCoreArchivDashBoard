using System;
using System.Collections.Generic;

namespace DotNetCoreCsharpProject.Entities
{
    public partial class Students
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public int? GroupId { get; set; }
        public string Cne { get; set; }
        public int IdFil { get; set; }
        public int IdNiv { get; set; }
        public string Cin { get; set; }

        public virtual Groupes Group { get; set; }
        public virtual Filieres IdFilNavigation { get; set; }
        public virtual Levels IdNivNavigation { get; set; }
        public virtual AspNetUsers IdUserNavigation { get; set; }
    }
}
