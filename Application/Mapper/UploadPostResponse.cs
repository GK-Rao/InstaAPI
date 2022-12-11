using InstaAPI.Application.Models;

namespace InstaAPI.Application.Mapper
{
    public class UploadPostResponse : BaseResponse
    {
        public MediaPost post { get; set; }
    }
}
