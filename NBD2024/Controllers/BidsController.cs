using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NBD2024.CustomControllers;
using NBD2024.Data;
using NBD2024.Models;
using NBD2024.Utilities;
using NBD2024.ViewModels;

namespace NBD2024.Controllers
{
    [Authorize]
    public class BidsController : ElephantController
    {
        private readonly NBDContext _context;

        public BidsController(NBDContext context)
        {
            _context = context;
        }

        // GET: Bids
        [Authorize(Roles = "Admin,Supervisor,Designer")]
        public async Task<IActionResult> Index(int? page, int? pageSizeID, string SearchString, string actionButton, int? MaterialID,
            int? LabourID)
        {
            //Supply SelectList for Materials and Labours
            ViewData["MaterialID"] = new SelectList(_context
                .Materials
                .OrderBy(m => m.Name), "ID", "Name", "Price");

            ViewData["LabourID"] = new SelectList(_context
                .Labours
                .OrderBy(m => m.Name), "ID", "Name", "Price");

            //Count the number of filters filters 
            ViewData["Filtering"] = "btn-outline-secondary";
            int numberFilters = 0;

            string[] sortOptions = new[] { ""};

           // Populate();

            var bids = _context.Bids
                .Include(b =>b.BidMaterials).ThenInclude(b=> b.Materials)
                .Include(b => b.BidLabours).ThenInclude(b => b.Labours)
                 .Include(b => b.Project)     
                .AsNoTracking();

            //Filters:
            
            if (!String.IsNullOrEmpty(SearchString))
            {
                bids = bids.Where(b => //b.Labour.Name.ToUpper().Contains(SearchString.ToUpper())
                 b.BidDate.ToString().Contains(SearchString)
                //|| b.Material.Name.ToUpper().Contains(SearchString.ToUpper())
                || b.Project.ProjectName.ToUpper().Contains(SearchString.ToUpper())
                || b.Project.City.Name.ToUpper().Contains(SearchString.ToUpper())
                || b.Project.City.Province.Name.ToUpper().Contains(SearchString.ToUpper())
                );
                numberFilters++;

            }

            if(numberFilters != 0)
            {
                ViewData["Filtering"] = "btn-danger";
                ViewData["numberFilters"] = "(" + numberFilters.ToString()
                    + " Filter" + (numberFilters > 1 ? "s" : "") + " Applied)";
                ViewData["ShowFilter"] = " show";
            }
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Bid>.CreateAsync(bids, page ?? 1, pageSize);
            return View(pagedData);
        }

        // GET: Bids/Details/5
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bids == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
                
                .Include(b => b.Project)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        // GET: Bids/Create
        [Authorize(Roles = "Admin,Supervisor")]
        public IActionResult Create()
        {
            Bid bid = new Bid();
            PopulateAssignedMaterialData(bid);
            PopulateAssignedLabourData(bid);
            //Agregar labour
           ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ProjectName");
           // Populate();
            return View();
        }

        // POST: Bids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Create([Bind("ID,BidDate,LabourHours,ProjectID")] Bid bid,
            string[] selectedOptions)
        {
            try
            {
                UpdateBidMaterials(selectedOptions, bid);
                if (ModelState.IsValid)
                {
                    _context.Add(bid);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { bid.ID });
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

            PopulateAssignedMaterialData(bid);
            PopulateAssignedLabourData(bid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ProjectName", bid.ProjectID);
            return View(bid);
        }

        // GET: Bids/Edit/5
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bids == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
                .Include(b => b.BidMaterials).ThenInclude(b => b.Materials)
                .Include(b => b.BidLabours).ThenInclude(b => b.Labours)
                .Include(b => b.Project)
                .FirstOrDefaultAsync(b => b.ID == id);


            if (bid == null)
            {
                return NotFound();
            }
            PopulateAssignedMaterialData(bid);
            PopulateAssignedLabourData(bid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ProjectName", bid.ProjectID);
            //Populate();
            return View(bid);
        }

        // POST: Bids/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Edit(int id, string[] selectedOptions, Byte[] RowVersion)
        {
            var bidToUpdate = await _context.Bids
                .Include(b => b.BidMaterials).ThenInclude(b => b.Materials)
                .Include(b => b.BidLabours).ThenInclude(b => b.Labours)
                .Include(b => b.Project)
                .FirstOrDefaultAsync (b => b.ID == id);

            if(bidToUpdate == null)
            {
                return NotFound();
            }

            UpdateBidMaterials(selectedOptions, bidToUpdate);
            _context.Entry(bidToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            if (await TryUpdateModelAsync<Bid>(bidToUpdate, "",
                b => b.BidDate, b=> b.ProjectID)) 
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { bidToUpdate.ID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BidExists(bidToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit"
                           + "was modified by another user. PLease go back and refresh.");
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }

            }
            PopulateAssignedMaterialData(bidToUpdate);
            PopulateAssignedLabourData(bidToUpdate);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ProjectName", bidToUpdate.ProjectID);
           
            return View(bidToUpdate);
        }

        // GET: Bids/Delete/5
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bids == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
              .Include(b => b.BidMaterials).ThenInclude(b => b.Materials)
              .Include(b => b.BidLabours).ThenInclude(b => b.Labours)
                .Include(b => b.Project)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        // POST: Bids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bids == null)
            {
                return Problem("No bids to delete");
            }
            var bid = await _context.Bids
                .Include(b => b.BidMaterials).ThenInclude(b => b.Materials)
              .Include(b => b.BidLabours).ThenInclude(b => b.Labours)
                .Include(b => b.Project)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            try
            {
                if (bid != null)
                {
                    _context.Bids.Remove(bid);
                }
                await _context.SaveChangesAsync();
                return Redirect(ViewData["returnURL"].ToString());

            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete Doctor. Remember, you cannot delete a Doctor that has patients assigned.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            return View(bid);
        }
        

        public PartialViewResult ListOfmaterialsDetails(int id)
        {
            var query = from b in _context.BidMaterials
                        .Include(b => b.Materials)
                        where b.BidID == id
                        orderby b.Materials.Name
                        select b;

            return PartialView("_ListOfMaterials", query.ToList());
        }

        public PartialViewResult ListOfLaboursDetails(int id)
        {
            var query = from b in _context.BidLabours
                       .Include(b => b.Labours)
                        where b.BidID == id
                        orderby b.Labours.Name
                        select b;

            return PartialView("_ListOfLabours", query.ToList());
        }
        //Adding Material and Labour
        private SelectList MaterialSelectList(string skip)
        {
            var MaterialQuery = _context.Materials
                .AsNoTracking();

            if (!String.IsNullOrEmpty(skip))
            {
                string[] avoidStrings = skip.Split('|');
                int[] skipKeys = Array.ConvertAll(avoidStrings, s => int.Parse(s));
                MaterialQuery = MaterialQuery
                    .Where(s => !skipKeys.Contains(s.ID));
            }
            return new SelectList(MaterialQuery.OrderBy(b => b.Name.ToLower()),
                "ID", "Name", "Price");
        }

        private SelectList LabourSelectList(string skip)
        {
            var LabourQuery = _context.Labours
                .AsNoTracking();

            if (!String.IsNullOrEmpty(skip))
            {
                string[] avoidStrings = skip.Split('|');
                int[] skipKeys = Array.ConvertAll(avoidStrings, s => int.Parse(s));
                LabourQuery = LabourQuery
                    .Where(s => !skipKeys.Contains(s.ID));
            }
            return new SelectList(LabourQuery.OrderBy(b => b.Name.ToLower()),
                "ID", "Name", "Price");
        }

        [HttpGet]
        public JsonResult GetMaterials(string skip)
        {
            return Json(MaterialSelectList(skip));
        }

        [HttpGet]
        public JsonResult GetLaboours(string skip)
        {
            return Json(LabourSelectList(skip));
        }

        //Hacer lo mismo para Labour
        private void PopulateAssignedMaterialData(Bid bid)
        {
            var allOptions = _context.Materials;
            var currentOprionsHS = new HashSet<int>(bid.BidMaterials
                .Select(b => b.MaterialID) );

            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();
            foreach (var m in allOptions)
            {
                if (currentOprionsHS.Contains(m.ID))
                {
                    selected.Add(new ListOptionVM
                    {
                        ID = m.ID,
                        DisplayText = m.Name,
                        Price = m.Price
                    });
                }
                else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = m.ID,
                        DisplayText = m.Name,
                        Price = m.Price
                    });
                }
            }

            ViewData["selOpts"] = new MultiSelectList(selected
                .OrderBy(s => s.DisplayText), "ID", "DisplayText", "Price");
            ViewData["availOpts"] = new MultiSelectList(available
                .OrderBy(s => s.DisplayText), "ID", "DisplayText", "Price");
        }

        private void PopulateAssignedLabourData(Bid bid)
        {
            var allOptions = _context.Labours;
            var currentOprionsHS = new HashSet<int>(bid.BidLabours
                .Select(b => b.LabourID));

            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();
            foreach (var m in allOptions)
            {
                if (currentOprionsHS.Contains(m.ID))
                {
                    selected.Add(new ListOptionVM
                    {
                        ID = m.ID,
                        DisplayText = m.Name,
                        Price = m.Price
                    });
                }
                else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = m.ID,
                        DisplayText = m.Name,
                        Price = m.Price
                    });
                }
            }

            ViewData["selOpts"] = new MultiSelectList(selected
                .OrderBy(s => s.DisplayText), "ID", "DisplayText", "Price");
            ViewData["availOpts"] = new MultiSelectList(available
                .OrderBy(s => s.DisplayText), "ID", "DisplayText", "Price");
        }

        private void UpdateBidMaterials(string[] selectedOptions, Bid bidToUpdate)
        {
            if(selectedOptions == null)
            {
                bidToUpdate.BidMaterials = new List<BidMaterial>();
                return;
            }

            var selectedOptionsHs = new HashSet<string>(selectedOptions);
            var currentOptionsHs = new HashSet<int>(bidToUpdate.BidMaterials.Select(m => 
                m.ID));

            foreach(var m in _context.Materials)
            {
                if (selectedOptionsHs.Contains(m.ID.ToString()))
                {
                    if(!currentOptionsHs.Contains(m.ID))
                    {
                        bidToUpdate.BidMaterials.Add(new BidMaterial
                        {
                            BidID = m.ID,
                            MaterialID = bidToUpdate.ID
                        });
                            
                    }
                }
                else
                {
                    if (currentOptionsHs.Contains(m.ID))
                    {
                        BidMaterial specToRemove = bidToUpdate.BidMaterials
                            .FirstOrDefault(b => b.MaterialID == m.ID);
                        _context.Remove(specToRemove);
                    }
                }
            }
        }

        private void UpdateBidLabours(string[] selectedOptions, Bid bidToUpdate)
        {
            if (selectedOptions == null)
            {
                bidToUpdate.BidLabours = new List<BidLabour>();
                return;
            }

            var selectedOptionsHs = new HashSet<string>(selectedOptions);
            var currentOptionsHs = new HashSet<int>(bidToUpdate.BidLabours.Select(m =>
                m.ID));

            foreach (var m in _context.Labours)
            {
                if (selectedOptionsHs.Contains(m.ID.ToString()))
                {
                    if (!currentOptionsHs.Contains(m.ID))
                    {
                        bidToUpdate.BidLabours.Add(new BidLabour
                        {
                            BidID = m.ID,
                            LabourID = bidToUpdate.ID
                        });

                    }
                }
                else
                {
                    if (currentOptionsHs.Contains(m.ID))
                    {
                        BidLabour specToRemove = bidToUpdate.BidLabours
                            .FirstOrDefault(b => b.LabourID == m.ID);
                        _context.Remove(specToRemove);
                    }
                }
            }
        }

        
        private bool BidExists(int id)
        {
          return _context.Bids.Any(e => e.ID == id);
        }
    }
}
