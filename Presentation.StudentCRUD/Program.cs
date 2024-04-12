using Domain.StudentCRUD;
using Infrastructure.StudentCRUD;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IStudentService, StudentService>();

//Add Authentication
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

//Add Authorization
builder.Services.AddAuthorizationBuilder();

//Configure DBContext
builder.Services.AddDbContext<ApplicationDBContext>();

builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddApiEndpoints();
// Add logging
builder.Logging.AddConsole();
var app = builder.Build();

// Seed roles into the database
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Define roles
    string[] roles = { "Admin", "User" };

    // Seed roles if they don't exist
    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));

        }
    }
}


app.MapIdentityApi<AppUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
