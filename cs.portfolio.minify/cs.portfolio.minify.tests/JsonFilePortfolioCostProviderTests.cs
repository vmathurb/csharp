using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cs.portfolio.minify.Interfaces;
using FluentAssertions;
using cs.portfolio.minify.PortfolioCostProviders;

namespace cs.portfolio.minify.tests
{
    [TestClass]
    public class JsonFilePortfolioCostProviderTests
    {
        IPortfolioCostProvider _portfolioCostProvider;

        [TestInitialize]
        public void Setup()
        {
            _portfolioCostProvider = new JsonFilePortfolioCostProvider(@"..\..\TestData");
        }

        [TestMethod]
        public void NullCostsShouldBeReturnedForNonExistentPortfolio()
        {
            var portfolioCosts = _portfolioCostProvider.GetPortfolioCosts("NONEXISTENT", DateTime.Parse("23 Jul 2017"));
            portfolioCosts.Should().BeNull("because the portfolio is non existent");
        }

        [TestMethod]
        public void LoadCostsForValidPortfolio()
        {
            var portfolioCosts = _portfolioCostProvider.GetPortfolioCosts("IPG", DateTime.Parse("23 Jul 2017"));
            portfolioCosts.Should().NotBeNull("because the portfolio is valid");
        }
    }
}
