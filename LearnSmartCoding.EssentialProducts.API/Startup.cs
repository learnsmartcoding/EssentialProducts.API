using LearnSmartCoding.EssentialProducts.API.Common.Filters;
using LearnSmartCoding.EssentialProducts.Data;
using LearnSmartCoding.EssentialProducts.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LearnSmartCoding.EssentialProducts.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<EssentialProductsDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DbContext")); //Use this option for real database
                //options.UseInMemoryDatabase("EssentialProducts"); // use this option and comment the other one if you want in memory database. Everytime you run app, the data is cleared.
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IWishlistItemService, WishlistItemService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });

            // Allowing CORS for all domains and methods for the purpose of the sample
            // In production, modify this with the actual domains you want to allow
            services.AddCors(o => o.AddPolicy("default", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            services.AddSwaggerGen(
                   options =>
                   {
                       options.SwaggerDoc("v1", new OpenApiInfo()
                       {
                           Title = "Learn Smart Coding - EssentialProducts API",
                           Version = "V1",
                           Description = "This API is design to show products that are essentials for customers on day to day basis. Owner of the product can add their products",
                           TermsOfService = new System.Uri("https://karthiktechblog.com/copyright"),
                           Contact = new OpenApiContact()
                           {
                               Name = "Karthik",
                               Email = "learnsmartcoding@gmail.com",
                               Url = new System.Uri("http://www.karthiktechblog.com")
                           },
                           License = new OpenApiLicense
                           {
                               Name = "Use under LICX",
                               Url = new System.Uri("https://karthiktechblog.com/copyright"),
                           }
                       });
                   }
            );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddApplicationInsightsTelemetry(Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("default");

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Learn Smart Coding - EssentialProducts API V1");
            });
        }
    }
}
