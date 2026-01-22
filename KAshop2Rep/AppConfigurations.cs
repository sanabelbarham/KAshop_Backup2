using BLL.Service;
using DAL.Repository;
using DAL.Utilis;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace KAshop2Rep

{
    public static class AppConfigurations
    {

        public static void Config( IServiceCollection Services) {
            // Repositories & Services
           Services.AddScoped<ICategoryRepository, CategoryRepository>();
        Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IAuthenticationServicecs, AuthenticationService>();
            Services.AddTransient<IEmailSender, EmailSender>();

            // Seed Data
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();

            Services.AddScoped<IFileService, FileService>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<ITokenService,TokenService >();
        }
    }
}
