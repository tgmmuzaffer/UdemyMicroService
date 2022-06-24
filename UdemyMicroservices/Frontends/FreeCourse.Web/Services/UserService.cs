using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interface;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserVievModel> GetUser()
        {
            return await _httpClient.GetFromJsonAsync<UserVievModel>("/api/user/getuser");
        }
    }
}
