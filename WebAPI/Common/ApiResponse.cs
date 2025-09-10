namespace GenericRepo_Dapper.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string MessageDetail { get; set; } 
        public T? Data { get; set; }

        public ApiResponse(bool success, string message, string messageDetail, T? data = default)
        {
            Success = success;
            Message = message;
            MessageDetail = messageDetail;
            Data = data;
        }
    }
}
