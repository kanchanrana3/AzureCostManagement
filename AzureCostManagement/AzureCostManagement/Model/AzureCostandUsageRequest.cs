using Microsoft.Identity.Client;

namespace AzureCostManagement.Model
{
    public class AzureCostandUsageRequest
    {

        public string? type { get; set; }
        public string? timeframe { get; set; }
        public TimePeriod?  timePeriod { get; set; }
        public DataSet? dataSet { get; set; }        
    }

    public class TimePeriod
    {
        public string? from { get; set; }
        public string? to { get; set; } 
    }

    public class DataSet
    {
        public string? granularity { get; set; }
        public Dictionary<string, Aggregation>? aggregation { get; set; }
        public Sort[]? sorting { get; set; }
        public AzureNameType[]? grouping { get; set; }

        public Filter? filter { get; set; }
    }
        
    public class Aggregation
    {
        public string? name { get; set; }
        public string? function { get; set; }
    }

    public class Sort
    {
        public string? direction { get; set; }
        public string? name { get; set; }
    }

    public class Filter
    {
        public Tags? tags { get; set; }
    }

    public class Tags
    {
        public string? name { get; set; }
        public string? @operator { get; set; } 
        public string[]? values { get; set; }
    }



}
