using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Mapping
{

    //normalde bir mvc ve ya api projesinde startup da services configuration altında automapper kullandığımızı belirterek işlem yapıyorduk. buradaki durumda bir dll proje olduğundan implemente ettiğimiz mapper ın nasıl cagirilacagi ile ilgili islem yaptık. Bu islem de lazy loading olarak static bir metotta bağlı olarak cagırılacak sekilde yazıldı.
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> mapper = new Lazy<IMapper>(()=> 
        {
            var config = new MapperConfiguration(cng => {

                cng.AddProfile<CustomMapping>();
            });
            return config.CreateMapper();
        });


        public static IMapper GetMapper => mapper.Value;
    }

}
