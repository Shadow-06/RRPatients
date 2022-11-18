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
    public class RRMedicationController : Controller
    {
        private readonly PatientsContext _context;

        public RRMedicationController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RRMedication
        public async Task<IActionResult> Index(string MedicationTypeID)
        { 
            if(MedicationTypeID != null)
            {
                Response.Cookies.Append("MedicationTypeID", MedicationTypeID);
                
            }
            else if (Request.Query["MedicationTypeID"].Any())
            {
                // Store in Cookie or Session
                Request.Query["MedicationTypeID"].ToString();
    Response.Cookies.Append("MedicationTypeID", MedicationTypeID);
                
            }
            else if (Request.Cookies["MedicationTypeID"] != null)
{
                // Retreuve the value
                MedicationTypeID = Request.Cookies["MedicationTypeID"].ToString();
}

else
{
    TempData["message"] = "Please select the Name of medication";
    return RedirectToAction("Index", "RRMedicationTypes");
}
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt16(MedicationTypeID)).FirstOrDefault();
            ViewData["M_id"] = MedicationTypeID;
            ViewData["MName"] = medType.Name;

            var patientsContext = _context.Medications
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .Where(m => m.MedicationTypeId == Convert.ToInt16(MedicationTypeID))
                .OrderBy(m => m.Name).ThenBy(m => m.Concentration);
            return View(await patientsContext.ToListAsync());
        }

        // GET: RRMedication/Details/5
        public async Task<IActionResult> Details(string id)
        {
            string MedId = string.Empty;

            if (Request.Cookies["MedicationTypeID"] != null)
            {
                // Retreuve the value
                MedId = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title according to the Medication Type
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(MedId)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // GET: RRMedication/Create
        public IActionResult Create()

        {
            string MedId = string.Empty;

            if (Request.Cookies["MedicationTypeID"] != null)
            {
                // Retreuve the value
                MedId = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title according to the Medication Type
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(MedId)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

           


            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits.OrderBy(a => a.ConcentrationCode), "ConcentrationCode", "ConcentrationCode");
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits.OrderBy(a=>a.DispensingCode), "DispensingCode", "DispensingCode");
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId");
            return View();
        }

        // POST: RRMedication/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            string MedId = string.Empty;

            if (Request.Cookies["MedicationTypeID"] != null)
            {
                // Retreuve the value
                MedId = Request.Cookies["MedicationTypeID"].ToString();
            }


            //Naming the title according to the Medication Type
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(MedId)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

             

            //var busRCode = _context.BusRoute.Where(a => a.BusRouteCode == MedId).FirstOrDefault();
            // ViewData["BCode"] = MedId;
            // ViewData["RName"] = busRCode.RouteName;

            

            

                var isDuplicate = _context.Medications.Where(a => a.Name == medication.Name && a.Concentration == medication.Concentration && a.ConcentrationCode == medication.ConcentrationCode);
                if (isDuplicate.Any())
                {
                    ModelState.AddModelError("", "Same record exists");
                TempData["message"] = "Record Already exists";
                return RedirectToAction("Create", "RRMedication");
            }
            
          //  if (ModelState.IsValid)
      //      {
                _context.Add(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
      //      }
         //   else
        //    {
                
         //   }

            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits.OrderBy(a => a.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits.OrderBy(a => a.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: RRMedication/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            string MedId = string.Empty;

            if (Request.Cookies["MedicationTypeID"] != null)
            {
                // Retreuve the value
                MedId = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title according to the Medication Type
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(MedId)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits.OrderBy(a => a.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits.OrderBy(a => a.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId", medication.MedicationTypeId);
            return View(medication);
        }

        // POST: RRMedication/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            string MedId = string.Empty;

            if (Request.Cookies["MedicationTypeID"] != null)
            {
                // Retreuve the value
                MedId = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title according to the Medication Type
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(MedId)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            medication.MedicationTypeId = Convert.ToInt32(MedId);

            if (id != medication.Din)
            {
                TempData["message"] = "Cannot Change Din";
                return RedirectToAction("Edit", "RRMedication");

                return NotFound();

            }

        //    if (ModelState.IsValid)
        //    {
                try
                {
                    _context.Update(medication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationExists(medication.Din))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
          //  else
            
                
            
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits.OrderBy(a => a.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits.OrderBy(a => a.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: RRMedication/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            string MedId = string.Empty;

            if (Request.Cookies["MedicationTypeID"] != null)
            {
                // Retreuve the value
                MedId = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title according to the Medication Type
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(MedId)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // POST: RRMedication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string MedId = string.Empty;

            if (Request.Cookies["MedicationTypeID"] != null)
            {
                // Retreuve the value
                MedId = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title according to the Medication Type
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(MedId)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;


            if (_context.Medications == null)
            {
                return Problem("Entity set 'PatientsContext.Medications'  is null.");
            }
            var medication = await _context.Medications.FindAsync(id);
            if (medication != null)
            {
                _context.Medications.Remove(medication);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationExists(string id)
        {
          return _context.Medications.Any(e => e.Din == id);
        }
    }
}
