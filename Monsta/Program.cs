using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Monsta.Data;
using Monsta.Services;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});

builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<AppService>();

builder.Services.AddScoped<HttpClient>();

builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("Settings:Token").Value)),

            ValidateIssuer = false,

            ValidateAudience = false,
        };

        // Other configs...
       options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                // Call this to skip the default logic and avoid using the default response
                context.HandleResponse();

                // Write to the response in any way you wish
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                context.Response.Headers.Append("my-custom-header", "custom-value");

                var errorObject = new
                {
                    message = "User Authentication failed."
                };

                var errorJson = JsonConvert.SerializeObject(errorObject);
                var errorBytes = Encoding.UTF8.GetBytes(errorJson);

                await context.Response.Body.WriteAsync(errorBytes, 0, errorBytes.Length);
            }
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors(x => x.AllowAnyHeader()
      .AllowAnyMethod()
      //.WithOrigins("http://localhost:4200")
      .AllowAnyOrigin()
      );

//use authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
