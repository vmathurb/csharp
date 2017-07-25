using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cs.portfolio.minify.Interfaces;
using FluentAssertions;
using cs.portfolio.minify.PortfolioProviders;

namespace cs.portfolio.minify.tests
{
    [TestClass]
    public class JsonPortfolioProviderTests
    {
        IPortfolioProvider _portfolioProvider;

        [TestInitialize]
        public void Setup()
        {
            _portfolioProvider = new JsonFilePortfolioProvider(@"..\..\TestData");
        }

        [TestMethod]
        public void NullShouldBeReturnedForNonExistentPortfolio()
        {
            var portfolio = _portfolioProvider.GetPortfolio("NONEXISTENT", DateTime.Parse("23 Jul 2017"));
            portfolio.Should().BeNull("because the portfolio is non existent");
        }

        [TestMethod]
        public void LoadValidPortfolio()
        {
            var portfolio = _portfolioProvider.GetPortfolio("EMRATES", DateTime.Parse("23 Jul 2017"));
            portfolio.Should().NotBeNull("because the portfolio is valid");
        }
    }
}
