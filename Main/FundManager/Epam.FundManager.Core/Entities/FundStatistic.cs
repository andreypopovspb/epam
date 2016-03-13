using Epam.FundManager.Common.Interfaces;

namespace Epam.FundManager.Core.Entities
{
    public class FundStatistic : BaseEntity, IFundStatistic
    {
        private string _name;
        private int _totalNumber;
        private decimal _totalStockWeight;
        private decimal _totalMarketValue;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        public int TotalNumber
        {
            get { return _totalNumber; }
            set
            {
                if (_totalNumber != value)
                {
                    _totalNumber = value;
                    RaisePropertyChanged(() => TotalNumber);
                }
            }
        }

        public decimal TotalStockWeight
        {
            get { return _totalStockWeight; }
            set
            {
                if (_totalStockWeight != value)
                {
                    _totalStockWeight = value;
                    RaisePropertyChanged(() => TotalStockWeight);
                }
            }
        }

        public decimal TotalMarketValue
        {
            get { return _totalMarketValue; }
            set
            {
                if (_totalMarketValue != value)
                {
                    _totalMarketValue = value;
                    RaisePropertyChanged(() => TotalMarketValue);
                }
            }
        }
    }
}