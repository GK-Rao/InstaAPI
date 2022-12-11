using InstaAPI.Application.Models;

namespace InstaAPI.Application.Mapper
{
    public class FetchUserResponse : BaseResponse
    {
        internal string allFollowers { get; set; }

        internal string allFollowing { get; set; }

        public string userName { get; set; }

        public string[] followers => !string.IsNullOrEmpty(allFollowers) ? allFollowers.Split(',') : new string[0];

        public string[] following => !string.IsNullOrEmpty(allFollowing) ? allFollowing.Split(",") : new string[0];

        public List<MediaPost> posts { get; set; } 

    }
}
