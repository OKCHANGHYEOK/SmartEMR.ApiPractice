using SmartEMR.ApiPractice.Entities;

namespace SmartEMR.ApiPractice.Validator;

public class PatientValidator
{
    private static readonly List<string> _genders = ["M", "F"];

    public static ValidateResult Validate(Patient_Req request)
    {
        var result = new ValidateResult() { IsSuccess = false };

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            result.Message = "이름은 필수 입력값입니다.";
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.Gender))
        {
            result.Message = "성별은 필수 입력값입니다.";
            return result;
        }

        if (!IsValidGender(request.Gender))
        {
            result.Message = "성별 입력값은 다음 둘 중 하나여야 합니다. [M, F]";
            return result;
        }

        result.IsSuccess = true;

        return result;
    }

    private static bool IsValidGender(string gender)
    {
        return _genders.Contains(gender);
    }
}
