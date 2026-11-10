using SmartEMR.ApiPractice.DTOs;

namespace SmartEMR.ApiPractice.Validator;

public class ReservationValidator : IBaseValidator<Reservation_Req>
{
    public static ValidateResult Validate(Reservation_Req request)
    {
        var result = new ValidateResult();

        if (request.PAT_Idx.GetValueOrDefault(0) == 0)
        {
            result.Message = "환자키는 필수 입력값입니다.";
            return result;
        }

        result.IsSuccess = true;
        
        return result;
    }
}
