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
    public class TicketsController : Controller
    {
        private readonly MVCCinemaProgrammeContext _context;

        public TicketsController(MVCCinemaProgrammeContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var mVCCinemaProgrammeContext = _context.Ticket.Include(t => t.Programme).Include(t => t.Seat);
            return View(await mVCCinemaProgrammeContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Programme)
                .Include(t => t.Seat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create(int id, int programmeId)
        {
            var seat = await _context.Seat.Include(s => s.Hall).FirstOrDefaultAsync(s => s.Id == id);
            var programme = await _context.Programme.Include(p => p.Movie).FirstOrDefaultAsync(p => p.Id == programmeId);

            if (seat == null || programme == null) return NotFound();


            var ticket = new Ticket {
                SeatId = id,
                Seat = seat,
                ProgrammeId = programmeId,
                Programme = programme };

            ViewBag.MovieTitle = programme.Movie.Title;
            ViewBag.Price = programme.Movie.TicketPrice;
            ViewBag.Begin = programme.Begin;
            ViewBag.ProgrammeId = programmeId;
            ViewBag.SeatId = id;

            return View(ticket);

        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeatId,ProgrammeId,CustomerEmail")] Ticket ticket, int seatId, int programmeId)
        {
            bool isReserved = await _context.Ticket.AnyAsync(t => t.SeatId == seatId && t.ProgrammeId == programmeId);
            if (isReserved)
            {
                return BadRequest("This seat is already taken for this screening!");
            }
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction("Processing", "Seats", new { id = ticket.SeatId, programmeId = ticket.ProgrammeId });
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ProgrammeId"] = new SelectList(_context.Programme, "Id", "Id", ticket.ProgrammeId);
            ViewData["SeatId"] = new SelectList(_context.Seat, "Id", "Id", ticket.SeatId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SeatId,ProgrammeId,CustomerEmail,PurchaseDate")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["ProgrammeId"] = new SelectList(_context.Programme, "Id", "Id", ticket.ProgrammeId);
            ViewData["SeatId"] = new SelectList(_context.Seat, "Id", "Id", ticket.SeatId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Programme)
                .Include(t => t.Seat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
