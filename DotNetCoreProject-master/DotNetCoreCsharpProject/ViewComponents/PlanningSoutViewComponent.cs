using DotNetCoreCsharpProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCsharpProject.ViewComponents
{
    public class PlanningSoutViewComponent:ViewComponent
    {
        private readonly DataContext _dataContext;

        public PlanningSoutViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
