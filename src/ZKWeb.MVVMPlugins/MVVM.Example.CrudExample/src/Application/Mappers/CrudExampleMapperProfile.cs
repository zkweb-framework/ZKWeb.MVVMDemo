using AutoMapper;
using ZKWeb.MVVMPlugins.MVVM.Example.CrudExample.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Example.CrudExample.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Example.CrudExample.src.Application.Mappers
{
    /// <summary>
    /// AutoMapper的配置
    /// </summary>
    [ExportMany]
    public class CrudExampleMapperProfile : Profile
    {
        public CrudExampleMapperProfile()
        {
            CreateMap<ExampleData, ExampleDataOutputDto>();
            CreateMap<ExampleDataInputDto, ExampleData>();
        }
    }
}
