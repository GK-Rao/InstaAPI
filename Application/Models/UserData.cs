
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaAPI.Application.Models;


[Table("UserData")]
public class UserData {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    public string username {get;set;}

    public string followers { get; set; } = "";

    public string following { get; set; } = "";  
    //public List<FollowersData> followersInfo {get;set;}
    
    public List<MediaPost> posts {get;set;}            
}

