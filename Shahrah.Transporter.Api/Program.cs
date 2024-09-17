using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shahrah.Framework.Extensions;
using Shahrah.Transporter.Api.Extensions.ServiceCollection;
using Shahrah.Transporter.Application;
using Shahrah.Transporter.Application.Common.Models;
using Shahrah.Transporter.Infrastructure;
using Shahrah.Transporter.Infrastructure.Persistence;
using SlimMessageBus;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
var appSettings = appSettingsSection.Get<AppSettings>();
builder.Services.Configure<AppSettings>(appSettingsSection);
builder.Services.AddSingleton(appSettings);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMediatR(Assembly.Load("Shahrah.Transporter.Application"));
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Shahrah.Transporter.Application"));

builder.Services.AddServices();
builder.Services.AddExternalServices();
builder.Services.AddPush(builder.Configuration);
builder.Services.AddCache(builder.Configuration);
builder.Services.AddCrossOrigin(builder.Configuration);
builder.Services.AddApiDoc();
builder.Services.AddPayment(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddQuartz(builder.Configuration);
builder.Services.AddGraphQlClient(builder.Configuration);
builder.Services.RegisterServices();

builder.Services.AddMessageBus(builder.Configuration);

builder.Host.UseLogging();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Initialise and seed database
    using var scope = app.Services.CreateScope();
    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initialiser.InitialiseAsync();
}

app.UseExceptionHandling();

app.UseRouting();

app.UseCrossOrigin(builder.Configuration);

// Auth
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireAuthorization();
});

app.UseApiDoc();

app.UseHttpsRedirection();

app.UsePush();

app.UsePayment();

app.Services.GetRequiredService<IMessageBus>();

app.Run();