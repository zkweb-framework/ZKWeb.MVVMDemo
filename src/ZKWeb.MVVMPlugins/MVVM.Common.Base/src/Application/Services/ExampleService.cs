using System.ComponentModel;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Services {
	[ExportMany]
	[Description("示例服务")]
	public class ExampleService : ApplicationServiceBase {
		public object GetObject() {
			return new { a = 1 };
		}

		public class TestParam {
			public string ParamName { get; set; }
		}

		public class TestDto {
			public string ReturnName { get; set; }
		}

		public TestDto GetDto(TestParam param) {
			return new TestDto();
		}
	}
}
