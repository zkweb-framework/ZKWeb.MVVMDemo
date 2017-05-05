using System;
using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;

namespace ZKWeb.MVVMPlugins.MVVM.Example.CrudExample.src.Application.Dtos
{
    [Description("示例数据的传出信息")]
    public class ExampleDataOutputDto : IOutputDto
    {
        [Description("数据Id")]
        public Guid Id { get; set; }
        [Description("数据名称")]
        public string Name { get; set; }
        [Description("数据描述")]
        public string Description { get; set; }
        [Description("创建时间")]
        public string CreateTime { get; set; }
        [Description("更新时间")]
        public string UpdateTime { get; set; }
    }
}
