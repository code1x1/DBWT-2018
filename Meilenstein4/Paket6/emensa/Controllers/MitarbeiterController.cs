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
    public class MitarbeiterController : Controller
    {
        private readonly emensaContext _context;

        public MitarbeiterController(emensaContext context)
        {
            _context = context;
        }

        // GET: Mitarbeiter
        public async Task<IActionResult> Index()
        {
            var emensaContext = _context.Mitarbeiter.Include(m => m.FkFhangeNavigation);
            return View(await emensaContext.ToListAsync());
        }

        // GET: Mitarbeiter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mitarbeiter = await _context.Mitarbeiter
                .Include(m => m.FkFhangeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mitarbeiter == null)
            {
                return NotFound();
            }

            return View(mitarbeiter);
        }

        // GET: Mitarbeiter/Create
        public IActionResult Create()
        {
            ViewData["FkFhange"] = new SelectList(_context.FhAngehörige, "Id", "Id");
            return View();
        }

        // POST: Mitarbeiter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Büro,Telefon,FkFhange,Id")] Mitarbeiter mitarbeiter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mitarbeiter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkFhange"] = new SelectList(_context.FhAngehörige, "Id", "Id", mitarbeiter.FkFhange);
            return View(mitarbeiter);
        }

        // GET: Mitarbeiter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mitarbeiter = await _context.Mitarbeiter.FindAsync(id);
            if (mitarbeiter == null)
            {
                return NotFound();
            }
            ViewData["FkFhange"] = new SelectList(_context.FhAngehörige, "Id", "Id", mitarbeiter.FkFhange);
            return View(mitarbeiter);
        }

        // POST: Mitarbeiter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Büro,Telefon,FkFhange,Id")] Mitarbeiter mitarbeiter)
        {
            if (id != mitarbeiter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mitarbeiter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MitarbeiterExists(mitarbeiter.Id))
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
            ViewData["FkFhange"] = new SelectList(_context.FhAngehörige, "Id", "Id", mitarbeiter.FkFhange);
            return View(mitarbeiter);
        }

        // GET: Mitarbeiter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mitarbeiter = await _context.Mitarbeiter
                .Include(m => m.FkFhangeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mitarbeiter == null)
            {
                return NotFound();
            }

            return View(mitarbeiter);
        }

        // POST: Mitarbeiter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mitarbeiter = await _context.Mitarbeiter.FindAsync(id);
            _context.Mitarbeiter.Remove(mitarbeiter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MitarbeiterExists(int id)
        {
            return _context.Mitarbeiter.Any(e => e.Id == id);
        }
    }
}
