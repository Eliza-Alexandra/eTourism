# Ghid Complet TermoPortal - Pentru Începători în Blazor

## Cuprins
1. [Introducere pentru Dezvoltatori Web](#introducere-pentru-dezvoltatori-web)
2. [Arhitectura Generală Explicată Simplu](#arhitectura-generală-explicată-simplu)
3. **TermoPortalWebApp - Frontend Blazor** (Detalii Complete)
4. **BackendServer - API Server** (Detalii Complete)
5. **DataLayer - Stratul de Date** (Detalii Complete)
6. **Core - Biblioteca Centrală** (Detalii Complete)
7. [Ghid Practic - Cum să Adaugi Funcționalități](#ghid-practic---cum-să-adaugi-funcționalități)
8. [Exemple Concrete de Cod](#exemple-concrete-de-cod)
9. [Depanare și Probleme Comune](#depanare-și-probleme-comune)

---

## Introducere pentru Dezvoltatori Web

### Ce este Blazor?
Dacă știi HTML, CSS și JavaScript, Blazor este o tehnologie Microsoft care îți permite să scrii aplicații web interactive folosind C# în loc de JavaScript. Gândește-te la Blazor ca la React sau Angular, dar cu C#.

### De ce Blazor Server?
- **Server-Side Rendering**: Codul C# rulează pe server, nu în browser
- **Real-time Updates**: Interfața se actualizează automat prin SignalR
- **Full C#**: Nu mai ai nevoie de JavaScript pentru logica aplicației

### Analogii cu Tehnologii Cunoscute:
| Blazor | React/Vue | ASP.NET MVC |
|--------|-----------|-------------|
| Component | Component | Controller + View |
| @page | Route | [Route] attribute |
| @inject | useContext | Dependency Injection |
| MudBlazor | Material-UI | Bootstrap |

---

## Arhitectura Generală Explicată Simplu

Imaginează-ți aplicația ca pe o clădire cu mai multe etaje:

```
┌─────────────────────────────────────┐
│   TermoPortalWebApp (Etajul 4)      │  ← Interfața utilizator (ce vezi)
│   - Pagini web interactive           │
│   - Butoane, formulare, meniuri      │
├─────────────────────────────────────┤
│   BackendServer (Etajul 3)           │  ← Servicii și API
│   - Procesare date                   │
│   - Autentificare utilizatori        │
│   - Calculuri complexe               │
├─────────────────────────────────────┤
│   DataLayer (Etajul 2)               │  ← Management baze de date
│   - Conexiuni la database            │
│   - Salvare/încărcare date           │
├─────────────────────────────────────┤
│   Core (Etajul 1)                    │  ← Fundația logică
│   - Modele de date                   │
│   - Reguli de business               │
└─────────────────────────────────────┘
```

**Fluxul datelor**: Utilizator dă click → Frontend trimite la Backend → Backend cere date de la DataLayer → DataLayer accesează baza de date → Răspunsul urcă înapoi la utilizator.

---

## TermoPortalWebApp - Frontend Blazor (DETALII COMPLETE)

### Structura Proiectului Explicată Pas cu Pas

#### 📁 **Program.cs** - Inima Aplicației
Acesta este fișierul care pornește totul. E ca `index.js` sau `app.js` din alte framework-uri.

```csharp
// 1. Crearea aplicației - ca app.use() din Express.js
var builder = WebApplication.CreateBuilder(args);

// 2. Adăugarea serviciilor - ca importurile și configurațiile
builder.Services.AddRazorComponents()  // Activează Blazor
    .AddInteractiveServerComponents(); // Modul server-side

builder.Services.AddMudServices();     // UI Framework (ca Material-UI)

// 3. Autentificare - ca passport.js din Node.js
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<TermoPortalWebAppContext>();

// 4. Construirea și pornirea aplicației
var app = builder.Build();
app.Run();
```

#### 📁 **Components/** - Componentele Blazor

##### **App.razor** - Scheletul HTML
Acesta este fișierul HTML principal, ca `index.html` din React:

```html
<!DOCTYPE html>
<html>
<head>
    <!-- CSS frameworks -->
    <link rel="stylesheet" href="@Assets["app.css"]"/>
    <link href="@Assets["_content/MudBlazor/MudBlazor.min.css"]" rel="stylesheet" />
</head>
<body>
    <!-- Aici se injectează componentele Blazor -->
    <Routes @rendermode="InteractiveServer"/>
    
    <!-- JavaScript pentru Blazor -->
    <script src="@Assets["_framework/blazor.web.js"]"></script>
</body>
</html>
```

##### **Routes.razor** - Router-ul Aplicației
Ca `App.js` cu React Router:

```razor
<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
        <FocusOnNavigate RouteData="@routeData" Selector="h1" />
    </Found>
    <NotFound>
        <PageTitle>Not found</PageTitle>
        <LayoutView Layout="@typeof(MainLayout)">
            <p role="alert">Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
</Router>
```

##### **Layout/MainLayout.razor** - Template-ul Principal
Ca `Layout.js` sau template-ul din MVC:

```razor
@inherits LayoutComponentBase

<!-- Provider-e pentru UI framework -->
<MudThemeProvider/>
<MudPopoverProvider/>

<!-- Layout-ul principal (ca navbar + content) -->
<MudLayout>
    <!-- Bara de navigare -->
    <MudAppBar Style="background-color: #b02a37;">
        <CustomAppBar/>  <!-- Componenta ta personalizată -->
    </MudAppBar>
    
    <!-- Conținutul paginii - se schimbă dinamic -->
    <MudMainContent>
        @Body  <!-- Aici se injectează pagina curentă -->
    </MudMainContent>
</MudLayout>
```

##### **Shared/CustomAppBar.razor** - Bara de Navigare
Componenta ta personalizată pentru meniu:

```razor
<!-- Exemplu de bară de navigare -->
<MudGrid>
    <MudItem xs="6">
        <MudText Typo="Typo.h6">TermoPortal</MudText>
    </MudItem>
    <MudItem xs="6" Class="d-flex justify-end">
        <!-- Butoane de navigare -->
        <MudButton Link="/home">Acasă</MudButton>
        <MudButton Link="/dashboard">Dashboard</MudButton>
        
        <!-- Dacă utilizatorul e autentificat -->
        @if (context.User.Identity?.IsAuthenticated ?? false)
        {
            <MudButton Link="/profile">Profil</MudButton>
            <MudButton OnClick="Logout">Logout</MudButton>
        }
        else
        {
            <MudButton Link="/login">Login</MudButton>
        }
    </MudItem>
</MudGrid>
```

#### 📁 **Pages/** - Paginile Aplicației

##### **Identity/** - Autentificare Utilizatori
Aceste pagini sunt generate automat de ASP.NET Core Identity:

- **Login.cshtml** - Pagina de login
- **Register.cshtml** - Creare cont nou
- **Logout.cshtml** - Delogare

Exemplu `Login.cshtml.cs` (code-behind):
```csharp
public class LoginModel : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            // Logica de login
            var result = await _signInManager.PasswordSignInAsync(
                Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
        }
        return Page();
    }
    
    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
```

---

## BackendServer - API Server (DETALII COMPLETE)

### Ce este acest proiect?
Acesta este serverul API care servește date pentru frontend. E ca backend-ul din Node.js/Express sau Django.

#### 📁 **Program.cs** - Configurarea Serverului

```csharp
// 1. Încărcare variabile de mediu (ca .env din Node.js)
string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
var envFile = environment switch
{
    "Development" => ".env.development",
    "Production" => ".env.production", 
    "Test" => ".env.test",
    _ => ".env.production"
};
Env.Load(envFile);

// 2. Configurare servicii
var builder = WebApplication.CreateBuilder(args);

// JWT Authentication (ca JWT din Node.js)
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// 3. Configurare Swagger (ca Swagger UI din Node.js)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. Configurare logging (ca Winston din Node.js)
builder.Host.UseSerilog((context, services, configuration) => configuration
    .WriteTo.Console()
    .WriteTo.File("logs/backend-.txt"));
```

#### 📁 **Controllers/** - Endpoints API

##### **Exemplu de Controller**
Ca rutele din Express.js:

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    // GET api/users - ca app.get('/api/users')
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users); // Status 200
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error"); // Status 500
        }
    }
    
    // GET api/users/5 - ca app.get('/api/users/:id')
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user == null)
        {
            return NotFound(); // Status 404
        }
        
        return Ok(user);
    }
    
    // POST api/users - ca app.post('/api/users')
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Status 400
        }
        
        var user = await _userService.CreateUserAsync(createUserDto);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
}
```

#### 📁 **Models/** - Modele de Date

##### **DTO (Data Transfer Objects)**
Ca tipurile din TypeScript:

```csharp
// Model pentru creare utilizator
public class CreateUserDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
}

// Model pentru răspuns utilizator
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

#### 📁 **Services/** - Logica de Business

##### **Exemplu de Serviciu**
Ca serviciile din Node.js:

```csharp
public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
}

public class UserService : IUserService
{
    private readonly IDataRepository _repository;
    private readonly ILogger<UserService> _logger;
    
    public UserService(IDataRepository repository, ILogger<UserService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        try
        {
            _logger.LogInformation("Getting all users");
            var users = await _repository.GetAllUsersAsync();
            
            return users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            throw;
        }
    }
}
```

---

## DataLayer - Stratul de Date (DETALII COMPLETE)

### Ce face acest proiect?
Acesta este stratul care vorbește cu baza de date. E ca ORM-ul (Prisma, Sequelize) din alte tehnologii.

#### 📁 **DbContext** - Conexiunea la Baza de Date

```csharp
public class TermoPortalDbContext : DbContext
{
    // Constructor cu dependency injection
    public TermoPortalDbContext(DbContextOptions<TermoPortalDbContext> options)
        : base(options)
    {
    }
    
    // Tabelele din baza de date (ca models din Prisma)
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    // Configurare relații și proprietăți
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurare User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique(); // Email unic
        });
        
        // Configurare relație One-to-Many
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);
    }
}
```

#### 📁 **Entities/** - Entități Bază de Date

##### **Exemplu de Entitate**
Ca schema din Prisma:

```csharp
public class User
{
    public int Id { get; set; } // Primary Key
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }
    
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } // Parola criptată
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relație One-to-Many
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Order
{
    public int Id { get; set; }
    
    [Required]
    public string ProductName { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    // Foreign Key
    public int UserId { get; set; }
    
    // Navigation Property
    public virtual User User { get; set; }
}
```

#### 📁 **Repositories/** - Pattern de Acces la Date

##### **Exemplu de Repository**
Ca repository pattern din alte framework-uri:

```csharp
public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
}

public class UserRepository : IUserRepository
{
    private readonly TermoPortalDbContext _context;
    private readonly ILogger<UserRepository> _logger;
    
    public UserRepository(TermoPortalDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Orders) // Include relații
            .OrderBy(u => u.Name)
            .ToListAsync();
    }
    
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Orders)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<User> CreateAsync(User user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            throw;
        }
    }
    
    public async Task<User> UpdateAsync(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
```

---

## Core - Biblioteca Centrală (DETALII COMPLETE)

### Ce este acest proiect?
Acesta este fundația aplicației - conține logica comună, modelele și regulile de business care sunt folosite în toate celelalte proiecte.

#### 📁 **Models/Common/** - Modele Comune

```csharp
// Model de bază pentru toate entitățile
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}

// Model pentru răspunsuri API standardizate
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
    
    public static ApiResponse<T> SuccessResult(T data, string message = "Operation successful")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
    
    public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
```

#### 📁 **Enums/** - Enumerări Comune

```csharp
public enum UserRole
{
    Guest = 0,
    User = 1,
    Moderator = 2,
    Admin = 3
}

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}

public enum PaymentMethod
{
    Cash = 0,
    Card = 1,
    BankTransfer = 2,
    PayPal = 3
}
```

#### 📁 **Interfaces/** - Interfețe de Servicii

```csharp
public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, string name);
    Task SendPasswordResetEmailAsync(string email, string resetToken);
    Task SendOrderConfirmationEmailAsync(string email, Order order);
}

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string folder);
    Task<bool> DeleteFileAsync(string filePath);
    Task<byte[]> GetFileAsync(string filePath);
}

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
}
```

#### 📁 **Validation/** - Reguli de Validare

```csharp
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");
            
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
            
        RuleFor(u => u.PasswordHash)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
    }
}
```

---

## Ghid Practic - Cum să Adaugi Funcționalități

### Scenariu: Vrei să adaugi o pagină "Produse"

#### Pasul 1: Creează Modelul în Core
```csharp
// Core/Models/Product.cs
public class Product : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [StringLength(500)]
    public string Description { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    public string ImageUrl { get; set; }
    public int Stock { get; set; }
}
```

#### Pasul 2: Adaugă în DataLayer
```csharp
// DataLayer/TermoPortalDbContext.cs
public DbSet<Product> Products { get; set; }

// DataLayer/Repositories/IProductRepository.cs
public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
}

// DataLayer/Repositories/ProductRepository.cs
public class ProductRepository : IProductRepository
{
    private readonly TermoPortalDbContext _context;
    
    public ProductRepository(TermoPortalDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Where(p => !p.IsDeleted)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
    
    // ... alte metode
}
```

#### Pasul 3: Creează API Endpoint în BackendServer
```csharp
// BackendServer/Controllers/ProductsController.cs
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;
    
    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var products = await _repository.GetAllAsync();
        return Ok(products);
    }
}
```

#### Pasul 4: Creează Serviciu în Frontend
```csharp
// TermoPortalWebApp/Services/ProductService.cs
public class ProductService
{
    private readonly HttpClient _httpClient;
    
    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<List<Product>> GetProductsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Product>>("api/products");
    }
}
```

#### Pasul 5: Înregistrează Serviciul
```csharp
// TermoPortalWebApp/Program.cs
builder.Services.AddScoped<ProductService>();
```

#### Pasul 6: Creează Componenta Blazor
```razor
@page "/products"
@inject ProductService ProductService

<PageTitle>Produse</PageTitle>

<MudContainer MaxWidth="MaxWidth.False">
    <MudGrid>
        @if (products == null)
        {
            <MudItem xs="12" Class="d-flex justify-center">
                <MudProgressCircular Indeterminate="true" />
            </MudItem>
        }
        else
        {
            @foreach (var product in products)
            {
                <MudItem xs="12" sm="6" md="4">
                    <MudCard Elevation="3">
                        <MudCardMedia Image="@product.ImageUrl" Height="200"/>
                        <MudCardContent>
                            <MudText Typo="Typo.h6">@product.Name</MudText>
                            <MudText Typo="Typo.body2">@product.Description</MudText>
                            <MudText Typo="Typo.h5" Color="Color.Primary">@product.Price.ToString("C")</MudText>
                        </MudCardContent>
                        <MudCardActions>
                            <MudButton Variant="Variant.Filled" Color="Color.Primary">
                                Adaugă în coș
                            </MudButton>
                        </MudCardActions>
                    </MudCard>
                </MudItem>
            }
        }
    </MudGrid>
</MudContainer>

@code {
    private List<Product>? products;
    
    protected override async Task OnInitializedAsync()
    {
        products = await ProductService.GetProductsAsync();
    }
}
```

#### Pasul 7: Adaugă Navigare
```razor
// Components/Shared/CustomAppBar.razor
<MudButton Link="/products">Produse</MudButton>
```

---

## Exemple Concrete de Cod

### Exemplul 1: Formular de Contact

#### Componenta Blazor:
```razor
@page "/contact"
@inject IEmailService EmailService

<MudContainer>
    <MudCard>
        <MudCardContent>
            <MudForm @ref="@form">
                <MudTextField @bind-Value="contact.Name" 
                             Label="Nume" 
                             Required="true" 
                             Margin="Margin.Dense"/>
                
                <MudTextField @bind-Value="contact.Email" 
                             Label="Email" 
                             Required="true" 
                             Margin="Margin.Dense"/>
                
                <MudTextField @bind-Value="contact.Message" 
                             Label="Mesaj" 
                             Lines="4" 
                             Required="true" 
                             Margin="Margin.Dense"/>
                
                <MudButton Variant="Variant.Filled" 
                           Color="Color.Primary"
                           OnClick="SubmitForm">
                    Trimite
                </MudButton>
            </MudForm>
        </MudCardContent>
    </MudContainer>
</MudContainer>

@code {
    private MudForm? form;
    private ContactForm contact = new();
    
    private async Task SubmitForm()
    {
        await form.Validate();
        
        if (form.IsValid)
        {
            await EmailService.SendContactEmailAsync(contact);
            
            // Afișează mesaj de succes
            Snackbar.Add("Mesaj trimis cu succes!", Severity.Success);
            
            // Reset form
            contact = new();
            form.Reset();
        }
    }
    
    public class ContactForm
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
```

### Exemplul 2: Tabel cu Date Dinamice

```razor
@page "/users"
@inject IUserService UserService

<MudContainer>
    <MudTable Items="@users" Hover="true" Elevation="0">
        <HeaderContent>
            <MudTh>Nume</MudTh>
            <MudTh>Email</MudTh>
            <MudTh>Data Creare</MudTh>
            <MudTh>Acțiuni</MudTh>
        </HeaderContent>
        <RowTemplate>
            <MudTd DataLabel="Nume">@context.Name</MudTd>
            <MudTd DataLabel="Email">@context.Email</MudTd>
            <MudTd DataLabel="Data Creare">@context.CreatedAt.ToShortDateString()</MudTd>
            <MudTd DataLabel="Acțiuni">
                <MudButton Size="Size.Small" 
                           Variant="Variant.Outlined"
                           OnClick="() => EditUser(context)">
                    Editare
                </MudButton>
                <MudButton Size="Size.Small" 
                           Variant="Variant.Outlined"
                           Color="Color.Error"
                           OnClick="() => DeleteUser(context)">
                    Ștergere
                </MudButton>
            </MudTd>
        </RowTemplate>
        <PagerContent>
            <MudPager PageSizeOptions="new int[]{10, 25, 50, 100}" />
        </PagerContent>
    </MudTable>
</MudContainer>

@code {
    private List<User> users = new();
    
    protected override async Task OnInitializedAsync()
    {
        users = await UserService.GetUsersAsync();
    }
    
    private void EditUser(User user)
    {
        NavigationManager.NavigateTo($"/users/edit/{user.Id}");
    }
    
    private async Task DeleteUser(User user)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Confirmare Ștergere",
            $"Ești sigur că vrei să ștergi utilizatorul {user.Name}?",
            yesText: "Da", noText: "Nu");
            
        if (confirmed == true)
        {
            await UserService.DeleteUserAsync(user.Id);
            users.Remove(user);
            StateHasChanged(); // Re-render component
        }
    }
}
```

---

## Depanare și Probleme Comune

### Problema 1: "Componenta nu se actualizează"
**Cauză**: Blazor Server nu detectează schimbările
**Soluție**: Folosește `StateHasChanged()`
```razor
@code {
    private async Task UpdateData()
    {
        data = await service.GetData();
        StateHasChanged(); // Forțează re-render
    }
}
```

### Problema 2: "Serviciul nu este injectat"
**Cauză**: Serviciul nu e înregistrat în Program.cs
**Soluție**: Adaugă serviciul în DI container
```csharp
builder.Services.AddScoped<IMyService, MyService>();
```

### Problema 3: "Database connection failed"
**Cauză**: Connection string greșit sau server oprit
**Soluție**: Verifică appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TermoPortal;User Id=postgres;Password=password;"
  }
}
```

### Problema 4: "CORS errors"
**Cauză**: Frontend și backend pe porturi diferite
**Soluție**: Configurează CORS în backend
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

---

## Resurse de Învățare

### Documentație Oficială
- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor/)
- [MudBlazor Documentation](https://mudblazor.com/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)

### Tutoriale Recomandate
- [Blazor Tutorial for Beginners](https://www.youtube.com/watch?v=hI45GmS9H3I)
- [MudBlazor Crash Course](https://www.youtube.com/watch?v=5E2N4lk3Q7Y)

### Proiecte Exemplu
- [BlazorShop](https://github.com/dotnet-presentations/blazor-shop)
- [BlazorTodo](https://github.com/EdCharbeneau/BlazorTodo)

---

Acest ghid complet ar trebui să îți ofere o înțelegere solidă a arhitecturii TermoPortal și să te ajute să începi dezvoltarea. Nu ezita să consulți componentele existente ca referință și să experimentezi cu funcționalități noi!
