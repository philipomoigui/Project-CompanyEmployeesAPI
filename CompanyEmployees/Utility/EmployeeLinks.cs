using Contracts;
using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Utility
{
    public class EmployeeLinks
    {
        private readonly IDataShaper<EmployeeDto> _dataShaper;
        private readonly LinkGenerator _linkGenerator;

        public EmployeeLinks(IDataShaper<EmployeeDto> dataShaper, LinkGenerator linkGenerator)
        {
            _dataShaper = dataShaper;
            _linkGenerator = linkGenerator;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeeDtos, string fields, Guid companyId, HttpContext httpContext)
        {
            var shapedEmployees = ShapeData(employeeDtos, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedEmployees(employeeDtos, fields, companyId, httpContext, shapedEmployees);


            return ReturnShapedEmployees(shapedEmployees);
        }

        private List<Entity> ShapeData(IEnumerable<EmployeeDto> employeeDto, string fields)
        {
            return _dataShaper.ShapeData(employeeDto, fields)
                .Select(e => e.Entity)
                .ToList();
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"]; 

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private LinkResponse ReturnShapedEmployees(List<Entity> ShapedEmployees)
        {
            return new LinkResponse { ShapedEntities = ShapedEmployees };
        }

        private LinkResponse ReturnLinkdedEmployees(IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext, List<Entity> shapedEmployees)
        {
            var employeeDtoList = employeesDto.ToList();

            for(var i = 0; i < employeeDtoList.Count(); i++)
            {
                var employeeLinks = CreateLinkForEmployee(httpContext, companyId, employeeDtoList[i].Id, fields);
                shapedEmployees[i].Add("Links", employeeLinks);
            }

            var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
            var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
        }

        private List<Link> CreateLinkForEmployee(HttpContext httpContext, Guid companyId, Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany", values: new {companyId, id, fields }), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteEmployeeForCompany", values: new {companyId, id }), "delete_employee", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateEmployeeForCompany", values: new {companyId, id }), "update_employee", "PUT"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateEmployeeForCompany", values: new {companyId, id }), "partially_update_employee", "PATCH")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext, LinkCollectionWrapper<Entity> employeesWrapper)
        {
            employeesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany", values: new { }), "self", "GET"));
            return employeesWrapper;
        }

    }
}
