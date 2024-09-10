using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NT.BL.FlowMngr;
using NT.BL.NoteMngr;
using NT.BL.PlatformMngr;
using NT.BL.ProjectMngr;
using NT.BL.RecognitionMngr;
using NT.BL.services;
using NT.BL.SessionMngr;
using NT.BL.StepMngr;
using NT.BL.UnitOfWorkPck;
using NT.BL.UserMngr;
using NT.BL.WebPlatformMngr;
using NT.DAL.EF;
using NT.DAL.FlowRep;
using NT.DAL.PlatformRep.HeadplatformPck;
using NT.DAL.PlatformRep.SharingplatformPck;
using NT.DAL.ProjectRep.ProjectPck;
using NT.DAL.ProjectRep.ThemePck;
using NT.DAL.SessionRep.RunningFlowPck;
using NT.DAL.SessionRep.SessionPck;
using NT.DAL.StepRep.AnswerPck;
using NT.DAL.StepRep.ConditionalPck;
using NT.DAL.StepRep.InformationPck;
using NT.DAL.StepRep.NotePck;
using NT.DAL.StepRep.QuestionPck;
using NT.DAL.StepRep.StepPck;
using NT.DAL.StepRep.UserAnswerPck;
using NT.DAL.User;
using NT.DAL.User.AnonymousUser;
using NT.DAL.User.ApplicationPck;
using NT.DAL.User.AttendentPck;
using NT.DAL.User.HeadOfPlatformPck;
using NT.DAL.User.OrganizationPck;
using NT.DAL.WebPlatformRep;

var builder = WebApplication.CreateBuilder(args );

#region Database

builder.Configuration.AddEnvironmentVariables();

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

string connectionString;

switch (env)
{
    case "Development":
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "../service-account-key.json");
        Environment.SetEnvironmentVariable("BUCKET_NAME", "ip-deployment-bucket");
        
        connectionString = builder.Configuration.GetConnectionString("PhygitalDbContextConnection") ??
                           throw new InvalidOperationException(
                               "Connection string 'PhygitalDbContextConnection' not found.");

        builder.Services.AddDbContext<PhygitalDbContext>(optionsBuilder =>
            optionsBuilder.UseSqlite(connectionString));
        Environment.SetEnvironmentVariable("MODERATION_GOOGLE_APPLICATION_CREDENTIALS", "../integratieproject-team19-12a263e2b620.json");
        break;
    case "Production":
        var host = Environment.GetEnvironmentVariable("NET_HOST");
        var database = Environment.GetEnvironmentVariable("NET_DATABASE");
        var port = Environment.GetEnvironmentVariable("NET_PORT");
        var username = Environment.GetEnvironmentVariable("NET_USERNAME");
        var password = Environment.GetEnvironmentVariable("NET_PASSWORD");
        connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};Include Error Detail=true";

        builder.Services.AddDbContext<PhygitalDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(connectionString)); 
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        break;
}

#endregion

builder.Services.AddControllersWithViews();

// builder.Services.AddDbContext<PhygitalDbContext>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>()
    {
        new CultureInfo("en-GB"),
        new CultureInfo("nl-BE"),
        new CultureInfo("fr-FR")
    };
    options.DefaultRequestCulture = new RequestCulture("en-GB");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

});
// User

#region User

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Tokens.EmailConfirmationTokenProvider = "Email";
        options.Tokens.AuthenticatorTokenProvider = "Authenticator";
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PhygitalDbContext>() 
    .AddDefaultTokenProviders()
    .AddTokenProvider<EmailTokenProvider<IdentityUser>>("Email")
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Authenticator");


builder.Services.Configure<DataProtectionTokenProviderOptions>(tokenOptions =>
    tokenOptions.TokenLifespan = TimeSpan.FromMinutes(10));

builder.Services.AddAuthentication().AddCookie();
builder.Services.AddAuthorization();

builder.Services.ConfigureApplicationCookie(cfg =>
{
    cfg.Events.OnRedirectToLogin += ctx =>
    {
        if (ctx.Request.Path.StartsWithSegments("/api"))
        {
            ctx.Response.StatusCode = 401;
        }

        return Task.CompletedTask;
    };
    cfg.Events.OnRedirectToAccessDenied += ctx =>
    {
        if (ctx.Request.Path.StartsWithSegments("/api"))
        {
            ctx.Response.StatusCode = 403;
        }

        return Task.CompletedTask;
    };
});

#endregion

// Repositories

#region Repositories

builder.Services.AddScoped<IFlowRepository, FlowRepository>();
builder.Services.AddScoped<IHeadplatformRepository, HeadplatformRepository>();
builder.Services.AddScoped<ISharingplatformRepository, SharingplatformRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IThemeRepository, ThemeRepository>();
builder.Services.AddScoped<IRunningFlowRepository, RunningFlowRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IConditionalPointRepository, ConditionalPointRepository>();
builder.Services.AddScoped<IInformationRepository, InformationRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IStepRepository, StepRepository>();
builder.Services.AddScoped<IUserAnswerRepository, UserAnswerRepository>();
builder.Services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IAttendentUserRepository, AttendentUserRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IHeadOfPlatformRepository, HeadOfPlatformRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IAnonymousUserRepository, AnonymousUserRepository>();

#endregion


// Managers

#region Managers
builder.Services.AddSingleton<CloudStorageService>();
builder.Services.AddSingleton<ModerationService>();
builder.Services.AddScoped<StatisticsService>();
builder.Services.AddScoped<CsvService>();
builder.Services.AddSingleton<EmailSender>();
builder.Services.AddSingleton<ColorDetection>();
builder.Services.AddScoped<IProjectManager, ProjectManager>();
builder.Services.AddScoped<IFlowManager, FlowManager>();
builder.Services.AddScoped<IGeneralUserManager, GeneralUserManager>();
builder.Services.AddScoped<ISessionManager, SessionManager>();
builder.Services.AddScoped<IStepManager, StepManager>();
builder.Services.AddScoped<IPlatformManager, PlatformManager>();
builder.Services.AddScoped<IWebPlatformManager, WebPlatformManager>();
builder.Services.AddScoped<INoteManager, NoteManager>();
builder.Services.AddScoped(x => new Lazy<IStepManager>(x.GetRequiredService<IStepManager>));

#endregion


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PhygitalDbContext>();
    if (context.CreateDatabase(databaseWipe: true))
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        DataSeeder.Seed(context, userManager, roleManager);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.MapRazorPages();

app.Run();
public partial class Program
{
}