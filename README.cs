///ĐÂY LÀ FILE ĐỒ NGHỀ THI PE:

// install online
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.5
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.5
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.5
dotnet add package Microsoft.Extensions.Configuration --version 8.0.0
dotnet add package Microsoft.Extensions.Configuration.Json --version 8.0.0
dotnet add package  Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.10


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
dotnet ef dbcontext scaffold "Server=DEVPHUCTRANN; Database=VaccinaCareDb; User Id=sa; Password=12345; TrustServerCertificate=True; Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer

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

// TRONG appsettings.json SETUP NHƯ SAU (miễn giống với scaffold là được): 

    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "Server=DEVPHUCTRANN; Database=Fall24PharmaceuticalDB; User Id=sa; Password=12345; TrustServerCertificate=True;Encrypt=False;"
  },
    "Jwt": {
        "Key": "0ccfeb299b126a479a64630e2d34e9e91e5fcbcaea8ac9e3347e224b0557a53e",
        "Issuer": "https://localhost:7075",
        "Audience": "https://localhost:7075"
  }


====================Bước 8===============================
//PROGRAM.CS

//JSON CYCLE
builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});

//Authen
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

====================Bước 9==============================
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
#endregion


//SAMPLE REPOSITORY
public class AccountRespository : DataAccessObject<SystemAccount>
{
    public AccountRespository(OilPaintingArt2024DbContext context) : base(context) { }

    public SystemAccount GetByEmailAndPassword(string email, string password)
    {
        return _context.SystemAccounts.FirstOrDefault(a => a.AccountEmail == email && a.AccountPassword == password);
    }

}


//SEARCH
//
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









