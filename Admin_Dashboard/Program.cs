using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Repositories;
using Talabat.Repositories.Data;
using Talabat.Repositories.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDBContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});
builder.Services.AddDbContext<AppIdentityDbcontext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<IMapper, Mapper>();

builder.Services.AddAutoMapper(typeof(Program));  // Adjust this if your AutoMapper profiles are in another assembly


builder.Services.AddIdentity<AppUser, IdentityRole>((options) =>
{

}).AddEntityFrameworkStores<AppIdentityDbcontext>();





 var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
