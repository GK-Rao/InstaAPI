using Microsoft.AspNetCore.Mvc;

namespace InstaAPI.Application.Models.Payload
{
    public class CreatePostRequestPayload
    {
        public string caption { get; set; }

        public string imageUrl { get; set; }
    }
}
