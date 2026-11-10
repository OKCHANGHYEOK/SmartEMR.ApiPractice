namespace SmartEMR.ApiPractice.Validator;

public interface IBaseValidator<T> where T : class
{
    abstract static ValidateResult Validate(T request);
}
