namespace NotificationIntegration.API.ApiResponse
{
    public class ApiBaseResponse<T>
    {
        public T Data { get; set; }
        public string Error { get; set; }
    }
}
