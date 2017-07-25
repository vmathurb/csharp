using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.portfolio.minify.Interfaces
{
    public interface IPortfolioMinifier
    {
        IPortfolio Minify(IPortfolio portfolio, List<TradeCost> portfolioCosts);
    }
}
