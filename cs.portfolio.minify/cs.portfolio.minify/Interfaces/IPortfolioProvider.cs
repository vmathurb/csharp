using System;

namespace cs.portfolio.minify.Interfaces
{
    public interface IPortfolioProvider
    {
        IPortfolio GetPortfolio(string portfolio, DateTime cobDate);
    }
}
