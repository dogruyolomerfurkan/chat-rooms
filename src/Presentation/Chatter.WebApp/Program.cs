using Chatter.Identity.Entities;
using Chatter.Identity.Extensions;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureIdentity(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UpdateIdentityDb();
app.SeedIdentity();

app.Run();

