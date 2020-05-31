using DotNetCoreCsharpProject.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCsharpProject.ViewComponents
{
    public class CalendrierViewComponent: ViewComponent
    {
        private readonly DataContext _dataContext;

        public CalendrierViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;

        }
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult<IViewComponentResult>(View());
        }

    }
}
