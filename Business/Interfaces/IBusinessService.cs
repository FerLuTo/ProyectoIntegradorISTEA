using Entities.ViewModels.Request;
using Entities.ViewModels.Response;

namespace Business.Interfaces
{
    public interface IBusinessService
    {
        IEnumerable<UserBusinessResponse> GetBusiness();
        Task<UserBusinessResponse> Edit(int id, UserBusinessRequest product);
        UserBusinessResponse GetBusinessById(int id);
    }
}
