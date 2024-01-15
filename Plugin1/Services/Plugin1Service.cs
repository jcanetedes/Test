using Test2.Core.Interfaces;
namespace Plugin1.Services
{
    public class Plugin1Service : IPlugin1Service, IService
    {
        public int CountAsync()
        {
            var random = new Random();
            return random.Next();
        }

    }
}
