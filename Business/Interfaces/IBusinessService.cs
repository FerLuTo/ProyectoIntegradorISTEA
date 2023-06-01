using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IBusinessService
    {
        Task<UserBusinessResponse> Create(UserBusinessRequest model);
        Task<UserBusinessResponse> Edit(int id, UserBusinessRequest product);
    }
}
