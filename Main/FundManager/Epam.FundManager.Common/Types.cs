using System;
using Epam.FundManager.Common.Interfaces;

namespace Epam.FundManager.Common
{
    [Serializable]
    public delegate void FundChangedDelegate(IFund fund, IStockProvider provider);
}