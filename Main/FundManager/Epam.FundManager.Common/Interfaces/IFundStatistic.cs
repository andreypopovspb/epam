namespace Epam.FundManager.Common.Interfaces
{
    public interface IFundStatistic
    {
        int TotalNumber { get; }

        decimal TotalStockWeight { get; }

        decimal TotalMarketValue { get; }
    }
}