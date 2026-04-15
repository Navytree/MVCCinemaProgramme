using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCCinemaProgramme.Data;
using MVCCinemaProgramme.Models;

namespace MVCCinemaProgramme.Controllers
{
    public class ProgrammesController : Controller
    {
        private readonly MVCCinemaProgrammeContext _context;

        public ProgrammesController(MVCCinemaProgrammeContext context)
        {
            _context = context;
        }

        // GET: Programmes
        public async Task<IActionResult> Index(string searchTitle, string searchGenre, int week = 0)
        {

            int diff = DateTime.Today.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0) diff += 7;
            DateTime startOfCurrentWeek = DateTime.Today.AddDays(-diff);
            DateTime startOfSelectedWeek = startOfCurrentWeek.AddDays(week * 7);
            DateTime endOfSelectedWeek = startOfSelectedWeek.AddDays(7);

            var query = _context.Programme.Include(p => p.Hall).Include(p => p.Movie)
                .Where(p => p.Begin >= startOfSelectedWeek && p.Begin < endOfSelectedWeek);

            if (!string.IsNullOrEmpty(searchTitle))
            { query = query.Where(p => p.Movie.Title.Contains(searchTitle)); }

            if (!string.IsNullOrEmpty(searchGenre))
            { query = query.Where(p => p.Movie.Genre.Contains(searchGenre));}

            var programmes = await query.OrderBy(p => p.Begin).ToListAsync();

            ViewBag.CurrentTitle = searchTitle;
            ViewBag.CurrentGenre = searchGenre;
            ViewBag.CurrentWeek = week;

            return View(programmes);
        }

        // GET: Programmes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programme = await _context.Programme
                .Include(p => p.Movie)
                .Include(p => p.Hall)
                .ThenInclude(p => p.Seats)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.ProgrammeId = id;

            if (programme == null)
            {
                return NotFound();
            }

            return View(programme);
        }









        // GET: Programmes/Create
        public IActionResult Create()
        {
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "Id", "Id");
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id");
            return View();
        }

        // POST: Programmes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieId,HallId,Begin,End")] Programme programme)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "Id", "Id", programme.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", programme.MovieId);
            return View(programme);
        }

        // GET: Programmes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programme = await _context.Programme.FindAsync(id);
            if (programme == null)
            {
                return NotFound();
            }
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "Id", "Id", programme.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", programme.MovieId);
            return View(programme);
        }

        // POST: Programmes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,HallId,Begin,End")] Programme programme)
        {
            if (id != programme.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgrammeExists(programme.Id))
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
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "Id", "Id", programme.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", programme.MovieId);
            return View(programme);
        }

        // GET: Programmes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programme = await _context.Programme
                .Include(p => p.Hall)
                .Include(p => p.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programme == null)
            {
                return NotFound();
            }

            return View(programme);
        }

        // POST: Programmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programme = await _context.Programme.FindAsync(id);
            if (programme != null)
            {
                _context.Programme.Remove(programme);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgrammeExists(int id)
        {
            return _context.Programme.Any(e => e.Id == id);
        }
    }
}
