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
    public class BilderController : Controller
    {
        private readonly emensaContext _context;

        public BilderController(emensaContext context)
        {
            _context = context;
        }

        // GET: Bilder/Index/1
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null){
                var bilderListe = await _context.Bilder.AsQueryable()
                .Take(4).ToListAsync();
                return View(bilderListe);
            }
            else{
                var bilderListe = await _context.Bilder.AsQueryable()
                .Where(x => x.Id >= 4 * (id - 1)).Take(4).ToListAsync();
                return View(bilderListe);    
            }
        }

        // GET: Bilder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilder = await _context.Bilder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bilder == null)
            {
                return NotFound();
            }

            return View(bilder);
        }

        // GET: Bilder/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bilder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AltText,Titel,Binärdaten,Copyright")] Bilder bilder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bilder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bilder);
        }

        // GET: Bilder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilder = await _context.Bilder.FindAsync(id);
            if (bilder == null)
            {
                return NotFound();
            }
            return View(bilder);
        }

        // POST: Bilder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AltText,Titel,Binärdaten,Copyright")] Bilder bilder)
        {
            if (id != bilder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bilder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BilderExists(bilder.Id))
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
            return View(bilder);
        }

        // GET: Bilder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilder = await _context.Bilder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bilder == null)
            {
                return NotFound();
            }

            return View(bilder);
        }

        // POST: Bilder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bilder = await _context.Bilder.FindAsync(id);
            _context.Bilder.Remove(bilder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BilderExists(int id)
        {
            return _context.Bilder.Any(e => e.Id == id);
        }
                // 
        // GET: /Bilder/getImage/id
        public async Task<FileContentResult> getImage(int id)
        {

            var bilder = await _context.Bilder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bilder == null)
            {
                return File(new byte[] {0}, "image/png");
            }
            return File(bilder.Binärdaten, "image/png");
        }
    }
}
