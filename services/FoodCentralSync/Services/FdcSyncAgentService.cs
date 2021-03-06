using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using FoodCentralSync.Models.FdcShemas.SrLegacy;
using FoodCentralSync.Models.FdcSyncAgent.FdcSyncOptions;

namespace FoodCentralSync.Services.FdcSyncAgentService
{
    public class FdcSyncAgentService : IFdcSyncAgentService
    {
        private readonly DataSources _dataSources;
        private readonly ILogger<FdcSyncAgentService> _log;
        public HttpClient Client { get; }

        public FdcSyncAgentService(HttpClient client, IOptions<DataSources> dataSources, ILogger<FdcSyncAgentService> log)
        {
            _dataSources = dataSources.Value;
            _log = log;

            client.BaseAddress = new Uri(_dataSources.Usda.FoodDataCentral.BaseUrl);
            Client = client;
        }

        public async Task<IList<SRLegacyFoodItem>> GetFoods(int[] fdcIds)
        {
            FdcRequestParams fdc = new FdcRequestParams(){
                fdcIds = fdcIds,
                format = _dataSources.Usda.RequestBody.format,
                nutrients = _dataSources.Usda.RequestBody.nutrients
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var fdcJsonContent = new StringContent(
                JsonSerializer.Serialize<FdcRequestParams>(fdc, options), 
                Encoding.UTF8, 
                "application/json"
            );
            
            var response = await Client.PostAsync(
                _dataSources.Usda.FoodDataCentral.Endpoints.Foods + _dataSources.Usda.FoodDataCentral.Key,
                fdcJsonContent
            );

            using var responseStream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<IList<SRLegacyFoodItem>>(responseStream, options);
        }
    }
}