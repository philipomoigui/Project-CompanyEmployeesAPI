using AutoMapper;
using CompanyEmployees.ActionFilters;
using CompanyEmployees.ModelBinders;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    //[ApiVersion("1.0")]
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    //[ResponseCache(CacheProfileName = "120SecondsDuration")]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompaniesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        /// <summary>
        /// Get A list of All Companies
        /// </summary>
        /// <returns>A List of Companies</returns>
        /// <response code="200">Returns the list of Companies</response>
        [HttpGet(Name = "GetCompanies"), Authorize(Roles = "Manager")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges: false);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(companiesDto);
        }


        /// <summary>
        /// Get A Company Through its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Company with the passed ID</returns>
        /// <response code="200">Returns the requested Company</response>
        [HttpGet("{id}", Name = "CompanyById")]
        //[ResponseCache(Duration = 60)]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var companyDto = _mapper.Map<CompanyDto>(company);
                return Ok(companyDto);
            }
        }


        /// <summary>
        /// Get A Collection of Company by providing ids of comma seperated companies as Parameter
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Returns a collection of Companies</returns>
        /// <response code="400">If No id is passed as parameter</response>
        /// <response code="404">If some or all the id(s) is not valid</response>
        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var companyEntities = await _repository.Company.GetByIdsAsync(ids, trackChanges: false);

            if(ids.Count() != companyEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return Ok(companiesToReturn);
        }


        /// <summary>
        /// Create A Company
        /// </summary>
        /// <param name="company"></param>
        /// <returns>A newly Created Company</returns>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is not valid</response>
        /// <response code="201">Returns the newly created Item</response>

        [HttpPost(Name = "CreateCompany")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateCompany([FromBody]CreateCompanyDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
        }

        /// <summary>
        /// Create A Collection of Company
        /// </summary>
        /// <param name="companyCollection"></param>
        /// <returns>Create a collection of companies</returns>
        /// <response code="400">If request body collection is null</response>
        /// <response code="201">return collection of company created</response>

        [HttpPost("collection")]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CreateCompanyDto> companyCollection)
        {
            if(companyCollection == null)
            {
                _logger.LogError("Company collection sent from client is null.");
                return BadRequest("Company collection is null");
            }

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }

            await _repository.SaveAsync();

            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("CompanyCollection", new { ids }, companyCollectionToReturn);
        }

        /// <summary>
        /// Delete A Company
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A No content 204 success message</returns>
        /// <response code="404">If the id is not valid</response>

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var company = HttpContext.Items["company"] as Company;

            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Update A Company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="company"></param>
        /// <returns>A No Content success Message</returns>
        /// <response code="404">If either the employee Id or company Id is not valid</response>
        /// <response code="400">if the request body is null</response>
        /// <response code="422">If the model is not valid</response>
        /// <response code="204">Returns a 204 No content message</response>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] UpdateCompanyDto company)
        {
            var companyEntity = HttpContext.Items["company"] as Company;

            _mapper.Map(company, companyEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Get Options Avaliable for this Actions
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            HttpContext.Response.Headers.Add("Allow", "GET, POST, OPTIONS");
            return Ok();
        }
    }
}