using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWebStandard.Ioc;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.JsonConverters
{
    /// <summary>
    /// 支持转换null到Guid.Empty
    /// </summary>
    [ExportMany]
    public class NullToDefaultGuidConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid);
        }

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = reader.Value as string;
            if (string.IsNullOrEmpty(str))
            {
                return Guid.Empty;
            }
            return Guid.Parse(str);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value?.ToString());
        }
    }
}
