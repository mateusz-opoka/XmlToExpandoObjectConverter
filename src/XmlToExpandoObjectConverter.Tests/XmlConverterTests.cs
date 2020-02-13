using FluentAssertions;
using NUnit.Framework;
using System.Xml.Linq;

namespace XmlToExpandoObjectConverter.Tests
{
    public class XmlConverterTests
    {
        [Test]
        public void Convert_OnBasicXml_OnElementWithValue_ShouldConvert()
        {
            const string xml = @"<el>value</el>";
            dynamic act = XmlConverter.Convert(XDocument.Parse(xml));
            ((string)act.el).Should().Be("value");
        }

        [Test]
        public void Convert_OnBasicXml_OnElementWithAttribute_ShouldConvert()
        {
            const string xml = @"<el att=""value"" />";
            dynamic act = XmlConverter.Convert(XDocument.Parse(xml));
            ((string)act.el.att).Should().Be("value");
        }

        [Test]
        public void Convert_OnBasicXml_OnElementWithAttributeAndValue_ShouldConvert()
        {
            const string xml = @"<el att=""value1"">value2</el>";
            dynamic act = XmlConverter.Convert(XDocument.Parse(xml));
            ((string)act.el.att).Should().Be("value1");
            ((string)act.el.el).Should().Be("value2");
        }

        [Test]
        public void Convert_OnBasicXml_OnElementAndAttributeWithTheSameName_ShouldConvert()
        {
            const string xml = @"<test test=""value1"">value2</test>";
            dynamic act = XmlConverter.Convert(XDocument.Parse(xml));
            ((string)act.test.test).Should().Be("value1");
        }

        [Test]
        public void Convert_OnBasicXml_OnNestedElements_ShouldConvert()
        {
            const string xml = @"<main><el1 att=""el1_att_value"" /><el2 att=""el2_att_value"" /></main>";
            dynamic act = XmlConverter.Convert(XDocument.Parse(xml));

            ((string)act.main.el1.att).Should().Be("el1_att_value");
            ((string)act.main.el2.att).Should().Be("el2_att_value");
        }

        [Test]
        public void ConvertToJson_OnBasicXml_ShouldConvertToJson()
        {
            const string xml = @"<main><el1 att1=""value""/><el2><el3 att3=""nested value""/></el2></main>";
            var act = XmlConverter.ConvertToJson(XDocument.Parse(xml));
            act.Should().Be("{\"main\":{\"el1\":{\"att1\":\"value\"},\"el2\":{\"el3\":{\"att3\":\"nested value\"}}}}");
        }
    }
}
