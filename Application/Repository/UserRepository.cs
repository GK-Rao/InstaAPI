
using InstaAPI.Application.Context;
using InstaAPI.Application.Mapper;
using InstaAPI.Application.Models;
using InstaAPI.Application.Models.Payload;
using Microsoft.EntityFrameworkCore;

namespace InstaAPI.Application.Repository;

public class UserRepository : IUserRepository
{

    private readonly InstaDbContext context;

    public UserRepository(InstaDbContext _context)
    {
        context = _context;
    }

    public async Task<BaseResponse> CreateUser(string userName)
    {

        if (!IsUserExists(userName))
        {
            var user = new UserData()
            {
                username = userName
            };

            var res = await context.InstaUser.AddAsync(user);
            await context.SaveChangesAsync();

            return new SignUpResponse
            {
                userName = user.username,
            };
        }

        return new SignUpResponse
        {
            Exception = new InstaException
            {
                Status ="failure",
                Reason = $"User: {userName} already exists"
            }
        };

    }

    public async Task<BaseResponse> FetchUser(string userName)
    {
        var userInfo = await context.InstaUser.Include(user => user.posts).FirstOrDefaultAsync(x => x.username.ToLower() == userName.ToLower());

        return BuildFetchUserResponse(userInfo);
    }

    public async Task<BaseResponse> FollowUser(string follower, string followee)
    {
        if (!IsUserExists(follower))
        {
            return new BaseResponse
            {
                Exception = new InstaException
                {
                    Status = "failure",
                    Reason = $"User: {follower} does not exists"
                }
            };
        }

        if (!IsUserExists(followee))
        {
            return new BaseResponse
            {
                Exception = new InstaException
                {
                    Status = "failure",
                    Reason = $"User: {followee} does not exists"
                }
            };
        }

        var followerData = context.InstaUser.FirstOrDefault(x => x.username.ToLower() == follower.ToLower());
        var followeeData = context.InstaUser.FirstOrDefault(x => x.username.ToLower() == followee.ToLower());

        if ((bool)followerData?.following?.Contains(followee))
        {
            return new BaseResponse
            {
                Exception = new InstaException
                {
                    Status = "failure",
                    Reason = $"You are already following {followee}"
                }
            };
        }

        followerData.following = followerData.following.Length > 0 ?  followerData.following + $",{followee}" : followerData.following + $"{followee}";
        followeeData.followers = followerData.followers.Length > 0 ? followerData.followers + $",{follower}" : followerData.followers + $"{follower}";

        await context.SaveChangesAsync();

        return new FollowUserResponse
        {
            status = "success",
        };

    }

    public async Task<BaseResponse> UploadPost(string userName, CreatePostRequestPayload requestPayload)
    {
        if (!IsUserExists(userName))
        {
            return new BaseResponse
            {
                Exception = new InstaException
                {
                    Status = "failure",
                    Reason = $"User: {userName} does not exists"
                }
            };
        }

        var newPost = new MediaPost
        {
            userId = GetUserId(userName),
            caption = requestPayload.caption,
            imageUrl = requestPayload.imageUrl,
        };

        var result = await context.MediaPosts.AddAsync(newPost);
        await context.SaveChangesAsync();

        return new UploadPostResponse { post = newPost };
    }


    public async Task<BaseResponse> FetchAllPosts(string userName)
    {
        if (!IsUserExists(userName))
        {
            return new BaseResponse
            {
                Exception = new InstaException
                {
                    Status = "failure",
                    Reason = $"User: {userName} does not exists"
                }
            };
        }

        var allFollowers = context.InstaUser.Where(x => x.username.ToLower() == userName.ToLower()).Select(e => e.following).ToArray();
        var followersUserIds = context.InstaUser.Where(e => allFollowers.Contains(e.username)).Select(x => x.Id.ToString()).ToArray();

        var allPosts = context.MediaPosts.Where(x => followersUserIds.Contains(x.userId.ToString())).ToList();

        return new FetchAllPostsResponse
        {
            allPosts = allPosts,
        };
    }


    #region Private Methods

    private BaseResponse BuildFetchUserResponse(UserData userInfo)
    {
        if (userInfo == null)
        {
            throw new Exception("user not exists");

            return new BaseResponse
            {
                Exception = new InstaException
                {
                    Status = "Failure",
                    Reason = $"User does not exists"
                }
            };
        }

        return new FetchUserResponse
        {
            userName = userInfo.username,
            allFollowers = userInfo.followers,
            allFollowing = userInfo.following,
            posts = userInfo.posts,
        };
    }

    private bool IsUserExists(string userName)
    {
        var result = context.InstaUser.FirstOrDefault(x => x.username.ToLower() == userName.ToLower());

        return result != null ? true : false;

    }

    private int GetUserId(string userName)
    {
        var userDetails = context.InstaUser.FirstOrDefault(x => x.username.ToLower() == userName.ToLower());

        return userDetails != null ? userDetails.Id : default;
    }

    #endregion Private Methods
}