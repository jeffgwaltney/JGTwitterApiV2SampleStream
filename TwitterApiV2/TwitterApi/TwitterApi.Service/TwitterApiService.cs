using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Parameters.TrendsClient;

namespace TwitterApi.Service
{
    public class TwitterApiService : ITwitterApiService
    {
        private IConfiguration _config;
        private ILogger _logger;

        private int totalTweetsReceived = 0;
        //TODO top 10 hashtags

        public TwitterApiService(ILogger<TwitterApiService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task ListenForTweets()
        {
            var twitterApiConfig = _config.GetSection("TwitterApiConfig");
            var apiKey = twitterApiConfig["ApiKey"];
            var apiSecret = twitterApiConfig["ApiSecret"];
            var bearerToken = twitterApiConfig["BearerToken"];

            var userClient = new TwitterClient(apiKey, apiSecret, bearerToken);

            var trendsClient = userClient.Trends;
            var trendLocations = await trendsClient.GetTrendLocationsAsync();
            var unitedStatesTrendLocation = trendLocations.FirstOrDefault(x => x.CountryCode == "US");

            if (unitedStatesTrendLocation != null)
            {
                var usTrends = await trendsClient.GetPlaceTrendsAtAsync(unitedStatesTrendLocation.WoeId);

                var topTrends = usTrends.Trends.OrderBy(x => x.TweetVolume).GroupBy(x => x.Name).Select(x => x.Key).Take(10);
                System.Console.WriteLine($"Top 10 hashtags in the United States @ {DateTime.Now.ToString()} :");
                foreach (var trend in topTrends)
                {
                    System.Console.WriteLine(trend);
                }
            }
            else
            {
                System.Console.WriteLine("No US trend data availabe");
                System.Console.WriteLine("...");
                System.Console.WriteLine("...");
                System.Console.WriteLine("...");
            }
            

            var sampleStreamV2 = userClient.StreamsV2.CreateSampleStream();

            try
            {
                sampleStreamV2.TweetReceived += (sender, args) =>
                {
                    totalTweetsReceived++;
                    System.Console.WriteLine($"Tweet #{totalTweetsReceived} recieved:");
                    System.Console.WriteLine(args.Tweet.Text);
                    System.Console.WriteLine($"...");
                    System.Console.WriteLine($"...");
                    System.Console.WriteLine($"...");
                };
            }
            catch (TwitterException e)
            {
                Console.WriteLine(e.ToString());
            }

            await sampleStreamV2.StartAsync();
        }
    }
}