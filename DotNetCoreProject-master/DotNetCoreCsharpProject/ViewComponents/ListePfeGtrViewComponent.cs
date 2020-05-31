using DotNetCoreCsharpProject.Entities;
using DotNetCoreCsharpProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCsharpProject.ViewComponents
{
    public class ListePfeGtrViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext;

        public ListePfeGtrViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<GroupsModel> grpsModel = new List<GroupsModel>();
            List<Groupes> grps = _dataContext.Groupes.ToList();

            Filieres filGtr = _dataContext.Filieres.Where(s => s.NomFiliere == "GTR").FirstOrDefault();
            GroupsModel testModel;
            foreach (Groupes grp in grps)
            {

                List<AspNetUsers> student = (from s in _dataContext.Students
                                             join u in _dataContext.AspNetUsers
                                             on s.IdUser equals u.Id
                                             where (s.GroupId == grp.Id && s.IdFil == filGtr.IdFiliere)

                                             select u).ToList();

                testModel = new GroupsModel();
                testModel.id = grp.Id;
                testModel.sujet = grp.Sujet;
                testModel.students = student;
                Societes societe = _dataContext.Societes.Where(s => grp.IdSociete == s.Id).FirstOrDefault();
                if (societe != null)
                {
                    testModel.ville = societe.Ville;
                }
                Professors professor = _dataContext.Professors.Where(s => grp.Idprof == s.Id).FirstOrDefault();
                if (professor != null)
                {
                    testModel.encadrant = _dataContext.AspNetUsers.Where(s => s.Id == professor.IdUser).FirstOrDefault();
                }
                grpsModel.Add(testModel);
            }
            return View(grpsModel);
        }
    }
}
