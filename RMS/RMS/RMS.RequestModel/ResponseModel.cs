

namespace RMS.RequestModel
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; } = null;
    }
}
