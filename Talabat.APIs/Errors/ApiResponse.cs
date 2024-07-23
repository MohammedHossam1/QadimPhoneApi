
namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode {  get; set; }
        public string Message {  get; set; }

        public ApiResponse(int status,string? message=null)
        {
            StatusCode = status;
            Message = message??GetMessageByCode(status);

        }

        private string? GetMessageByCode(int status)
        {
            return status switch
            {
                400 => "Bad Request",
                401 => "Un Authorized",
                404 => "Not Found",
                500 => "Server Error",
                _ => null
            };
        }
    }
}
