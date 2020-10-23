using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class CreateCompanyDto : CompanyManipulationDto
    {
        public IEnumerable<CreateEmployeeDto> Employees { get; set; }
    }
}
