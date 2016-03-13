using System;
using Epam.FundManager.Common.Interfaces;

namespace Epam.FundManager.Core.Providers
{
    public class EquityStockProvider : BaseStockProvider
    {
        #region Constants

        /// <summary>
        /// "200 000"
        /// </summary>
        public const decimal ConstTolerance = 200000;

        /// <summary>
        /// "0.5%"
        /// </summary>
        public const decimal ConstTransaction = 0.5m;

        /// <summary>
        /// "Equity"
        /// </summary>
        public string ConstTitle = @"Equity";

        #endregion

        public override decimal Tolerance
        {
            get { return ConstTolerance; }
        }

        public override decimal Transaction
        {
            get { return ConstTransaction; }
        }

        public override string Title
        {
            get { return ConstTitle; }
        }

        public override decimal GetTolerance(IStock stock)
        {
            if (stock == null)
                throw new ArgumentNullException("stock");

            return ConstTolerance;
        }
    }
}