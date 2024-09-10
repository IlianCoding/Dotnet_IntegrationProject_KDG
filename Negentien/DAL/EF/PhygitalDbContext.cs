using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.platformpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.BL.Domain.sessionpck;
using NT.BL.Domain.users;
using NT.BL.Domain.webplatformpck;

namespace NT.DAL.EF;

public class PhygitalDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Flow> Flows { get; set; }
    public DbSet<ConditionalPoint> ConditionalPoints { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<Theme> Themes { get; set; }
    public DbSet<AnswerOpen> AnswerOpen { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<QuestionWithOption> QuestionWithOptions { get; set; }
    public DbSet<QuestionOpen> OpenQuestions { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<AttendentUser> AttendentUsers { get; set; }
    public DbSet<HeadOfPlatformUser> HeadOfPlatformUsers { get; set; }
    public DbSet<OrganizationUser> OrganisationUsers { get; set; }
    public DbSet<AnonymousUser> AnonymousUsers { get; set; }
    public DbSet<Content> Content { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<InformationContent> InformationContent { get; set; }
    public DbSet<RunningFlow> RunningFlows { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<LikedComment> LikedComments { get; set; }
    public DbSet<Note> Notes { get; set; }

    public PhygitalDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "Phygital.db");

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (!optionsBuilder.IsConfigured)
        {
            switch (env)
            {
                case "Development":
                    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "../service-account-key.json");
                    Environment.SetEnvironmentVariable("PROJECT_ID", "integratieproject1-424411");
                    Environment.SetEnvironmentVariable("BUCKET_NAME", "ip-deployment-bucket");
                    
                    optionsBuilder.UseSqlite($"Data Source={dbPath}");

                    break;
                case "Production":
                    var host = Environment.GetEnvironmentVariable("NET_HOST");
                    var database = Environment.GetEnvironmentVariable("NET_DATABASE");
                    var port = Environment.GetEnvironmentVariable("NET_PORT");
                    var username = Environment.GetEnvironmentVariable("NET_USERNAME");
                    var password = Environment.GetEnvironmentVariable("NET_PASSWORD");
                    dbPath =
                        $"Host={host};Port={port};Database={database};Username={username};Password={password};Include Error Detail=true";

                    optionsBuilder.UseNpgsql(dbPath);
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                    AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
                    // optionsBuilder.UseNpgsql(connectionString, x => x.MigrationsHistoryTable("_EfMigrations", "public")));
                    break;
            }
        }

        optionsBuilder.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Session

        modelBuilder.Entity<Session>()
            .Property(s => s.StartTime)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

        modelBuilder.Entity<Session>()
            .Property(s => s.EndTime)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

        modelBuilder.Entity<RunningFlow>()
            .Property(r => r.RunningFlowTime)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );


        modelBuilder.Entity<Session>()
            .Property(r => r.ExecusionTime)
            .HasColumnType("time")
            .HasConversion(
                timeOnly => timeOnly.ToTimeSpan(),
                timeSpan => TimeOnly.FromTimeSpan(timeSpan)
            );


        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.Session)
            .WithMany(s => s.UserAnswers);

        modelBuilder.Entity<AnonymousUser>()
            .HasOne(s => s.Session)
            .WithOne(au => au.EndUser)
            .HasForeignKey<AnonymousUser>(au => au.Id);

        modelBuilder.Entity<Session>()
            .HasOne(s => s.ApplicationUser)
            .WithMany(au => au.Sessions);

        #endregion

        #region Question Config

        modelBuilder.Entity<QuestionWithOption>()
            .HasMany(q => q.AnswerOptions)
            .WithOne()
            .HasForeignKey(a => a.QuestionId)
            .IsRequired(false);

        modelBuilder.Entity<QuestionOpen>()
            .HasMany(q => q.AnswerOpen)
            .WithOne()
            .HasForeignKey(a => a.QuestionId)
            .IsRequired(false);

        #endregion

        #region Context config

        modelBuilder.Entity<Step>()
            .HasKey(st => st.Id);
        modelBuilder.Entity<Step>()
            .HasOne(st => st.Content);

        #endregion
    }


    public bool CreateDatabase(bool databaseWipe)
    {
        if (databaseWipe)
        {
            Database.EnsureDeleted();
        }

        return Database.EnsureCreated();
    }
}