using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using NguyenPhuocAn_SE17D10_A01.DataAccess;
using NguyenPhuocAn_SE17D10_A01.Models;
using NguyenPhuocAn_SE17D10_A01.Repositories;
using NguyenPhuocAn_SE17D10_A01.Services;
using StudentName_ClassCode_A01_BE.Services;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configure OData
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<Account>("Accounts");
odataBuilder.EntitySet<Category>("Categories");
odataBuilder.EntitySet<NewsArticle>("NewsArticles");
odataBuilder.EntitySet<Tag>("Tags");

builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", odataBuilder.GetEdmModel()).Filter().Select().Expand().OrderBy().Count());

// Configure Entity Framework Core
builder.Services.AddDbContext<NewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories (Scoped)
builder.Services.AddScoped<IRepository<Account>, AccountRepository>();
builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IRepository<NewsArticle>, NewsArticleRepository>();
builder.Services.AddScoped<IRepository<Tag>, TagRepository>();

// Register Services (Singleton)
builder.Services.AddSingleton<CategoryService>(sp =>
    CategoryService.Instance(sp.GetRequiredService<IRepository<Category>>()));
builder.Services.AddSingleton<AccountService>(sp =>
    AccountService.Instance(sp.GetRequiredService<IRepository<Account>>()));
builder.Services.AddSingleton<NewsArticleService>(sp =>
    NewsArticleService.Instance(sp.GetRequiredService<IRepository<NewsArticle>>()));
builder.Services.AddSingleton<TagService>(sp =>
    TagService.Instance(sp.GetRequiredService<IRepository<Tag>>()));

// Register the hosted service
builder.Services.AddHostedService<DatabaseInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
