using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
using Application.Infrastructure.Repositories;
using Application.Services;
//using Application.WebSockets;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sepending.Application.Services;
using WebAPIDocker.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// setup nếu dùng RateLimit ở program.cs
//builder.Services.AddRateLimiter(options =>
//{
//    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
//        RateLimitPartition.GetFixedWindowLimiter(
//            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "global",
//            factory: _ => new FixedWindowRateLimiterOptions
//            {
//                PermitLimit = 5,                // số request cho phép
//                Window = TimeSpan.FromSeconds(10), // trong 10 giây
//                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
//                QueueLimit = 0
//            }));

//    // Khi bị giới hạn, chạy callback này
//    options.OnRejected = async (context, token) =>
//    {
//        var httpContext = context.HttpContext;

//        httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
//        httpContext.Response.ContentType = "application/json";

//        // Có thể thêm Retry-After để client biết khi nào thử lại
//        httpContext.Response.Headers["Retry-After"] = "10";

//        var result = JsonSerializer.Serialize(new
//        {
//            status = 429,
//            error = "Too many requests",
//            message = "Bạn đã vượt quá giới hạn yêu cầu. Vui lòng thử lại sau."
//        });

//        await httpContext.Response.WriteAsync(result, token);
//    };
//});

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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:5173") // domain frontend
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});

builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Sepending API",
        Version = "v1"
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = 
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddSingleton<WebSocketConnectionManager>();
//builder.Services.AddSingleton<WebSocketHandler>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

var app = builder.Build();

app.UseWebSockets();

app.Map("/chat", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = 400;
        return;
    }

    var webSocket = await context.WebSockets.AcceptWebSocketAsync();

    if (!int.TryParse(context.Request.Query["userId"], out var userId))
    {
        await webSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.PolicyViolation, "Missing userId", CancellationToken.None);
        return;
    }

    Console.WriteLine($"User {userId} connected");

    var buffer = new byte[1024 * 4];

    // Lưu connection
    WebSocketConnectionManager.Instance.AddConnection(userId, webSocket);

    try
    {
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!result.CloseStatus.HasValue)
        {
            var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"Received from {userId}: {msg}");

            // Xử lý gửi tới người nhận
            await WebSocketConnectionManager.Instance.SendToUserAsync(msg, userId);

            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
    finally
    {
        WebSocketConnectionManager.Instance.RemoveConnection(userId, webSocket);
        Console.WriteLine($"User {userId} disconnected");
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapPost("/ws/clear", (WebSocketConnectionManager manager) =>
//{
//    manager.ClearAllConnections();
//    return Results.Ok("All WebSocket connections cleared.");
//});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();
    db.Database.Migrate(); // tự động tạo DB hoặc cập nhật schema nếu có migration mới
}


//var wsHandler = app.Services.GetRequiredService<WebSocketHandler>();
//wsHandler.StartServer("ws://0.0.0.0"); // không chỉ định port

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseMiddleware<ExceptionMiddleware>();

// app.UseMiddleware<RateLimitMiddleware>(5, 10);
//app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
