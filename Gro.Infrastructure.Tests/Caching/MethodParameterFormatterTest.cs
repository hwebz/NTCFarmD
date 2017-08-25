using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Gro.Infrastructure.Data.Interceptors.MethodFormatter;

namespace Gro.Infrastructure.Tests.Caching
{
    internal class FormatterTestClass
    {
        public void MethodNoParameter()
        {
        }

        public void MethodOneParameter(string param)
        {
        }

        public void MethodClassParameter(string str, NestedData nested)
        {
        }

        public class NestedData
        {
            public string PropOne { get; set; }
            public int PropTwo { get; set; }
        }
    }

    [TestClass]
    public class MethodParameterFormatterTest
    {
        private static FormatterTestClass GetTestClass() => new FormatterTestClass();

        [TestMethod]
        public void TestFormatNoParameterNoValue()
        {
            var target = GetTestClass();
            var formatString = "_Format";
            var paramsInfo = target.GetType().GetMethod(nameof(FormatterTestClass.MethodNoParameter)).GetParameters();
            var output = FormatWithParameters(formatString, paramsInfo, new object[0]);

            Assert.AreEqual(output, formatString);
        }

        [TestMethod]
        public void TestFormatWithParameter()
        {
            var target = GetTestClass();
            var formatString = "_Format_{param}";
            var paramsInfo = target.GetType().GetMethod(nameof(FormatterTestClass.MethodOneParameter)).GetParameters();
            var param = "object";
            var output = FormatWithParameters(formatString, paramsInfo, new object[] {param});

            Assert.AreEqual(output, $"_Format_{param}");
        }


        [TestMethod]
        public void TestFormatWithNestedParameterExpression()
        {
            var nestedData = new FormatterTestClass.NestedData
            {
                PropOne = "One",
                PropTwo = 2
            };
            var target = GetTestClass();
            var formatString = "_Format_{str}_{nested.PropOne}_{nested.PropTwo}";
            var paramsInfo = target.GetType().GetMethod(nameof(FormatterTestClass.MethodClassParameter)).GetParameters();
            var strParam = "string";
            var output = FormatWithParameters(formatString, paramsInfo, new object[] {strParam, nestedData});

            Assert.AreEqual(output, $"_Format_{strParam}_{nestedData.PropOne}_{nestedData.PropTwo}");
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException),
            "Cannot find parameter with name or path nested.PropThree")]
        public void TestFormatWithWrongPathName()
        {
            var nestedData = new FormatterTestClass.NestedData
            {
                PropOne = "One",
                PropTwo = 2
            };
            var target = GetTestClass();
            var formatString = "_Format_{str}_{nested.PropOne}_{nested.PropThree}";
            var paramsInfo = target.GetType().GetMethod(nameof(FormatterTestClass.MethodClassParameter)).GetParameters();
            var strParam = "string";
            FormatWithParameters(formatString, paramsInfo, new object[] {strParam, nestedData});
        }
    }
}
