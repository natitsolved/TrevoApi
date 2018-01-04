using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Trevo.API.Models;
using Trevo.Core.Model.User;
using Trevo.Services.Users;
using System.Linq;
using System.Threading.Tasks;
using Trevo.Core.Model;
using System.Web;
using System.IO;
using Trevo.API.Helper;
using Trevo.Services.Language;
using Trevo.Services.Country;
using Trevo.Services.HobbyService;
using Trevo.Core.Model.Hobbies;
using Trevo.Services.UserBlockService;
using Trevo.Services.TransliterationService;
using Trevo.Core.Model.UserTransliteration;

namespace Trevo.API.Controllers.User
{
    //[RoutePrefix("Api")]
    [EnableCors("*", "*", "*")]
    public class UserController : ApiController
    {
        private readonly IUserService _UserService;
        private readonly ILanguageService _languageService;
        private readonly ICountryService _countryService;
        private readonly IUserLanguageService _userLangService;
        private readonly IHobbiesService _hobbiesService;
        private readonly IBlockService _blockService;
        private readonly IUserTransliterationService _transliterationService;
        private static string imagePath = ConfigurationManager.AppSettings["CountryIcon"].ToString();
        private static string noImagePath = ConfigurationManager.AppSettings["NoImagePath"].ToString();
        private static string proImagePath = ConfigurationManager.AppSettings["ProfileImages"].ToString();
        public UserController(IUserService userService, ILanguageService languageService, ICountryService countryService,
            IUserLanguageService userLangService, IHobbiesService hobbiesService, IBlockService blockService,
            IUserTransliterationService transliterationService)
        {
            _UserService = userService;
            _languageService = languageService;
            _countryService = countryService;
            _userLangService = userLangService;
            _hobbiesService = hobbiesService;
            _blockService = blockService;
            _transliterationService = transliterationService;
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllUser")]

        public IHttpActionResult GetAllUsers()
        {
            IList<TrevoUsers> userList = new List<TrevoUsers>();
            try
            {
                userList = _UserService.GetAllUsers();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(userList);
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllUserWithCountryIcon")]

        public IHttpActionResult GetAllUserWithCountryIcon(RequestModel model)
        {
            IList<UserWithCountryIconInfo> obj = new List<UserWithCountryIconInfo>();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User Id is required."));
                }
                long userID = Convert.ToInt64(model.Id);
                var userList = _UserService.GetAllUsersWithCountryFlag();
                foreach (var item in userList)
                {
                    bool isAdd = false;
                    var blockList = _blockService.GetBlockedUserListByUserId(userID).ToList();
                    if (blockList.Count > 0)
                    {
                        var blockDetails = blockList.Where(a => a.BlockedUserId == item.User_Id).FirstOrDefault();
                        if (blockDetails == null)
                        {
                            isAdd = true;
                        }
                    }
                    else
                    {
                        blockList = _blockService.GetBlockedUserListByUserId(item.User_Id).ToList();
                        if (blockList.Count > 0)
                        {
                            var blockDetails = blockList.Where(a => a.BlockedUserId == userID).FirstOrDefault();
                            if (blockDetails == null)
                            {
                                isAdd = true;
                            }
                        }
                        else
                        {
                            isAdd = true;
                        }
                    }
                    if (isAdd)
                    {
                        UserWithCountryIconInfo details = new UserWithCountryIconInfo();
                        details.Address = item.Address;
                        details.Country_Id = item.Country_Id;
                        details.CreatedTime = item.CreatedTime;
                        details.DeviceId = item.DeviceId;
                        details.Dob = item.Dob;
                        details.Email_Id = item.Email_Id;
                        details.Flag_Icon = item.Flag_Icon;
                        details.Gender = item.Gender;
                        details.Icon_Path = imagePath + item.Flag_Icon;
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
                        //details.ImagePath = item.ImagePath == null ?  noImagePath: item.ImagePath == string.Empty ? noImagePath: proImagePath + item.ImagePath;
                        details.Interests = item.Interests;
                        details.IsVerified = item.IsVerified;
                        details.LagLevel_ID = item.LagLevel_ID;
                        details.Name = item.Name;
                        details.QR_Code = item.QR_Code;
                        details.Self_Introduction = item.Self_Introduction;
                        details.TravelDestination_CId = item.TravelDestination_CId;
                        details.TrevoId = item.TrevoId;
                        details.User_Id = item.User_Id;
                        var languageDetails = _languageService.GetLanguageDetailsById(item.Native_LanguageId);
                        if (languageDetails != null)
                        {
                            details.NativeAbbrv = languageDetails.Abbreviation;
                        }
                        languageDetails = _languageService.GetLanguageDetailsById(item.Learning_LanguageId);
                        if (languageDetails != null)
                        {
                            details.LearningAbbrv = languageDetails.Abbreviation;
                        }
                        obj.Add(details);
                    }
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(obj);
        }

        /// <summary>
        /// Get Users List with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllUsersWithPagingBasedOnNativeLang")]

        public IHttpActionResult GetAllUsersWithPagingBasedOnNativeLang(RequestModel model)
        {

            List<TrevoUsers> userList = new List<TrevoUsers>();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Native Language is required."));
                }
                long nativeLangId = Convert.ToInt64(model.Id);
                userList = _UserService.GetAllUsersWithNativeLangId(nativeLangId).ToList();
                if (model.pageNo == 1)
                {

                    userList = userList.Take(model.pageSize).ToList();

                }
                else
                {
                    model.pageNo = model.pageNo - 1;
                    userList = userList.Skip(model.pageNo * model.pageSize).Take(model.pageSize).ToList();
                }


            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(userList);
        }

        /// <summary>
        /// Upload user's profile image by user Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadUserProfileImage")]

        public async Task<IHttpActionResult> UploadUserProfileImage()
        {
            ReturnMsg msg = new ReturnMsg();
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Content(HttpStatusCode.BadRequest, "Unsupported media type.");
            }
            try
            {
                string root = HttpContext.Current.Server.MapPath("~/ProfileImages");
                var provider = new MultipartFormDataStreamProvider(root);

                await Request.Content.ReadAsMultipartAsync(provider);
                string StoragePath = HttpContext.Current.Server.MapPath("~/ProfileImages/OriginalSize");

                TrevoUsers userDetails = new TrevoUsers();
                if (provider.FormData.Count == 0)
                {
                    File.Delete(provider.FileData[0].LocalFileName);
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }
                long userId = Convert.ToInt64(provider.FormData.GetValues(0)[0]);
                //userDetails.UserID =provider.FormData.GetValues(0)[0];

                //moving the file to the required destination
                string temp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileName1 = string.Empty;
                string path1 = string.Empty;
                foreach (MultipartFileData fileData in provider.FileData)
                {
                    string fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }
                    string extension = fileName.Split('.')[1];
                    if (extension.ToLower() != "jpg" && extension.ToLower() != "jpeg" && extension.ToLower() != "png")
                    {
                        File.Delete(fileData.LocalFileName);
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Please upload a file with extension in jpg or jpeg or png."));
                    }
                    fileName1 = fileName.Split('.')[0] + "_" + temp + "." + extension;
                    path1 = Path.Combine(StoragePath, fileName1);
                    File.Move(fileData.LocalFileName, path1);
                    File.Delete(fileData.LocalFileName);

                    //saving the file in medium size;
                    int width = 0;
                    string folderName = string.Empty;
                    width = 400;
                    //Saving the medium size
                    folderName = "ProfileImages";
                    ImageUploadHelper imageUpload = new ImageUploadHelper { Width = width };
                    ImageResult imageResult = imageUpload.RenameUploadFile(fileData, true, "", fileName1, folderName);

                    //saving the file in thumbnail size

                    width = 150;
                    imageUpload = new ImageUploadHelper { Width = width };
                    imageResult = imageUpload.RenameUploadFile(fileData, false, "", fileName1, folderName);
                }

                var user = _UserService.GetUserDetailsById(userId);

                if (user != null)
                {

                    msg = _UserService.UpdateUserProfileImage(fileName1, user.User_Id);

                    if (!msg.IsSuccess)
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "There is some error"));
                    }
                    else
                    {

                        msg.IsSuccess = true;
                        msg.Message = "Your profile image is successfully updated.";
                    }


                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "User not found."));
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, e.Message));
            }
            return Ok(msg);
        }


        /// <summary>
        /// Get User Details by External Auth Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserDetailsByExternalAuthId")]

        public IHttpActionResult GetUserDetailsByExternalAuthId(RequestModel model)
        {
            TrevoUsers userDetails = new TrevoUsers();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "External User Id is required."));
                }
                userDetails = _UserService.GetUserDetailsByExternalAuthId(model.Id);
                if (userDetails == null)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User not found."));
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(userDetails);
        }


        /// <summary>
        /// update User's Self Introduction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateUserSelfIntroduction")]

        public IHttpActionResult UpdateUserSelfIntroduction(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }

                long userId = Convert.ToInt64(model.Id);
                var userDetails = _UserService.GetUserDetailsById(userId);
                if (userDetails != null)
                {
                    model.Content = model.Content == null ? string.Empty : model.Content;
                    obj = _UserService.UpdateUserSelfIntroduction(model.Content, userId);
                    if (obj.IsSuccess)
                    {
                        obj.Message = "Introduction updated successfully.";
                    }
                    else
                    {
                        obj.Message = "Sorry!! There is some error. Please try again later.";
                    }
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "No records found to update."));
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }

        /// <summary>
        /// update User's Name
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateUserName")]

        public IHttpActionResult UpdateUserName(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }

                long userId = Convert.ToInt64(model.Id);
                var userDetails = _UserService.GetUserDetailsById(userId);
                if (userDetails != null)
                {
                    model.Content = model.Content == null ? string.Empty : model.Content;
                    obj = _UserService.UpdateUserName(model.Content, userId);
                    if (obj.IsSuccess)
                    {
                        obj.Message = "Name updated successfully.";
                    }
                    else
                    {
                        obj.Message = "Sorry!! There is some error. Please try again later.";
                    }
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "No records found to update."));
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }

        /// <summary>
        /// Update Users's Address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateUserAddress")]

        public IHttpActionResult UpdateUserAddress(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }

                long userId = Convert.ToInt64(model.Id);
                var userDetails = _UserService.GetUserDetailsById(userId);
                if (userDetails != null)
                {
                    model.Content = model.Content == null ? string.Empty : model.Content;
                    obj = _UserService.UpdateUserAddress(model.Content, userId);
                    if (obj.IsSuccess)
                    {
                        obj.Message = "Address updated successfully.";
                    }
                    else
                    {
                        obj.Message = "Sorry!! There is some error. Please try again later.";
                    }
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "No records found to update."));
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }

        /// <summary>
        /// Get User Details By Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserDetailsById")]
        public IHttpActionResult GetUserDetailsById(RequestModel model)
        {
            UserWithCountryIconInfo userDetails = new UserWithCountryIconInfo();
            try
            {
                List<HobbiesDetails> hobbiesList = new List<HobbiesDetails>();
                hobbiesList = _hobbiesService.GetAllHobbies();
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }
                long userId = Convert.ToInt64(model.Id);
                var details = _UserService.GetUserDetailsById(userId);
                if (details != null)
                {
                    userDetails.Address = details.Address;
                    userDetails.Country_Id = details.Country_Id;
                    userDetails.CreatedTime = details.CreatedTime;
                    userDetails.DeviceId = details.DeviceId;
                    userDetails.Dob = details.Dob;
                    userDetails.Email_Id = details.Email_Id;
                    var countryDetails = _countryService.GetCountryDetailsById(details.Country_Id);
                    if (countryDetails != null)
                    {
                        userDetails.Flag_Icon = countryDetails.Flag_Icon;
                    }
                    userDetails.Gender = details.Gender;
                    userDetails.Icon_Path = imagePath + countryDetails.Flag_Icon;
                }
                //userDetails.ImagePath = details.ImagePath == null ? noImagePath : details.ImagePath == string.Empty ? noImagePath : proImagePath + details.ImagePath;
                if (string.IsNullOrEmpty(details.ImagePath))
                {
                    userDetails.ImagePath = noImagePath;
                }
                else
                {
                    var extension = details.ImagePath.Split('.')[1];
                    var imageName = details.ImagePath.Split('.')[0] + "_thumbnail";
                    userDetails.ImagePath = proImagePath + imageName + "." + extension;
                }
                userDetails.Interests = details.Interests;
                userDetails.IsVerified = details.IsVerified;
                userDetails.LagLevel_ID = details.LagLevel_ID;
                var userLangDetails = _userLangService.GetLanguageListByUserId(details.User_Id);
                if (userLangDetails != null)
                {
                    var langDetails = _languageService.GetLanguageDetailsById(userLangDetails.Native_LanguageId);
                    userDetails.Native_LanguageId = userLangDetails.Native_LanguageId;
                    if (langDetails != null)
                    {
                        userDetails.NativeAbbrv = langDetails.Abbreviation;
                    }
                    langDetails = _languageService.GetLanguageDetailsById(userLangDetails.Learning_LanguageId);
                    userDetails.Learning_LanguageId = userLangDetails.Learning_LanguageId;
                    if (langDetails != null)
                    {
                        userDetails.LearningAbbrv = langDetails.Abbreviation;
                    }
                }
                userDetails.Name = details.Name;
                userDetails.Self_Introduction = details.Self_Introduction;
                userDetails.TravelDestination_CId = details.TravelDestination_CId;
                userDetails.TrevoId = details.TrevoId;
                userDetails.User_Id = details.User_Id;

                var userHobbies = _hobbiesService.GetUserHobbiesByUserId(details.User_Id);
                foreach (var item in userHobbies)
                {
                    if (string.IsNullOrEmpty(userDetails.UserHobbies))
                    {
                        userDetails.UserHobbies = hobbiesList.Where(a => a.HobbiesId == item.HobbiesId).FirstOrDefault().Name;
                    }
                    else
                    {

                        userDetails.UserHobbies = userDetails.UserHobbies + " ," + hobbiesList.Where(a => a.HobbiesId == item.HobbiesId).FirstOrDefault().Name;
                    }
                }

                UserTransliterationDetails transliterationDetails = _transliterationService.GetTransliterationDetailsByUserId(userDetails.User_Id);
                if (transliterationDetails != null)
                {
                    userDetails.FavCount = transliterationDetails.FavouritesCount;
                    userDetails.TTSCount = transliterationDetails.TTSCount;
                    userDetails.SpellCheckCount = transliterationDetails.SpellCheckCount;
                    userDetails.TranslateCount = transliterationDetails.TranslateCount;
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(userDetails);
        }

        /// <summary>
        /// Insert Hobbies Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertHobbies")]


        public IHttpActionResult InsertHobbies(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Content))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Name is required."));
                }
                HobbiesDetails details = new HobbiesDetails();
                details.Name = model.Content;
                _hobbiesService.InsertHobbiesDetails(details);
                obj.IsSuccess = true;
                obj.Message = "Hobbies Details inserted successfully.";
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }

        /// <summary>
        /// Get All Hobbies List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllHobbies")]


        public IHttpActionResult GetAllHobbies()
        {
            List<HobbiesDetails> hobbiesList = new List<HobbiesDetails>();
            try
            {
                hobbiesList = _hobbiesService.GetAllHobbies();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(hobbiesList);
        }

        /// <summary>
        /// Insert User Hobbies Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertUserHobbies")]

        public IHttpActionResult InsertUserHobbies(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }

                long userId = Convert.ToInt64(model.Id);
                ReturnMsg det = _hobbiesService.DeleteUserHobbiesByUserId(userId);
                if (string.IsNullOrEmpty(model.ScheduleId))
                {
                }
                else
                {
                    List<long> hobbiesList = new List<long>();
                    var sceduleIds = model.ScheduleId.Split(',').ToList();
                    foreach (var item in sceduleIds)
                    {
                        hobbiesList.Add(Convert.ToInt64(item));
                    }
                    if (det.IsSuccess)
                    {
                        foreach (var item in hobbiesList)
                        {
                            UserHobbiesDetails details = new UserHobbiesDetails();
                            details.HobbiesId = item;
                            details.User_Id = userId;
                            _hobbiesService.InsertUserHobbiesDetails(details);
                        }
                        obj.IsSuccess = true;
                        obj.Message = "User Hobbies is inserted successfully.";
                    }
                }

            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }


        /// <summary>
        /// Delete User Details By Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteUserDetailsById")]
        public IHttpActionResult DeleteUserDetailsById(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }
                long userId = Convert.ToInt64(model.Id);
                _UserService.DeleteUserDetails(userId);
                obj.IsSuccess = true;
                obj.Message = "User deleted successfully.";
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
            return Ok(obj);
        }
    }
}