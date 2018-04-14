using System.Collections.Generic;
using ArmLancer.API.Enums;

namespace ArmLancer.API.Models.Responses
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public ResponseStatus? Status { get; set; }
        
        public BaseResponse()
        {
            Success = true;
        }
        
        public BaseResponse(string error, ResponseStatus status = ResponseStatus.None)
        {
            Success = false;
            Errors = new List<string>() { error };
            Status = status;
        }
        
        public BaseResponse(List<string> errors, ResponseStatus status = ResponseStatus.None)
        {
            Errors = errors;
            Success = false;
            Status = status;
        }
    }
}