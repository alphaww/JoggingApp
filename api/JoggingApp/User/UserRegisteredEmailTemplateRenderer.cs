using JoggingApp.Core.Templating;
using JoggingApp.Core.Users;
using System.Dynamic;
using System.Threading.Tasks;

namespace JoggingApp.Users
{
    public class UserRegisteredEmailTemplateRenderer
    {
        private readonly ITemplateRenderer _templateRenderer;
        public UserRegisteredEmailTemplateRenderer(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }
        public async Task<string> RenderForUserActivationToken(UserActivationToken activationToken)
        {
            dynamic model = new ExpandoObject();
            model.Email = activationToken.User.Email;

            string url = "http://localhost:5034";

            model.TemporaryLink = $"{url}/{activationToken.Id}/confirm-account";

            var mailBody = await _templateRenderer.RenderTemplateToStringAsync("Templates/confirm-account.cshtml", model);

            return mailBody;
        }
    }
}
