using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBD2024.CustomControllers;
using NBD2024.Data;
using NBD2024.Models;
using NBD2024.Utilities;

namespace NBD2024.Controllers
{
    public class BidLaboursController : ElephantController
    {
        private readonly NBDContext _context;

        public BidLaboursController(NBDContext context)
        {
            _context = context;
        }

        // GET: Labours
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Index(int? page, int? pageSizeID)
        {
            var lab = _context.Labours
                .AsNoTracking();

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Labour>.CreateAsync(lab, page ?? 1, pageSize);
            return View(pagedData);
        }

        // GET: Labours/Details/5
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Labours == null)
            {
                return NotFound();
            }

            var labour = await _context.Labours
                
                .FirstOrDefaultAsync(m => m.ID == id);
            if (labour == null)
            {
                return NotFound();
            }

            return View(labour);
        }

        // GET: Labours/Create
        [Authorize(Roles = "Admin,Supervisor")]
        public IActionResult Create()
        {
            //ViewData["LabourTypeID"] = new SelectList(_context.LabourTypes, "ID", "Name");
            return View();
        }

        // POST: Labours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Create([Bind("ID,LabourHours,LabourDescription,LabourUnitPrice,LabourTypeID")] Labour labour)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["LabourTypeID"] = new SelectList(_context.LabourTypes, "ID", "Name", labour.LabourTypeID);
            return View(labour);
        }

        // GET: Labours/Edit/5
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Labours == null)
            {
                return NotFound();
            }

            var labour = await _context.Labours.FindAsync(id);
            if (labour == null)
            {
                return NotFound();
            }
           // ViewData["LabourTypeID"] = new SelectList(_context.LabourTypes, "ID", "Name", labour.LabourTypeID);
            return View(labour);
        }

        // POST: Labours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LabourHours,LabourDescription,LabourUnitPrice,LabourTypeID")] Labour labour)
        {
            if (id != labour.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabourExists(labour.ID))
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
           // ViewData["LabourTypeID"] = new SelectList(_context.LabourTypes, "ID", "Name", labour.LabourTypeID);
            return View(labour);
        }

        // GET: Labours/Delete/5
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Labours == null)
            {
                return NotFound();
            }

            var labour = await _context.Labours
        
                .FirstOrDefaultAsync(m => m.ID == id);
            if (labour == null)
            {
                return NotFound();
            }

            return View(labour);
        }

        // POST: Labours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Labours == null)
            {
                return Problem("Entity set 'NBDContext.Labours'  is null.");
            }
            var labour = await _context.Labours.FindAsync(id);
            if (labour != null)
            {
                _context.Labours.Remove(labour);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabourExists(int id)
        {
          return _context.Labours.Any(e => e.ID == id);
        }
    }
}
