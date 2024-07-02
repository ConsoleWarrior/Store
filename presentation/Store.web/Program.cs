using Store;
using Store.Contractors;
using Store.Messages;
using Store.web.App;
using Store.web.Contractors;
using Store.YandexKassa;
using Store.Data.EF;
using Store.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Внедрение зависимостей
builder.Services.AddControllersWithViews(options =>
{
	options.Filters.Add(typeof(ExceptionFilter));
});
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEfRepositories(builder.Configuration.GetConnectionString("Store"));
builder.Services.AddSingleton<INotificationService, DebugNotificationService>();
builder.Services.AddSingleton<IDeliveryService, PostamateDeliveryService>();
builder.Services.AddSingleton<IDeliveryService, PickupDeliveryService>();
builder.Services.AddSingleton<IPaymentService, CashPaymentService>();
builder.Services.AddSingleton<IPaymentService, YandexKassaPaymentService>();
builder.Services.AddSingleton<IWebContractorService, YandexKassaPaymentService>();
builder.Services.AddDistributedMemoryCache(); //для корзины
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(20); //время жизни сессии
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
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
						//IssuerSigningKey = new RsaSecurityKey(RSAExtensions.GetPublicKey())
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
					};
				});

var app = builder.Build();

// Configure the HTTP request pipeline. Конфигурирование
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
app.UseExceptionHandler("/Home/Error");
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapAreaControllerRoute(
//	name: "yandex.cassa",
//    areaName: "YandexCassa",
//	pattern: "YandexCassa/{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "areas",
		pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
