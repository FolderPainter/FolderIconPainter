using Application.Extensions;
using Client.Infrastructure;
using FluentValidation.AspNetCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Extensions;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddControllers();
//builder.Services.AddControllers().AddFluentValidation(options =>
//{
//    // Validate child properties and root collection elements
//    options.ImplicitlyValidateChildProperties = true;
//    options.ImplicitlyValidateRootCollectionElements = true;

//    // Automatic registration of validators in assembly
//    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//});
builder.Services.AddControllersWithViews().AddFluentValidation();
//builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddHttpClient("LocalApi", client => client.BaseAddress = new Uri("https://localhost:7132/"));

builder.Services.AddClientServices();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddSignalR(e =>
{
    e.MaximumReceiveMessageSize = 102400000;
});

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureMappings();
builder.Services.AddRepositories();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
    app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.InitializeAsync(app.Configuration);

app.Run();
