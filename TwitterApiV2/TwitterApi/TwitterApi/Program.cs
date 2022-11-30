using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwitterApi.Console;
using TwitterApi.Service;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services
        .AddScoped<ITwitterApiService, TwitterApiService>())
        .ConfigureAppConfiguration((context, builder) =>
        {             
             builder.AddJsonFile("appsettings.json");
        })
    .Build();

await EntryPoint(host.Services);

await host.RunAsync();

static async Task EntryPoint(IServiceProvider services)
{
    using IServiceScope serviceScope = services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    var logger = provider.GetRequiredService<ILogger<ApiSampleListener>>();
    var twitterService = provider.GetRequiredService<ITwitterApiService>();
    var listener = new ApiSampleListener(logger, twitterService);

    await listener.ListenForTweets();    
}