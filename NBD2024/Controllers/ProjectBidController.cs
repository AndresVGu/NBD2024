using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBD2024.CustomControllers;
using NBD2024.Data;
using NBD2024.Models;
using NBD2024.Utilities;

namespace NBD2024.Controllers
{
    public class ProjectBidController : ElephantController
    { 
        private readonly NBDContext _context;

        public ProjectBidController(NBDContext context)
        {
            _context = context;
        }

        //GET: BID
        public async Task<IActionResult> Index(int? ProjectID, int? page,
            int? pageSizeID, int? BidID, string actionButton,
            string SearchString, string sortDirection = "desc",
            string sortField = "Bid")
        {
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Projects");

            PopulateDropDownLists();

            string[] sortOptions = new[] { ""};

            var pbid = from a in _context.Bids
                       .Include(a => a.BidMaterials).ThenInclude(a => a.Materials)
                       .Include(a => a.BidLabours).ThenInclude(a => a.Labours)
                       .Include(a => a.Project)
                       where a.ProjectID == ProjectID.GetValueOrDefault()
                       select a;

            Project project = await _context.Projects
                .Include(p => p.Client)
                .Include(p => p.City)
                .Where(p => p.ID == ProjectID.GetValueOrDefault())
                .AsNoTracking()
                .FirstOrDefaultAsync();

            ViewBag.Project=project;

            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<Bid>.CreateAsync(pbid.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }
       
        private SelectList LabourSelectList(int? id)
        {
            var query = from d in _context.Labours
                        orderby d.Name
                        select d;
            return new SelectList(query, "ID", "Name", id);
        }


        private SelectList MaterialSelectList(int? id)
        {
            var query = from d in _context.Materials
                        orderby d.Name
                        select d;
            return new SelectList(query, "ID", "Name",  id);
        }
        private void PopulateDropDownLists(Bid bid = null)
        {
            ViewData["MaterialID"] = MaterialSelectList(bid?.BidMaterials.FirstOrDefault().MaterialID);
            ViewData["MaterialID"] = LabourSelectList(bid?.BidLabours.FirstOrDefault().LabourID);
        }

        private bool ProjectBidExists(int id)
        {
            return _context.Bids.Any(b => b.ID == id);
        }
    }
}
