using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InstaAPI.Application.Models.Payload
{
    public class CreatePostRequestPayload
    {
        [Required]
        [MinLength(3)]
        public string caption { get; set; }

        [Required]
        [MinLength(3)]
        public string imageUrl { get; set; }
    }
}
