using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LiftService.Domain.Model;

namespace LiftService.Controller.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LiftController : ControllerBase
    {
        private readonly LiftContext _context;

        public LiftController(LiftContext context)
        {
            _context = context;

            if (_context.Lifts.Count() == 0)
            {
                _context.Lifts.Add(new Lift { Name = "Bicep Curl" });
                _context.Lifts.Add(new Lift { Name = "Hammer Curl" });
                _context.Lifts.Add(new Lift { Name = "Reverse Curl" });
                _context.Lifts.Add(new Lift { Name = "Bench Press" });
                _context.Lifts.Add(new Lift { Name = "Incline Bench Press" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lift>>> GetAllLifts()
        {
            return await _context.Lifts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lift>> GetLiftById(long id)
        {
            var lift = await _context.Lifts.FindAsync(id);

            if (lift == null)
            {
                return NotFound();
            }

            return lift;
        }

        // Not working properly getting a 500 Internal server error - TODO
        [HttpPost]
        public async Task<ActionResult<Lift>> PostLift(Lift lift)
        {
            _context.Lifts.Add(lift);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLiftById), new { id = lift.Id }, lift);
        }
    }
}
