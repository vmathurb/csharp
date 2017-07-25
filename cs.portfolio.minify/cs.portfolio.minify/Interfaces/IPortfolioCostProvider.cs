using System;
using System.Collections.Generic;

namespace cs.portfolio.minify.Interfaces
{
    public interface IPortfolioCostProvider
    {
        List<TradeCost> GetPortfolioCosts(string portfolioName, DateTime cobDate);
    }
}
