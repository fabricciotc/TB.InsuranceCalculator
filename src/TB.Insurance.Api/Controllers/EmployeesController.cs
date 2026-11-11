using Microsoft.AspNetCore.Mvc;
using TB.Insurance.Application.Employees;

namespace TB.Insurance.Api.Controllers;

[ApiController]
[Route("api/employees")]
public sealed class EmployeesController(SaveEmployeeHandler saveEmployee) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<SaveEmployeeResponse>> SaveEmployeeData(
        [FromBody] SaveEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var response = await saveEmployee.HandleAsync(request, cancellationToken);

        return Ok(response);
    }
}
