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
    public class BestellungenController : Controller
    {
        private readonly emensaContext _context;

        public BestellungenController(emensaContext context)
        {
            _context = context;
        }


                // GET: Bestellungen/Create
        public IActionResult Warenkorb()
        {
            return View();
        }

        // POST: Bestellungen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Warenkorb([Bind("Nummer,BestellZeitpunkt,Abholzeitpunkt,Endpreis")] Bestellungen bestellungen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bestellungen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bestellungen);
        }





    #region Scaffolding

        // GET: Bestellungen
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bestellungen.ToListAsync());
        }

        // GET: Bestellungen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellungen = await _context.Bestellungen
                .FirstOrDefaultAsync(m => m.Nummer == id);
            if (bestellungen == null)
            {
                return NotFound();
            }

            return View(bestellungen);
        }

        // GET: Bestellungen/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bestellungen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nummer,BestellZeitpunkt,Abholzeitpunkt,Endpreis")] Bestellungen bestellungen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bestellungen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bestellungen);
        }

        // GET: Bestellungen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellungen = await _context.Bestellungen.FindAsync(id);
            if (bestellungen == null)
            {
                return NotFound();
            }
            return View(bestellungen);
        }

        // POST: Bestellungen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nummer,BestellZeitpunkt,Abholzeitpunkt,Endpreis")] Bestellungen bestellungen)
        {
            if (id != bestellungen.Nummer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bestellungen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BestellungenExists(bestellungen.Nummer))
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
            return View(bestellungen);
        }

        // GET: Bestellungen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellungen = await _context.Bestellungen
                .FirstOrDefaultAsync(m => m.Nummer == id);
            if (bestellungen == null)
            {
                return NotFound();
            }

            return View(bestellungen);
        }

        // POST: Bestellungen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bestellungen = await _context.Bestellungen.FindAsync(id);
            _context.Bestellungen.Remove(bestellungen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BestellungenExists(int id)
        {
            return _context.Bestellungen.Any(e => e.Nummer == id);
        }
    #endregion

    }

}
