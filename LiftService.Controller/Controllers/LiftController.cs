using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LiftService.Domain.Model;

namespace LiftService.Controller.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LiftController : ControllerBase
    {
        public LiftController()
        {
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Content("Restricted resource!");
        }
    }
}
