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
    public class GastController : Controller
    {
        private readonly emensaContext _context;

        public GastController(emensaContext context)
        {
            _context = context;
        }

        // GET: Gast
        public async Task<IActionResult> Index()
        {
            var emensaContext = _context.Gäste.Include(g => g.FkBenutzerNavigation);
            return View(await emensaContext.ToListAsync());
        }

        // GET: Gast/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gäste = await _context.Gäste
                .Include(g => g.FkBenutzerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gäste == null)
            {
                return NotFound();
            }

            return View(gäste);
        }

        // GET: Gast/Create
        public IActionResult Create()
        {
            ViewData["FkBenutzer"] = new SelectList(_context.Benutzer, "Nummer", "EMail");
            return View();
        }

        // POST: Gast/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Grund,Ablaufdatum,FkBenutzer,Id")] Gäste gäste)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gäste);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkBenutzer"] = new SelectList(_context.Benutzer, "Nummer", "EMail", gäste.FkBenutzer);
            return View(gäste);
        }

        // GET: Gast/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gäste = await _context.Gäste.FindAsync(id);
            if (gäste == null)
            {
                return NotFound();
            }
            ViewData["FkBenutzer"] = new SelectList(_context.Benutzer, "Nummer", "EMail", gäste.FkBenutzer);
            return View(gäste);
        }

        // POST: Gast/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Grund,Ablaufdatum,FkBenutzer,Id")] Gäste gäste)
        {
            if (id != gäste.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gäste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GästeExists(gäste.Id))
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
            ViewData["FkBenutzer"] = new SelectList(_context.Benutzer, "Nummer", "EMail", gäste.FkBenutzer);
            return View(gäste);
        }

        // GET: Gast/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gäste = await _context.Gäste
                .Include(g => g.FkBenutzerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gäste == null)
            {
                return NotFound();
            }

            return View(gäste);
        }

        // POST: Gast/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gäste = await _context.Gäste.FindAsync(id);
            _context.Gäste.Remove(gäste);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GästeExists(int id)
        {
            return _context.Gäste.Any(e => e.Id == id);
        }
    }
}
