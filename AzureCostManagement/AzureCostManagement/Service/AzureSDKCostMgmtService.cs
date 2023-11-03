using Azure.Core;
using Azure.ResourceManager.CostManagement.Models;
using Azure.ResourceManager;
using Azure;
using System.Globalization;
using Azure.ResourceManager.CostManagement;

namespace AzureCostManagement.Service
{
    public class AzureSDKCostMgmtService
    {
        public async Task<Response<QueryResult>> GetCostExplorerDataAsync(string subscriptionId, DateTime? startDate, DateTime? endDate)
        {
            //Create Cost ManagementClient
            var queryScope = $"/subscriptions/{subscriptionId}";

            //create client
            TokenCredential tokenCredential = Helper.Auth.getClientCredentials();
            var client = new ArmClient(tokenCredential, subscriptionId);
            ResourceIdentifier resourceIdentifier = new ResourceIdentifier(queryScope);

            startDate = startDate ?? DateTime.UtcNow.AddDays(-3).Date;
            endDate = endDate ?? DateTime.UtcNow.AddHours(23).AddMinutes(59).AddSeconds(59);


            QueryDefinition qd = new QueryDefinition
            (
                exportType: ExportType.ActualCost,
                timeframe: TimeframeType.Custom,
                dataset: new QueryDataset
                {
                    Granularity = GranularityType.Daily
                }
            )
            {
                TimePeriod = new QueryTimePeriod((DateTimeOffset)startDate, (DateTimeOffset)endDate)
            };

            Response<QueryResult> results = await client.UsageQueryAsync(resourceIdentifier, qd);

            return results;
        }

        public async Task<Response<QueryResult>> GetCostExplorerDataAsync(string subscriptionId, DateTime? startDate, DateTime? endDate, string filterKey, List<string>? filterValues)
        {
            //Create Cost ManagementClient
            var queryScope = $"/subscriptions/{subscriptionId}";

            //create client
            TokenCredential tokenCredential = Helper.Auth.getClientCredentials();
            var client = new ArmClient(tokenCredential, subscriptionId);
            ResourceIdentifier resourceIdentifier = new ResourceIdentifier(queryScope);

            startDate = startDate ?? DateTime.UtcNow.AddDays(-3).Date;
            endDate = endDate ?? DateTime.UtcNow.AddHours(23).AddMinutes(59).AddSeconds(59);

            QueryDefinition qd = new QueryDefinition
             (
                 ExportType.ActualCost,
                 TimeframeType.Custom,
                 new QueryDataset
                 {
                     Granularity = GranularityType.Daily,
                     Aggregation =
                     {
                         ["Cost"] = new QueryAggregation("Cost", FunctionType.Sum),
                         ["CostUSD"] = new QueryAggregation("CostUSD", FunctionType.Sum)
                     },

                     Grouping =
                     {
                         new QueryGrouping(name: "ServiceName", columnType: QueryColumnType.Dimension)
                     },
                     Filter =  new QueryFilter
                     {
                         Tags = new QueryComparisonExpression(name: filterKey, @operator: "In", values: filterValues)
                     }

                 })
            {
                TimePeriod = new QueryTimePeriod((DateTimeOffset)startDate, (DateTimeOffset)endDate)
            };


            Response<QueryResult> costData = await client.UsageQueryAsync(resourceIdentifier, qd);

            return costData;
        }

        public async Task<Response<QueryResult>> GetCostExplorerDataAsync(string subscriptionId, DateTime? startDate, DateTime? endDate, bool multipleFilters)
        {
            //Create Cost ManagementClient
            var queryScope = $"/subscriptions/{subscriptionId}";

            //create client
            TokenCredential tokenCredential = Helper.Auth.getClientCredentials();
            var client = new ArmClient(tokenCredential, subscriptionId);
            ResourceIdentifier resourceIdentifier = new ResourceIdentifier(queryScope);

            startDate = startDate ?? DateTime.UtcNow.AddDays(-3).Date;
            endDate = endDate ?? DateTime.UtcNow.AddHours(23).AddMinutes(59).AddSeconds(59);

            QueryDefinition qd = new QueryDefinition(
                ExportType.ActualCost,
                TimeframeType.Custom,
                new QueryDataset()
                {
                    Granularity = GranularityType.Daily,
                    Filter = new QueryFilter()
                    {
                        And =
                      {
                         new QueryFilter()
                         {
                             Or =
                             {
                                 new QueryFilter()
                                 {
                                     Dimensions = new QueryComparisonExpression("ResourceLocation",QueryOperatorType.In,new string[]
                                     {
                                         "east us", "australia east"
                                     }),
                                 },new QueryFilter()
                                 {
                                     Tags = new QueryComparisonExpression("one-nz-cost-center",QueryOperatorType.In,new string[]
                                     {
                                         "12357074"
                                     }),
                                 }
                             },
                         },new QueryFilter()
                         {
                             Dimensions = new QueryComparisonExpression("ResourceGroup", QueryOperatorType.In,new string[]
                             {
                                 "ServiceName", "ChargeType"
                             }),
                         }
                      },
                    },
                })
            {
                TimePeriod = new QueryTimePeriod((DateTimeOffset)startDate, (DateTimeOffset) endDate)
            };

            Response<QueryResult> costData = await client.UsageQueryAsync(resourceIdentifier, qd);

            return costData;
        }
    }
}
