using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using emensa.Models;

namespace emensa.Controllers
{
    public class ZutatenController : Controller
    {
        private readonly emensaContext _context;

        public ZutatenController(emensaContext context)
        {
            _context = context;
        }

        // GET: Zutaten
        public async Task<IActionResult> Index()
        {
            var zutatenListeSortiert = await _context.Zutaten
            .OrderBy(c => c.Bio)
            .ThenBy(n => n.Name)
            .ToListAsync();
            
            return View(zutatenListeSortiert);
        }

        // GET: Zutaten/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zutaten = await _context.Zutaten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zutaten == null)
            {
                return NotFound();
            }

            return View(zutaten);
        }

        // GET: Zutaten/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zutaten/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Bio,Vegetarisch,Vegan,Glutenfrei")] Zutaten zutaten)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zutaten);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zutaten);
        }

        // GET: Zutaten/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zutaten = await _context.Zutaten.FindAsync(id);
            if (zutaten == null)
            {
                return NotFound();
            }
            return View(zutaten);
        }

        // POST: Zutaten/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Bio,Vegetarisch,Vegan,Glutenfrei")] Zutaten zutaten)
        {
            if (id != zutaten.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zutaten);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZutatenExists(zutaten.Id))
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
            return View(zutaten);
        }

        // GET: Zutaten/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zutaten = await _context.Zutaten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zutaten == null)
            {
                return NotFound();
            }

            return View(zutaten);
        }

        // POST: Zutaten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zutaten = await _context.Zutaten.FindAsync(id);
            _context.Zutaten.Remove(zutaten);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZutatenExists(int id)
        {
            return _context.Zutaten.Any(e => e.Id == id);
        }
    }
}
