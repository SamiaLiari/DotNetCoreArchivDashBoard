using DotNetCoreCsharpProject.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCsharpProject.Models
{
    public class SearchEngineModel
    {
        public SelectList filieres { get; set; }
        public SelectList levels { get; set; }
        public SelectList professors { get; set; }

        public SelectList societes { get; set; }
    }
}
