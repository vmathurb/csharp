using System.Collections.Generic;

namespace cs.portfolio.minify.Interfaces
{
    public interface ITrade
    {
        string TradeType { get; set; }
        string TCN { get; set; }
        List<string> MarketDependencies { get; set; }
    }
}