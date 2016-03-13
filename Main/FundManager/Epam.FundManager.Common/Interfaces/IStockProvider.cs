
namespace Epam.FundManager.Common.Interfaces
{
    public interface IStockProvider
    {
        string Title { get; }

        decimal GetTolerance(IStock stock);

        string GetName(IStock stock);

        decimal CalcMarketValue(IStock stock);

        decimal CalcTransactionCost(IStock stock);

        decimal CalcStockWeight(IFund fund, IStock stock);

        bool IsRed(IStock stock);
    }
}