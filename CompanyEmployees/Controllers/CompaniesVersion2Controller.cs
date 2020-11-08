using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    //[ApiVersion("2.0", Deprecated = true)]
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
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
