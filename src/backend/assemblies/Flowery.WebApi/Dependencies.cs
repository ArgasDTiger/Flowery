using System.IO.Compression;
using Flowery.Infrastructure;
using Flowery.Shared;
using Microsoft.AspNetCore.ResponseCompression;

namespace Flowery.WebApi;

public static class Dependencies
{
    extension(IServiceCollection services)
    {
        public void AddServices(IConfiguration config)
        {
            services.AddInfrastructureDependencies(config);
            services.AddDomainDependencies();
            services.AddCompression();
        }

        private void AddCompression()
        {
            services.AddResponseCompression(opt =>
            {
                opt.EnableForHttps = true;
                opt.Providers.Add<BrotliCompressionProvider>();
                opt.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<GzipCompressionProviderOptions>(opt =>
            {
                opt.Level = CompressionLevel.Fastest;
            });

            services.Configure<BrotliCompressionProviderOptions>(opt =>
            {
                opt.Level = CompressionLevel.Fastest;
            });
        }
    }
}