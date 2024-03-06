using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBD2024.Data;
using NBD2024.Models;

namespace NBD2024.Controllers
{
    public class LabourTypesController : Controller
    {
        private readonly NBDContext _context;

        public LabourTypesController(NBDContext context)
        {
            _context = context;
        }

        // GET: LabourTypes
        public async Task<IActionResult> Index()
        {
              return View(await _context.LabourTypes.ToListAsync());
        }

        // GET: LabourTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LabourTypes == null)
            {
                return NotFound();
            }

            var labourType = await _context.LabourTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (labourType == null)
            {
                return NotFound();
            }

            return View(labourType);
        }

        // GET: LabourTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LabourTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] LabourType labourType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labourType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(labourType);
        }

        // GET: LabourTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LabourTypes == null)
            {
                return NotFound();
            }

            var labourType = await _context.LabourTypes.FindAsync(id);
            if (labourType == null)
            {
                return NotFound();
            }
            return View(labourType);
        }

        // POST: LabourTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] LabourType labourType)
        {
            if (id != labourType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labourType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabourTypeExists(labourType.ID))
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
            return View(labourType);
        }

        // GET: LabourTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LabourTypes == null)
            {
                return NotFound();
            }

            var labourType = await _context.LabourTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (labourType == null)
            {
                return NotFound();
            }

            return View(labourType);
        }

        // POST: LabourTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LabourTypes == null)
            {
                return Problem("Entity set 'NBDContext.LabourTypes'  is null.");
            }
            var labourType = await _context.LabourTypes.FindAsync(id);
            if (labourType != null)
            {
                _context.LabourTypes.Remove(labourType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabourTypeExists(int id)
        {
          return _context.LabourTypes.Any(e => e.ID == id);
        }
    }
}
