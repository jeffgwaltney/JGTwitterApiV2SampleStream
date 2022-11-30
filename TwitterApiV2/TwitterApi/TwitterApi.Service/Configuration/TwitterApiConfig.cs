using Microsoft.Extensions.Configuration;
using Tweetinvi;

namespace TwitterApi.Service.Configuration
{
    public class TwitterApiConfig
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string BearerToken { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}