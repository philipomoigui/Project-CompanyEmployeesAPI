using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/companies")]
    [ApiController]
    public class CompaniesVersion2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;

        public CompaniesVersion2Controller(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IActionResult> GetCompanies()
        {
           var companies =  await _repositoryManager.Company.GetAllCompaniesAsync(trackChanges: false);

            return Ok(companies);
        }
    }
}
