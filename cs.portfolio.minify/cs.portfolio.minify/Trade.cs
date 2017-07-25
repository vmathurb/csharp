using cs.portfolio.minify.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.portfolio.minify
{
    public class Trade : ITrade
    {
        public string TradeType { get; set; }
        public string TCN { get; set; }
        public List<string> MarketDependencies { get; set; }
    }
}
