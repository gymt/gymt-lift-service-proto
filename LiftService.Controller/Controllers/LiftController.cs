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
        private readonly GymtContext _db;

        public LiftController(GymtContext db)
        {
            _db = db;

            if (_db.Lifts.Count() == 0)
            {
                _db.Lifts.Add(new Lift { Name = "Bicep Curl" });
                _db.Lifts.Add(new Lift { Name = "Hammer Curl" });
                _db.Lifts.Add(new Lift { Name = "Reverse Curl" });
                _db.Lifts.Add(new Lift { Name = "Bench Press" });
                _db.Lifts.Add(new Lift { Name = "Incline Bench Press" });
                _db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lift>>> GetAllLifts()
        {
            return await _db.Lifts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lift>> GetLiftById(long id)
        {
            var lift = await _db.Lifts.FindAsync(id);

            if (lift == null)
            {
                return NotFound();
            }

            return lift;
        }

        [HttpPost]
        public async Task<ActionResult<Lift>> PostLift(Lift lift)
        {
            _db.Lifts.Add(lift);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLiftById), new { id = lift.Id }, lift);
        }
    }
}
