using Microsoft.AspNetCore.Hosting;
using travoul.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace travoul.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}