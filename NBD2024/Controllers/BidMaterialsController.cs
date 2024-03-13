using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NBD2024.CustomControllers;
using NBD2024.Data;
using NBD2024.Models;
using NBD2024.Utilities;

namespace NBD2024.Controllers
{
    public class BidMaterialsController : ElephantController
    {
        private readonly NBDContext _context;

        public BidMaterialsController(NBDContext context)
        {
            _context = context;
        } 

        // GET: Materials
        public async Task<IActionResult> Index(int? page, int? pageSizeID)
        {
            var materials = _context.Materials
                
                 .AsNoTracking();

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Material>.CreateAsync(materials, page ?? 1, pageSize);
            return View(pagedData);
            
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                               
                .FirstOrDefaultAsync(m => m.ID == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            Material material = new Material();
            PopulateDropDownLists();
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Quantity,Area,PerYardCharge,ClientID,ProyectID,InventoryID")] Material material,
            string[] selectedOptions)
        {
            try
            {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new {material.ID});
            }
               

            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateDropDownLists(material);
           return View(material);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                
             
                .FirstOrDefaultAsync(m => m.ID == id);
                
            if (material == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(material);
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Quantity,PerUnitCharge,ProyectID,InventoryID")] Material material)
        {
            var materialToUpdate = await _context.Materials
                
               
                .FirstOrDefaultAsync(m => m.ID == id);

            if (materialToUpdate == null)
            {
                return NotFound();
            }
            if(await TryUpdateModelAsync<Material>(materialToUpdate, ""
          ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { materialToUpdate.ID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(materialToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

                }
                return RedirectToAction("Index", "Lookup", new { Tab = ControllerName() + "-Tab" });

            }
            PopulateDropDownLists(materialToUpdate);
            return View(materialToUpdate);
          
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
             
               
                .FirstOrDefaultAsync(m => m.ID == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Materials == null)
            {
                return Problem("No Materials To delete");
            }
            var material = await _context.Materials.FindAsync(id);
            try
            {
                
            if (material != null)
            {
                _context.Materials.Remove(material);
            }
            
            await _context.SaveChangesAsync();
            return Redirect(ViewData["returnURL"].ToString());
            }
            catch (DbUpdateException)
            {
                //Note: there is really no reason a delete should fail if you can "talk" to the database.
                ModelState.AddModelError("", "Unable to delete record. Try again, and if the problem persists see your system administrator.");
            }
            return View(material);
        }

        private SelectList ClientSelectList(int? selectedId)
        {
            return new SelectList(_context.Clients
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName), "ID", "FormalName", selectedId);
        }

        private SelectList ProjectSelectList(int? selectId)
        {
            return new SelectList(_context.Projects
                .OrderBy(p => p.ProjectName), "ID", "ProjectName", selectId);
        }

        

        private void PopulateDropDownLists(Material material = null)
        {
           
          //  ViewData["InventoryID"] = InventorySelectList(material?.InventoryID);
        }

        private bool MaterialExists(int id)
        {
          return _context.Materials.Any(e => e.ID == id);
        }
    }
}
