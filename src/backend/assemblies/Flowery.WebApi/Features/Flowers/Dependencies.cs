using Flowery.Infrastructure.Jobs;
using Flowery.Infrastructure.Jobs.Images;
using Flowery.WebApi.Features.Flowers.Helpers;
using Flowery.WebApi.Features.Flowers.Jobs;

namespace Flowery.WebApi.Features.Flowers;

public static class Dependencies
{
    // TODO: create with code generators
    extension(IServiceCollection services)
    {
        public void AddFlowersDependencies()
        {
            services.AddSingleton<IFlowerImageProcessor, FlowerImageProcessor>();

            services.AddJobs();
        }

        private void AddJobs()
        {
            services.AddSingleton<IJobExecutor<ProcessFlowerImages>, ProcessFlowerImagesJob>();
        }
    }
}