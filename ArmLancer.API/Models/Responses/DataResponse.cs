using System.Collections.Generic;

namespace ArmLancer.API.Models.Responses
{
    public class DataResponse<T> : BaseResponse
    {
        public object Data { get; set; }
        
        public DataResponse(T res)
        {
            Data = res;
        }
        
        public DataResponse(IList<T> res)
        {
            Data = res;
        }
    }
}