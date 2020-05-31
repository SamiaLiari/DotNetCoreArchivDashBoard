using System;
using System.Collections.Generic;

namespace DotNetCoreCsharpProject.Entities
{
    public partial class TypeReports
    {
        public TypeReports()
        {
            Reports = new HashSet<Reports>();
        }

        public int IdType { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Reports> Reports { get; set; }
    }
}
