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
    public class RRPatientDiagnosisController : Controller
    {
        private readonly PatientsContext _context;

        public RRPatientDiagnosisController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RRPatientDiagnosis
        public async Task<IActionResult> Index(string PatientID)
        {
            if (PatientID != null)
            {
                Response.Cookies.Append("PatientID", PatientID);
                
            }
            else if (Request.Query["PatientID"].Any())
            {
                Request.Query["PatientID"].ToString();
                Response.Cookies.Append("PatientID", PatientID);
               
            }
            else if (Request.Cookies["PatientID"] != null)
            {
                PatientID = Request.Cookies["PatientID"].ToString();
                
            }
            else
            {
                TempData["message"] = "Please select any Patient Name";
                return RedirectToAction("Index", "RRPatients");
            }
            var patientsContext = _context.PatientDiagnoses.Include(a => a.Diagnosis).Include(a => a.Patient).Include(a => a.PatientTreatments).Where(m => m.PatientId == Convert.ToInt16(PatientID));
            return View(await patientsContext.OrderBy(a=>a.Patient.LastName).ToListAsync());
        }

        // GET: RRPatientDiagnosis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PatientDiagnoses == null)
            {
                return NotFound();
            }

            var patientDiagnosis = await _context.PatientDiagnoses
                .Include(p => p.Diagnosis)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PatientDiagnosisId == id);
            if (patientDiagnosis == null)
            {
                return NotFound();
            }

            return View(patientDiagnosis);
        }

        // GET: RRPatientDiagnosis/Create
        public IActionResult Create()
        {
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "DiagnosisId", "DiagnosisId");
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId");
            return View();
        }

        // POST: RRPatientDiagnosis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientDiagnosisId,PatientId,DiagnosisId,Comments")] PatientDiagnosis patientDiagnosis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientDiagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "DiagnosisId", "DiagnosisId", patientDiagnosis.DiagnosisId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId", patientDiagnosis.PatientId);
            return View(patientDiagnosis);
        }

        // GET: RRPatientDiagnosis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PatientDiagnoses == null)
            {
                return NotFound();
            }

            var patientDiagnosis = await _context.PatientDiagnoses.FindAsync(id);
            if (patientDiagnosis == null)
            {
                return NotFound();
            }
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "DiagnosisId", "DiagnosisId", patientDiagnosis.DiagnosisId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId", patientDiagnosis.PatientId);
            return View(patientDiagnosis);
        }

        // POST: RRPatientDiagnosis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientDiagnosisId,PatientId,DiagnosisId,Comments")] PatientDiagnosis patientDiagnosis)
        {
            if (id != patientDiagnosis.PatientDiagnosisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientDiagnosis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientDiagnosisExists(patientDiagnosis.PatientDiagnosisId))
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
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "DiagnosisId", "DiagnosisId", patientDiagnosis.DiagnosisId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId", patientDiagnosis.PatientId);
            return View(patientDiagnosis);
        }

        // GET: RRPatientDiagnosis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PatientDiagnoses == null)
            {
                return NotFound();
            }

            var patientDiagnosis = await _context.PatientDiagnoses
                .Include(p => p.Diagnosis)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PatientDiagnosisId == id);
            if (patientDiagnosis == null)
            {
                return NotFound();
            }

            return View(patientDiagnosis);
        }

        // POST: RRPatientDiagnosis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PatientDiagnoses == null)
            {
                return Problem("Entity set 'PatientsContext.PatientDiagnoses'  is null.");
            }
            var patientDiagnosis = await _context.PatientDiagnoses.FindAsync(id);
            if (patientDiagnosis != null)
            {
                _context.PatientDiagnoses.Remove(patientDiagnosis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientDiagnosisExists(int id)
        {
          return _context.PatientDiagnoses.Any(e => e.PatientDiagnosisId == id);
        }
    }
}
