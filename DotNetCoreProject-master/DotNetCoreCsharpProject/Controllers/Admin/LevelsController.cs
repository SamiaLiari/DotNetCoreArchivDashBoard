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
    public class LevelsController : Controller
    {
        private readonly DataContext _context;

        public LevelsController(DataContext context)
        {
            _context = context;
        }

        // GET: Levels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Levels.ToListAsync());
        }
        

        // GET: Levels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var levels = await _context.Levels
                .FirstOrDefaultAsync(m => m.IdNiveau == id);
            if (levels == null)
            {
                return NotFound();
            }

            return View(levels);
        }

        // GET: Levels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Levels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNiveau,NomNiveau")] Levels levels)
        {
            if (ModelState.IsValid)
            {
                _context.Add(levels);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(levels);
        }

        // GET: Levels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var levels = await _context.Levels.FindAsync(id);
            if (levels == null)
            {
                return NotFound();
            }
            return View(levels);
        }

        // POST: Levels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNiveau,NomNiveau")] Levels levels)
        {
            if (id != levels.IdNiveau)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(levels);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LevelsExists(levels.IdNiveau))
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
            return View(levels);
        }

        // GET: Levels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var levels = await _context.Levels
                .FirstOrDefaultAsync(m => m.IdNiveau == id);
            if (levels == null)
            {
                return NotFound();
            }

            return View(levels);
        }

        // POST: Levels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var levels = await _context.Levels.FindAsync(id);
            _context.Levels.Remove(levels);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LevelsExists(int id)
        {
            return _context.Levels.Any(e => e.IdNiveau == id);
        }
    }
}
