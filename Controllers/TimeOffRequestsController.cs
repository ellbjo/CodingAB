using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodingAB.Models;

namespace CodingAB.Controllers
{
    public class TimeOffRequestsController : Controller
    {
        private readonly CodingABContext _context;

        public TimeOffRequestsController(CodingABContext context)
        {
            _context = context;
        }

        // GET: TimeOffRequests
        public async Task<IActionResult> Index()
        {
            var codingABContext = _context.TimeOffRequests.Include(t => t.Employee);
            return View(await codingABContext.ToListAsync());
        }

        // GET: TimeOffRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TimeOffRequests == null)
            {
                return NotFound();
            }

            var timeOffRequest = await _context.TimeOffRequests
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeOffRequest == null)
            {
                return NotFound();
            }

            return View(timeOffRequest);
        }

        // GET: TimeOffRequests/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            return View();
        }

        // POST: TimeOffRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,StartDate,EndDate,Type")] TimeOffRequest timeOffRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeOffRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", timeOffRequest.EmployeeId);
            return View(timeOffRequest);
        }

        // GET: TimeOffRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TimeOffRequests == null)
            {
                return NotFound();
            }

            var timeOffRequest = await _context.TimeOffRequests.FindAsync(id);
            if (timeOffRequest == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", timeOffRequest.EmployeeId);
            return View(timeOffRequest);
        }

        // POST: TimeOffRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,StartDate,EndDate,Type")] TimeOffRequest timeOffRequest)
        {
            if (id != timeOffRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeOffRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeOffRequestExists(timeOffRequest.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", timeOffRequest.EmployeeId);
            return View(timeOffRequest);
        }

        // GET: TimeOffRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TimeOffRequests == null)
            {
                return NotFound();
            }

            var timeOffRequest = await _context.TimeOffRequests
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeOffRequest == null)
            {
                return NotFound();
            }

            return View(timeOffRequest);
        }

        // POST: TimeOffRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TimeOffRequests == null)
            {
                return Problem("Entity set 'CodingABContext.TimeOffRequests'  is null.");
            }
            var timeOffRequest = await _context.TimeOffRequests.FindAsync(id);
            if (timeOffRequest != null)
            {
                _context.TimeOffRequests.Remove(timeOffRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeOffRequestExists(int id)
        {
          return (_context.TimeOffRequests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        // GET: TimeOffRequests/Report
        public IActionResult Report()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View();
        }

        // POST: TimeOffRequests/Report
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report([Bind("Id,EmployeeId,Type,StartDate,EndDate")] TimeOffRequest timeOffRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeOffRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", timeOffRequest.EmployeeId);
            return View(timeOffRequest);
        }

    }
}
