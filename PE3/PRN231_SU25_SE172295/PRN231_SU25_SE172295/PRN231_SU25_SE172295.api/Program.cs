using BOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repos;
using Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//add odata
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<CheetahProfile>("CheetahProfile");

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    })
    .AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata",
        modelBuilder.GetEdmModel()));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Dependency Injection
builder.Services.AddScoped<ICheetahAccountRepo, CheetahAccountRepo>();
builder.Services.AddScoped<ICheetahProfileRepo, CheetahProfileRepo>();
builder.Services.AddScoped<ICheetahAccountService, CheetahAccountService>();
builder.Services.AddScoped<ICheetahProfileService, CheetahProfileService>();

builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});
IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

// Setup JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT:Issuer"],
            ValidAudience = configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]))
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "4567",
        policyBuilder => policyBuilder.RequireAssertion(
                       context => context.User.HasClaim(claim => claim.Type == "Role")
                                  && (context.User.FindFirst(claim => claim.Type == "Role").Value == "1"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "4"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "5"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "6"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "7")));
    options.AddPolicy(
        "56",
        policyBuilder => policyBuilder.RequireAssertion(
                       context => context.User.HasClaim(claim => claim.Type == "Role")
                                  && (context.User.FindFirst(claim => claim.Type == "Role").Value == "5"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "6")));

    options.AddPolicy(
       "1234567",
       policyBuilder => policyBuilder.RequireAssertion(
                      context => context.User.HasClaim(claim => claim.Type == "Role")
                                  && (context.User.FindFirst(claim => claim.Type == "Role").Value == "1"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "4"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "5"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "6"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "7"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "1"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "2"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "3")));

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

//use authen
app.UseAuthentication();
app.UseAuthorization();


//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
