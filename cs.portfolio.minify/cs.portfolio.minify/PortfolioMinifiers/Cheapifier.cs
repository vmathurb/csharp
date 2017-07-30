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
            IPortfolio cover = new Portfolio(String.Format("{0}_MINI", portfolio.Name), new List<Trade>());
            List<int> coverCosts = new List<int>();

            Dictionary<string, int> tradeCosts = new Dictionary<string, int>();
            portfolioCosts.ForEach(tc => tradeCosts.Add(tc.TCN, tc.Cost));

            HashSet<string> nonCoveredDependencies = new HashSet<string>();
            portfolio.MarketDependencies.ForEach(d => nonCoveredDependencies.Add(d));

            while (nonCoveredDependencies.Count != 0)
            {
                var minWeightedCoveringTrade = FindMinWeightedCoveringTrade(portfolio, nonCoveredDependencies, tradeCosts, out int minTradeCost);
                cover.Trades.Add(minWeightedCoveringTrade);
                coverCosts.Add(minTradeCost);
                minWeightedCoveringTrade.MarketDependencies.ForEach(md => nonCoveredDependencies.Remove(md));
            }

            return cover;
        }

        private ITrade FindMinWeightedCoveringTrade(IPortfolio portfolio, HashSet<string> nonCoveredDependencies, Dictionary<string, int> tradeCosts, out int minTradeCost)
        {
            double minTradeCostFactor = int.MaxValue;
            int minTradeIndex = -1;

            for (int i = 0; i < portfolio.Trades.Count; i++)
            {
                ITrade trade = portfolio.Trades[i];
                int tradeCost = tradeCosts[trade.TCN];

                int dependenciesCoveredByThisTrade = nonCoveredDependencies.Intersect(trade.MarketDependencies).Count();
                if (dependenciesCoveredByThisTrade > 0)
                {
                    double costFactor = tradeCost / dependenciesCoveredByThisTrade;
                    if (costFactor < minTradeCostFactor)
                    {
                        minTradeCostFactor = costFactor;
                        minTradeIndex = i;
                    }
                }
            }

            minTradeCost = tradeCosts[portfolio.Trades[minTradeIndex].TCN];
            return portfolio.Trades[minTradeIndex];
        }
    }
}
