using MudBlazor.Services;
using UseCaseDrivenDevelopment.CaseManagement.Compiler;
using UseCaseDrivenDevelopment.CaseManagement.Service;
using UseCaseDrivenDevelopment.CaseManagement.Test.Service;

namespace UseCaseDrivenDevelopment.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        // application services
        builder.Services.AddSingleton<RuntimeService>();
        builder.Services.AddSingleton<CaseService>();
        builder.Services.AddSingleton<CaseFieldService>();
        builder.Services.AddSingleton<CaseValueService>();
        builder.Services.AddSingleton<CaseAvailableTestService>();
        builder.Services.AddSingleton<CaseBuildTestService>();
        builder.Services.AddSingleton<CaseValidateTestService>();

        // mud blazor
        builder.Services.AddMudServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}