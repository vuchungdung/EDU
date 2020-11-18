using System.Collections.Generic;
using System.Text;

namespace EDU.Common.Response
{
    public class ValidationRule
    {
        public string PropertyName;
        public string Rule;

        public ValidationRule(string propertyName, string rule)
        {
            PropertyName = propertyName;
            Rule = rule;
        }
    }

    public static class ValidationRuleExtension
    {
        public static string ToString(this List<ValidationRule> list, string separator)
        {
            StringBuilder text = new StringBuilder();
            if (list != null)
            {
                foreach (ValidationRule rule in list)
                {
                    text.Append(rule.Rule);
                    text.Append(separator);
                }
                return text.ToString();
            }
            else
                return string.Empty;
        }
    }
}