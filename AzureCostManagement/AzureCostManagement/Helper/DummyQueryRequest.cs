using AzureCostManagement.Model;
using System.Text.Json;
using Azure.ResourceManager.CostManagement.Models;
using System.Data;
using DataSet = AzureCostManagement.Model.DataSet;

namespace AzureCostManagement.Helper
{
    public class DummyQueryRequest
    {
        public static string getRequestBody()
        {
            AzureCostandUsageRequest request = new AzureCostandUsageRequest
            {
                type = "ActualCost",
                timeframe = "Custom",

                timePeriod = new TimePeriod
                {
                    from = "2023-11-01T00:00:00.000Z",
                    to = "2023-11-02T23:59:59.000Z"
                },

                dataSet = new DataSet
                {
                    granularity = "Daily",
                    aggregation = new Dictionary<string, Aggregation>
                    {
                        ["totalCost"] = new Aggregation
                        {
                            name = "Cost",
                            function = "Sum"
                        },
                        ["totalCostUSD"] = new Aggregation
                        {
                            name = "CostUSD",
                            function = "Sum"
                        }
                    },

                    sorting = new Sort[]
                    {
                        new Sort
                        {
                            direction = "ascending",
                            name = "UsageDate"
                        }
                    },

                    grouping = new AzureNameType[]
                    {
                        new AzureNameType
                        {
                            type = "Dimension",
                            name = "ServiceName"
                        },
                        new AzureNameType
                        {
                            type = "Dimension",
                            name = "ChargeType"
                        },
                        new AzureNameType
                        {
                             type = "Dimension",
                             name = "PublisherType"
                        }
                    },

                    filter = new Filter
                    {
                        tags = new Tags
                        {
                            name = "cost-center",
                            @operator = "In",
                            values = new string[]
                            {
                                "122012", "343223" 
                            }
                        }
                    }
                }
            };

            //Serialize the request body
            var requestBody = JsonSerializer.Serialize(request);

            Console.WriteLine($"Request Body: {requestBody}");

            return requestBody;
        }

    }
}
