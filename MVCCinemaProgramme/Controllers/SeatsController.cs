using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MVCCinemaProgramme.Data;
using MVCCinemaProgramme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCinemaProgramme.Controllers
{
    public class SeatsController : Controller
    {
        private readonly MVCCinemaProgrammeContext _context;

        public SeatsController(MVCCinemaProgrammeContext context)
        {
            _context = context;
        }

        // GET: Seats
        public async Task<IActionResult> Index()
        {
            var mVCCinemaProgrammeContext = _context.Seat.Include(s => s.Hall);
            return View(await mVCCinemaProgrammeContext.ToListAsync());
        }

        // GET: Seats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seat
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // GET: Seats/Create
        public IActionResult Create()
        {
            ViewData["HallId"] = new SelectList(_context.Hall, "Id", "Id");
            return View();
        }

        // POST: Seats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Taken,HallId")] Seat seat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HallId"] = new SelectList(_context.Hall, "Id", "Id", seat.HallId);
            return View(seat);
        }





        // GET: Seats/Edit/5 aaaaaaaaaaaaaaaaaaaaaaaaa
        public async Task<IActionResult> Edit(int? id, int? programmeId)
        {
            if (id == null || programmeId == null) { return NotFound(); }

            var seat = await _context.Seat.Include(s=>s.Hall).FirstOrDefaultAsync(s=>s.Id == id);
            if (seat == null) { return NotFound(); }

            var screening = await _context.Programme.Include(p=>p.Movie).AsNoTracking().FirstOrDefaultAsync(p=>p.Id == programmeId);

            if (screening != null)
            {
                ViewBag.ProgrammeId = programmeId;
                ViewBag.MovieTitle = screening.Movie.Title;
                ViewBag.Price = screening.Movie.TicketPrice;
                ViewBag.Begin = screening.Begin;
            }
            return View(seat);
        }

        // POST: Seats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Taken,HallId")] Seat seat, string email, int? programmeId)
        {
            if (id != seat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var seatData = await _context.Seat.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                    if (seatData == null) { return NotFound(); }
                    seat.Number = seatData.Number;
                    seat.HallId = seatData.HallId;
                    _context.Update(seat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatExists(seat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Processing", new { id = seat.Id, programmeId = programmeId });
            }
            ViewBag.ProgrammeId = programmeId;
            return View(seat);
        }

        public IActionResult Processing(int id, int? programmeId)
        {
            ViewBag.Id = id;
            ViewBag.ProgrammeId = programmeId;
            return View();
        }

        public async Task<IActionResult> ConfirmationScreen(int? id, int? programmeId)
        {
            if (id == null || programmeId == null) return NotFound();
            var screening = await _context.Programme.Include(p => p.Movie).AsNoTracking().FirstOrDefaultAsync(p => p.Id == programmeId);
            var seat = await _context.Seat.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

            if (screening == null || seat == null)
            {
                return NotFound();
            }

                ViewBag.MovieTitle = screening.Movie.Title;
                ViewBag.Price = screening.Movie.TicketPrice;
                ViewBag.Begin = screening.Begin;
                ViewBag.Number = seat.Number;
                ViewBag.HallId = seat.HallId;

            return View();
        }





        // GET: Seats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seat
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // POST: Seats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seat = await _context.Seat.FindAsync(id);
            if (seat != null)
            {
                _context.Seat.Remove(seat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeatExists(int id)
        {
            return _context.Seat.Any(e => e.Id == id);
        }
    }
}
