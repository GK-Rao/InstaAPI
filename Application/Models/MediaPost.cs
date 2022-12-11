using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaAPI.Application.Models;

public class MediaPost {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int postId  {get;set;}   
    public string imageUrl {get;set;}
    public string caption {get;set;}
    public int  upvotes {get;set;}

    public int userId { get;set;}

    [JsonIgnore]
    public virtual UserData userDetail { get;set;}

}