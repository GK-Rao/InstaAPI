using InstaAPI.Application.Helpers;
using InstaAPI.Application.Mapper;
using InstaAPI.Application.Models.Payload;
using InstaAPI.Application.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaAPI.Controllers;

[ApiController]
[Route("[controller]/api")]
public class InstaAppController : ControllerBase{

    private readonly ILogger<InstaAppController> _logger;

    private readonly IUserRepository _repository;

    private readonly IResultBuilder _builder;

    public InstaAppController(ILogger<InstaAppController> logger, IUserRepository repository, IResultBuilder builder)
    {
        _logger = logger;
        _repository = repository;
        _builder = builder;
    }

    /// <summary>
    /// Creates a new user using the user name
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpPost("create-user")]
    public async Task<IActionResult> SignUp(string userName){
        var result = await _repository.CreateUser(userName);
        if (result?.Exception?.Status != "" && result?.Exception?.Status == "failure")
            return new JsonResult(result.Exception) { StatusCode = StatusCodes.Status400BadRequest };

        return new JsonResult(result) { StatusCode = StatusCodes.Status201Created };
        //return _builder.BuildClientResult(result, result?.Exception?.Status);

    }


    /// <summary>
    /// Fetch user information using the user name
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpGet("get-user/{userName}")]
    public async Task<IActionResult> GetUser(string userName)
    {
            var result = await _repository.FetchUser(userName);
            return _builder.BuildClientResult(result, result?.Exception?.Status); 
    }


    /// <summary>
    /// Creates Follow request
    /// </summary>
    /// <param name="follower"></param>
    /// <param name="followee"></param>
    /// <returns></returns>
    [HttpPost("/api/follow/{follower}/{followee}")]
    public async Task<IActionResult> FollowUser(string follower, string followee)
    {
        var result = await _repository.FollowUser(follower, followee);

        if (result?.Exception?.Status != "" && result?.Exception?.Status == "failure")
            return new JsonResult(result.Exception) { StatusCode = StatusCodes.Status400BadRequest };

        return new JsonResult(result) { StatusCode = StatusCodes.Status202Accepted };

        //return _builder.BuildClientResult(result, result?.Exception?.Status);
    }


    [HttpPost("/api/create-post/{userName}")]
    public async Task<IActionResult> CreatePost(string userName, CreatePostRequestPayload requestPayload)
    {
        var result = await _repository.UploadPost(userName, requestPayload);

        return _builder.BuildClientResult(result, result?.Exception?.Status);
    }

    [HttpGet("/api/all-posts/{userName}")]
    public async Task<IActionResult> FetchPost(string userName)
    {
        var result = await _repository.FetchAllPosts(userName);

        return _builder.BuildClientResult(result, result?.Exception?.Status);
    }

}