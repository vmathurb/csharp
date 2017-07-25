using cs.portfolio.minify.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.portfolio.minify.PortfolioMinifiers
{
    public class Cheapifier : IPortfolioMinifier
    {
        public IPortfolio Minify(IPortfolio portfolio, List<TradeCost> portfolioCosts)
        {
            return portfolio;
        }
    }
}
