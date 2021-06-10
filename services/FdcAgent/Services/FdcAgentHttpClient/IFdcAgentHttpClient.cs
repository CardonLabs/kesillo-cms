using System.Collections.Generic;
using System.Threading.Tasks;

using FdcAgent.Models.FdcShemas;

namespace FdcAgent.Services.FoodStreamService
{
    public interface IFdcAgentHttpClient {
        Task<IList<SRLegacyFoodItem>> GetFoods(int[] fdcIds);
        
    }
}