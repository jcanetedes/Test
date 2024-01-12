using BlazorApp2.Shared;

namespace RazorClassLibrary1.Services
{
    internal sealed class RazorClassLibraryService : IRazorClassLibraryService, IService
    {
        public Task<int> CountAsync()
        {
            var random = new Random();
            return Task.FromResult(random.Next());
        }
    }
}
