using System.Net;
using System.Runtime.Serialization;
using System.Web;


namespace Trevo.API.Resonse
{
    [DataContract]
    public class ApiResponse
    {
        [DataMember]
        public int StatusCode { get; set; }

        [DataMember]
        public string Status{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ErrorMessage { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public object Result { get; set; }

        public ApiResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            StatusCode = (int)statusCode;
            Status = HttpWorkerRequest.GetStatusDescription(StatusCode);
            Result = result;
            ErrorMessage = errorMessage;
        }
    }
}