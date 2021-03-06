using Epinova.ElasticSearch.Core.EPiServer.Providers;
using EPiServer.Core;
using EPiServer.Core.Internal;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Search;

namespace Epinova.ElasticSearch.Core.EPiServer.Commerce.Providers
{
    [SearchProvider]
    public class ProductSearchProvider : SearchProviderBase<IContent, IContent, ContentType>
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        private static Injected<DefaultContentProvider> DefaultContentProvider { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        public ProductSearchProvider() : base("product")
        {
            IconClass = Constants.CommerceCatalogIconCssClass;
            AreaName = Constants.CommerceCatalogArea;
            ForceRootLookup = true;
        }

        protected override string GetSearchRoot()
        {
            return Constants.CatalogRootLink.ID.ToString();
        }

        protected override string[] GetProviderKeys()
        {
            return new[]
            {
                ProviderConstants.CatalogProviderKey,
                DefaultContentProvider.Service.ProviderKey ?? ProviderConstants.DefaultProviderKey
            };
        }
    }
}