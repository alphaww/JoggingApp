using JoggingApp.Core.Templating;

namespace JoggingApp.Tests
{
    public class FakeTemplateRenderer : ITemplateRenderer
    {
        public bool WasCalled { get; private set; }
        public Task<string> RenderTemplateToStringAsync<TModel>(string viewName, TModel model)
        {
            WasCalled = true;
            return Task.FromResult("test");
        }
    }
}
