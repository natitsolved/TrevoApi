using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Core.Model.MomentDetails;

namespace Trevo.Services.MomentService
{
    public interface IMomentsService
    {
        bool InsertMomentDetails(Moments details);
        ReturnMsg UpdateMomentDetails(Moments details);
        Moments GetMomentDetailsById(long momentId);
        ReturnMsg DeleteMoments(long momentId);
        List<MomentDetailsWithImage> GetAllMoments();
        List<MomentDetailsWithImage> GetAllMomentsLIstByParentId(long id);
        List<MomentDetailsWithImage> GetMomentsListByUserId(long id);
        List<MomentDetailsWithImage> GetMomentListByNativeLearnLang(string nativeLang, string learningLang);
        List<MomentDetailsWithImage> GetMomentsListByFollowerUserId(long id);
        List<MomentDetailsWithLang> GetMomentListByUserIdForExchange(long nativeId, long learningId);
    }
}
