using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugsApi.Controllers
{
    public class BugsController : Controller
    {
        private readonly AppDbContext _context;

        public BugsController(AppDbContext context)
        {
            _context = context;
        }
        // GET: Bugs
        public async Task<IActionResult> Index()
        {
            //var bugslist = await new BGController(_context).GetBugs();

            var bugslist = await _context.Bugs.Include(u => u.User).ToListAsync();
            
            return View(bugslist);
        }

        // GET: Bugs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var bug = await _context.Bugs.FirstOrDefaultAsync(b =>b.Id == id);
            var bug = await _context.Bugs.Include(u => u.User).FirstOrDefaultAsync(b => b.Id == id);

            if (bug == null)
            {
                return NotFound();
            }

            return View(bug);
        }

        // GET: Bugs/Create
        public ActionResult Create()
        {

            return View(new Bug
            {
                Title = "",
                Description = "",
                Opened = DateTime.UtcNow,
                IsOpen = true
            });

        }

        // POST: Bugs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection, Bug bug)
        {
            if(ModelState.IsValid)
            {
                if (_context.User.Any(u => u.Name == bug.User.Name) & bug.User.Name != null)
                {
                    UsersModel user = await _context.User.FirstAsync(u => u.Name == bug.User.Name);
                    //add new bug to existing users
                    Bug newbug = new Bug
                    {
                        Description = bug.Description,
                        Title = bug.Title,
                        IsOpen = bug.IsOpen,
                        Opened = bug.Opened,
                        User = user
                    };
                    _context.Bugs.Add(bug);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Bug newbug = new Bug
                    {
                        Description = bug.Description,
                        Title = bug.Title,
                        IsOpen = bug.IsOpen,
                        Opened = bug.Opened,
                        User = null
                    };
                    _context.Bugs.Add(bug);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(bug);
        }

        // GET: Bugs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = await _context.Bugs.Include(u => u.User).FirstOrDefaultAsync(b => b.Id == id);

            if (bug == null)
            {
                return NotFound();
            }

            return View(bug);

        }

        // POST: Bugs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, IFormCollection collection, Bug bug)
        {
            if (id != bug.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    if(_context.User.Any(u => u.Name == bug.User.Name))
                    {
                        bug.User = await _context.User.FirstAsync(u => u.Name == bug.User.Name);
                        _context.Update(bug);
                        await _context.SaveChangesAsync();
                    } else
                    {
                        return NotFound();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!BugExists(bug.Id))
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
            return View(bug);
        }

        // GET: Bugs/Close/5
        public async Task<IActionResult> Close(int id)
        {
            try
            {

                var bug = await _context.Bugs.Include(u => u.User).FirstOrDefaultAsync(b => b.Id == id);
                bug.IsOpen = false;
                _context.Update(bug);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: Bugs/Open/5
        public async Task<IActionResult> Open(int id)
        {
            try
            {

                var bug = await _context.Bugs.Include(u => u.User).FirstOrDefaultAsync(b => b.Id == id);
                bug.IsOpen = true;
                _context.Update(bug);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return NotFound();
            }
        }

        private bool BugExists(int id)
        {
            return _context.Bugs.Any(b => b.Id == id);
        }
    }
}