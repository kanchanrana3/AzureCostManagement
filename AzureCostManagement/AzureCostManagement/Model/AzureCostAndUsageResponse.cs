using AzureCostManagement.Model;

namespace AzureCostManagement.Model
{
    public class AzureCostAndUsageResponse
    {

        public string? id { get; set; }
        public string? name { get; set; }
        public string? type { get; set; }
        public string? location { get; set; }
        public string? sku { get; set; }
        public string? eTag { get; set; }
        public Property? properties { get; set; }        
    }

    public class Property
    {
        public string? nextLink { get; set; }
        public List<AzureNameType>? columns { get; set; }
        public List<List<object>>? rows { get; set; }
    }
}
