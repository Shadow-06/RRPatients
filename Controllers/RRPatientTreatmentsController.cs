using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RRPatients.Models;
using Microsoft.AspNetCore.Http;

namespace RRPatients.Controllers
{
    public class RRPatientTreatmentsController : Controller
    {
        private readonly PatientsContext _context;

        public RRPatientTreatmentsController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RRPatientTreatments
        public async Task<IActionResult> Index(String patientDiagnosisId)
        {
            if (patientDiagnosisId != null)
            {
                // Store in Cookie or Session
                Response.Cookies.Append("patientDiagnosisId", patientDiagnosisId);
                HttpContext.Session.SetString("patientDiagnosisId", patientDiagnosisId);
            }
            else if (Request.Query["MedicationTypeId"].Any())
            {
                // Store in Cookie or Session
                Request.Query["patientDiagnosisId"].ToString();
                Response.Cookies.Append("patientDiagnosisId", patientDiagnosisId);
                HttpContext.Session.SetString("patientDiagnosisId", patientDiagnosisId);
            }
            else if (Request.Cookies["patientDiagnosisId"] != null)
            {
                // Retreive the value
                patientDiagnosisId = Request.Query["patientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("patientDiagnosisId") != null)
            {
                patientDiagnosisId = HttpContext.Session.GetString("patientDiagnosisId");
            }
            else
            {
                TempData["message"] = "Please select the  Diagnosis";
                return RedirectToAction("Index", "RRPatientDiagnosis");
            }

            var filteredTreatment = _context.PatientDiagnoses.Where(p => p.PatientDiagnosisId == Convert.ToInt16(patientDiagnosisId)).FirstOrDefault();
            int diagnosisId = filteredTreatment.DiagnosisId;
            int patientId = filteredTreatment.PatientId;
            var patientName = _context.Patients.Where(p => p.PatientId == patientId).FirstOrDefault();
            var filteredDiagnosis = _context.Diagnoses.Where(p => p.DiagnosisId == diagnosisId).FirstOrDefault();
            ViewData["DiagnosisName"] = filteredDiagnosis.Name;

            ViewData["PatientFirstName"] = patientName.FirstName;
            ViewData["PatientLastName"] = patientName.LastName;


            var patientsContext = _context.PatientTreatments
                .Where(p => p.PatientDiagnosisId == Convert.ToInt16(patientDiagnosisId))
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .OrderBy(p => p.DatePrescribed);
            return View(await patientsContext.ToListAsync());
        }

        // GET: RRPatientTreatments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PatientTreatments == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatments
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // GET: RRPatientTreatments/Create
        public IActionResult Create()
        {
            string patientDiagnosisId = String.Empty;

            if (Request.Cookies["patientDiagnosisId"] != null)
            {
                // Retreive the value
                patientDiagnosisId = Request.Cookies["patientDiagnosisId"].ToString();
            }

            var filteredTreatment = _context.PatientDiagnoses.Where(p => p.PatientDiagnosisId == Convert.ToInt16(patientDiagnosisId)).FirstOrDefault();
            int diagnosisId = filteredTreatment.DiagnosisId;
            int patientId = filteredTreatment.PatientId;
            var patientName = _context.Patients.Where(p => p.PatientId == patientId).FirstOrDefault();
            var filteredDiagnosis = _context.Diagnoses.Where(p => p.DiagnosisId == diagnosisId).FirstOrDefault();

            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnoses, "PatientDiagnosisId", "PatientDiagnosisId");
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == diagnosisId), "TreatmentId", "Name");
            ViewData["PatientFirstName"] = patientName.FirstName;
            ViewData["PatientLastName"] = patientName.LastName;
            ViewData["DiagnosisName"] = filteredDiagnosis.Name;
            return View();
        }

        // POST: RRPatientTreatments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
       public async Task<IActionResult> Create([Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            string patientDiagnosisId = String.Empty;

            if (Request.Cookies["patientDiagnosisId"] != null)
            {
                // Retreive the value
                patientDiagnosisId = Request.Query["patientDiagnosisId"].ToString();
            }

            var filteredTreatment = _context.PatientDiagnoses.Where(p => p.PatientDiagnosisId == Convert.ToInt16(patientDiagnosisId)).FirstOrDefault();
            int diagnosisId = filteredTreatment.DiagnosisId;
            int patientId = filteredTreatment.PatientId;
            var patientName = _context.Patients.Where(p => p.PatientId == patientId).FirstOrDefault();
            var filteredDiagnosis = _context.Diagnoses.Where(p => p.DiagnosisId == diagnosisId).FirstOrDefault();
            ViewData["DiagnosisName"] = filteredDiagnosis.Name;

            if (ModelState.IsValid)
            {
                _context.Add(patientTreatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnoses, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == diagnosisId), "TreatmentId", "Name", patientTreatment.TreatmentId);
            ViewData["PatientFirstName"] = patientName.FirstName;
            ViewData["PatientLastName"] = patientName.LastName;
            return View(patientTreatment);
        }

        // GET: RRPatientTreatments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            string patientDiagnosisId = String.Empty;

            if (Request.Cookies["patientDiagnosisId"] != null)
            {
                // Retreive the value
                patientDiagnosisId = Request.Cookies["patientDiagnosisId"].ToString();
            }

            var filteredTreatment = _context.PatientDiagnoses.Where(p => p.PatientDiagnosisId == Convert.ToInt16(patientDiagnosisId)).FirstOrDefault();
            int diagnosisId = filteredTreatment.DiagnosisId;
            int patientId = filteredTreatment.PatientId;
            var patientName = _context.Patients.Where(p => p.PatientId == patientId).FirstOrDefault();
            var filteredDiagnosis = _context.Diagnoses.Where(p => p.DiagnosisId == diagnosisId).FirstOrDefault();
            ViewData["DiagnosisName"] = filteredDiagnosis.Name;


            if (id == null || _context.PatientTreatments == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatments.FindAsync(id);
            if (patientTreatment == null)
            {
                return NotFound();
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnoses, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments, "TreatmentId", "Name", patientTreatment.TreatmentId);
            ViewData["PatientFirstName"] = patientName.FirstName;
            ViewData["PatientLastName"] = patientName.LastName;
            return View(patientTreatment);
        }
        // POST: RRPatientTreatments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            string patientDiagnosisId = String.Empty;

            if (Request.Cookies["patientDiagnosisId"] != null)
            {
                // Retreive the value
                patientDiagnosisId = Request.Cookies["patientDiagnosisId"].ToString();
            }

            var filteredTreatment = _context.PatientDiagnoses.Where(p => p.PatientDiagnosisId == Convert.ToInt16(patientDiagnosisId)).FirstOrDefault();
            int diagnosisId = filteredTreatment.DiagnosisId;
            int patientId = filteredTreatment.PatientId;
            var patientName = _context.Patients.Where(p => p.PatientId == patientId).FirstOrDefault();
            var filteredDiagnosis = _context.Diagnoses.Where(p => p.DiagnosisId == diagnosisId).FirstOrDefault();
            ViewData["DiagnosisName"] = filteredDiagnosis.Name;

            if (id != patientTreatment.PatientTreatmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientTreatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTreatmentExists(patientTreatment.PatientTreatmentId))
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
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnoses, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments, "TreatmentId", "Name", patientTreatment.TreatmentId);
            ViewData["PatientFirstName"] = patientName.FirstName;
            ViewData["PatientLastName"] = patientName.LastName;
            return View(patientTreatment);
        }

        // GET: RRPatientTreatments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PatientTreatments == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatments
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // POST: RRPatientTreatments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PatientTreatments == null)
            {
                return Problem("Entity set 'PatientsContext.PatientTreatments'  is null.");
            }
            var patientTreatment = await _context.PatientTreatments.FindAsync(id);
            if (patientTreatment != null)
            {
                _context.PatientTreatments.Remove(patientTreatment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientTreatmentExists(int id)
        {
          return _context.PatientTreatments.Any(e => e.PatientTreatmentId == id);
        }
    }
}
