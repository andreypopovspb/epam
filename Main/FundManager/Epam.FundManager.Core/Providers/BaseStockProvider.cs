using System;
using System.Diagnostics;
using Epam.FundManager.Common.Interfaces;
using Epam.FundManager.Core.Entities;

namespace Epam.FundManager.Core.Providers
{
    [DebuggerDisplay("{Title}")]
    public abstract class BaseStockProvider : BaseEntity, IStockProvider
    {
        public abstract decimal Tolerance { get; }

        public abstract decimal Transaction { get; }

        public abstract string Title { get; }

        public abstract decimal GetTolerance(IStock stock);

        public virtual string GetName(IStock stock)
        {
            if (stock == null)
                throw new ArgumentNullException("stock");

            return string.Format("{0}{1}", Title, stock.Id);
        }

        public virtual decimal CalcTransactionCost(IStock stock)
        {
            if (stock == null)
                throw new ArgumentNullException("stock");

            return Transaction * stock.MarketValue / 100.0m;
        }

        public virtual decimal CalcStockWeight(IFund fund, IStock stock)
        {
            if (fund == null)
                throw new ArgumentNullException("fund");
            if (stock == null)
                throw new ArgumentNullException("stock");

            decimal marketValue = stock.MarketValue;
            IFundStatistic fundStatistic = fund.Statistic;
            //IFundStatistic fundStatistic = fund.GetStatistic(this);
            decimal totalMarketValue = fundStatistic.TotalMarketValue;
            decimal stockWeight = (totalMarketValue != 0) ? 100.0m * (marketValue / totalMarketValue) : 100.0m;
            if (stockWeight > 100.0m)
            {
                stockWeight = 100.0m;
            }
            return stockWeight;
        }

        public virtual bool IsRed(IStock stock)
        {
            if (stock == null)
                throw new ArgumentNullException("stock");

            //Market Value is < 0 or Transaction Cost > Tolerance
            decimal marketValue = stock.MarketValue;
            decimal transactionCost = stock.TransactionCost;
            decimal tolerance = GetTolerance(stock);
            return ((marketValue < 0) || (transactionCost > tolerance));
        }

        public virtual decimal CalcMarketValue(IStock stock)
        {
            if (stock == null)
                throw new ArgumentNullException("stock");

            return stock.Price*stock.Quantity;
        }
    }
}