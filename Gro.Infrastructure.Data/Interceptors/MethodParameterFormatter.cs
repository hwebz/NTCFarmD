using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Gro.Infrastructure.Data.Interceptors
{
    public static class MethodFormatter
    {
        private static readonly Regex AttributeRegex = new Regex("({[^}]*})");

        private static object GetParameterValue(IReadOnlyList<ParameterInfo> parameters, string propertyExpression, object[] values)
        {
            var propertyPath = propertyExpression.Split('.');
            var parameterName = propertyPath[0];
            for (var i = 0; i < parameters.Count; i++)
            {
                if (parameters[i].Name != parameterName) continue;

                object value;
                try
                {
                    value = GetPropertyValue(values[i], propertyPath);
                }
                catch (MissingMethodException)
                {
                    continue;
                }

                return value;
            }

            throw new InvalidOperationException($"Cannot find parameter with name or path {propertyExpression}");
        }

        private static object GetPropertyValue(object target, string[] propertyPath)
        {
            var value = target;
            if (value == null) return null;
            for (var i = 1; i < propertyPath.Length; i++)
            {
                value = value.GetType().GetProperty(propertyPath[i])?.GetValue(target);
                if (value == null)
                {
                    throw new MissingMethodException();
                }
            }

            return value;
        }

        public static string FormatWithParameters(string keyFormat, ParameterInfo[] parameters, object[] arguments)
        {
            var matches = AttributeRegex.Matches(keyFormat);
            var result = keyFormat;
            foreach (Match match in matches)
            {
                var value = GetParameterValue(parameters, match.ToString().Replace("{", "").Replace("}", ""), arguments);
                if (value == null) continue;

                result = result.Replace(match.ToString(), value.ToString());
            }
            return result;
        }
    }
}
