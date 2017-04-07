using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services {
	[ExportMany]
	[Description("示例服务")]
	public class ExampleService : ApplicationServiceBase {
		[Description("获取对象")]
		public object GetObject(int x) {
			return new { a = x };
		}

		[Description("测试参数")]
		public class TestParam {
			[Description("参数名称")]
			public string ParamName { get; set; }
		}

		[Description("测试Dto")]
		public class TestDto {
			[Description("返回名称")]
			public string ReturnName { get; set; }
		}

		[Description("获取Dto")]
		public TestDto GetDto([Description("参数")]TestParam param) {
			return new TestDto() { ReturnName = param?.ParamName };
		}
	}
}
