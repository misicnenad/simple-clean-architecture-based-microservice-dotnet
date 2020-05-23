using System;

using UserManager.Infrastructure.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UserManager.Tests.Functional.API
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var dbName = Guid.NewGuid().ToString();
            services.AddDbContext<UserManagerDbContext>(opt =>
                opt.UseInMemoryDatabase(dbName));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
