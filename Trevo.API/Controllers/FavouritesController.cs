using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http.Cors;
using Trevo.API.Models;
using Trevo.Services.FavouriteService;
using Trevo.Core.Model.Favourites;
using System;
using System.Linq;
using System.Collections.Generic;
using Trevo.Core.Model;
using Trevo.Services.UserFollowService;
using Trevo.Core.Model.UserFollower;
using Trevo.Services.UserBlockService;
using Trevo.Core.Model.Block;
using System.Globalization;
using System.Configuration;
using Trevo.Services.TransliterationService;
using Trevo.Core.Model.UserTransliteration;
using Trevo.Services.MomentService;
using Trevo.Services.UserUploadService;
using Trevo.Services.Country;

namespace Trevo.API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class FavouritesController : ApiController
    {
        private readonly IFavouritesService _favService;
        private readonly IUserFollowDetailService _userFollowService;
        private readonly IBlockService _blockservice;
        private readonly ITransliterationService _transService;
        private readonly IMomentsService _momentsService;
        private readonly IUserUploadsService _userUploadsService;
        private readonly IUserTransliterationService _transliterationService;
        private readonly ICountryService _countryService;
        private static string momentUploadImagePath = ConfigurationManager.AppSettings["MomentUploadImagePath"].ToString();
        private static string momentUploadAudioPath = ConfigurationManager.AppSettings["MomentUploadAudioPath"].ToString();
        private static string imagePath = ConfigurationManager.AppSettings["CountryIcon"].ToString();
        private static string noImagePath = ConfigurationManager.AppSettings["NoImagePath"].ToString();
        private static string proImagePath = ConfigurationManager.AppSettings["ProfileImages"].ToString();
        public FavouritesController(IFavouritesService favService, IUserFollowDetailService userFollowService, IBlockService blockservice,
            IUserTransliterationService transliterationService, ITransliterationService transService,
            IMomentsService momentsService, IUserUploadsService userUploadsService,
            ICountryService countryService)
        {
            _favService = favService;
            _userFollowService = userFollowService;
            _blockservice = blockservice;
            _transliterationService = transliterationService;
            _transService = transService;
            _momentsService = momentsService;
            _userUploadsService = userUploadsService;
            _countryService = countryService;
        }

        /// <summary>
        /// Insert Favourites Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertFavouriteDetails")]

        public IHttpActionResult InsertFavouriteDetails(FavouritesModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Sender Id and Reciever Id are required."));
                }

                FavouritesDetails details = new FavouritesDetails();
                details.FavouriteUserId = model.FavouriteUserId;
                details.IsSender = model.IsSender;
                details.Message = model.Message;
                details.MomentId = model.MomentId;
                details.SenderRecieverId = model.SenderRecieverId;
                details.LocalMessageId = model.LocalMessageId;
                var dateTime = DateTime.UtcNow;
                details.AddedDate = dateTime.ToString(@"yyyy/MM/dd hh:mm tt", new CultureInfo("en-US"));
                _favService.InsertFavourites(details);

                UserTransliterationDetails transliterationDetails = _transliterationService.GetTransliterationDetailsByUserId(details.FavouriteUserId);
                //TransliterationDetails transDetails = new TransliterationDetails();
                if (transliterationDetails != null)
                {

                    transliterationDetails.FavouritesCount = transliterationDetails.FavouritesCount + 1;

                    ReturnMsg obj = _transliterationService.UpdateTransliterationByUserId(transliterationDetails);
                }
                else
                {
                    transliterationDetails = new UserTransliterationDetails();

                    transliterationDetails.FavouritesCount = 1;

                    transliterationDetails.User_Id = model.FavouriteUserId;
                    _transliterationService.InsertTransliterationDetails(transliterationDetails);
                }
                //transDetails.IsFavourite = 1;


            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok("Favourites details inserted successfully.");
        }

        /// <summary>
        /// Get favourites List By User Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetfavouritesListByUserId")]

        public IHttpActionResult GetfavouritesListByUserId(RequestModel model)
        {
            List<FavouritesModel> returnObj = new List<FavouritesModel>();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }
                long id = Convert.ToInt64(model.Id);
                var favList = _favService.GetFavouritesListByUserId(id).ToList();
                foreach (var item in favList)
                {
                    FavouritesModel details = new FavouritesModel();
                    details.FavouritesId = item.FavouritesId;
                    details.FavouritesUserName = item.FavouritesUserName;
                    details.FavouriteUserId = item.FavouriteUserId;
                    details.IsSender = item.IsSender;
                    if (item.MomentId==0 && item.Message.Contains(','))
                    {
                        details.IsCorrected = 1;
                        details.IncorrectedText = item.Message.Split(',')[0];
                        details.CorrectedText = item.Message.Split(',')[1];
                    }
                    else
                    {
                        details.Message = item.Message;
                    }
                    details.LocalMessageId =Convert.ToInt32( item.LocalMessageId);
                    details.MomentId = item.MomentId;
                    details.SenderRecieverId = item.SenderRecieverId;
                    details.SenderRecieverName = item.SenderRecieverName;
                    if (!string.IsNullOrEmpty(item.AddedDate))
                    {
                        int index = item.AddedDate.LastIndexOf("/");
                        string date = item.AddedDate.Substring(0,index+3);
                        details.AddedDate = date;
                    }
                    else
                    {
                        details.AddedDate = item.AddedDate;
                    }
                    if (string.IsNullOrEmpty(item.ImagePath))
                    {
                        details.ImagePath = noImagePath;
                    }
                    else
                    {
                        var extension = item.ImagePath.Split('.')[1];
                        var imageName = item.ImagePath.Split('.')[0] + "_thumbnail";
                        details.ImagePath = proImagePath + imageName + "." + extension;
                    }
                    //details.ImagePath = proImagePath + item.ImagePath;
                    var countryDetails = _countryService.GetCountryDetailsById(item.CountryId);
                    if (countryDetails != null)
                    {
                        details.Icon_Path = imagePath + countryDetails.Flag_Icon;
                    }
                    returnObj.Add(details);
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(returnObj);
        }

        /// <summary>
        /// Remove Favourites By User and Moment Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveFavByUserAndMomentId")]


        public IHttpActionResult RemoveFavByUserAndMomentId(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id) || (string.IsNullOrEmpty(model.ScheduleId)))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id and Moment Id are required."));
                }
                long userId = Convert.ToInt64(model.Id);
                long momentId = Convert.ToInt64(model.ScheduleId);
                obj = _favService.DeleteFavByUserAndMomentId(userId, momentId);
            }
            catch (Exception e)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(obj);
        }

        /// <summary>
        /// Insert User Follow Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertUserFollowDetails")]

        public IHttpActionResult InsertUserFollowDetails(UserFollowerModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Follower User Id and Following User Id are required."));
                }
                UserFollowerDetails details = new UserFollowerDetails();
                details.FollowerUserId = model.FollowerUserId;
                details.FollowingUserId = model.FollowingUserId;
                _userFollowService.InsertUserFollowingDetails(details);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok("User Follow Details successfully inserted.");
        }

        /// <summary>
        /// Remove User Follow Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveUserFollowDetails")]

        public IHttpActionResult RemoveUserFollowDetails(UserFollowerModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Follower User Id and Following User Id are required."));
                }
                obj = _userFollowService.RemoveUserFollowDetailsByUserId(model.FollowerUserId, model.FollowingUserId);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }

        /// <summary>
        /// Get User Follower List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserFollowerFollowingList")]

        public IHttpActionResult GetUserFollowerFollowingList(RequestModel model)
        {
            List<UserFollowerModel> objList = new List<UserFollowerModel>();
            List<UserFollowDetailsWithUserName> userList = new List<UserFollowDetailsWithUserName>();
            try
            {
                if (string.IsNullOrEmpty(model.Id) || (string.IsNullOrEmpty(model.ScheduleId)))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please give all the required fields."));
                }
                long userId = Convert.ToInt64(model.Id);
                int isFollowerList = Convert.ToInt32(model.ScheduleId);
                if (isFollowerList == 1)
                {
                    userList = _userFollowService.GetFollowerListByUserId(userId).ToList();
                }
                else
                {
                    userList = _userFollowService.GetFollowingListByUserId(userId).ToList();
                }
                foreach (var item in userList)
                {
                    UserFollowerModel details = new UserFollowerModel();
                    details.FollowerName = item.FollowerName;
                    details.FollowerUserId = item.FollowerUserId;
                    details.FollowingName = item.FollowingName;
                    details.FollowingUserId = item.FollowingUserId;
                    details.UserFollowId = item.UserFollowId;
                    objList.Add(details);
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }


            return Ok(objList);

        }


        /// <summary>
        /// Check If the Users is Following Anothet User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckIfTheUserIsFollowingAnotherUser")]

        public IHttpActionResult CheckIfTheUserIsFollowingAnotherUser(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id) || (string.IsNullOrEmpty(model.ScheduleId)))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please give all the required fields."));
                }
                long followerId = Convert.ToInt64(model.Id);
                long followingId = Convert.ToInt64(model.ScheduleId);
                var userFollowDetails = _userFollowService.GetFollowerListByUserId(followingId).Where(a => a.FollowerUserId == followerId).ToList();
                if (userFollowDetails.Count > 0)
                {
                    obj.IsSuccess = true;
                }
                else
                {
                    obj.IsSuccess = false;
                }
            }
            catch (Exception e)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }


        /// <summary>
        /// Insert Block Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertBlockDetails")]

        public IHttpActionResult InsertBlockDetails(RequestModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Id) || (string.IsNullOrEmpty(model.ScheduleId)))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Blocking User Id and Blocked User Id are required."));
                }

                UserBlockDetails details = new UserBlockDetails();
                details.BlockedUserId = Convert.ToInt64(model.ScheduleId);
                details.BlockingUserId = Convert.ToInt64(model.Id);

                UserFollowerDetails userFollowDetails = new UserFollowerDetails();
                var userFollowList = _userFollowService.GetAllUserFollowList().Where(a => a.FollowerUserId == details.BlockedUserId && a.FollowingUserId == details.BlockingUserId).ToList();
                foreach (var item in userFollowList)
                {
                    _userFollowService.RemoveUserFollowDetailsByUserId(item.FollowerUserId, item.FollowingUserId);
                }
                userFollowList = _userFollowService.GetAllUserFollowList().Where(a => a.FollowerUserId == details.BlockingUserId && a.FollowingUserId == details.BlockedUserId).ToList();
                foreach (var item in userFollowList)
                {
                    _userFollowService.RemoveUserFollowDetailsByUserId(item.FollowerUserId, item.FollowingUserId);
                }
                var dateTime = DateTime.UtcNow;
                details.BlockingTime = dateTime.ToString(@"yyyy/MM/dd hh:mm tt", new CultureInfo("en-US"));
                _blockservice.InsertBlockDetails(details);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok("Block details inserted successfully.");
        }

        /// <summary>
        /// Delete Block Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteBlockDetails")]

        public IHttpActionResult DeleteBlockDetails(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id) || (string.IsNullOrEmpty(model.ScheduleId)))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Blocking User Id and Blocked User Id are required."));
                }

                long blockingUserId = Convert.ToInt64(model.Id);
                long blockedUserId = Convert.ToInt64(model.ScheduleId);
                obj = _blockservice.DeleteBlockDetails(blockingUserId, blockedUserId);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(obj);
        }

        /// <summary>
        /// Get Blocked User List By User Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetBlockedUserListByUserId")]

        public IHttpActionResult GetBlockedUserListByUserId(RequestModel model)

        {
            List<UserBlockWithAllInfo> blockedUserList = new List<UserBlockWithAllInfo>();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User Id is required."));
                }
                long userId = Convert.ToInt64(model.Id);
                blockedUserList = _blockservice.GetBlockedUserListByUserId(userId).ToList();
                foreach (var item in blockedUserList)
                {
                    if (string.IsNullOrEmpty(item.ImagePath))
                    {
                        item.ImagePath = noImagePath;
                    }
                    else
                    {
                        var extension = item.ImagePath.Split('.')[1];
                        var imageName = item.ImagePath.Split('.')[0] + "_thumbnail";
                        item.ImagePath = proImagePath + imageName + "." + extension;
                    }
                    //item.ImagePath = item.ImagePath == null ? noImagePath : item.ImagePath == string.Empty ? noImagePath : proImagePath + item.ImagePath;
                    item.Flag_Icon = item.Flag_Icon == null ? noImagePath : item.Flag_Icon == string.Empty ? noImagePath : imagePath + item.Flag_Icon;
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(blockedUserList);
        }

        /// <summary>
        /// Insert UserTransliteration Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]

        [Route("InsertUserTransliterationDetails")]

        public IHttpActionResult InsertUserTransliterationDetails(TransliterationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User Id is required."));
                }

                UserTransliterationDetails details = _transliterationService.GetTransliterationDetailsByUserId(model.User_Id);
                //TransliterationDetails transDetails = new TransliterationDetails();
                if (details != null)
                {
                    if (model.IsSpellCheck == 1)
                    {
                        details.SpellCheckCount = details.SpellCheckCount + 1;
                        // transDetails.IsSpellCheck = 1;
                    }
                    else if (model.IsTranslate == 1)
                    {
                        details.TranslateCount = details.TranslateCount + 1;
                        //transDetails.IsTranslate = 1;
                    }
                    else if (model.IsTTS == 1)
                    {
                        details.TTSCount = details.TTSCount + 1;
                        //transDetails.IsTTS = 1;
                    }
                    else
                    {
                        details.FavouritesCount = details.FavouritesCount + 1;
                        //transDetails.IsFavourite = 1;
                        //transDetails.IsMoment = 0;
                    }
                    ReturnMsg obj = _transliterationService.UpdateTransliterationByUserId(details);
                    if (!obj.IsSuccess)
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is some error."));
                    }
                    else
                    {
                        //transDetails.Details = model.Details;
                        //transDetails.User_Id = model.User_Id;
                        //_transService.InsertTransliteration(transDetails);
                    }
                }
                else
                {
                    details = new UserTransliterationDetails();
                    if (model.IsSpellCheck == 1)
                    {
                        details.SpellCheckCount = 1;
                        // transDetails.IsSpellCheck = 1;
                    }
                    else if (model.IsTranslate == 1)
                    {
                        details.TranslateCount = 1;
                        //transDetails.IsTranslate = 1;
                    }
                    else if (model.IsTTS == 1)
                    {
                        details.TTSCount = 1;
                        //transDetails.IsTTS = 1;
                    }
                    else
                    {
                        details.FavouritesCount = 1;
                        //transDetails.IsFavourite = 1;
                        // transDetails.IsMoment = 0;
                    }
                    //transDetails.Details = model.Details;
                    // transDetails.User_Id = model.User_Id;
                    details.User_Id = model.User_Id;
                    _transliterationService.InsertTransliterationDetails(details);
                    // _transService.InsertTransliteration(transDetails);
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok("Details inserted successfully.");
        }

        /// <summary>
        /// Get Transliteration List By User Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTransliterationListByUserId")]

        public IHttpActionResult GetTransliterationListByUserId(TransliterationRequestModel model)
        {
            List<TransliterationModel> objList = new List<TransliterationModel>();
            try
            {
                var transList = _transService.GetTransliterationDetailsByUserId(model.User_Id);
                if (model.IsTTS == 1)
                {
                    transList = transList.Where(a => a.IsTTS == 1).ToList();
                }
                else if (model.IsSpellCheck == 1)
                {
                    transList = transList.Where(a => a.IsSpellCheck == 1).ToList();
                }
                else if (model.IsTranslate == 1)
                {
                    transList = transList.Where(a => a.IsTranslate == 1).ToList();
                }
                else if (model.IsFavourite == 1)
                {
                    transList = transList.Where(a => a.IsFavourite == 1).ToList();
                }
                foreach (var item in transList)
                {
                    TransliterationModel obj = new TransliterationModel();
                    obj.User_Id = item.User_Id;
                    obj.Details = item.Details;
                    if (item.IsFavourite == 1 && item.IsMoment == 1)
                    {
                        long momentId = Convert.ToInt64(item.Details);
                        var momentDetails = _momentsService.GetMomentDetailsById(momentId);
                        if (momentDetails != null && momentDetails.UserUploadedId > 0)
                        {
                            var userUploadedDetails = _userUploadsService.GetUserUploadsById(momentDetails.UserUploadedId);
                            if (userUploadedDetails != null)
                            {
                                obj.UploadedImagePath = userUploadedDetails.UploadedImagePath == null ? string.Empty : momentUploadImagePath + userUploadedDetails.UploadedImagePath;
                                obj.UploadedAudioPath = userUploadedDetails.UploadedAudioPath == null ? string.Empty : momentUploadAudioPath + userUploadedDetails.UploadedAudioPath;
                            }
                        }
                        else if (momentDetails != null)
                        {
                            obj.Message = momentDetails.Message;
                        }
                    }
                    if (string.IsNullOrEmpty(item.ImagePath))
                    {
                        obj.ImagePath = noImagePath;
                    }
                    else
                    {
                        var extension = item.ImagePath.Split('.')[1];
                        var imageName = item.ImagePath.Split('.')[0] + "_thumbnail";
                        obj.ImagePath = proImagePath + imageName + "." + extension;
                    }
                    // obj.ImagePath = item.ImagePath == null ? noImagePath : item.ImagePath == string.Empty ? noImagePath : proImagePath + item.ImagePath;
                    obj.Flag_Icon = imagePath + item.Flag_Icon;
                    objList.Add(obj);
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(objList);
        }


        /// <summary>
        /// Delete Favorites By Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteFavoritesById")]
        public IHttpActionResult DeleteFavoritesById(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Favorites Id is required."));
                }
                long id = Convert.ToInt64(model.Id);
                obj = _favService.DeleteFavoritesById(id);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(obj);
        }

    }
}
