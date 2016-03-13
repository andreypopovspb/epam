using System.Linq;
using System.Windows.Input;
using Epam.FundManager.Common.Interfaces;
using Epam.FundManager.Core.Entities;
using Epam.FundManager.Core.Entities.Collections;
using Epam.FundManager.Core.Providers;
using Epam.FundManager.UI.Views;
using Epam.FundManager.WPFLibrary.Extensions;

namespace Epam.FundManager.UI.Presenters
{
    public class MainPresenter : BaseEntity
    {
        private readonly Fund _fund = new Fund();
        private readonly ObservableCollectionEx<StockProviderView> _providers = new ObservableCollectionEx<StockProviderView>();
        private readonly ObservableCollectionEx<FundStatistic> _statistics = new ObservableCollectionEx<FundStatistic>();
        private bool _addMode;
        private StockInput _stockInput;
        private readonly Stocks _stocks;

        public MainPresenter()
        {
            var equityProvider = new EquityStockProvider();
            var bondProvider = new BondStockProvider();

            var equityStatistic = (FundStatistic)_fund.GetStatistic(equityProvider);
            var bondStatistic = (FundStatistic)_fund.GetStatistic(bondProvider);
            var fundStatistic = (FundStatistic)_fund.Statistic;
            _statistics.Add(equityStatistic);
            _statistics.Add(bondStatistic);
            _statistics.Add(fundStatistic);

            _stocks = _fund.Stocks;
            _fund.FundChanged += FundOnFundChanged;

            _providers.Add(new StockProviderView(equityProvider));
            _providers.Add(new StockProviderView(bondProvider));

            SelectedProvider = _providers.First();
        }

        private void FundOnFundChanged(IFund fund, IStockProvider provider)
        {
        }

        public bool CanExecute(RoutedUICommand command)
        {
            if (command == null)
            {
                return false;
            }
            if (command == DataCommands.AddStock)
            {
                return (!AddMode);
            }
            if (command == DataCommands.Cancel)
            {
                return (AddMode);
            }
            if (command == DataCommands.BuyStock)
            {
                return (AddMode) && (StockInput != null) && (StockInput.IsValid);
            }
            return true;
        }

        public void Execute(RoutedUICommand command)
        {
            if (command == null)
            {
                return;
            }
            if (command == DataCommands.AddStock)
            {
                AddMode = true;
                return;
            }
            if (command == DataCommands.Cancel)
            {
                AddMode = false;
                return;
            }
            if (command == DataCommands.BuyStock)
            {
                BaseStockProvider provider = (SelectedProvider != null) ? SelectedProvider.Provider : null;
                if ((provider != null) && (_stockInput != null) && (_stockInput.PriceValue != null) && (_stockInput.QuantityValue != null))
                {
                    _fund.Buy(provider, _stockInput.PriceValue.Value, _stockInput.QuantityValue.Value);
                }
                AddMode = false;
            }
        }

        public ObservableCollectionEx<StockProviderView> Providers
        {
            get { return _providers; }
        }

        public Stocks Stocks
        {
            get { return _stocks; }
        }

        public ObservableCollectionEx<FundStatistic> Statistics
        {
            get { return _statistics; }
        }

        public StockProviderView SelectedProvider { get; set; }

        public bool AddMode
        {
            get { return _addMode; }
            set
            {
                if (_addMode != value)
                {
                    _stockInput = (value) ? new StockInput() : null;
                    _addMode = value;

                    RaisePropertyChanged(() => AddMode);
                    RaisePropertyChanged(() => StockInput);
                }
            }
        }

        public StockInput StockInput
        {
            get { return _stockInput; }
        }
    }
}