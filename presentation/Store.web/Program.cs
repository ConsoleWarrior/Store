using Store;
using Store.Contractors;
using Store.Messages;
using Store.web.App;
using Store.web.Contractors;
using Store.YandexKassa;
using Store.Data.EF;
using Store.Web;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container. ��������� ������������
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
builder.Services.AddSingleton<IPaymentService, CashPaymentService>();
builder.Services.AddSingleton<IPaymentService, YandexKassaPaymentService>();
builder.Services.AddSingleton<IWebContractorService, YandexKassaPaymentService>();
builder.Services.AddDistributedMemoryCache(); //��� �������
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); //����� ����� ������
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline. ����������������
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
app.UseExceptionHandler("/Home/Error");
app.UseStaticFiles();

app.UseRouting();

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
