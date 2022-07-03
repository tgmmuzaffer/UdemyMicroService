using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using System.Text.Json;
using System.Threading.Tasks;
using FreeCourse.Shared.Messages;
using System.Collections.Generic;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response<bool>> DeleteBasket(string userid)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userid);
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("basket not found", 404);

        }

        public async Task<Response<BasketDto>> GetBasket(string userid)
        {
            var existbasket = await _redisService.GetDb().StringGetAsync(userid);
            if (string.IsNullOrEmpty(existbasket))
            {
                return Response<BasketDto>.Fail("Basketnot found", 404);
            }

            return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existbasket), 200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            try
            {
                var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));
               
                
                return status ? Response<bool>.Success(204) : Response<bool>.Fail("basket could not update or save", 500);
            }
            catch (System.Exception e)
            {

                throw new System.Exception(e.Message);
            }

        }
    }
}
