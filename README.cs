# tạo solution (để trống tên để dễ copy)
dotnet new sln - n <name> 

# tạo 2 class library
dotnet new classlib - n Service
dotnet new classlib - n Repository

# thêm vào solution
dotnet sln add Service/Service.csproj
dotnet sln add Repository/Repository.csproj

# tham chiếu giữa các layer
dotnet add Service/Service.csproj reference Repository/Repository.csproj


///ĐÂY LÀ FILE ĐỒ NGHỀ THI PE:

// Repository
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.5
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.5
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.5
dotnet add package Microsoft.Extensions.Configuration --version 8.0.0
dotnet add package Microsoft.Extensions.Configuration.Json --version 8.0.0
dotnet add package  Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.10

// ApiLayer
dotnet add package Microsoft.AspNetCore.OData --version 8.2.5

// Service
dotnet add package FluentValidation.AspNetCore --version 8.5.1


// install offline (Mở file csproj của Repo rồi paste content của tag <ItemGroup> </ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Http.Features" Version="2.2.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.5" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.5">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive </ IncludeAssets >
      < PrivateAssets > all </ PrivateAssets >
    </ PackageReference >
    < PackageReference Include = "Microsoft.EntityFrameworkCore.SqlServer" Version = "8.0.5" />
    < PackageReference Include = "Microsoft.EntityFrameworkCore.Tools" Version = "8.0.5" >
      < IncludeAssets > runtime; build; native; contentfiles; analyzers; buildtransitive </ IncludeAssets >
      < PrivateAssets > all </ PrivateAssets >
    </ PackageReference >
    < PackageReference Include = "Microsoft.Extensions.Configuration" Version = "8.0.0" />
    < PackageReference Include = "Microsoft.Extensions.Configuration.Json" Version = "8.0.0" />

//DATABASE SCAFFOLD ( tự đổi tên server + database của đề)
dotnet ef dbcontext scaffold "Server=103.211.201.141,1433; Database=spring2025productinventorydb; User Id=sa; Password=YourStrong!Passw0rd; TrustServerCertificate=True; Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer --output-dir Entities

//BỎ ĐỐNG NÀY VÀO DBCONTEXT 
public static string GetConnectionString(string connectionStringName)
{
    var config = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

    return config.GetConnectionString(connectionStringName);
}

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));

//// TRONG appsettings.json SETUP NHƯ SAU (miễn giống với scaffold là được): 

{
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "Server=103.211.201.141,1433; Database=spring2025productinventorydb; User Id=sa; Password=YourStrong!Passw0rd; TrustServerCertificate=True; Encrypt=False"
  },
    "Jwt": {
        "Key": "0ccfeb299b126a479a64630e2d34e9e91e5fcbcaea8ac9e3347e224b0557a53e",
        "Issuer": "https://localhost:7075",
        "Audience": "https://localhost:7075"
  }
}



//====================Bước 8===============================
//PROGRAM.CS

//JSON CYCLE
static IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EntitySet<Style>("Style"); // ENTITY
    odataBuilder.EntitySet<WatercolorsPainting>("WatercolorsPainting"); // ENTITY
    return odataBuilder.GetEdmModel();
}

builder.Services.AddControllers().AddOData(options =>
{
    options.Select().Filter().OrderBy().Expand().SetMaxTop(null).Count();
    options.AddRouteComponents("odata", GetEdmModel());
});

builder.Services.AddSwaggerGen();

//DbContext
builder.Services.AddDbContext<WatercolorsPainting2024DbContext>();

//DI
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<UserAccountRepo>();
builder.Services.AddScoped<WatercolorsPaintingRepo>();
builder.Services.AddScoped<IWatercolorsPaintingService, WatercolorsPaintingService>();
builder.Services.AddValidatorsFromAssemblyContaining<WatercolorsPaintingValidator>();
builder.Services.AddScoped<IValidator<WatercolorsPainting>, WatercolorsPaintingValidator>();

//
builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddSwaggerGen(option =>
{
    ////JWT Config
    option.DescribeAllParametersInCamelCase();
    option.ResolveConflictingActions(conf => conf.First());
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
//====================Bước 9==============================
// Generic Repository/Data Access Object
#region
public class DataAccessObject<T> where T : class
{
    protected OilPaintingArt2024DbContext _context;

    // Default constructor, initializes the DbContext if null
    public DataAccessObject()
    {
        _context ??= new OilPaintingArt2024DbContext();
    }

    // Constructor that accepts a DbContext
    public DataAccessObject(OilPaintingArt2024DbContext context)
    {
        _context = context;
    }

    // Synchronous method to retrieve all entities
    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    // Asynchronous method to retrieve all entities
    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    // Synchronous method to create a new entity
    public void Create(T entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
    }

    // Asynchronous method to create a new entity
    public async Task<int> CreateAsync(T entity)
    {
        _context.Add(entity);
        return await _context.SaveChangesAsync();
    }

    // Synchronous method to update an entity
    public void Update(T entity)
    {
        var tracker = _context.Attach(entity);
        tracker.State = EntityState.Modified;
        _context.SaveChanges();
    }

    // Asynchronous method to update an entity
    public async Task<int> UpdateAsync(T entity)
    {
        var tracker = _context.Attach(entity);
        tracker.State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    // Synchronous method to remove an entity
    public bool Remove(T entity)
    {
        _context.Remove(entity);
        _context.SaveChanges();
        return true;
    }

    // Asynchronous method to remove an entity
    public async Task<bool> RemoveAsync(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    // Retrieve entity by integer ID (synchronously)
    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    // Retrieve entity by integer ID (asynchronously)
    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    // Retrieve entity by string ID (synchronously)
    public T GetById(string code)
    {
        return _context.Set<T>().Find(code);
    }

    // Retrieve entity by string ID (asynchronously)
    public async Task<T> GetByIdAsync(string code)
    {
        return await _context.Set<T>().FindAsync(code);
    }

    // Retrieve entity by Guid (synchronously)
    public T GetById(Guid code)
    {
        return _context.Set<T>().Find(code);
    }

    // Retrieve entity by Guid (asynchronously)
    public async Task<T> GetByIdAsync(Guid code)
    {
        return await _context.Set<T>().FindAsync(code);
    }

    #region Separating assign entity and save operations

    // Prepare an entity for creation (no save)
    public void PrepareCreate(T entity)
    {
        _context.Add(entity);
    }

    // Prepare an entity for update (no save)
    public void PrepareUpdate(T entity)
    {
        var tracker = _context.Attach(entity);
        tracker.State = EntityState.Modified;
    }

    // Prepare an entity for removal (no save)
    public void PrepareRemove(T entity)
    {
        _context.Remove(entity);
    }

    // Save changes synchronously
    public int Save()
    {
        return _context.SaveChanges();
    }

    // Save changes asynchronously
    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

//SAMPLE REPOSITORY
public class UserAccountRepo : DataAccessObject<UserAccount>
    {
        public UserAccountRepo(WatercolorsPainting2024DbContext context) : base(context) { }

        public async Task<UserAccount> GetByEmailAndPassword(string email, string password)
        {
            return await _context.UserAccounts.FirstOrDefaultAsync(a => a.UserEmail == email && a.UserPassword == password && i);
        }
    }

//SAMPLE INTERFACE & SERVICE
public interface IUserAccountService
{
    public Task<UserAccount> Authenticate(string username, string password);
}

public class UserAccountService : IUserAccountService
{
    private readonly UserAccountRepo _repo;

    public UserAccountService(UserAccountRepo repo)
    {
        _repo = repo;
    }

    public async Task<UserAccount> Authenticate(string username, string password)
    {
        return await _repo.GetByEmailAndPassword(username, password);
    }
}

//SAMPLE CONTROLLER
[Route("api/[controller]")]
[ApiController]
public class UserAccountController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IUserAccountService _userAccountService;

    public UserAccountController(IConfiguration configuration, IUserAccountService userAccountService)
    {
        _configuration = configuration;
        _userAccountService = userAccountService;
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _userAccountService.Authenticate(request.UserName, request.Password);

        if (user == null || user.Result == null)
            return Unauthorized();

        var token = GenerateJSONWebToken(user.Result);

        return Ok(token);
    }

    private string GenerateJSONWebToken(UserAccount systemUserAccount)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"]
                , _configuration["Jwt:Audience"]
                , new Claim[]
                {
            new(ClaimTypes.Name, systemUserAccount.UserEmail),
            new(ClaimTypes.Role, systemUserAccount.Role.ToString()),
                },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }


    public sealed record LoginRequest(string UserName, string Password);
}

// CRUD entity chính và phụ
// SAMPLE REPO: 
public class WatercolorsPaintingRepo : DataAccessObject<WatercolorsPainting>
{
    public WatercolorsPaintingRepo()
    {

    }

    public async Task<List<WatercolorsPainting>> GetAllAsync()
    {
        var items = await _context.WatercolorsPaintings.Include(i => i.Style).ToListAsync();
        return items;
    }

    public async Task<WatercolorsPainting> GetByIdAsync(string id)
    {
        var item = await _context.WatercolorsPaintings.Include(i => i.Style).FirstOrDefaultAsync(t => t.PaintingId == id);
        if (item == null)
        {
            _context.Entry(item).State = EntityState.Detached;
        }
        return item;
    }

    public async Task<List<WatercolorsPainting>> Search(int? item1, string? item2)
    {
        return await _context.WatercolorsPaintings
            .Include(i => i.Style)
            .Where(u => (string.IsNullOrEmpty(item2) || u.PaintingAuthor.ToLower().Contains(item2.ToLower()))
                        && (!item1.HasValue || u.PublishYear == item1.Value))
            .ToListAsync();
    }
}

//SEARCH
//Search điều kiện And
    public async Task<List<Team>> GetAllPaged(int PageSize, int pageNumber, int? Position, string GroupName)// 1 string, 1 int
{
    return await _context.Teams
        .Include(i => i.Group)
        .Where(u => (u.Group.GroupName.ToLower().Contains(GroupName.ToLower()) || string.IsNullOrEmpty(GroupName))
                    && ((!Position.HasValue) || (u.Position == Position.Value)))
        .Skip((pageNumber - 1) * PageSize)
        .Take(PageSize)
        .ToListAsync();
}
    //Search điều kiện OR
    public async Task<List<OilPaintingArt>> GetAllPaged(int PageSize, int pageNumber, string OilPaintingArtStyle, string Artist) // 2 string
    {
        return await _context.OilPaintingArts
            .Include(i => i.Supplier)
            .Where(u => (string.IsNullOrEmpty(OilPaintingArtStyle) && string.IsNullOrEmpty(Artist))
                        || (!string.IsNullOrEmpty(OilPaintingArtStyle) && u.OilPaintingArtStyle.ToLower().Contains(OilPaintingArtStyle.ToLower()))
                        || ((!string.IsNullOrEmpty(Artist) && u.Artist.ToLower().Contains(Artist.ToLower()))))
            .Skip((pageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }
    public async Task<List<Team>> GetAllPaged(int pageSize, int pageNumber, int? position, string groupName)// 1 string , 1 int
    {
        return await _context.Teams
            .Include(i => i.Group)
            .Where(u =>
                (!position.HasValue && string.IsNullOrEmpty(groupName))
                || (position.HasValue && u.Position == position.Value)
                || (!string.IsNullOrEmpty(groupName) && u.Group.GroupName.ToLower().Contains(groupName.ToLower())))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

//SAMPLE SERVICE & INTERFACE
//interface
public interface IWatercolorsPaintingService
{
    Task<List<WatercolorsPainting>> GetAll();
    Task<WatercolorsPainting> GetById(int id);
    Task<int> Create(WatercolorsPainting watercolorsPainting);
    Task<bool> Delete(int id);
    Task<string> CreateWithValidation(WatercolorsPainting watercolorsPainting);
    //Task<string> UpdateWithValidation(WatercolorsPainting watercolorsPainting);
}

//validator
//2)Tạo thư mục Validators trong Service, sau đó tạo file CosmeticInformationValidator.cs.

//3)trong CosmeticInformationValidator.cs.
public class WatercolorsPaintingValidator : AbstractValidator<WatercolorsPainting>
{
    public WatercolorsPaintingValidator()
    {
        RuleFor(x => x.PaintingName)
        .NotEmpty().WithMessage("WaterColor is required.")
        .Length(2, 80).WithMessage("WaterColor must be between 2 and 80 characters.")
        .Matches(@"^([A-Z][a-z0-9@#]*\s?)+$")
        .WithMessage("Each word in WaterColor must begin with a capital letter and contain only valid characters.");

        RuleFor(x => x.PaintingAuthor)
            .NotEmpty().WithMessage("PaintingAuthor is required.")
            .Length(2, 80).WithMessage("PaintingAuthor must be between 2 and 80 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}

//service
public class WatercolorsPaintingService : IWatercolorsPaintingService
{
    private readonly WatercolorsPaintingRepo _repo;
    private readonly IValidator<WatercolorsPainting> _validator;

    public WatercolorsPaintingService(WatercolorsPaintingRepo repo, IValidator<WatercolorsPainting> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public Task<int> Create(WatercolorsPainting watercolorsPainting)
    {
        watercolorsPainting.CreatedDate = DateTime.Now;
        return _repo.CreateAsync(watercolorsPainting);
    }

    public async Task<string> CreateWithValidation(WatercolorsPainting watercolorsPainting)
    {
        // Kiểm tra dữ liệu với FluentValidation
        var validationResult = await _validator.ValidateAsync(watercolorsPainting);
        if (!validationResult.IsValid)
        {
            return string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
        }

        watercolorsPainting.PaintingId = GenerateId();
        var result = await _repo.CreateAsync(watercolorsPainting);
        if (result == 1)
        {
            return "Thêm Thành công";
        }
        return "Thêm thất bại";
    }

    public string GenerateId()
    {
        return "WP" + DateTime.UtcNow.ToString("yyyyMMddHHmmss").Substring(0, 3) + Guid.NewGuid().ToString("N").Substring(0, 3);
    }

    public async Task<bool> Delete(string id)
    {
        var item = _repo.GetById(id);
        return await _repo.RemoveAsync(item);
    }

    public async Task<List<WatercolorsPainting>> GetAll()
    {
        return await _repo.GetAllAsync();
    }

    public async Task<WatercolorsPainting> GetById(string id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<List<WatercolorsPainting>> Search(int? item1, string? item2)
    {
        return await _repo.Search(item1, item2);
    }


}

//SAMPLE CONTROLLER
[Route("api/[controller]")]
[ApiController]
public class WatercolorsPaintingController : Controller
{
    private readonly IWatercolorsPaintingService _watercolorsPaintingService;
    public WatercolorsPaintingController(IWatercolorsPaintingService watercolorsPaintingService)
    {
        _watercolorsPaintingService = watercolorsPaintingService;
    }

    [HttpGet("search")]
    [Authorize(Roles = "1,2")]
    public async Task<IEnumerable<WatercolorsPainting>> Get(string? author, int? date)
    {
        return await _watercolorsPaintingService.Search(date, author);
    }
    [HttpGet]
    [Authorize(Roles = "1,2")]
    [EnableQuery]
    public async Task<IEnumerable<WatercolorsPainting>> Get()
    {
        return await _watercolorsPaintingService.GetAll();
    }


    [HttpGet("{id}")]
    [Authorize(Roles = "1,2")]

    public async Task<WatercolorsPainting> Get(string id)
    {
        return await _watercolorsPaintingService.GetById(id);
    }

    //[HttpPost]
    //[Authorize(Roles = "1")]
    //public async Task<IActionResult> Post(WatercolorsPainting transaction)
    //{
    //    var result = await _watercolorsPaintingService.CreateWithValidation(transaction);
    //    if (result.Contains("Thêm Thành công"))
    //    {
    //        return Ok(new
    //        {
    //            Message = "Create successful",
    //            Data = result
    //        });
    //    }
    //    return BadRequest(new
    //    {
    //        Message = "Validation failed",
    //        Errors = result
    //    });
    //}

    //[HttpPut()]
    //[Authorize(Roles = "1")]
    //public async Task<IActionResult> Put(WatercolorsPainting watercolorsPainting)
    //{
    //    var result = await _watercolorsPaintingService.UpdateWithValidation(transaction);
    //    if (result.Contains("Edit thành công"))
    //    {
    //        return Ok(new
    //        {
    //            Message = "Edit successful",
    //            Data = result
    //        });
    //    }
    //    return BadRequest(new
    //    {
    //        Message = "Validation failed",
    //        Errors = result
    //    });
    //}

    [HttpDelete("{id}")]
    [Authorize(Roles = "1")]
    public async Task<bool> Delete(string id)
    {
        return await _watercolorsPaintingService.Delete(id);
    }
}
//-----------FE----------------
//1) tạo project name
ASP.NET Core web app(Model-View-Controller)

//2) add package:
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.3.0

//3) Program.cs
builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = new PathString("/Account/Login");
options.AccessDeniedPath = new PathString("/Account/Forbidden");
options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    });

app.UseAuthentication();//phải đúng thứ tự như này, khác là sai
app.UseAuthorization();

//4) tao folder Account -> Login.cshtml(Trong folder Views)


//5) them trong layout của folder Shared
 <div class= "nav-item text-nowrap text-success" >
     Welcome
     < strong > @Context.Request.Cookies["UserName"].ToString() </ strong >
     |  @*<a href="/Account/Logout">LogOut</a>*@
     < a asp - controller = "Account" asp - action = "Logout" class= "text-danger" > Logout </ a >
 </ div >

//6) tạo folder entity chính (nhớ thêm s ở cuối tên) trong folder Views => add view có entity vô mượn tạm Model Repository

//7) Bỏ vô index.cshtml của entity chính
<form method="get" asp-action="Index" class= "mb-3" >
    < div class= "form-row search" >
        < div class= "form-group col-md-3" >
            < label > TransactionType </ label >
            < input type = "text" name = "transactionType" class= "form-control" value = "@Context.Request.Query["transactionType"]" />
        </div>
        <div class= "form-group col-md-3" >
            < label > Amount </ label >
            < input type = "number" name = "amount" class= "form-control" value = "@Context.Request.Query["amount"]" />
        </div>
        <div class= "form-group col-md-3" >
            < label > PaymentMethod </ label >
            < input type = "text" name = "paymentMethod" class= "form-control" value = "@Context.Request.Query["paymentMethod"]" />
        </div>
        <div class= "form-group col-md-3 align-self-end" >
            < button type = "submit" class= "btn btn-primary" > Search </ button >
        </ div >
    </ div >
</ form >

//============================trong Folder Controller======================================
//================================ AccountController.cs =================================
//++++Tao controller cho Account cho Controllers.
public class AccountController : Controller
{
    private string APIEndPoint = "https://localhost:7140/api/";
    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest login)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsJsonAsync(APIEndPoint + "UserAccount/Login", login))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var tokenString = await response.Content.ReadAsStringAsync();

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var jwtToken = tokenHandler.ReadToken(tokenString) as JwtSecurityToken;

                        if (jwtToken != null)
                        {
                            var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                            var roleId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Role, roleId),
                    };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                            Response.Cookies.Append("UserName", userName);
                            Response.Cookies.Append("Role", roleId);
                            Response.Cookies.Append("TokenString", tokenString);

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }

        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        ModelState.AddModelError("", "Login failure");
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //delete cookies
        Response.Cookies.Delete("UserName");
        Response.Cookies.Delete("Role");
        Response.Cookies.Delete("TokenString");

        return RedirectToAction("Login", "Account");
    }

    public async Task<IActionResult> Forbidden()
    {
        return View();
    }
}

//2) LoginRequest.cs trong models
public class LoginRequest
{
    public string UserName { get; set; }
    public string Password {  get; set; }
}

//----------------View Login---------------------------------
@model Login.MVCWebApp.Models.LoginRequest // lay trong loginRequest cua models

@{
    Layout = null;
}

<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Đăng nhập</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
 <style>
     body {
         height: 100vh;
         display: flex;
         justify-content: center;
         align-items: center;
     }

     .login-container {
         background: white;
         padding: 30px;
         border-radius: 10px;
         box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
         width: 350px;
     }
 </style>
</head>
<body>
    <div class="login-container">
        <h2 class="login-title">Đăng nhập</h2>
        <form asp-action="Login">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <div class="mb-3">
                <label asp-for="UserName" class="form-label"></label>
                <input asp-for="UserName" class="form-control" placeholder="Nhập tài khoản" />
                <span asp-validation-for="UserName" class="text-danger"></span>
            </div>
            <div class="mb-3">
                <label asp-for="Password" class="form-label"></label>
                <input type="password" asp-for="Password" class="form-control" placeholder="Nhập mật khẩu" />
                <span asp-validation-for="Password" class="text-danger"></span>
            </div>
            <div class="mb-3 text-center">
                <input type="submit" value="Đăng nhập" class="btn btn-primary" />
            </div>
        </form>
    </div>
</body>
</html>
//================================TransactionController.cs=================================

//1) thêm vào đầu của 2 cái controller  => Nhớ có homeController nha
 [Authorize]

//2) thêm endpoint vào contructor============================================================================
private string APIEndPoint = "https://localhost:7140/api/";
public TransactionsController() { }

//3) thêm search=============================================================================
public async Task<IActionResult> Index(string? transactionType, string? amount, string? paymentMethod)
{
    using (var httpClient = new HttpClient())
    {
        #region Add Token to header of Request

        var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;

        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

        #endregion

        var queryParams = new List<string>();
        if (!string.IsNullOrEmpty(transactionType))
            queryParams.Add($"transactionType={Uri.EscapeDataString(transactionType)}");
        if (!string.IsNullOrEmpty(amount))
            queryParams.Add($"amount={Uri.EscapeDataString(amount)}");
        if (!string.IsNullOrEmpty(paymentMethod))
            queryParams.Add($"paymentMethod={Uri.EscapeDataString(paymentMethod)}");

        string endpoint = "Transaction";
        if (queryParams.Count > 0)
            endpoint += "/search?" + string.Join("&", queryParams);

        using (var response = await httpClient.GetAsync(APIEndPoint + endpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Transaction>>(content);

                if (result != null)
                {
                    return View(result);
                }
            }
        }
    }

    return View(new List<Transaction>());
}
4) them vào layout
<li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Transactions" asp-action="Index">Transaction</a>
                        </li>

//đổi endpoint https://localhost:7273/api================ //!important
// we can stop here
//-----------VALIDATION--------
#region Required Attributes

[Required]
[StringLength(max, MinimumLength = min)]
[Range(min, max)]

#endregion

#region Basic Patterns

// Letters Only
[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]

// Letters and Spaces Only
[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]

// Letters and Numbers Only
[RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Only letters and numbers are allowed.")]

// Numbers Only
[RegularExpression(@"^\d+$", ErrorMessage = "Only numeric characters are allowed.")]

#endregion

#region Length Constraints

// Minimum Characters
[MinLength(5, ErrorMessage = "Minimum 5 characters required.")]

// Maximum Characters
[MaxLength(50, ErrorMessage = "Maximum 50 characters allowed.")]

// Specific Length Range
[RegularExpression(@"^.{5,10}$", ErrorMessage = "The input must be between 5 and 10 characters long.")]

// No Spaces, Min Length 6
[RegularExpression(@"^\S{6,}$", ErrorMessage = "The input must be at least 6 characters long and must not contain spaces.")]

#endregion

#region Character Requirements

// Contains Upper and Lower Case
[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z]).+$", ErrorMessage = "The input must contain at least one uppercase letter and one lowercase letter.")]

// Letters Only, No Numbers
[RegularExpression(@"^(?=.*[a-zA-Z])(?!.*\d).+$", ErrorMessage = "The input must contain at least one letter and no numbers.")]

// Special Character, No Spaces
[RegularExpression(@"^(?=.*[@#$&])\S+$", ErrorMessage = "The input must contain at least one special character and no spaces.")]

// Special Character, No Numbers
[RegularExpression(@"^(?=.*[@#$&])\D+$", ErrorMessage = "The input must contain at least one special character and no numbers.")]

// At Least One Letter, One Number, No Special Characters
[RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z0-9]+$", ErrorMessage = "The input must contain at least one letter and one number, and no special characters.")]

#endregion

#region Number Constraints

// Only Positive Integers
[RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only positive integers are allowed.")]

// Only Non-Negative Integers (including zero)
[RegularExpression(@"^\d+$", ErrorMessage = "Only non-negative integers are allowed.")]

// Specific Range of Numbers (1-100)
[Range(1, 100, ErrorMessage = "Number must be between 1 and 100.")]

// Decimal Number with 2 Decimal Places
[RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only decimal numbers up to 2 decimal places are allowed.")]

// Positive or Negative Integers
[RegularExpression(@"^-?\d+$", ErrorMessage = "Only integers, positive or negative, are allowed.")]

// Positive or Negative Decimal Numbers
[RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = "Only decimal numbers, positive or negative, are allowed.")]

#endregion

#region Format-Specific Patterns

// Capitalized Words (3-10 characters)
[RegularExpression(@"^[A-Z][a-zA-Z0-9]{2,9}$", ErrorMessage = "Each word must start with a capital letter, and be 3-10 characters long.")]

// Capitalized Words with Special Characters Allowed
[RegularExpression(@"^(?:[A-Z][a-zA-Z0-9@#$&()]*\s?){1,9}$", ErrorMessage = "Each word must start with a capital letter and be less than 10 characters, allowing letters, numbers, and special characters.")]

// Password with Upper, Lower, Number, and Special Character
[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]

// Date Format dd/MM/yyyy
[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "Date must be in the format dd/MM/yyyy.")]

#endregion

#region Common Data Annotations

// Email
[EmailAddress(ErrorMessage = "Invalid email format.")]

// Phone
[Phone(ErrorMessage = "Invalid phone number.")]

// Credit Card
[CreditCard(ErrorMessage = "Invalid credit card number.")]

// Password Confirmation
[Compare("Password", ErrorMessage = "Passwords do not match.")]

// Date Range
[Range(typeof(DateTime), "01/01/2020", "01/01/2030", ErrorMessage = "Date must be between 2020 and 2030.")]

#endregion









