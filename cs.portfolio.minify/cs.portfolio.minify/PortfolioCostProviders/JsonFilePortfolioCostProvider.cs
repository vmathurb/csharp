using System;
using System.IO;
using System.Collections.Generic;
using cs.portfolio.minify.Interfaces;
using Newtonsoft.Json;

namespace cs.portfolio.minify.PortfolioCostProviders
{
    public class JsonFilePortfolioCostProvider : IPortfolioCostProvider
    {
        private string _baseDirectory;

        public JsonFilePortfolioCostProvider(string baseDirectory)
        {
            _baseDirectory = baseDirectory;
        }

        public List<TradeCost> GetPortfolioCosts(string portfolioName, DateTime cobDate)
        {
            List<TradeCost> costs = null;

            var fullCostFilePath = Path.Combine(_baseDirectory, String.Format("{0}\\{1}\\{1}_{0}_COSTS.json", cobDate.ToString("yyyyMMdd"), portfolioName));

            if (File.Exists(fullCostFilePath))
            {
                using (StreamReader file = File.OpenText(fullCostFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    costs = (List<TradeCost>)serializer.Deserialize(file, typeof(List<TradeCost>));
                }
            }

            return costs;
        }
    }
}