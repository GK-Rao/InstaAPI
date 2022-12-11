using InstaAPI.Application.Models;

namespace InstaAPI.Application.Mapper
{
    public class FetchAllPostsResponse : BaseResponse
    {
        public List<MediaPost> allPosts { get; set; }

    }
}
