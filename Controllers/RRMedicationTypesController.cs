using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RRPatients.Models;
using static System.Net.WebRequestMethods;

namespace RRPatients.Controllers
{
    public class RRMedicationTypesController : Controller
    {
        private readonly PatientsContext _context;

        public RRMedicationTypesController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RRMedicationTypes
        // here  it is used to get details of MedicationType.
        public async Task<IActionResult> Index()
        {
              return View(await _context.MedicationTypes.OrderBy(a=>a.Name).ToListAsync());
        }

        // GET: RRMedicationTypes/Details/5
        //used to get details of specific MedicationType.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MedicationTypes == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationTypes
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        // GET: RRMedicationTypes/Create
        // To create a new MedicationType
        public IActionResult Create()
        {
            return View();
        }

        // POST: RRMedicationTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST is a method for sending information to a server to add or update resources. The request body of the HTTP request contains the data that was sent to the server via POST.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicationType);
        }

        // GET: RRMedicationTypes/Edit/5
        //Used to edit the single MedicationType.

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MedicationTypes == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationTypes.FindAsync(id);
            if (medicationType == null)
            {
                return NotFound();
            }
            return View(medicationType);
        }

        // POST: RRMedicationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (id != medicationType.MedicationTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationTypeExists(medicationType.MedicationTypeId))
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
            return View(medicationType);
        }

        // GET: RRMedicationTypes/Delete/5
        //One to confirm the deletion by displaying the chosen record.To delete the record after confirmation, one requires HTTP POST. This would share the confirm page's name and parameter.

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MedicationTypes == null)
            {
                return NotFound();
            }
            var medicationType = await _context.MedicationTypes
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        // POST: RRMedicationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MedicationTypes == null)
            {
                return Problem("Entity set 'PatientsContext.MedicationTypes'  is null.");
            }
            var medicationType = await _context.MedicationTypes.FindAsync(id);
            if (medicationType != null)
            {
                _context.MedicationTypes.Remove(medicationType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationTypeExists(int id)
        {
          return _context.MedicationTypes.Any(e => e.MedicationTypeId == id);
        }
    }
}
