using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Collections.Generic;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMDemo.AspNetCore.Swagger
{
    /// <summary>
    /// Swagger Api列表提供器
    /// </summary>
    public class ZKWebSwaggerApiProvider : IApiDescriptionGroupCollectionProvider
    {
        /// <summary>
        /// 提供Api列表
        /// </summary>
        public ApiDescriptionGroupCollection ApiDescriptionGroups
        {
            get
            {
                var provider = ZKWeb.Application.Ioc
                    .Resolve<IApiDescriptionGroupCollectionProvider>(IfUnresolved.ReturnDefault, "Plugin");
                if (provider != null)
                {
                    return provider.ApiDescriptionGroups;
                }
                else
                {
                    return new ApiDescriptionGroupCollection(new List<ApiDescriptionGroup>(), 1);
                }
            }
        }
    }
}
