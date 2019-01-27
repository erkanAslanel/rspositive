using System;
namespace AuthAPI.Domain
{
    public class OperationResult
    {
       public bool IsSuccess { get; set; }
       public bool ErrorMessage { get; set; }
        public bool ErrorMessageDetail { get; set; }
    }
}
