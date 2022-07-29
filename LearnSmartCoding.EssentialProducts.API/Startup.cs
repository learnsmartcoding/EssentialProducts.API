using LearnSmartCoding.EssentialProducts.API.AuthorizationPolicies;
using LearnSmartCoding.EssentialProducts.Data;
using LearnSmartCoding.EssentialProducts.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddMicrosoftIdentityWebApi(options =>
                  {
                      Configuration.Bind("AzureAdB2C", options);

                      options.TokenValidationParameters.NameClaimType = "name";
                  },
          options => { Configuration.Bind("AzureAdB2C", options); });

            services.AddDbContextPool<EssentialProductsDbContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("DbContext")); //Use this option for real database
                options.UseInMemoryDatabase("EssentialProducts"); // use this option and comment the other one if you want in memory database. Everytime you run app, the data is cleared.
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IWishlistItemService, WishlistItemService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();

            //if you want to deploy to server, here server address will come.//E.g. https://lsc-essential-products-web.azurewebsites.net/
            services.AddCors(options =>
            {
                options.AddPolicy("AllRequests", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(
                        origin => origin == "http://localhost:4200")
                    .AllowCredentials();
                });
            });

            services.AddAuthorization(options =>
            {
                // Create policy to check for the scope 'read'
                options.AddPolicy("ProductReadScope",
                    policy => policy.Requirements.Add(
                        new ScopesRequirement(
                            "https://learnsmartcoding.onmicrosoft.com/essentialproducts/api/products.read"))
                    );

                // check for write
                options.AddPolicy("ProductWriteScope",
                    policy => policy.Requirements.Add(
                        new ScopesRequirement(
                            "https://learnsmartcoding.onmicrosoft.com/essentialproducts/api/products.write"))
                    );
                // Create policy to check for the scope 'read'
                options.AddPolicy("categoriesReadScope",
                    policy => policy.Requirements.Add(
                        new ScopesRequirement(
                            "https://learnsmartcoding.onmicrosoft.com/essentialproducts/api/categories.read"))
                    );

                // check for write
                options.AddPolicy("categoriesWriteScope",
                    policy => policy.Requirements.Add(
                        new ScopesRequirement(
                            "https://learnsmartcoding.onmicrosoft.com/essentialproducts/api/categories.write"))
                    );
            });


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

            app.UseAuthentication();
            app.UseAuthorization();

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
