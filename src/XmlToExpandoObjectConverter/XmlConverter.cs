using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;
using System.Xml.Linq;

namespace XmlToExpandoObjectConverter
{
    public static class XmlConverter
    {
        public static ExpandoObject Convert(XDocument xml)
        {
            var result = new ExpandoObject();

            foreach (var element in xml.Elements())
            {
                result.TryAdd(element.Name.LocalName, ConvertElement(element));
            }

            return result;
        }

        public static string ConvertToJson(XDocument xml) => JsonSerializer.Serialize(Convert(xml));

        private static object ConvertElement(XElement element)
        {
            if (!element.HasAttributes && !element.HasElements) return element.Value;

            var nestedObject = new ExpandoObject();

            foreach (var attribute in element.Attributes())
            {
                nestedObject.TryAdd(attribute.Name.LocalName, attribute.Value);
            }

            foreach (var nestedElement in element.Elements())
            {
                nestedObject.TryAdd(nestedElement.Name.LocalName, ConvertElement(nestedElement));
            }

            if (!string.IsNullOrEmpty(element.Value))
            {
                nestedObject.TryAdd(element.Name.LocalName, element.Value);
            }

            return nestedObject;
        }
    }
}
