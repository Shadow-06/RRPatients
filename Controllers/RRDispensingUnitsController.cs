using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RRPatients.Models;

namespace RRPatients.Controllers
{
    public class RRDispensingUnitsController : Controller
    {
        private readonly PatientsContext _context;

        public RRDispensingUnitsController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RRDispensingUnits
        // here  it is used to get details of DispensingUnits.
        public async Task<IActionResult> Index()
        {
              return View(await _context.DispensingUnits.ToListAsync());
        }

        // GET: RRDispensingUnits/Details/5
        //used to get details of specific DispensingUnits.
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.DispensingUnits == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnits
                .FirstOrDefaultAsync(m => m.DispensingCode == id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }

            return View(dispensingUnit);
        }

        // GET: RRDispensingUnits/Create
        // To create a new DispensingUnits
        public IActionResult Create()
        {
            return View();
        }

        // POST: RRDispensingUnits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST is a method for sending information to a server to add or update resources. The request body of the HTTP request contains the data that was sent to the server via POST.



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DispensingCode")] DispensingUnit dispensingUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispensingUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dispensingUnit);
        }

        // GET: RRDispensingUnits/Edit/5
        //Used to edit the single DispensingUnits.

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.DispensingUnits == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnits.FindAsync(id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }
            return View(dispensingUnit);
        }

        // POST: RRDispensingUnits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DispensingCode")] DispensingUnit dispensingUnit)
        {
            if (id != dispensingUnit.DispensingCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dispensingUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispensingUnitExists(dispensingUnit.DispensingCode))
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
            return View(dispensingUnit);
        }

        // GET: RRDispensingUnits/Delete/5
        //One to confirm the deletion by displaying the chosen record.  To delete the record after confirmation, one requires HTTP POST. This would share the confirm page's name and parameter.

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.DispensingUnits == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnits
                .FirstOrDefaultAsync(m => m.DispensingCode == id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }

            return View(dispensingUnit);
        }

        // POST: RRDispensingUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.DispensingUnits == null)
            {
                return Problem("Entity set 'PatientsContext.DispensingUnits'  is null.");
            }
            var dispensingUnit = await _context.DispensingUnits.FindAsync(id);
            if (dispensingUnit != null)
            {
                _context.DispensingUnits.Remove(dispensingUnit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DispensingUnitExists(string id)
        {
          return _context.DispensingUnits.Any(e => e.DispensingCode == id);
        }
    }
}
