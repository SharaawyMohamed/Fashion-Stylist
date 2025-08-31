using System.Net;

namespace Web.Domain.Response
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; }
        public object Data { get; set; }
        public List<string> errors { get; set; } = new List<string>();
        public int? TotalCount { get; set; }
        public static async Task<BaseResponse> Success(object data = null, string message = "Success")
        {
            return new BaseResponse
            {
                Message = message,
                Data = data
            };
        }
        public static async Task<BaseResponse> New(object data = null, string message = "Success", int totalCount = 0)
        {
            return new BaseResponse
            {
                Message = message,
                Data = data,
                TotalCount = totalCount
            };
        }
        public static async Task<BaseResponse> Fail(List<string> error = null, string message = "Failed", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new BaseResponse
            {
                StatusCode = statusCode,
                Message = message,
                errors = error
            };
        }
        public static async Task<BaseResponse> Faild(string message = "Failed", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new BaseResponse
            {
                StatusCode = statusCode,
                Message = message,

            };
        }
        public static async Task<BaseResponse> ValidationError(List<string> errors, string message = "Validation Error", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new BaseResponse
            {
                StatusCode = statusCode,
                Message = message,
                errors = errors
            };
        }
    }
}
