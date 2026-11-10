using SmartEMR.ApiPractice.DTOs;

namespace SmartEMR.ApiPractice.Validator;

public class PatientValidator : IBaseValidator<Patient_Req>
{
    private static readonly List<string> _sexes = ["M", "F"];

    public static ValidateResult Validate(Patient_Req request)
    {
        var result = new ValidateResult();

        if (string.IsNullOrWhiteSpace(request.PAT_Name))
        {
            result.Message = "이름은 필수 입력값입니다.";
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.PAT_Sex))
        {
            result.Message = "성별은 필수 입력값입니다.";
            return result;
        }

        if (!IsValidPAT_Sex(request.PAT_Sex))
        {
            result.Message = "성별 입력값은 다음 둘 중 하나여야 합니다. [M, F]";
            return result;
        }

        result.IsSuccess = true;

        return result;
    }

    private static bool IsValidPAT_Sex(string PAT_Sex)
    {
        return _sexes.Contains(PAT_Sex);
    }
}
