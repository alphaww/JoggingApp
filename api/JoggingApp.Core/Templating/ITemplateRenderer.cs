namespace JoggingApp.Core.Templating
{
    public interface ITemplateRenderer
    {
        Task<string> RenderTemplateToStringAsync<TModel>(string viewName, TModel model);
    }
}