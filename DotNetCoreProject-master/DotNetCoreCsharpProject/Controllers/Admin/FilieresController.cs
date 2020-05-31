using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetCoreCsharpProject.Entities;

namespace DotNetCoreCsharpProject.Controllers.Admin
{
    public class FilieresController : Controller
    {
        private readonly DataContext _context;

        public FilieresController(DataContext context)
        {
            _context = context;
        }

        // GET: Filieres
        public async Task<IActionResult> Index()
        {
            return View(await _context.Filieres.ToListAsync());
        }

        // GET: Filieres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filieres = await _context.Filieres
                .FirstOrDefaultAsync(m => m.IdFiliere == id);
            if (filieres == null)
            {
                return NotFound();
            }

            return View(filieres);
        }

        // GET: Filieres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Filieres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFiliere,NomFiliere")] Filieres filieres)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filieres);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(filieres);
        }

        // GET: Filieres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filieres = await _context.Filieres.FindAsync(id);
            if (filieres == null)
            {
                return NotFound();
            }
            return View(filieres);
        }

        // POST: Filieres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFiliere,NomFiliere")] Filieres filieres)
        {
            if (id != filieres.IdFiliere)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filieres);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilieresExists(filieres.IdFiliere))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(filieres);
        }

        // GET: Filieres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filieres = await _context.Filieres
                .FirstOrDefaultAsync(m => m.IdFiliere == id);
            if (filieres == null)
            {
                return NotFound();
            }

            return View(filieres);
        }

        // POST: Filieres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filieres = await _context.Filieres.FindAsync(id);
            _context.Filieres.Remove(filieres);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilieresExists(int id)
        {
            return _context.Filieres.Any(e => e.IdFiliere == id);
        }
    }
}
