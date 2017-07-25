using System;
using System.IO;
using System.Collections.Generic;
using cs.portfolio.minify.Interfaces;
using Newtonsoft.Json;

namespace cs.portfolio.minify.PortfolioProviders
{
    public class JsonFilePortfolioProvider : IPortfolioProvider
    {
        private string _baseDirectory;

        public JsonFilePortfolioProvider(string baseDirectory)
        {
            _baseDirectory = baseDirectory;
        }

        public IPortfolio GetPortfolio(string portfolioName, DateTime cobDate)
        {
            IPortfolio portfolio = null;

            var fullFilePath = Path.Combine(_baseDirectory, String.Format("{0}\\{1}\\{1}_{0}.json", cobDate.ToString("yyyyMMdd"), portfolioName));

            if (File.Exists(fullFilePath))
            {
                using (StreamReader file = File.OpenText(fullFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    portfolio = (IPortfolio)serializer.Deserialize(file, typeof(Portfolio));
                }
            }

            return portfolio;
        }
    }
}