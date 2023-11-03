using AzureCostManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager.CostManagement;
using Azure.ResourceManager.CostManagement.Models;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Routing;
using System.Net.Http.Json;
using System.Text;
using AzureCostManagement.Helper;
using AzureCostManagement.Service;

namespace AzureCostManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureCostManagementController : Controller
    {
        private readonly AzureHttpClientCostMgmtService _azureHttpClientCostMgmtService;
        private readonly AzureSDKCostMgmtService _azureSDKCostMgmtService;

        public AzureCostManagementController(AzureHttpClientCostMgmtService service, AzureSDKCostMgmtService azureSDKService)
        {
            _azureHttpClientCostMgmtService = service;
            _azureSDKCostMgmtService = azureSDKService;
        }

        [HttpGet("{subscriptionId}")]
        public async Task<AzureCostAndUsageResponse> GetCostExplorerDataAsync(string subscriptionId)
        {
            return await  _azureHttpClientCostMgmtService.GetCostExplorerDataAsync(subscriptionId);
        }

        [HttpGet("sdk/{subscriptionId}")]
        public async Task<QueryResult> GetCostExplorerDataAsync(string subscriptionId, DateTime? startDate, DateTime? endDate)
        {
            return await _azureSDKCostMgmtService.GetCostExplorerDataAsync(subscriptionId, startDate, endDate);
        }

        [HttpPost("sdk/{subscriptionId}/filter/{filterKey}")]
        public async Task<QueryResult> GetCostExplorerDataWithTagFilterAsync(string subscriptionId, DateTime? startDate, DateTime? endDate, string filterKey, [FromBody] List<string>? filterValues)
        {
            filterValues = filterValues ?? new List<string> { "12204611", "aoai-your-data-service" };

            return await _azureSDKCostMgmtService.GetCostExplorerDataAsync(subscriptionId, startDate, endDate, filterKey, filterValues);
        }

        [HttpGet("sdk/{subscriptionId}/multipleFilters")]
        public async Task<QueryResult> GetCostExplorerDataWithFiltersAsync(string subscriptionId, DateTime? startDate, DateTime? endDate)
        {
            return await _azureSDKCostMgmtService.GetCostExplorerDataAsync(subscriptionId, startDate, endDate, true);
        }

    }
}
