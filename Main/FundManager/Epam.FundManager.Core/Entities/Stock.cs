using System;
using System.Runtime.Serialization;
using Epam.FundManager.Common;
using Epam.FundManager.Common.Interfaces;

namespace Epam.FundManager.Core.Entities
{
    [DataContract(Namespace = Constants.NamespaceData)]
    public sealed class Stock : BaseEntity, IStock, IDisposable
    {
        private IFund _fund;
        private IStockProvider _provider;
        private bool _disposed;

        private void OnFundChanged(IFund fund, IStockProvider provider)
        {
            StockWeight = _provider.CalcStockWeight(fund, this);
            RaisePropertyChanged(() => StockWeight);
        }

        private Stock()
        {
        }

        public Stock(IFund fund, IStockProvider provider, int id, decimal price, int quantity)
        {
            if (fund == null)
                throw new ArgumentNullException("fund");
            if (provider == null)
                throw new ArgumentNullException("provider");

            _fund = fund;
            _provider = provider;

            Id = id;
            Price = price;
            Quantity = quantity;
            Name = provider.GetName(this);
            MarketValue = provider.CalcMarketValue(this);
            TransactionCost = provider.CalcTransactionCost(this);
            StockWeight = provider.CalcStockWeight(fund, this);
            IsRed = provider.IsRed(this);

            _fund.FundChanged += OnFundChanged;
            CreatedAt = DateTime.Now;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                //Free any other managed objects here.
                _provider = null;
                if (_fund != null)
                {
                    _fund.FundChanged -= OnFundChanged;
                    _fund = null;
                }
            }
            GC.SuppressFinalize(this);
        }

        public IFund Fund
        {
            get { return _fund; }
        }

        public IStockProvider Provider
        {
            get { return _provider; }
        }

        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public decimal Price { get; private set; }

        [DataMember]
        public decimal MarketValue { get; private set; }

        [DataMember]
        public decimal TransactionCost { get; private set; }

        [DataMember]
        public decimal StockWeight { get; private set; }

        [DataMember]
        public int Quantity { get; private set; }

        [DataMember]
        public bool IsRed { get; private set; }

        [DataMember]
        public DateTime CreatedAt { get; private set; }
    }
}