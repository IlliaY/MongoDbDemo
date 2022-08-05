using System;
using Microsoft.AspNetCore.Mvc;
using MongoDbDemo;
using MongoDbDemo.Entities;
using MongoDbDemo.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/Users/Get", async (IUserRepository repository) =>
{
    var users = await repository.GetAllAsync();
    return users;
});

app.MapGet("/api/Users/Get/{id}", async (IUserRepository repository, [FromQuery] Guid id) =>
{
    var users = await repository.FindAsync(u => u.Id == id);
    return users.FirstOrDefault();
});

app.MapPost("/api/Users/Add", async (IUserRepository repository, [FromBody] User user) =>
{
    await repository.InsertAsync(user);
});

app.MapPut("/api/Users/Update", async (IUserRepository repository, [FromBody] User user) =>
{
    await repository.UpdateAsync(user);
});

app.MapDelete("/api/Users/Delete", async (IUserRepository repository, [FromBody] User user) =>
{
    await repository.DeleteAsync(user);
});

app.Run();
