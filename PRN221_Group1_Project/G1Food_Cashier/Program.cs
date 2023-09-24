using G1Food_Cashier.Hubs;
using G1Food_Cashier.MiddlewareExtensions;
using G1Food_Cashier.SqlDependencies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<OrderHub>();
builder.Services.AddSingleton<OrderPendingDependency>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapHub<OrderHub>("/orderHub");

app.UseOrderPendingDependency();

app.Run();
