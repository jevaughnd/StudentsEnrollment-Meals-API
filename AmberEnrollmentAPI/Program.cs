using AmberEnrollmentAPI;
using AmberEnrollmentAPI.Data;
using AmberEnrollmentAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//-------------------------------------------------New Things


//Add Identity User // Store -------
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 5;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//add ------
builder.Services.AddTransient<IAuthService, AuthService>();



// Dipendency Injection for ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("JevConnection")));





//Authorization -------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.Requirements.Add(new AdminAcces())); //generate new type, after auth api controller
});






//Authentication ------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JWTConfig: Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JWTConfig: Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTConfig:Key").Value))
    };
});
//------------------






var app = builder.Build();




//Enable the serving of static files: 1
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath,
                                            "api/server/StudentUploads")),
    RequestPath = "/images/api/server/StudentUploads" // Change this to the URL path you want to use
});


//Enable the serving of static files: 2
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath,
                                            "api/server/menuUploads")),
    RequestPath = "/images/api/server/menuUploads" // Change this to the URL path you want to use
});



//Enable the serving of static files: 3
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath,
                                            "api/server/menu-upload-upadted")),
    RequestPath = "/images/api/server/menu-upload-upadted" // Change this to the URL path you want to use
});





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Added
app.UseAuthentication();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace AmberEnrollmentAPI
{
    class AdminAcces : IAuthorizationRequirement
    {
    }
}