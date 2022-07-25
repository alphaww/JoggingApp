using JoggingApp.Core.Users;

namespace JoggingApp.Core
{
    public interface ITokenWriter
    {
        string Write(User user);
    }
}
