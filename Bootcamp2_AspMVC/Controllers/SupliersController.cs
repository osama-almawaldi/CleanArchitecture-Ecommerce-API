using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Models;
using Bootcamp2_AspMVC.Filters;

namespace Bootcamp2_AspMVC.Controllers
{
    [SessionAuthorize]
    public class SupliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Supliers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Supliers.ToListAsync());
        }

        // GET: Supliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supliers = await _context.Supliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supliers == null)
            {
                return NotFound();
            }

            return View(supliers);
        }

        // GET: Supliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Supliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Address,Salary,AddDate")] Supliers supliers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supliers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supliers);
        }

        // GET: Supliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supliers = await _context.Supliers.FindAsync(id);
            if (supliers == null)
            {
                return NotFound();
            }
            return View(supliers);
        }

        // POST: Supliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Address,Salary,AddDate")] Supliers supliers)
        {
            if (id != supliers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supliers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupliersExists(supliers.Id))
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
            return View(supliers);
        }

        // GET: Supliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supliers = await _context.Supliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supliers == null)
            {
                return NotFound();
            }

            return View(supliers);
        }

        // POST: Supliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supliers = await _context.Supliers.FindAsync(id);
            if (supliers != null)
            {
                _context.Supliers.Remove(supliers);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupliersExists(int id)
        {
            return _context.Supliers.Any(e => e.Id == id);
        }
    }
}
