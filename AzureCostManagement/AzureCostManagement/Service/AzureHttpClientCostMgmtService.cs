using AzureCostManagement.Helper;
using AzureCostManagement.Model;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System;

namespace AzureCostManagement.Service
{
    public class AzureHttpClientCostMgmtService
    {        
        private const string baseUrl = "https://management.azure.com";
        private const string grantType = "client_credentials";
        private const string version = "2023-03-01";
        public async Task<AzureCostAndUsageResponse> GetCostExplorerDataAsync(string subscriptionId)
        {
            var costManagementUrl = $"{baseUrl}/subscriptions/{subscriptionId}/providers/Microsoft.CostManagement/query?api-version={version}";

            var token = await Auth.getAuthToken();
            var query = DummyQueryRequest.getRequestBody();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("POST"), costManagementUrl);
                requestMessage.Content = new StringContent(query, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.SendAsync(requestMessage).Result;

                string apiResponse = response.Content.ReadAsStringAsync().Result;
                try
                {
                    AzureCostAndUsageResponse costData = new AzureCostAndUsageResponse();
                    // Attempt to deserialise the reponse to the desired type, otherwise throw an exception with the response from the api.
                    if (apiResponse != "")
                        return JsonSerializer.Deserialize<AzureCostAndUsageResponse>(apiResponse);
                    else
                        throw new Exception();
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error ocurred while calling the API. It responded with the following message: {response.StatusCode} {response.ReasonPhrase}");
                }
            }
        }
    }
}
