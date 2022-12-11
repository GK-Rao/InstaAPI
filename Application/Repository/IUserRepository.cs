using InstaAPI.Application.Mapper;
using InstaAPI.Application.Models.Payload;
using System.Runtime.InteropServices;

namespace InstaAPI.Application.Repository;

public interface IUserRepository {
    Task<BaseResponse> CreateUser(string userName);

    Task<BaseResponse> FetchUser(string userName);

    Task<BaseResponse> FollowUser(string follower, string followee);

    Task<BaseResponse> UploadPost(string userName, CreatePostRequestPayload requestPayload);

    Task<BaseResponse> FetchAllPosts(string userName);
}