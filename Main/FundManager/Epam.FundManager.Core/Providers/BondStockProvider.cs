using System;
using Epam.FundManager.Common.Interfaces;

namespace Epam.FundManager.Core.Providers
{
    public class BondStockProvider : BaseStockProvider
    {
        #region Constants

        /// <summary>
        /// "100 000"
        /// </summary>
        public const decimal ConstTolerance = 100000;

        /// <summary>
        /// "2.0%"
        /// </summary>
        public const decimal ConstTransaction = 2.0m;

        /// <summary>
        /// "Bond"
        /// </summary>
        public string ConstTitle = @"Bond";

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