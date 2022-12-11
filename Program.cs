using InstaAPI.Application.Context;
using InstaAPI.Application.Helpers;
using InstaAPI.Application.Models;
using InstaAPI.Application.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddDbContext<InstaDbContext>(options => options.UseInMemoryDatabase(databaseName:"InstaDb"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding all dependencies.
SetupDependencies(builder.Services);

var app = builder.Build();

AddDefaultData(app);

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void SetupDependencies(IServiceCollection _services)
{
    _services.AddScoped<IUserRepository, UserRepository>();
    _services.AddScoped<IResultBuilder, ResultBuilder>();
}

//Adding dummy data to DB
void AddDefaultData(WebApplication app){

    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<InstaDbContext>();

    using (context){
        var users = new List<UserData>{
                new UserData
                {
                    username = "Ganesh",
                    followers = "",
                    following = "",
                    posts = new List<MediaPost> {
                        new MediaPost {
                            caption = "First Post",
                            imageUrl = "https://google.com",
                            upvotes = 10
                        }
                    }
                }
            };
            context?.InstaUser.AddRange(users);
            context?.SaveChanges();
    }
        
}