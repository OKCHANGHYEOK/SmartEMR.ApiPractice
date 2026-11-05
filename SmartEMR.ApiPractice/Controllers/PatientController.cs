using Microsoft.AspNetCore.Mvc;
using SmartEMR.ApiPractice.Entities;
using SmartEMR.ApiPractice.Services;
using SmartEMR.ApiPractice.Validator;

namespace SmartEMR.ApiPractice.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService PatientService)
    {
        _patientService = PatientService;
    }

    [HttpGet]
    public IActionResult GetPatients()
    {
        return Ok(_patientService.GetPatients());
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetPatientById(int id)
    {
        var targetPatient = _patientService.GetPatientById(id);
        if (targetPatient is null)
        {
            return StatusCode(404, new { Error = "해당하는 환자가 존재하지 않습니다." });
        }

        return Ok(targetPatient);
    }

    [HttpPost]
    public IActionResult SetPatient([FromBody] Patient_Req request)
    {
        var validate = PatientValidator.Validate(request);
        if (!validate.IsSuccess)
        {
            return StatusCode(400, validate.Message);
        }

        var result = _patientService.SetPatient(request);
        if (result is null)
        {
            return StatusCode(500, new { Error = "환자 등록 중 오류가 발생했습니다. 다시 시도해주세요." });
        }

        return CreatedAtAction(
            nameof(GetPatientById),
            new { id = result.PatientId },
            result
        );
    }
}
