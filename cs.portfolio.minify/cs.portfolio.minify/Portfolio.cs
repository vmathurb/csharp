using cs.portfolio.minify.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.portfolio.minify
{
    public class Portfolio : IPortfolio
    {
        public string Name { get; set; }

        public List<ITrade> Trades { get; set; }

        public List<string> MarketDependencies
        {
            get
            {
                return this.Trades.Select(t => t.MarketDependencies).SelectMany(md => md).Distinct().ToList();
            }
        }

        public Portfolio(string name, List<Trade> trades)
        {
            this.Name = name;
            this.Trades = trades.Cast<ITrade>().ToList();
        }
    }
}
