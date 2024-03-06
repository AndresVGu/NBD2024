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
    public class ProjectsController : ElephantController
    {
        private readonly NBDContext _context;

        public ProjectsController(NBDContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index(string SearchString, int? ClientID,
           int? page, int? pageSizeID, string actionButton, string sortDirection = "asc", string sortField = "ProjectName")
        {
            //Count the number of filters applied - start by assuming no filters
            ViewData["Filtering"] = "btn-outline-secondary";
            int numberFilters = 0;

            string[] sortOptions = new[] { "ProjectName", "StartTime", "EndTime", "ProjectSite", "City", "Client" };

            PopulateDropDownLists();

            var projects = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.City)
                .AsNoTracking();

            #region Filters
            //filters:
            if (ClientID.HasValue)
            {
                projects = projects.Where(p => p.ClientID == ClientID);
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchString))
            {
                projects = projects.Where(p => p.ProjectSite.ToUpper().Contains(SearchString.ToUpper())
                            || p.BidDate.ToString().Contains(SearchString) || p.StartTime.ToString().Contains(SearchString)
                            || p.EndTime.ToString().Contains(SearchString)
                            || p.Client.FirstName.ToUpper().Contains(SearchString.ToUpper())
                            || p.ProjectName.ToUpper().Contains(SearchString.ToUpper())
                            || p.Client.LastName.ToUpper().Contains(SearchString.ToUpper())
                            || p.Client.CompanyName.ToUpper().Contains(SearchString.ToUpper())
                            || p.City.Name.ToUpper().Contains(SearchString.ToUpper())
                            || p.City.Province.Name.ToUpper().Contains(SearchString.ToUpper())
                            );
                numberFilters++;
            }
            //Feedback about the state of the filters
            if (numberFilters != 0)
            {
                //Toggle the Open/Closed state of the collapse depending in if we are filtering
                ViewData["Filtering"] = "btn-danger";
                //Show how many filters have been applied
                ViewData["numberFilters"] = "(" + numberFilters.ToString()
                    + " Filter" + (numberFilters > 1 ? "s" : "") + " Applied)";
                //Keep the Bootstrap collapse open
                @ViewData["ShowFilter"] = " show";
            }


            #endregion

            #region Sorting:
            //Before we sort, see if we have called for a change of filtering or sorting
            if (!String.IsNullOrEmpty(actionButton))//form submitted
            {
                page = 1;
                if (sortOptions.Contains(actionButton))//Change of sort is requested
                {
                    if (actionButton == sortField) // Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton; // sort by the button clicked
                }
            }
            //sort itself
            if (sortField == "City")
            {
                if (sortDirection == "asc")
                {
                    projects = projects
                        .OrderBy(p => p.City.Name);
                }
                else
                {
                    projects = projects
                        .OrderByDescending(p => p.City.Name);
                }
            }
            else if (sortField == "ProjectSite")
            {
                if (sortDirection == "asc")
                {
                    projects = projects
                        .OrderBy(p => p.ProjectSite);
                }
                else
                {
                    projects = projects
                        .OrderByDescending(p => p.ProjectSite);
                }
            }
            else if (sortField == "EndTime")
            {
                if (sortDirection == "asc")
                {
                    projects = projects
                        .OrderByDescending(p => p.EndTime);
                }
                else
                {
                    projects = projects
                        .OrderBy(p => p.EndTime);
                }
            }
            else if (sortField == "StartTime")
            {
                if (sortDirection == "asc")
                {
                    projects = projects
                        .OrderByDescending(p => p.StartTime);
                }
                else
                {
                    projects = projects
                        .OrderBy(p => p.StartTime);
                }
            }
            else if (sortField == "ProjectName")
            {
                if (sortDirection == "asc")
                {
                    projects = projects
                        .OrderByDescending(p => p.ProjectName);
                }
                else
                {
                    projects = projects
                        .OrderBy(p => p.ProjectName);
                }
            }
            else //sorting by Client Name
            {
                if(sortDirection == "asc")
                {
                    projects = projects
                        .OrderBy(p => p.Client.FirstName)
                        .ThenBy(p => p.Client.LastName);
                }
                else
                {
                    projects = projects
                        .OrderByDescending(p => p.Client.FirstName)
                        .ThenByDescending(p => p.Client.LastName);
                }
            }

            //set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            #endregion

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Project>.CreateAsync(projects.AsNoTracking(), page ?? 1, pageSize);


            return View(pagedData);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .AsNoTracking()
                .Include(p => p.Client)
                .Include(p => p.City)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            var project = new Project();
            PopulateCityDropDownLists();
            PopulateDropDownLists();
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProjectName,BidDate,StartTime,EndTime,ProjectSite,SetupNotes,CityID,ClientID")] Project project,
            string[]selectedOptions)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new {ProjectID = project.ID});
                    //Deberia agragar una tabla ProjectMaterials
                    
                }
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to create record. Try again, and if the problem persists see your administrator.");
            }
           
            PopulateDropDownLists(project);
            PopulateCityDropDownLists(project);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.City)
                .FirstOrDefaultAsync(p => p.ID == id);
            if (project == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(project);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedOptions,
            Byte[] RowVersion)
        {
            var projectToUpdate = await _context.Projects
                .Include(p => p.City)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (projectToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Project>(projectToUpdate, "",
                p => p.ProjectName,
                p => p.BidDate, p => p.StartTime, p => p.EndTime, p => p.CityID,
                p => p.ProjectSite, p => p.SetupNotes, p => p.ClientID))
            {
                try
                {
                    // await _context.SaveChangesAsync();
                    // return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(projectToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to create record. Try again, and if the problem persists see your administrator.");
                }

            }
            PopulateDropDownLists(projectToUpdate);
            PopulateCityDropDownLists(projectToUpdate);
            await _context.SaveChangesAsync();
           // return RedirectToAction(nameof(Index));
            return View(projectToUpdate);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .AsNoTracking()
                .Include(p => p.Client)
                .Include(p => p.City)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Project has alredy been deleted from the system.");
            }
            var project = await _context.Projects
               .Include(p => p.Client)
               .Include(p => p.City)
               .FirstOrDefaultAsync(m => m.ID == id);
            try
            {
                if (project != null)
                {
                    _context.Projects.Remove(project);
                }

                await _context.SaveChangesAsync();
               return Redirect(ViewData["returnURL"].ToString());
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to create record. Try again, and if the problem persists see your administrator.");
            }
            return View(project);

        }

        #region Project Methods:
        //Select Clients
        private SelectList ClientSelectList(int? selectedId)
        {
            return new SelectList(_context.Clients
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName), "ID", "FormalName", selectedId);
        }
        //Populate DropdownLists
        private void PopulateDropDownLists(Project project = null)
        {
            var dQuery = from c in _context.Clients
                         orderby c.CompanyName
                         select c;
            ViewData["ClientID"] = new SelectList(dQuery, "ID", "ClientName", project?.ClientID);
            ViewData["ClientID"] = ClientSelectList(project?.ClientID);
        }

        //Province Select List
        private SelectList ProvinceSelectList(string selectedID)
        {
            return new SelectList(_context.Provinces
                .OrderBy(c => c.Name), "ID", "Name", selectedID);
        }
        //City Select List:
        private SelectList CitySelectList(string ProvinceID, int? selectedID)
        {
            var query = from c in _context.Cities
                        where c.ProvinceID == ProvinceID
                        select c;
            return new SelectList(query.OrderBy(c => c.Name), "ID", "Summary", selectedID);
        }

        private void PopulateCityDropDownLists(Project project = null)
        {
           
            if ((project?.CityID).HasValue)
            {
                //Careful: CityID might have a value but the city object is missing
                if (project.City == null)
                {
                    project.City = _context.Cities.Find(project.CityID);
                }
                ViewData["ProvinceID"] = ProvinceSelectList(project.City.ProvinceID);
                ViewData["CityID"] = CitySelectList(project.City.ProvinceID, project.CityID);
            }
            else
            {
                ViewData["ProvinceID"] = ProvinceSelectList(null);
                ViewData["CityID"] = CitySelectList(null, null);
            }

        }
        [HttpGet]
        public JsonResult GetCities(string ProvinceID)
        {
            return Json(CitySelectList(ProvinceID, null));
        }
        #endregion

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ID == id);
        }
    }
}
