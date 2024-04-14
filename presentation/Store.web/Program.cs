using Store;
using Store.Contractors;
using Store.Memory;
using Store.Messages;
using Store.web.Contractors;
using Store.YandexCassa;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container. ��������� ������������
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<INotificationService, DebugNotificationService>();
builder.Services.AddSingleton<IDeliveryService, PostamateDeliveryService>();
builder.Services.AddSingleton<IPaymentService, CashPaymentService>();
builder.Services.AddSingleton<IPaymentService, YandexCassaPaymentService>();
builder.Services.AddSingleton<IWebContractorService, YandexCassaPaymentService>();
builder.Services.AddDistributedMemoryCache(); //��� �������
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); //����� ����� ������
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline. ����������������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
	name: "yandex.cassa",
    areaName: "YandexCassa",
	pattern: "YandexCassa/{controller=Home}/{action=Index}/{id?}");

app.Run();
