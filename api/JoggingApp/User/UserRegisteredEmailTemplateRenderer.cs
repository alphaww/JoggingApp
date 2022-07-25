using JoggingApp.Core.Templating;
using JoggingApp.Core.Users;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using System.Threading.Tasks;

namespace JoggingApp.Users
{
    public class UserRegisteredEmailTemplateRenderer
    {
        private readonly ITemplateRenderer _templateRenderer;
        private readonly IConfiguration _configuration;
        public UserRegisteredEmailTemplateRenderer(ITemplateRenderer templateRenderer, IConfiguration configuration)
        {
            _templateRenderer = templateRenderer;
            _configuration = configuration;
        }
        public async Task<string> RenderForUserActivationToken(UserActivationToken activationToken)
        {
            dynamic model = new ExpandoObject();
            model.Email = activationToken.User.Email;

            string url = _configuration["Service:Url"];

            model.ActivationLink = $"{url}/{activationToken.Id}/confirm-account";

            var mailBody = await _templateRenderer.RenderTemplateToStringAsync("User/Templates/confirm-account.cshtml", model);

            return mailBody;
        }
    }
}
