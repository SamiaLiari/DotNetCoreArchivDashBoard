using System;
using System.Collections.Generic;

namespace DotNetCoreCsharpProject.Entities
{
    public partial class Reports
    {
        public int Id { get; set; }
        public int IdType { get; set; }
        public int IdGroup { get; set; }
        public DateTime DateDepot { get; set; }
        public string PathReport { get; set; }

        public virtual Groupes IdGroupNavigation { get; set; }
        public virtual TypeReports IdTypeNavigation { get; set; }
    }
}
