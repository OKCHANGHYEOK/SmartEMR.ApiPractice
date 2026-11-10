using SmartEMR.ApiPractice.Common;
using SmartEMR.ApiPractice.DTOs;
using SmartEMR.ApiPractice.Entities;
using SmartEMR.ApiPractice.Repository;
using SmartEMR.ApiPractice.Validator;

namespace SmartEMR.ApiPractice.Services;

public partial class ReservationService : IReservationService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IReservationRepository _reservationRepository;

    public ReservationService(IPatientRepository patientRepository, IReservationRepository reservationRepository)
    {
        _patientRepository = patientRepository;
        _reservationRepository = reservationRepository;
    }

    public ServiceResult<Reservation_Res> GetReservations()
    {
        var result = new ServiceResult<Reservation_Res>();
        var ret = _reservationRepository.GetReservations();

        if (ret is null)
        {
            result.Message = "예약내역이 존재하지 않습니다.";
            result.StatusCode = 404;

            return result;
        }

        var items = ClassMapper<Reservation, Reservation_Res>.MapToList(ret);

        result.Items = items;
        result.IsSucess = true;

        return result;
    }

    public ServiceResult<Reservation_Res> SetReservation(Reservation_Req request)
    {
        var result = new ServiceResult<Reservation_Res>();
        var validateResult = ReservationValidator.Validate(request);

        if (!validateResult.IsSuccess)
        {
            result.StatusCode = 400;
            result.Message = validateResult.Message;

            return result;
        }

        var verifyResult = VerifySaveData(request);
        if (verifyResult != 200)
        {
            result.StatusCode = verifyResult;
            result.Message = verifyResult switch
            {
                400 => "예약일자는 현재보다 과거일 수 없습니다.",
                404 => "해당하는 환자가 존재하지 않습니다.",
                409 => "해당일시에 이미 예약이 존재합니다.",
                _ => "알 수 없는 오류가 발생했습니다."
            };

            return result;
        }

        var item = ClassMapper<Reservation_Req, Reservation>.MapTo(request);
        var ret =_reservationRepository.SetReservation(item);
        if (ret is null)
        {
            result.Message = "예약 등록 중 오류가 발생했습니다.";
            result.StatusCode = 400;

            return result;
        }

        result.Item = ClassMapper<Reservation, Reservation_Res>.MapTo(ret);
        result.IsSucess = true;

        return result;
    }

    private int VerifySaveData(Reservation_Req request)
    {
        var retPAT = _patientRepository.GetPatientById(request.PAT_Idx.GetValueOrDefault(0));
        if (retPAT is null)
        {
            return 404;
        }

        if (request.RES_Date < DateTime.Now)
        {
            return 400;
        }

        var retRES = _reservationRepository.GetReservation(new Reservation { PAT_Idx = request.PAT_Idx, RES_Date =request.RES_Date });
        if (retRES != null)
        {
            return 409;
        }

        return 200;
    }
}