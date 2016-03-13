using System;
using System.ComponentModel;
using Epam.FundManager.Common;
using Epam.FundManager.Common.Extensions;
using Epam.FundManager.Core.Entities;

namespace Epam.FundManager.UI.Views
{
    public sealed class StockInput : BaseEntity, IDataErrorInfo
    {
        private string _price;
        private string _quantity;
        private decimal? _priceValue;
        private int? _quantityValue;

        public string Price
        {
            get { return _price; }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    _priceValue = value.TryParseToDecimal();

                    RaisePropertyChanged(() => Price);
                    RaisePropertyChanged(() => PriceValue);
                    RaisePropertyChanged(() => PriceValid);
                    RaisePropertyChanged(() => IsValid);
                }
            }
        }

        public decimal? PriceValue
        {
            get { return _priceValue; }
        }

        public bool PriceValid
        {
            get { return ((PriceValue != null) && (PriceValue > 0)); }
        }

        public string Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    _quantityValue = value.TryParseToInt();

                    RaisePropertyChanged(() => Quantity);
                    RaisePropertyChanged(() => QuantityValue);
                    RaisePropertyChanged(() => QuantityValid);
                    RaisePropertyChanged(() => IsValid);
                }
            }
        }

        public int? QuantityValue
        {
            get { return _quantityValue; }
        }

        public bool QuantityValid
        {
            get { return ((QuantityValue != null) && (QuantityValue > 0)); }
        }

        public bool IsValid
        {
            get { return (PriceValid) && (QuantityValid); }
        }

        public string this[string columnName]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(columnName))
                {
                    return null;
                }
                if (columnName == Utility.ExtractPropertyName(() => Price))
                {
                    return (PriceValid) ? null : "Enter a price (decimal value greater than 0)";
                }
                if (columnName == Utility.ExtractPropertyName(() => Quantity))
                {
                    return (QuantityValid) ? null : "Enter a quantity (integer value greater than 0)";
                }
                return null;
            }
        }

        public string Error { get { throw new NotSupportedException(); } }
    }
}