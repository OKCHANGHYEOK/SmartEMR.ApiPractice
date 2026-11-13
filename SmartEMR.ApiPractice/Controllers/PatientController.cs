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
    public async Task<IActionResult> GetPatients([FromQuery] Patient_Req request)
    {
        var result = await _patientService.GetPatients(request);
        if (result.Items is null || !result.IsSucess)
        {
            return StatusCode(result.StatusCode, new { Error = result.Message });
        }

        return Ok(result.Items);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        var result = await _patientService.GetPatientById(id);
        if (result.Item is null || !result.IsSucess)
        {
            return StatusCode(result.StatusCode, new { Error = result.Message });
        }

        return Ok(result.Item);
    }

    [HttpPost]
    public async Task<IActionResult> SetPatient([FromBody] Patient_Req request)
    {
        var result = await _patientService.SetPatient(request);
        if (result.Item is null || !result.IsSucess)
        {
            return StatusCode(result.StatusCode, new { Error = "환자 등록 중 오류가 발생했습니다. 다시 시도해주세요.", Message = result.Message });
        }

        return CreatedAtAction(
            nameof(SetPatient),
            new { id = result.Item.PAT_Idx },
            result
        );
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient_Req request)
    {
        var result = await _patientService.UpdatePatient(id, request);
        if (result.Item is null || !result.IsSucess)
        {
            return StatusCode(result.StatusCode, new { Error = "환자 수정 중 오류가 발생했습니다. 다시 시도해주세요.", Message = result.Message });
        }

        return Ok(result.Item);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var result = await _patientService.DeletePatient(id);
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
