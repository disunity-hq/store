using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Shared.Startup.Binders {

    public class AuthenticationBinder : IStartupBinder {

        private readonly IConfiguration _config;

        public AuthenticationBinder(IConfiguration config) {
            _config = config;
        }

        public void Bind(IServiceCollection services) {
            var authenticationBuilder = services.AddAuthentication();
            var githubClientId = _config.GetValue<string>("Auth:Github:ClientId");
            var githubClientSecret = _config.GetValue<string>("Auth:Github:ClientSecret");
            var discordClientId = _config.GetValue<string>("Auth:Discord:ClientId");
            var discordClientSecret = _config.GetValue<string>("Auth:Discord:ClientSecret");

            if (!string.IsNullOrWhiteSpace(githubClientId) && !string.IsNullOrWhiteSpace(githubClientSecret)) {
                authenticationBuilder.AddGitHub(options => {
                    options.ClientId = githubClientId;
                    options.ClientSecret = githubClientSecret;
                });
            }

            if (!string.IsNullOrWhiteSpace(discordClientId) && !string.IsNullOrWhiteSpace(discordClientSecret)) {
                authenticationBuilder.AddDiscord(options => {
                    options.ClientId = discordClientId;
                    options.ClientSecret = discordClientSecret;
                });
            }

        }

    }

}