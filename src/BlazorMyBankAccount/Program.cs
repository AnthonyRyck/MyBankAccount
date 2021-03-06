using BlazorMyBankAccount.Areas.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args); 

#if (RELEASEDOCKER)
    string connectionDb = builder.Configuration.GetConnectionString("MySqlConnection");

    // *** Dans le cas ou une utilisation avec DOCKER
    // *** voir post sur : https://www.ctrl-alt-suppr.dev/2021/02/01/connectionstring-et-image-docker/
    string databaseAddress = Environment.GetEnvironmentVariable("DB_HOST");
    string login = Environment.GetEnvironmentVariable("LOGIN_DB");
    string mdp = Environment.GetEnvironmentVariable("PASSWORD_DB");
    string dbName = Environment.GetEnvironmentVariable("DB_NAME");

    connectionDb = connectionDb.Replace("USERNAME", login)
                            .Replace("YOURPASSWORD", mdp)
                            .Replace("YOURDB", dbName)
                            .Replace("YOURDATABASE", databaseAddress);
#elif DEBUG
string connectionDb = "server=127.0.0.1;user id=root;password=PassDevAccountBank;database=banking";
#else
string connectionDb = builder.Configuration.GetConnectionString("MySqlConnection");
#endif

// Pour les tables Indentity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionDb, ServerVersion.AutoDetect(connectionDb)));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Pour les tables "banking"
builder.Services.AddDbContextPool<bankingContext>(opt => opt.UseMySql(connectionDb, ServerVersion.AutoDetect(connectionDb)));

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

// Services Radzen
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

// Services pour l'app
// Acces data
builder.Services.AddScoped<ICompteBanking, CompteBanking>();
builder.Services.AddScoped<IBudgetBanking, BudgetBanking>();
builder.Services.AddScoped<ISuiviCompteData, SuiviCompteData>();
builder.Services.AddScoped<ITransactionData, TransactionData>();
builder.Services.AddScoped<IConfigBanking, ConfigBanking>();
// pour les composants/pages
builder.Services.AddScoped<IHomePage, HomePageViewModel>();
builder.Services.AddScoped<IUsersViewModel, UsersViewModel>();
builder.Services.AddScoped<ICompteConfig, CompteConfigViewModel>();
builder.Services.AddScoped<IBudgetConfig, BudgetConfigViewModel>();
builder.Services.AddScoped<ISuiviCompteViewModel, SuiviCompteViewModel>();
builder.Services.AddScoped<IConfigTransaction, ConfigTransactionViewModel>();
builder.Services.AddScoped<IAppConfig, AppConfigViewModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Pour forcer l'application en Francais.
var cultureInfo = new CultureInfo("fr-Fr");
cultureInfo.NumberFormat.CurrencySymbol = "???";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Vrai si la base de donnees est a creer, false si elle existait deja.
    if (db.Database.EnsureCreated())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var dbContext = scope.ServiceProvider.GetRequiredService<bankingContext>();

        var databaseCreator = dbContext.GetService<IRelationalDatabaseCreator>();
        databaseCreator.CreateTables();

        // Ajout dans la base de l'utilisateur "root"
        await DataInitializer.InitIdentityData(roleManager, userManager);
        // Ajout donn??e de base pour sch??ma Bank
        await DataInitializer.InitDataBank(dbContext);
    }
}

// Pour les logs.
// ATTENTION : il faut que la table Logs (cree par Serilog) soit faites APRES
// la creation des tables ASP, sinon "db.Database.EnsureCreated" considere que la
// base est deja creee.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.MySQL(connectionDb, "Logs")
    .CreateLogger();

app.Run();