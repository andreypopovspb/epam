namespace Epam.FundManager.Common.Interfaces
{
    public interface IStock
    {
        IStockProvider Provider { get; }

        int Id { get; }

        string Name { get; }

        decimal MarketValue { get; }

        decimal TransactionCost { get; }

        decimal StockWeight { get; }

        decimal Price { get; }

        int Quantity { get; }
    }
}