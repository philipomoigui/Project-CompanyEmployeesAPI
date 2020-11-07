using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Entities.DataTransferObjects;
using CompanyEmployees.ActionFilters;

namespace CompanyEmployees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;

        public AuthenticationController(IMapper mapper, ILoggerManager loggerManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager)
        {
            _mapper = mapper;
            _loggerManager = loggerManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register([FromBody] UserRegisterationDto userRegisteration)
        {
            var user = _mapper.Map<ApplicationUser>(userRegisteration);

            var result = await _userManager.CreateAsync(user, userRegisteration.Password);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

           foreach(var role in userRegisteration.Roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return StatusCode(201);
        }
    }
}
