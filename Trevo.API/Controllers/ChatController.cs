using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Trevo.API.Models;
using Trevo.Core.Model;
using Trevo.Core.Model.Chat;
using Trevo.Services.Chat;

namespace Trevo.API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ChatController : ApiController
    {
        private readonly IChatOfflineService _chatOfflineService;


        public ChatController(IChatOfflineService chatOfflineService)
        {
            _chatOfflineService = chatOfflineService;
        }

        /// <summary>
        /// Insert Offline Chat Message Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertOfflineChatMessages")]

        public IHttpActionResult InsertOfflineChatMessages(OfflineChatModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Sender Id and Reciever Id are required."));
                }

                ChatOfflineMessageDetails details = new ChatOfflineMessageDetails();
                details.ImageUrl = model.image;
                details.RecieverId = model.recieverId;
                details.SenderId = model. senderId;
                details.TextMessage = model.message;
                details.VideoUrl = model.video;
                _chatOfflineService.InsertOfflineChatDetails(details);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok("Message details inserted successfully.");
        }

        /// <summary>
        /// Get Offline Messages List By User Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOfflineChatMessagesByUserId")]
        public IHttpActionResult GetOfflineChatMessagesByUserId(RequestModel model)
        {
            List<OfflineChatModel> offlineMessageList = new List<OfflineChatModel>();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }
                long id = Convert.ToInt64(model.Id);
               var messageList = _chatOfflineService.GetOfflineMessagesByUserId(id);
                foreach (var item in messageList)
                {
                    OfflineChatModel details = new OfflineChatModel();
                    details.image = item.ImageUrl;
                    details.message = item.TextMessage;
                    details.recieverId = item.RecieverId;
                    details.senderId = item.SenderId;
                    details.video = item.VideoUrl;
                    offlineMessageList.Add(details);
                }
              
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

            return Ok(offlineMessageList);
        }


        /// <summary>
        /// Delete Offline Chat Message By User Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteOfflineMessage")]

        public IHttpActionResult DeleteOfflineMessagesByUserId(RequestModel model)
        {
            ReturnMsg obj = new ReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Id is required."));
                }
                long id = Convert.ToInt64(model.Id);
                obj = _chatOfflineService.DeleteOfflineMessageByUserId(id);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            return Ok(obj);
        }

    }
}
