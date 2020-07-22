using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class VisitHistoriesController : Controller
    {
        private readonly DataContext _context;

        public VisitHistoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: VisitHistories
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.VisitHistories.Include(v => v.Patient);
            return View(await dataContext.ToListAsync());
        }

        // GET: VisitHistories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitHistory = await _context.VisitHistories
                .Include(v => v.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visitHistory == null)
            {
                return NotFound();
            }

            return View(visitHistory);
        }

        // GET: VisitHistories/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "FIO", "FIO");
            return View();
        }

        // POST: VisitHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientId,Doctor,DoctorName,Diagnosis,Complaint,VisitDate")] VisitHistory visitHistory)
        {
            if (ModelState.IsValid)
            {
                visitHistory.Id = Guid.NewGuid();
                _context.Add(visitHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", visitHistory.PatientId);
            return View(visitHistory);
        }

        // GET: VisitHistories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitHistory = await _context.VisitHistories.FindAsync(id);
            if (visitHistory == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", visitHistory.PatientId);
            return View(visitHistory);
        }

        // POST: VisitHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,PatientId,Doctor,DoctorName,Diagnosis,Complaint,VisitDate")] VisitHistory visitHistory)
        {
            if (id != visitHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visitHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitHistoryExists(visitHistory.Id))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", visitHistory.PatientId);
            return View(visitHistory);
        }

        // GET: VisitHistories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitHistory = await _context.VisitHistories
                .Include(v => v.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visitHistory == null)
            {
                return NotFound();
            }

            return View(visitHistory);
        }

        // POST: VisitHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var visitHistory = await _context.VisitHistories.FindAsync(id);
            _context.VisitHistories.Remove(visitHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitHistoryExists(Guid id)
        {
            return _context.VisitHistories.Any(e => e.Id == id);
        }
    }
}
