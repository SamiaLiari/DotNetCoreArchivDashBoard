using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCsharpProject.ViewModels
{
    public class UsersTabViewModel
    {
        public Tab ActiveTab { get; set; }
    }
    public enum Tab
    {
        Calendrier,
        ListePfeInfo,
        ListePfeGtr,
        PlanningSout
    }
}
