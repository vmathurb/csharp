using System.Collections.Generic;

namespace cs.portfolio.minify.Interfaces
{
    public interface IPortfolio
    {
        string Name { get; set; }

        List<ITrade> Trades { get; set; }
    }
}