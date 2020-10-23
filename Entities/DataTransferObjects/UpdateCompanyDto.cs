using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class UpdateCompanyDto : CompanyManipulationDto
    {
        public IEnumerable<CreateEmployeeDto> Employees { get; set; }
    }
}
