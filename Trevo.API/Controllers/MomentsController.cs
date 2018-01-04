using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Trevo.API.Helper;
using Trevo.Core.Model;
using Trevo.Core.Model.MomentDetails;
using Trevo.Services.MomentService;
using System.Linq;
using Trevo.Services.UserUploadService;
using Trevo.Core.Model.UserUploadDetails;
using Trevo.API.Models;
using Trevo.Services.Users;
using Trevo.Services.Language;
using System.Globalization;
using System.Configuration;
using Trevo.Services.FavouriteService;

namespace Trevo.API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class MomentsController : ApiController
    {
        private readonly IMomentsService _momentsService;
        private readonly IUserUploadsService _userUploadsService;
        private readonly IUserLanguageService _userLangService;
        private readonly ILanguageService _langService;
        private readonly IFavouritesService _favService;
        private static string imagePath = ConfigurationManager.AppSettings["CountryIcon"].ToString();
        private static string noImagePath = ConfigurationManager.AppSettings["NoImagePath"].ToString();
        private static string proImagePath = ConfigurationManager.AppSettings["ProfileImages"].ToString();
        private static string momentUploadImagePath = ConfigurationManager.AppSettings["MomentUploadImagePath"].ToString();
        private static string momentUploadAudioPath = ConfigurationManager.AppSettings["MomentUploadAudioPath"].ToString();
        private readonly static List<string> validFormats = new List<string>() { "mp3", "aac", "wav", "3gpp", "smf", "ogg", "jpg", "png", "jpeg" };
        private readonly static List<string> audioFormats = new List<string>() { "mp3", "aac", "wav", "3gpp", "smf", "ogg" };
        private readonly static List<string> imageFormats = new List<string>() { "jpg", "png", "jpeg" };
        public MomentsController(IMomentsService momentsService, IUserUploadsService userUploadsService, IUserLanguageService userLangService, ILanguageService langService,
            IFavouritesService favService)
        {
            _momentsService = momentsService;
            _userUploadsService = userUploadsService;
            _userLangService = userLangService;
            _langService = langService;
            _favService = favService;
        }

        /// <summary>
        /// Insert Moments Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertMomentsDetails")]


        public async Task<IHttpActionResult> InsertMomentsDetails()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Content(HttpStatusCode.BadRequest, "Unsupported media type.");
            }
            ImageResult imageResult = new ImageResult();
            try
            {
                bool isAudio = false;
                bool isImage = false;
                string root = HttpContext.Current.Server.MapPath("~/MomentUploads");
                var provider = new MultipartFormDataStreamProvider(root);

                await Request.Content.ReadAsMultipartAsync(provider);
                string StoragePath = string.Empty;
                Moments momentDetails = new Moments();
                if (provider.FormData.Count == 0 && provider.FileData.Count == 0)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }
                else if (provider.FormData.Count == 0 && provider.FileData.Count > 0)
                {
                    foreach (var item in provider.FileData)
                    {
                        File.Delete(item.LocalFileName);
                    }

                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }
                else if (provider.FormData.Count == 1 && provider.FileData.Count == 0)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Either  an image or an audio or text and user id and parent id is required. "));
                }
                long userId = Convert.ToInt64(provider.FormData.GetValues(0)[0]);
                long parentId = Convert.ToInt64(provider.FormData.GetValues(1)[0]);
                string message = string.Empty;
                if (provider.FormData.Count > 2)
                {
                    message = provider.FormData.GetValues(2)[0];
                }
                //string message = provider.FormData.GetValues(0)[1];


                //moving the file to the required destination
                string temp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileName1 = string.Empty;
                string path1 = string.Empty;
                ReturnMsg msg = new ReturnMsg();
                foreach (MultipartFileData fileData in provider.FileData)
                {
                    string fileName = fileData.Headers.ContentDisposition.FileName;
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
                        File.Delete(fileData.LocalFileName);
                        if (provider.FileData.Count == 1)
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Please upload an image or an audio file."));
                        else
                            break;
                    }
                    fileName1 = fileName.Split('.')[0] + "_" + temp + "." + extension;
                    if (audioFormats.Any(a => extension.Contains(a)))
                    {
                        isImage = false;
                        isAudio = true;
                        StoragePath = HttpContext.Current.Server.MapPath("~/MomentUploads/Audio");
                    }
                    else if (imageFormats.Any(a => extension.Contains(a)))
                    {
                        isImage = true;
                        isAudio = false;
                        StoragePath = HttpContext.Current.Server.MapPath("~/MomentUploads/Images/OriginalSize");
                    }
                    path1 = Path.Combine(StoragePath, fileName1);
                    File.Move(fileData.LocalFileName, path1);
                    File.Delete(fileData.LocalFileName);

                    if (isImage)
                    {
                        //saving the file in medium size;
                        int width = 0;
                        string folderName = string.Empty;
                        width = 400;
                        //Saving the medium size
                        folderName = "MomentUploads/Images";
                        ImageUploadHelper imageUpload = new ImageUploadHelper { Width = width };
                        imageResult = imageUpload.RenameUploadFile(fileData, true, "", fileName1, folderName);

                        //saving the file in thumbnail size

                        width = 150;
                        imageUpload = new ImageUploadHelper { Width = width };
                        imageResult = imageUpload.RenameUploadFile(fileData, false, "", fileName1, folderName);
                    }
                    UserUploads userUploads = new UserUploads();
                    userUploads.UserId = userId;
                    if (isImage)
                    {
                        userUploads.UploadedImagePath = imageResult.ImageName;
                        userUploads.UploadedAudioPath = string.Empty;
                    }
                    else if (isAudio)
                    {
                        userUploads.UploadedAudioPath = fileName1;
                        userUploads.UploadedImagePath = string.Empty;
                    }
                    msg = _userUploadsService.InsertUserUploadDetails(userUploads);
                }


                //inserting moments in the database;
                long userUploadedId = 0;

                userUploadedId = Convert.ToInt64(msg.Message);

                momentDetails.Message = message;
                momentDetails.ParentId = parentId;
                momentDetails.PosterUserId = userId;
                momentDetails.UserUploadedId = userUploadedId;
                var dateTime = DateTime.UtcNow;
                momentDetails.PostingTime = dateTime.ToString(@"yyyy/MM/dd hh:mm tt", new CultureInfo("en-US"));
                _momentsService.InsertMomentDetails(momentDetails);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, e.Message));
            }
            return Ok("Moment Details inserted successfully.");
        }

        /// <summary>
        /// Get All Moments List With Language Information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllMomentsWithLanguage")]

        public IHttpActionResult GetAllMomentsWithLanguage()
        {
            List<MomentDetailsList> momentsList = new List<MomentDetailsList>();
            try
            {
                var objList = _momentsService.GetAllMoments().Where(a => a.ParentId == 0).ToList();
                foreach (var item in objList)
                {
                    MomentDetailsList details = new MomentDetailsList();
                    details.Message = item.Message;
                    details.MomentId = item.MomentId;
                    details.Name = item.Name;
                    details.ParentId = item.ParentId;
                    details.PosterUserId = item.PosterUserId;
                    details.PostingTime = item.PostingTime;
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
                    // details.ImagePath = item.ImagePath == null ? noImagePath : item.ImagePath == string.Empty ? noImagePath : proImagePath + item.ImagePath;
                    var userLangDetails = _userLangService.GetLanguageListByUserId(item.PosterUserId);
                    if (userLangDetails != null)
                    {
                        var langDetails = _langService.GetLanguageDetailsById(userLangDetails.Learning_LanguageId);
                        if (langDetails != null)
                        {
                            details.LearningLangAbbrev = langDetails.Abbreviation;
                        }
                        langDetails = _langService.GetLanguageDetailsById(userLangDetails.Native_LanguageId);
                        if (langDetails != null)
                        {
                            details.NativeLangAbbrev = langDetails.Abbreviation;
                        }

                    }

                    if (item.UserUploadedId > 0)
                    {
                        var userUploads = _userUploadsService.GetUserUploadsById(item.UserUploadedId);
                        if (userUploads != null)
                        {
                            details.UploadedImage = userUploads.UploadedImagePath == null ? string.Empty : momentUploadImagePath + userUploads.UploadedImagePath;
                            details.UploadedAudio = userUploads.UploadedAudioPath == null ? string.Empty : momentUploadAudioPath + userUploads.UploadedAudioPath;
                        }
                    }
                    
                    details.CountryImagePath = imagePath + item.Flag_Icon;
                    details.RepliesCount = _momentsService.GetAllMomentsLIstByParentId(details.MomentId).Count;
                    details.FavouritesCount = _favService.GetFavouritesListBymomentId(details.MomentId).Count;
                    momentsList.Add(details);
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, e.Message));
            }

            return Ok(momentsList);
        }

        /// <summary>
        /// Get All Moments List By Parent Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAllMomentsLIstByParentId")]
        public IHttpActionResult GetAllMomentsLIstByParentId(RequestModel model)
        {
            List<MomentDetailsList> momentsList = new List<MomentDetailsList>();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Moment Id is required."));
                }
                long id = Convert.ToInt64(model.Id);
                var objList = _momentsService.GetAllMomentsLIstByParentId(id).ToList();
                foreach (var item in objList)
                {
                    MomentDetailsList details = new MomentDetailsList();
                    details.Message = item.Message;
                    details.MomentId = item.MomentId;
                    details.Name = item.Name;
                    details.ParentId = item.ParentId;
                    details.PosterUserId = item.PosterUserId;
                    details.PostingTime = item.PostingTime;
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
                    // details.ImagePath = item.ImagePath == null ? noImagePath : item.ImagePath == string.Empty ? noImagePath : proImagePath + item.ImagePath;
                    var userLangDetails = _userLangService.GetLanguageListByUserId(item.PosterUserId);
                    if (userLangDetails != null)
                    {
                        var langDetails = _langService.GetLanguageDetailsById(userLangDetails.Learning_LanguageId);
                        if (langDetails != null)
                        {
                            details.LearningLangAbbrev = langDetails.Abbreviation;
                        }
                        langDetails = _langService.GetLanguageDetailsById(userLangDetails.Native_LanguageId);
                        if (langDetails != null)
                        {
                            details.NativeLangAbbrev = langDetails.Abbreviation;
                        }

                    }

                    if (item.UserUploadedId > 0)
                    {
                        var userUploads = _userUploadsService.GetUserUploadsById(item.UserUploadedId);
                        if (userUploads != null)
                        {
                            details.UploadedImage = userUploads.UploadedImagePath == null ? string.Empty : momentUploadImagePath + userUploads.UploadedImagePath;
                            details.UploadedAudio = userUploads.UploadedAudioPath == null ? string.Empty : momentUploadAudioPath + userUploads.UploadedAudioPath;
                        }
                    }
                    details.CountryImagePath = imagePath + item.Flag_Icon;
                    momentsList.Add(details);
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, e.Message));
            }

            return Ok(momentsList);
        }

        /// <summary>
        /// Get MOments List By User Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMomentsListByUserId")]

        public IHttpActionResult GetMomentsListByUserId(RequestModel model)
        {
            List<MomentDetailsList> momentsList = new List<MomentDetailsList>();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }
                long userId = Convert.ToInt64(model.Id);
                var objList = _momentsService.GetMomentsListByUserId(userId).Where(a => a.ParentId == 0).ToList();
                foreach (var item in objList)
                {
                    MomentDetailsList details = new MomentDetailsList();
                    details.Message = item.Message;
                    details.MomentId = item.MomentId;
                    details.Name = item.Name;
                    details.ParentId = item.ParentId;
                    details.PosterUserId = item.PosterUserId;
                    details.PostingTime = item.PostingTime;
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
                    // details.ImagePath = item.ImagePath == null ? noImagePath : item.ImagePath == string.Empty ? noImagePath : proImagePath + item.ImagePath;
                    var userLangDetails = _userLangService.GetLanguageListByUserId(item.PosterUserId);
                    if (userLangDetails != null)
                    {
                        var langDetails = _langService.GetLanguageDetailsById(userLangDetails.Learning_LanguageId);
                        if (langDetails != null)
                        {
                            details.LearningLangAbbrev = langDetails.Abbreviation;
                        }
                        langDetails = _langService.GetLanguageDetailsById(userLangDetails.Native_LanguageId);
                        if (langDetails != null)
                        {
                            details.NativeLangAbbrev = langDetails.Abbreviation;
                        }

                    }
                    details.RepliesCount = _momentsService.GetAllMomentsLIstByParentId(details.MomentId).Count;
                    details.FavouritesCount = _favService.GetFavouritesListBymomentId(details.MomentId).Count;
                    if (item.UserUploadedId > 0)
                    {
                        var userUploads = _userUploadsService.GetUserUploadsById(item.UserUploadedId);
                        if (userUploads != null)
                        {
                            details.UploadedImage = userUploads.UploadedImagePath == null ? string.Empty : momentUploadImagePath + userUploads.UploadedImagePath;
                            details.UploadedAudio = userUploads.UploadedAudioPath == null ? string.Empty : momentUploadAudioPath + userUploads.UploadedAudioPath;
                        }
                    }
                    momentsList.Add(details);
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, e.Message));
            }

            return Ok(momentsList);
        }

        /// <summary>
        /// Get Moment List Based on Different Criteria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>


        [HttpPost]
        [Route("GetMomentListByCriteria")]
        public IHttpActionResult GetMomentListByCriteria(RequestModel model)
        {
            List<MomentDetailsList> momentsList = new List<MomentDetailsList>();
            List<MomentDetailsWithImage> list = new List<MomentDetailsWithImage>();
            try
            {
                if (string.IsNullOrEmpty(model.NativeLang) && (string.IsNullOrEmpty(model.LearningLang)) && model.FollowingUserId==0)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Either both Native and Learning Language or Following User Id is required."));
                }
                if (model.FollowingUserId == 0)
                {
                    list = _momentsService.GetMomentListByNativeLearnLang(model.NativeLang, model.LearningLang);
                }
                else
                {
                    list = _momentsService.GetMomentsListByFollowerUserId(model.FollowingUserId);
                }
                foreach (var item in list)
                {
                    MomentDetailsList details = new MomentDetailsList();
                    details.CountryImagePath= imagePath + item.Flag_Icon;
                    details.RepliesCount = _momentsService.GetAllMomentsLIstByParentId(item.MomentId).Count;
                    details.FavouritesCount = _favService.GetFavouritesListBymomentId(item.MomentId).Count;
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
                    //details.ImagePath= item.ImagePath == null ? noImagePath : item.ImagePath == string.Empty ? noImagePath : proImagePath + item.ImagePath;
                    var userLangDetails = _userLangService.GetLanguageListByUserId(item.PosterUserId);
                    if (userLangDetails != null)
                    {
                        var langDetails = _langService.GetLanguageDetailsById(userLangDetails.Learning_LanguageId);
                        if (langDetails != null)
                        {
                            details.LearningLangAbbrev = langDetails.Abbreviation;
                        }
                        langDetails = _langService.GetLanguageDetailsById(userLangDetails.Native_LanguageId);
                        if (langDetails != null)
                        {
                            details.NativeLangAbbrev = langDetails.Abbreviation;
                        }

                    }
                    details.Message = item.Message;
                    details.MomentId = item.MomentId;
                    details.Name = item.Name;
                    details.ParentId = item.ParentId;
                    details.PosterUserId = item.PosterUserId;
                    details.PostingTime = item.PostingTime;
                    if (item.UserUploadedId > 0)
                    {
                        var userUploads = _userUploadsService.GetUserUploadsById(item.UserUploadedId);
                        if (userUploads != null)
                        {
                            details.UploadedImage = userUploads.UploadedImagePath == null ? string.Empty : momentUploadImagePath + userUploads.UploadedImagePath;
                            details.UploadedAudio = userUploads.UploadedAudioPath == null ? string.Empty : momentUploadAudioPath + userUploads.UploadedAudioPath;
                        }
                    }
                    momentsList.Add(details);
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(momentsList);
        }



        /// <summary>
        /// Get MOments List By User Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMomentsListByLangForExchange")]

        public IHttpActionResult GetMomentsListByLangForExchange(RequestModel model)
        {
            List<MomentDetailsList> momentsList = new List<MomentDetailsList>();
            try
            {
                if ((string.IsNullOrEmpty(model.NativeLang)) || (string.IsNullOrEmpty(model.LearningLang)))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Learning Language and Native Language are required."));
                }
                var language = _langService.GetAllLangugaes().ToList();
                long nativeId = language.Where(a => a.Abbreviation.ToLower() == model.LearningLang.ToLower()).FirstOrDefault().Language_Id;
                long learningLangId = language.Where(a => a.Abbreviation.ToLower() == model.NativeLang.ToLower()).FirstOrDefault().Language_Id;
                var objList = _momentsService.GetMomentListByUserIdForExchange(nativeId,learningLangId).Where(a => a.ParentId == 0).ToList();
                foreach (var item in objList)
                {
                    MomentDetailsList details = new MomentDetailsList();
                    details.Message = item.Message;
                    details.MomentId = item.MomentId;
                    details.Name = item.Name;
                    details.ParentId = item.ParentId;
                    details.PosterUserId = item.PosterUserId;
                    details.PostingTime = item.PostingTime;
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
                    // details.ImagePath = item.ImagePath == null ? noImagePath : item.ImagePath == string.Empty ? noImagePath : proImagePath + item.ImagePath;
                    var userLangDetails = _userLangService.GetLanguageListByUserId(item.PosterUserId);
                    if (userLangDetails != null)
                    {
                        var langDetails = _langService.GetLanguageDetailsById(userLangDetails.Learning_LanguageId);
                        if (langDetails != null)
                        {
                            details.LearningLangAbbrev = langDetails.Abbreviation;
                        }
                        langDetails = _langService.GetLanguageDetailsById(userLangDetails.Native_LanguageId);
                        if (langDetails != null)
                        {
                            details.NativeLangAbbrev = langDetails.Abbreviation;
                        }

                    }
                    details.RepliesCount = _momentsService.GetAllMomentsLIstByParentId(details.MomentId).Count;
                    details.FavouritesCount = _favService.GetFavouritesListBymomentId(details.MomentId).Count;
                    if (item.UserUploadedId > 0)
                    {
                        var userUploads = _userUploadsService.GetUserUploadsById(item.UserUploadedId);
                        if (userUploads != null)
                        {
                            details.UploadedImage = userUploads.UploadedImagePath == null ? string.Empty : momentUploadImagePath + userUploads.UploadedImagePath;
                            details.UploadedAudio = userUploads.UploadedAudioPath == null ? string.Empty : momentUploadAudioPath + userUploads.UploadedAudioPath;
                        }
                    }
                    momentsList.Add(details);
                }
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, e.Message));
            }

            return Ok(momentsList);
        }
    }
}