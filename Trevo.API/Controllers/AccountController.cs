using Microsoft.AspNet.Identity;
using Trevo.API.Helper;
using Trevo.API.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using Trevo.Core.Model;
using Trevo.Core.Model.User;
using Trevo.Services.Users;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Threading.Tasks;
using Trevo.Services.Language;
using Trevo.Services.FavouriteService;
using System.Collections.Generic;
using Trevo.Services.UserFollowService;
using Trevo.Services.Country;
using Trevo.Core.Model.Language;
using Trevo.Core.Model.Country;

namespace Trevo.API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AccountController : ApiController
    {
        private readonly string InfoMail = ConfigurationManager.AppSettings["InfoMail"].Trim();
        private static string imagePath = ConfigurationManager.AppSettings["ChatUploads"].ToString();
        private static string countryIconPath = ConfigurationManager.AppSettings["CountryIcon"].ToString();
        private static string videoPath = ConfigurationManager.AppSettings["ChatVideoUploads"].ToString();
        private static string proImagePath = ConfigurationManager.AppSettings["ProfileImages"].ToString();
        private static string noImagePath = ConfigurationManager.AppSettings["NoImagePath"].ToString();
        private readonly static List<string> validFormats = new List<string>() { "mp4", "jpg", "png", "jpeg" };
        private readonly static List<string> imageFormats = new List<string>() { "jpg", "png", "jpeg" };
        private readonly static List<string> videoFormats = new List<string>() { "mp4" };
        private readonly IUserService _userService;
        private readonly IUserLanguageService _userLanguageService;
        private readonly ILanguageService _langService;
        private readonly IFavouritesService _favService;
        private readonly IUserFollowDetailService _userFollowDetailService;
        private readonly ICountryService _countryService;
        public AccountController(IUserService userService, IUserLanguageService userLanguageService,
            ILanguageService langService, IFavouritesService favService,
            IUserFollowDetailService userFollowDetailService, ICountryService countryService)
        {
            _userService = userService;
            _userLanguageService = userLanguageService;
            _langService = langService;
            _favService = favService;
            _userFollowDetailService = userFollowDetailService;
            _countryService = countryService;
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User Details</returns>
        [HttpPost]
        [Route("Login")]

        public IHttpActionResult Login(LoginViewModel model)
        {
            string errorMessage = string.Empty;
            UserInfoViewModel obj = new UserInfoViewModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    errorMessage = String.Join(",", ErrorHelper.GetErrorListFromModelState(ModelState));

                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessage));
                }
                bool isValidEmail = ValidateEmail(model.Email);
                if (!isValidEmail)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please give a valid email."));
                }
                if (string.IsNullOrEmpty(model.Email))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Either First Name or Email or Phone is required."));
                }

                var userDetails = _userService.GetUserDetailsByEmail(model.Email);
                if (userDetails != null)
                {
                    string salt = userDetails.Password;
                    string hashedPassword = PasswordAndTrevoHelper.EncodePassword(model.Password, salt);
                    if (userDetails.PasswordHash != hashedPassword)
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Password does not match."));
                    }
                    obj.Email = userDetails.Email_Id;
                    if (string.IsNullOrEmpty(userDetails.ImagePath))
                    {
                        obj.ImagePath = noImagePath;
                    }
                    else
                    {
                        var extension = userDetails.ImagePath.Split('.')[1];
                        var imageName = userDetails.ImagePath.Split('.')[0] + "_thumbnail";
                        obj.ImagePath = proImagePath + imageName + "." + extension;
                    }
                    obj.IsVerified = userDetails.IsVerified;
                    obj.Name = userDetails.Name;
                    obj.UserID = userDetails.User_Id;
                    var userLangDetails = _userLanguageService.GetLanguageListByUserId(obj.UserID);
                    if (userLangDetails != null)
                    {
                        var langDetails = _langService.GetLanguageDetailsById(userLangDetails.Native_LanguageId);
                        if (langDetails != null)
                        {
                            obj.NativeLangugae = langDetails.Abbreviation;
                        }
                        langDetails = _langService.GetLanguageDetailsById(userLangDetails.Learning_LanguageId);
                        if (langDetails != null)
                        {
                            obj.LearningLanguage = langDetails.Abbreviation;
                        }
                    }
                    obj.FavMomentList = new List<long>();
                    var momentList = _favService.GetFavouritesListByUserId(userDetails.User_Id).Where(a => a.MomentId != 0).ToList();
                    foreach (var item in momentList)
                    {
                        obj.FavMomentList.Add(item.MomentId);
                    }
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User not found with the email."));
                }
            }
            catch (System.Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }

        private static bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Error if found</returns>
        [Route("ChangePassword")]

        public IHttpActionResult ChangePassword(ChangePasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string errorMessage = string.Empty;
                    errorMessage = string.Join(",", ErrorHelper.GetErrorListFromModelState(ModelState));
                    return BadRequest(errorMessage);
                }
                bool isValidEmail = ValidateEmail(model.Email);
                if (!isValidEmail)
                {
                    return BadRequest("Please give a valid email.");
                }
                if (model.OldPassword.ToLower().Equals(model.NewPassword.ToLower()))
                {
                    return BadRequest("Old Password and New Password must not be same.");
                }
                bool isNewPassValid = ErrorHelper.IsValidPassword(model.NewPassword);
                if (!isNewPassValid)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "The password must contain atleast one number and one upper case character."));
                }
                var userDetails = _userService.GetUserDetailsByEmail(model.Email);
                if (userDetails != null)
                {
                    string salt = userDetails.Password;
                    string hashedPassword = string.Empty;
                    hashedPassword = PasswordAndTrevoHelper.EncodePassword(model.OldPassword, salt);
                    if (hashedPassword == userDetails.PasswordHash)
                    {
                        salt = PasswordAndTrevoHelper.GeneratePassword(30);
                        hashedPassword = PasswordAndTrevoHelper.EncodePassword(model.NewPassword, salt);
                        userDetails.Password = salt;
                        userDetails.PasswordHash = hashedPassword;
                        userDetails.ExternalAuthType= userDetails.ExternalAuthType == null ? string.Empty : userDetails.ExternalAuthType;
                        userDetails.ExternalAuthUserId = userDetails.ExternalAuthUserId == null ? string.Empty : userDetails.ExternalAuthUserId;
                        ReturnMsg msg = _userService.UpdateUserDetails(userDetails);
                        if (!msg.IsSuccess)
                        {
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Please try again later."));
                        }
                        string SiteURL = ConfigurationManager.AppSettings["SiteURL"].ToString();
                        string InfoMail = ConfigurationManager.AppSettings["InfoMail"].ToString();
                        StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/EmailTemplate/ChangePassword.html"));
                        string readFile = reader.ReadToEnd();
                        string mailBody = "";
                        mailBody = readFile;
                        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                        mailBody = mailBody.Replace("$$UserName$$", myTI.ToTitleCase(userDetails.Name));
                        ReturnMsg mailResult = SendMail.SendEmail(InfoMail, userDetails.Email_Id, "Password Changed", mailBody);

                    }
                    else
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Passwords do not match."));
                    }
                }
                else
                {
                    return BadRequest("User not found.");
                }


            }
            catch (System.Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }


            return Ok("Password is successfully changed.");
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Error if found</returns>
        [Route("ForgotPassword")]


        public IHttpActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (!ModelState.IsValid)
                {
                    string message = string.Join("", ErrorHelper.GetErrorListFromModelState(ModelState));
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, message));
                }
                bool isValidEmail = ValidateEmail(model.Email);
                if (!isValidEmail)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Please give a valid email."));
                }
                var userDetails = _userService.GetUserDetailsByEmail(model.Email);
                if (userDetails != null)
                {
                    string password = "";
                    string salt = "";
                    string ranPass = "";
                    PasswordAndTrevoHelper.GetRandomPassword(ref password, ref salt, ref ranPass);
                    userDetails.Password = salt;
                    userDetails.PasswordHash = password;
                    _userService.UpdateUserDetails(userDetails);


                    string SiteURL = ConfigurationManager.AppSettings["SiteURL"].ToString();
                    string InfoMail = ConfigurationManager.AppSettings["InfoMail"].ToString();
                    StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/EmailTemplate/ForgotPasswordReset.html"));
                    string readFile = reader.ReadToEnd();
                    string mailBody = "";
                    mailBody = readFile;
                    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                    mailBody = mailBody.Replace("$$UserName$$", myTI.ToTitleCase(userDetails.Name));
                    mailBody = mailBody.Replace("$$Password$$", ranPass);
                    ReturnMsg mailResult = SendMail.SendEmail(InfoMail, userDetails.Email_Id, "Password Reset", mailBody);
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User not found."));
                }


            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }


            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "Your password has been sent into your mailbox."));
        }


        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="model"></param>
        /// <returns>result </returns>
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public IHttpActionResult Register(RegisterModel model)
        {
            ResponseModel obj = new ResponseModel();
            string errorMessage = string.Empty;
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {

                if (!ModelState.IsValid)
                {
                    errorMessage = String.Join(",", ErrorHelper.GetErrorListFromModelState(ModelState));
                    return BadRequest(errorMessage);
                }
                bool isEmail = ValidateEmail(model.Email);
                if (isEmail == false)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please give a valid email id."));
                }
                var userDetailsByEmail = _userService.GetUserDetailsByEmail(model.Email);
                if (userDetailsByEmail == null)
                {

                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "This email is already taken."));
                }
                bool isValid = ErrorHelper.IsValidPassword(model.Password);
                if (!isValid)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "The password must contain atleast one number and one upper case character."));
                }
                var keyNew = PasswordAndTrevoHelper.GeneratePassword(30);
                var password = PasswordAndTrevoHelper.EncodePassword(model.Password, keyNew);
                //string salt = Crypto.GenerateSalt();
                //string hashedPassword = Crypto.HashPassword(salt + model.Password);

                //string salt = PasswordAndTrevoHelper.GetSaltString();
                //string finalPassword = model.Password + salt;
                //string hashedPassword = PasswordAndTrevoHelper.GetPasswordHashAndSalt(finalPassword);

                //string hashedPassword = PasswordAndTrevoHelper.HashPassword(model.Password);
                var trevoId = PasswordAndTrevoHelper.GenerateTrevoId(model.Name);
                TrevoUsers user = new TrevoUsers();
                user.Address = model.Address == null ? string.Empty : model.Address;
                user.Country_Id = model.CountryId;
                user.DeviceId = model.DeviceId;
                user.Dob = model.Dob;
                user.Email_Id = model.Email;
                user.Gender = model.Gender;
                //user.UsersLanguageId = model.Id;
                user.Interests = model.Interests == null ? string.Empty : model.Interests;
                user.LagLevel_ID = model.LanguageLevelId;
                user.Name = model.Name;
                user.Password = keyNew;
                user.PasswordHash = password;
                user.QR_Code = string.Empty;
                user.Self_Introduction = model.SelfIntroduction == null ? string.Empty : model.SelfIntroduction;
                user.TravelDestination_CId = model.TravelDestination == null ? string.Empty : model.TravelDestination;
                user.TrevoId = trevoId;
                user.IsVerified = 0;
                user.ImagePath = string.Empty;
                user.CreatedTime = DateTime.UtcNow;
                user.ExternalAuthType = model.ExternalAuthType == null ? string.Empty : model.ExternalAuthType;
                user.ExternalAuthUserId = model.ExternalAuthUserId == null ? string.Empty : model.ExternalAuthUserId;

                //_userService.InsertUserDetails(user);
                //insert the user language first
                var isSuccess = _userService.InsertUserDetails(user);
                if (isSuccess.IsSuccess)
                {
                    UsersLanguage userLanguage = new UsersLanguage();
                    userLanguage.User_Id = Convert.ToInt64(isSuccess.Message);
                    obj.UserID = userLanguage.User_Id;
                    userLanguage.Learning_LanguageId = model.LearningLanguageId;
                    userLanguage.Native_LanguageId = model.NativeLanguageId;
                    _userLanguageService.InsertUserLanguageDetails(userLanguage);
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "There is some error."));
                }
                //send mail after successfull registration
                StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/EmailTemplate/RegistrationActivation.html"));
                string readFile = reader.ReadToEnd();
                string mailBody = "";
                mailBody = readFile;
                mailBody = mailBody.Replace("$$UserName$$", model.Name);
                string otpCode = PasswordAndTrevoHelper.CreateRandomNumber(5);
                string encryptedUserName = SSTCryptographer.Encrypt(model.Name, SSTCryptographer.Key = "Activation");
                mailBody = mailBody.Replace(" $$OTPCode$$", otpCode);
                ReturnMsg mailResult = SendMail.SendEmail(InfoMail, model.Email, "Account Activation", mailBody);
                obj.Email = model.Email;
                obj.Name = model.Name;
                var userLanDetails = _userLanguageService.GetLanguageListByUserId(obj.UserID);
                if (userLanDetails != null)
                {
                    var langDetails = _langService.GetLanguageDetailsById(userLanDetails.Native_LanguageId);
                    if (langDetails != null)
                    {
                        obj.NativeLangugae = langDetails.Abbreviation;
                    }
                    langDetails = _langService.GetLanguageDetailsById(userLanDetails.Learning_LanguageId);
                    if (langDetails != null)
                    {
                        obj.LearningLanguage = langDetails.Abbreviation;
                    }
                }
                obj.ImagePath = noImagePath;
            }
            catch (System.Exception e)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);

        }

        /// <summary>
        /// Upload file in text chat
        /// </summary>
        /// <returns></returns>
        [Route("UploadFile")]
        [HttpPost]

        public async Task<IHttpActionResult> UploadFile()
        {


            if (!Request.Content.IsMimeMultipartContent())
            {
                return Content(HttpStatusCode.BadRequest, "Unsupported media type.");
            }
            string path = string.Empty;
            try
            {
                bool isVideo = false;
                bool isImage = false;
                string root = HttpContext.Current.Server.MapPath("~/ChatUploads");
                var provider = new MultipartFormDataStreamProvider(root);

                await Request.Content.ReadAsMultipartAsync(provider);
                string temp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileName1 = string.Empty;
                string StoragePath = string.Empty;
                foreach (MultipartFileData item in provider.FileData)
                {
                    string fileName = item.Headers.ContentDisposition.FileName;
                    fileName = fileName.Replace(" ", "_");
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }
                    string extension = fileName.Split('.')[1].ToLower();
                    if (!validFormats.Any(a => extension.Contains(a)))
                    {
                        File.Delete(item.LocalFileName);
                        if (provider.FileData.Count == 1)
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Please upload an image or an audio file."));
                        else
                            break;
                    }
                    if (imageFormats.Any(a => extension.Contains(a)))
                    {
                        isImage = true;
                        isVideo = false;
                        StoragePath = HttpContext.Current.Server.MapPath("~/ChatUploads/Uploads/OriginalSize");
                    }
                    else
                    {
                        isImage = false;
                        isVideo = true;
                        StoragePath= HttpContext.Current.Server.MapPath("~/ChatUploads/Uploads/Videos");
                    }
                    fileName1 = fileName.Split('.')[0] + "_" + temp + "." + extension;
                    string path1 = Path.Combine(StoragePath, fileName);
                    File.Move(item.LocalFileName, path1);
                    if (isImage)
                        path = imagePath + fileName;
                    else if (isVideo)
                        path = videoPath + fileName;
                    File.Delete(item.LocalFileName);
                    if (isImage)
                    {
                        //saving the file in medium size;
                        int width = 0;
                        string folderName = string.Empty;
                        width = 400;
                        //Saving the medium size
                        folderName = "ChatUploads/Uploads";
                        ImageUploadHelper imageUpload = new ImageUploadHelper { Width = width };
                        ImageResult imageResult = imageUpload.RenameUploadFile(item, true, "", fileName, folderName);

                        //saving the file in thumbnail size

                        width = 150;
                        imageUpload = new ImageUploadHelper { Width = width };
                        imageResult = imageUpload.RenameUploadFile(item, false, "", fileName, folderName);
                        path = imagePath + imageResult.ImageName;
                    }
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }


            return Ok(path);
        }


        /// <summary>
        /// Social Register
        /// </summary>
        /// <param name="model"></param>
        /// <returns>result </returns>
        [AllowAnonymous]
        [Route("ExternalAuthRegister")]
        [HttpPost]
        public IHttpActionResult ExternalAuthRegister(RegisterModel model)
        {
            ResponseModel obj = new ResponseModel();
            string errorMessage = string.Empty;
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {

                if (!ModelState.IsValid)
                {
                    errorMessage = String.Join(",", ErrorHelper.GetErrorListFromModelState(ModelState));
                    return BadRequest(errorMessage);
                }
                if (string.IsNullOrEmpty(model.ExternalAuthType))
                {
                    return BadRequest("External Auth Type is required.");
                }

                if (string.IsNullOrEmpty(model.ExternalAuthUserId))
                {
                    return BadRequest("External Auth User Id is required.");
                }
                bool isEmail = ValidateEmail(model.Email);
                if (isEmail == false)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please give a valid email id."));
                }
                var userDetailsByEmail = _userService.GetUserDetailsByEmail(model.Email);
                if (userDetailsByEmail == null)
                {

                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "This email is already taken."));
                }

                var trevoId = PasswordAndTrevoHelper.GenerateTrevoId(model.Name);
                TrevoUsers user = new TrevoUsers();
                user.Address = model.Address == null ? string.Empty : model.Address;
                user.Country_Id = model.CountryId;
                user.DeviceId = model.DeviceId;
                user.Dob = model.Dob;
                user.Email_Id = model.Email;
                user.Gender = model.Gender;
                //user.UsersLanguageId = model.Id;
                user.Interests = model.Interests == null ? string.Empty : model.Interests;
                user.LagLevel_ID = model.LanguageLevelId;
                user.Name = model.Name;
                user.Password = string.Empty;
                user.PasswordHash = string.Empty;
                user.QR_Code = string.Empty;
                user.Self_Introduction = model.SelfIntroduction == null ? string.Empty : model.SelfIntroduction;
                user.TravelDestination_CId = model.TravelDestination == null ? string.Empty : model.TravelDestination;
                user.TrevoId = trevoId;
                user.IsVerified = 0;
                user.ImagePath = string.Empty;
                user.CreatedTime = DateTime.UtcNow;
                user.ExternalAuthType = model.ExternalAuthType;
                user.ExternalAuthUserId = model.ExternalAuthUserId;

                //_userService.InsertUserDetails(user);
                //insert the user language first
                var isSuccess = _userService.InsertUserDetails(user);
                if (isSuccess.IsSuccess)
                {
                    UsersLanguage userLanguage = new UsersLanguage();
                    userLanguage.User_Id = Convert.ToInt64(isSuccess.Message);
                    obj.UserID = userLanguage.User_Id;
                    userLanguage.Learning_LanguageId = model.LearningLanguageId;
                    userLanguage.Native_LanguageId = model.NativeLanguageId;
                    _userLanguageService.InsertUserLanguageDetails(userLanguage);
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "There is some error."));
                }
                //send mail after successfull registration
                StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/EmailTemplate/RegistrationActivation.html"));
                string readFile = reader.ReadToEnd();
                string mailBody = "";
                mailBody = readFile;
                mailBody = mailBody.Replace("$$UserName$$", model.Name);
                string otpCode = PasswordAndTrevoHelper.CreateRandomNumber(5);
                string encryptedUserName = SSTCryptographer.Encrypt(model.Name, SSTCryptographer.Key = "Activation");
                mailBody = mailBody.Replace(" $$OTPCode$$", otpCode);
                ReturnMsg mailResult = SendMail.SendEmail(InfoMail, model.Email, "Account Activation", mailBody);
                obj.Email = model.Email;
                obj.Name = model.Name;
            }
            catch (System.Exception e)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);

        }

        /// <summary>
        /// Get All Country List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCountry")]
        public IHttpActionResult GetAllCountryList()
        {
            List<Core.Model.Country.Country> countryList = new List<Core.Model.Country.Country>();
            try
            {
                countryList = _countryService.GetAllCountry().OrderBy(a=>a.Name).ToList();
                foreach (var item in countryList)
                {
                    item.ImagePath = countryIconPath + item.Flag_Icon;
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(countryList);
        }

        /// <summary>
        /// Get All Language List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllLanguages")]

        public IHttpActionResult GetAllLanguages()
        {
            List<LanguageDetails> languageList = new List<LanguageDetails>();
            try
            {
                languageList = _langService.GetAllLangugaes().ToList();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(languageList);
        }

        /// <summary>
        /// Insert  Language Details
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertLanguageDetails")]
        public IHttpActionResult InsertLanguageDetails(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id) || (string.IsNullOrEmpty(model.ScheduleId)))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Both Name and Abbreviation are required."));
                }

                LanguageDetails details = new LanguageDetails();
                details.Abbreviation = model.ScheduleId;
                details.Name = model.Id;
                details.ImagePath = string.Empty;
                _langService.InsertLanguageDetails(details);
                obj.IsSuccess = true;
                obj.Message = "Language inserted successfully.";
            }
            catch (Exception e)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }



        /// <summary>
        /// Insert Country Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertCountryDetails")]

        public IHttpActionResult InsertCountryDetails(CountryModel model)
        {
            ReturnMsg returnObj = new ReturnMsg();
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Both name and flag icon are required."));
                }
                Country countryObj = new Country();
                countryObj.Flag_Icon = model.Flag_Icon;
                countryObj.Name = model.Name;
                countryObj.ImagePath = string.Empty;
                _countryService.InsertCountryDetails(countryObj);
                returnObj.IsSuccess = true;
                returnObj.Message = "Country Details inserted successfully.";
                
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(returnObj);
        }

     
        /// <summary>
        /// Get User List based on Advanced Search
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AdvancedSearch")]

        public IHttpActionResult AdvancedSearch(AdvancedSearchModel model)

        {
            List<UserWithCountryIconInfo> userList = new List<UserWithCountryIconInfo>();
            try
            {
                var allUserList = _userService.GetAllUsersWithCountryFlag().ToList();
                if (!string.IsNullOrEmpty(model.Address))
                {
                    model.Address = model.Address.ToLower();
                    allUserList = allUserList.Where(a => a.Address.ToLower().Contains(model.Address)).ToList();
                }
                if (model.NatioanlityId > 0)
                {
                    allUserList = allUserList.Where(a => a.Country_Id == model.NatioanlityId).ToList();
                }
                if (model.LangLevelId > 0)
                {
                    allUserList = allUserList.Where(a => a.LagLevel_ID == model.LangLevelId).ToList();
                }
                var userLangList = _userLanguageService.GetAllUsersLanguages().ToList();
                if (model.LearningLangId > 0)
                {
                    userLangList = userLangList.Where(a => a.Learning_LanguageId == model.LearningLangId).ToList();
                }
                if (model.NativeLangId > 0)
                {

                    userLangList = userLangList.Where(a => a.Native_LanguageId == model.NativeLangId).ToList();
                }
                string userIdList = string.Empty;
                if (userLangList.Count > 0)
                {
                    foreach (var item in userLangList)
                    {
                        if (string.IsNullOrEmpty(userIdList))
                        {
                            userIdList = item.User_Id.ToString();
                        }
                        else
                        {
                            userIdList = userIdList + "," + item.User_Id.ToString();
                        }
                    }
                }

                List<LanguageDetails> langList = _langService.GetAllLangugaes().ToList();
                foreach (var item in allUserList)
                {
                    if (!string.IsNullOrEmpty(userIdList))
                    {
                        if (userIdList.Contains(item.User_Id.ToString()))
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
                            details.Icon_Path = countryIconPath + item.Flag_Icon;
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
                            // details.ImagePath = proImagePath + item.ImagePath;
                            details.Interests = item.Interests;
                            details.IsVerified = item.IsVerified;
                            details.LagLevel_ID = item.LagLevel_ID;
                            details.LearningAbbrv = langList.Where(a => a.Language_Id == item.Learning_LanguageId).FirstOrDefault().Abbreviation;
                            details.Name = item.Name;
                            details.NativeAbbrv = langList.Where(a => a.Language_Id == item.Native_LanguageId).FirstOrDefault().Abbreviation;
                            details.Self_Introduction = item.Self_Introduction;
                            details.TravelDestination_CId = item.TravelDestination_CId;
                            details.TrevoId = item.TrevoId;
                            details.User_Id = item.User_Id;
                            userList.Add(details);

                        }
                    }
                }

            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(userList);
        }


        /// <summary>
        /// Update UserDetails
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateUserDetails")]

        public IHttpActionResult UpdateUserDetails(UserDetailsModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }
                TrevoUsers userDetails = new TrevoUsers();
                userDetails.Address = model.Address == null ? string.Empty : model.Address;
                userDetails.Country_Id = model.Country_Id;
                userDetails.Email_Id = model.Email_Id == null ? string.Empty : model.Email_Id;
                userDetails.Name = model.Name == null ? string.Empty : model.Name;
                userDetails.Password = model.Password == null ? string.Empty : model.Password;
                userDetails.PasswordHash = model.PasswordHash == null ? string.Empty : model.PasswordHash;
                userDetails.QR_Code = model.QR_Code == null ? string.Empty : model.QR_Code;
                userDetails.Self_Introduction = model.Self_Introduction == null ? string.Empty : model.Self_Introduction;
                userDetails.TravelDestination_CId = model.TravelDestination_CId == null ? string.Empty : model.TravelDestination_CId;
                userDetails.TrevoId = model.TrevoId;
                userDetails.User_Id = model.User_Id;
                userDetails.CreatedTime = model.CreatedTime;
                userDetails.DeviceId = model.DeviceId==null?string.Empty:model.DeviceId;
                userDetails.Dob = model.Dob == null ? string.Empty : model.Dob;
                userDetails.ExternalAuthType = model.ExternalAuthType == null ? string.Empty : model.ExternalAuthType;
                userDetails.ExternalAuthUserId = model.ExternalAuthUserId == null ? string.Empty : model.ExternalAuthUserId;
                userDetails.Gender = model.Gender == null ? string.Empty : model.Gender;
                userDetails.ImagePath = model.ImagePath == null ? string.Empty : model.ImagePath;
                userDetails.Interests = model.Interests == null ? string.Empty : model.Interests;
                userDetails.IsVerified = model.IsVerified;
                userDetails.LagLevel_ID = model.LagLevel_ID;
                _userService.UpdateUserDetails(userDetails);
                obj.IsSuccess = true;
                obj.Message = "User Details updated successfully.";
                
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }

    }
}


