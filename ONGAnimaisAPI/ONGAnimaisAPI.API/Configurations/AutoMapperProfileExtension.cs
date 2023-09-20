using AutoMapper;
using ONGAnimaisAPI.API.Mappings;

namespace ONGAnimaisAPI.API.Configurations
{
    public static class AutoMapperProfileExtension
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            var mapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new ONGProfile());
            });

            return mapper;
        }
    }
}
