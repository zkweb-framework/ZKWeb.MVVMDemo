using System;
using System.Collections.Generic;
using System.Text;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.CORSExposeHeaders.Interfaces
{
    /// <summary>
    /// 允许跨站请求返回的头
    /// </summary>
    public interface ICORSExposeHeader
    {
        string ExposeHeader { get; }
	}
}
