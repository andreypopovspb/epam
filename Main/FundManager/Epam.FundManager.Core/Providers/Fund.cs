using System;
using System.Collections.Generic;
using Epam.FundManager.Common;
using Epam.FundManager.Common.Interfaces;
using Epam.FundManager.Core.Entities;
using Epam.FundManager.Core.Entities.Collections;

namespace Epam.FundManager.Core.Providers
{
    public class Fund : BaseEntity, IFund
    {
        private sealed class Container
        {
            public IStockProvider Provider;

            public int Id;

            public FundStatistic Statistic;
        }

        private readonly FundStatistic _statistic = new FundStatistic { Name = "Total" };
        private readonly FundStatistics _statistics = new FundStatistics();
        private readonly List<Container> _providers = new List<Container>();
        private readonly object _lock = new object();
        private readonly Stocks _stocks = new Stocks();

        private Container GetProviderContainer(IStockProvider provider)
        {
            lock (_providers)
            {
                int count = _providers.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ReferenceEquals(_providers[i].Provider, provider))
                    {
                        return _providers[i];
                    }
                }
                var statistic = new FundStatistic {Name = provider.Title};
                var container = new Container {Provider = provider, Statistic = statistic};
                _statistics.Add(statistic);
                _providers.Add(container);
                return container;
            }
        }

        private void CalcFundStatistics(Container container, Stock stock)
        {
            container.Statistic.TotalNumber++;
            container.Statistic.TotalMarketValue += stock.MarketValue;

            _statistic.TotalNumber++;
            _statistic.TotalMarketValue += stock.MarketValue;

            decimal totalMarketValue = _statistic.TotalMarketValue;
            int count = _providers.Count;
            for (int i = 0; i < count; i++)
            {
                container = _providers[i];
                container.Statistic.TotalStockWeight =
                    (totalMarketValue != 0)
                        ? 100.0m*container.Statistic.TotalMarketValue/totalMarketValue
                        : 100.0m;
            }
            _statistic.TotalStockWeight = 100.0m;

            RaisePropertyChanged(() => Statistic);
        }

        public IStock Buy(IStockProvider provider, decimal price, int quantity)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (price <= 0)
                throw new ArgumentOutOfRangeException("price", string.Format("Price \"{0}\" is less or equal zero.", price));
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException("quantity", string.Format("Quantity \"{0}\" is less or equal zero.", quantity));

            Stock stock;
            lock (_lock)
            {
                Container container = GetProviderContainer(provider);
                container.Id++;
                stock = new Stock(this, provider, container.Id, price, quantity);
                CalcFundStatistics(container, stock);
                _stocks.Add(stock);
            }

            if (FundChanged != null)
            {
                FundChanged(this, provider);
            }

            return stock;
        }

        public IFundStatistic GetStatistic(IStockProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            Container container = GetProviderContainer(provider);
            return container.Statistic;
        }

        public IFundStatistic Statistic
        {
            get { return _statistic; }
        }

        public FundStatistics Statistics
        {
            get { return _statistics; }
        }

        public Stocks Stocks
        {
            get { return _stocks; }
        }

        public event FundChangedDelegate FundChanged;
    }
}