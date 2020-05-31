using DotNetCoreCsharpProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCsharpProject.ViewModels
{
    public class GroupsModel
    {
        public List<AspNetUsers> students { get; set; }
        public int id { get; set; }
        public string sujet { get; set; }

        public string ville { get; set; }

        public AspNetUsers encadrant { get; set; }
    }
}

