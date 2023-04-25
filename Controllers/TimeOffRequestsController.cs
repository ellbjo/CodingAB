using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodingAB.Models;
using System.Text.Json.Serialization.Metadata;

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
            var timeOffRequests = await _context.TimeOffRequests
                .Include(t => t.Employee)
                .ToListAsync();

            return View(timeOffRequests);
        }

        // GET: TimeOffRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TimeOffRequests == null)
            {
                return NotFound();
            }

            var timeOffRequest = await _context.TimeOffRequests
                .Include(t => t.EmployeeId)
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
            var employees = _context.Employees.ToList();
            var employeeSelectList = new SelectList(employees, "Id", "FirstName");
            ViewBag.EmployeeId = employeeSelectList;
            ViewBag.TimeOffTypes = Enum.GetValues(typeof(TimeOffType))
            .Cast<TimeOffType>()
            .Select(t => new SelectListItem
            {
                Text = t.ToString(),
                Value = ((int)t).ToString()
            }).ToList();

            return View();
        }

        // POST: TimeOffRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,StartDate,EndDate,Type")] TimeOffRequest timeOffRequest)
        {
            ModelState.Remove("Employee");
            if (ModelState.IsValid)
            {
                timeOffRequest.RequestSubmissionTime = DateTime.Now;
                _context.Add(timeOffRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FirstName", timeOffRequest.EmployeeId);
            ViewBag.TimeOffTypes = Enum.GetValues(typeof(TimeOffType))
                .Cast<TimeOffType>()
                .Select(t => new SelectListItem
                {
                    Text = t.ToString(),
                    Value = ((int)t).ToString()
                }).ToList();

            return View(timeOffRequest);
        }
        public IActionResult TimeOffHistory(int? employeeId, int? month)
        {
            IQueryable<TimeOffRequest> timeOffRequests = _context.TimeOffRequests.Include(t => t.Employee);

            if (employeeId.HasValue)
            {
                timeOffRequests = timeOffRequests.Where(t => t.EmployeeId == employeeId);
            }

            if (month.HasValue)
            {
                timeOffRequests = timeOffRequests.Where(t => t.StartDate.Month == month);
            }

            timeOffRequests = timeOffRequests.OrderByDescending(t => t.RequestSubmissionTime);

            ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FirstName");

            return View(timeOffRequests.ToList());
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
                .Include(t => t.EmployeeId)
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", timeOffRequest.EmployeeId);
            return View(timeOffRequest);
        }

    }
}
