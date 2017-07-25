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

        public Portfolio(string name, List<Trade> trades)
        {
            this.Name = name;
            this.Trades = trades.Cast<ITrade>().ToList();
        }
    }
}
