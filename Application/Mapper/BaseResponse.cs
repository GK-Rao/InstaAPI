namespace InstaAPI.Application.Mapper
{
    public class BaseResponse
    {
        //public T Result { get; set; }
        public InstaException Exception { get; set; }   
        
    }
    public class InstaException
    {
        public string Status { get; set; }

        public string Reason { get; set; }
    }
}
