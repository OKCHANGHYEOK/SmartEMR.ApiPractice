using Microsoft.AspNetCore.Mvc;
using SmartEMR.ApiPractice.DTOs;
using SmartEMR.ApiPractice.Services;

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
        var result = _patientService.GetPatients();
        if (result.Items is null || !result.IsSucess)
        {
            return StatusCode(result.StatusCode, new { Error = result.Message });
        }

        return Ok(_patientService.GetPatients());
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetPatientById(int id)
    {
        var result = _patientService.GetPatientById(id);
        if (result.Item is null || !result.IsSucess)
        {
            return StatusCode(result.StatusCode, new { Error = result.Message });
        }

        return Ok(result.Item);
    }

    [HttpPost]
    public IActionResult SetPatient([FromBody] Patient_Req request)
    {
        var result = _patientService.SetPatient(request);
        if (result.Item is null || !result.IsSucess)
        {
            return StatusCode(result.StatusCode, new { Error = "환자 등록 중 오류가 발생했습니다. 다시 시도해주세요.", Message = result.Message });
        }

        return CreatedAtAction(
            nameof(GetPatientById),
            new { id = result.Item.PAT_Idx },
            result
        );
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdatePatient(int id, [FromBody] Patient_Req request)
    {
        var result = _patientService.UpdatePatient(id, request);
        if (result.Item is null || !result.IsSucess)
        {
            return StatusCode(result.StatusCode, new { Error = "환자 수정 중 오류가 발생했습니다. 다시 시도해주세요.", Message = result.Message });
        }

        return Ok(result.Item);
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeletePatient(int id)
    {
        var result = _patientService.DeletePatient(id);
        if (!result.IsSucess)
        {
            var response = new
            {
                Error = "환자 삭제에 실패했습니다.",
                result.Message
            };

            return result.StatusCode switch
            {
                404 => NotFound(response),
                409 => Conflict(response),
                _ => StatusCode(result.StatusCode, response)
            };
        }

        return NoContent();
    }
}
