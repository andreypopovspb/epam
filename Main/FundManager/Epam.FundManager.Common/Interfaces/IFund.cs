
namespace Epam.FundManager.Common.Interfaces
{
    public interface IFund
    {
        IStock Buy(IStockProvider provider, decimal price, int quantity);

        IFundStatistic GetStatistic(IStockProvider provider);

        IFundStatistic Statistic { get; }

        event FundChangedDelegate FundChanged;
    }
}