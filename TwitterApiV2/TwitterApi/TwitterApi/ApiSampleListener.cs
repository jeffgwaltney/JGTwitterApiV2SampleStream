using Microsoft.Extensions.Logging;
using TwitterApi.Service;

namespace TwitterApi.Console
{
    public class ApiSampleListener
    {
        private ITwitterApiService _twitterApiService;
        private ILogger _logger;

        public ApiSampleListener(ILogger<ApiSampleListener> logger, ITwitterApiService twitterApiService)
        { 
            _logger = logger;
            _twitterApiService = twitterApiService;
        }

        public async Task ListenForTweets()
        {
            await _twitterApiService.ListenForTweets();
        }  
    }
}
