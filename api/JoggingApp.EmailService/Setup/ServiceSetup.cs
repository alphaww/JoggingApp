namespace JoggingApp.EemailService
{
    public static class ServiceSetup
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
