using DD4T.ContentModel.Contracts.Providers;

namespace Tridion.TestProviders
{
    public class BaseProvider : IProvider
    {
        public int PublicationId { get; set; }
    }
}
