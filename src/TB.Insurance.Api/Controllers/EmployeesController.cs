using Microsoft.AspNetCore.Mvc;
using TB.Insurance.Application.Employees;

namespace TB.Insurance.Api.Controllers;

[ApiController]
[Route("api/employees")]
[Produces("application/json")]
public sealed class EmployeesController(SaveEmployeeHandler saveEmployee) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<SaveEmployeeResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<SaveEmployeeResponse>> SaveEmployeeData(
        [FromBody] SaveEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var response = await saveEmployee.HandleAsync(request, cancellationToken);

        return CreatedAtAction(nameof(SaveEmployeeData), new { employeeId = response.EmployeeId }, response);
    }
}
