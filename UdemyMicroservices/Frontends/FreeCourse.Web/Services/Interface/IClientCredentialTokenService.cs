using System;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Interface
{
    public interface IClientCredentialTokenService
    {
        Task<String> GetToken();

    }
}
