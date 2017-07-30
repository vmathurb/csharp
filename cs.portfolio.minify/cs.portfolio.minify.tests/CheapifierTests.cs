using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cs.portfolio.minify.Interfaces;
using cs.portfolio.minify.PortfolioCostProviders;
using cs.portfolio.minify.PortfolioProviders;
using cs.portfolio.minify.PortfolioMinifiers;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace cs.portfolio.minify.tests
{
    [TestClass]
    public class CheapifierTests
    {
        IPortfolioCostProvider _portfolioCostProvider;
        IPortfolioProvider _portfolioProvider;

        [TestInitialize]
        public void Setup()
        {
            _portfolioCostProvider = new JsonFilePortfolioCostProvider(@"..\..\TestData");
            _portfolioProvider = new JsonFilePortfolioProvider(@"..\..\TestData");
        }

        [TestMethod]
        public void CheapifiedPortfolioHasOneTradeAtLeastPerTradeType()
        {
            var portfolioName = "EMRATES";
            var cobDate = DateTime.Parse("23 Jul 2017");

            var portfolio = _portfolioProvider.GetPortfolio(portfolioName, cobDate);
            IPortfolioMinifier cheapifier = new Cheapifier();

            var cheapifiedPortfolio = cheapifier.Minify(portfolio, _portfolioCostProvider.GetPortfolioCosts(portfolioName, cobDate));

            var tradeTypeUniverse = portfolio.Trades.Select(t => t.TradeType).Distinct();
            var cheapifiedPortfolioTradeTypeUniverse = cheapifiedPortfolio.Trades.Select(t => t.TradeType).Distinct();

            tradeTypeUniverse.Intersect(cheapifiedPortfolioTradeTypeUniverse).Count().Should().Be(tradeTypeUniverse.Count());
        }

        [TestMethod]
        public void CheapifiedPortfolioProvidesFullDependencyCover()
        {
            var portfolioName = "EMRATES";
            var cobDate = DateTime.Parse("23 Jul 2017");

            var portfolio = _portfolioProvider.GetPortfolio(portfolioName, cobDate);
            IPortfolioMinifier cheapifier = new Cheapifier();

            var cheapifiedPortfolio = cheapifier.Minify(portfolio, _portfolioCostProvider.GetPortfolioCosts(portfolioName, cobDate));

            portfolio.MarketDependencies.Intersect(cheapifiedPortfolio.MarketDependencies).Count().Should().Be(portfolio.MarketDependencies.Count());
        }
    }
}
