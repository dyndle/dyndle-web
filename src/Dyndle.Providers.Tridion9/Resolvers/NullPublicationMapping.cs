using Tridion.ContentDelivery.DynamicContent;

namespace Dyndle.Providers.Resolvers
{
    public class NullPublicationMapping : IPublicationMapping
    {
        public int PublicationId => 0;

        public string Protocol => null;

        public string Domain => null;

        public string Port => null;

        public string Path => null;

        public int PathScanDepth => 0;

        public int NamespaceId => 1;
    }
}
