using System;
using Epam.FundManager.Core.Entities;
using Epam.FundManager.Core.Providers;

namespace Epam.FundManager.UI.Views
{
    public class StockProviderView : BaseEntity
    {
        private readonly BaseStockProvider _provider;

        public StockProviderView(BaseStockProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            _provider = provider;
        }

        public BaseStockProvider Provider
        {
            get { return _provider; }
        }

        public string Name
        {
            get { return _provider.Title; }
        }

        public string Title
        {
            get
            {
                return string.Format("{0} (Tolerance: {1:0.00}, Transaction: {2:0.00} %)", Name, Provider.Tolerance, Provider.Transaction);
            }
        }
    }
}