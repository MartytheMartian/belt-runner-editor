using System.Globalization;
using System.Windows.Controls;

namespace BeltRunnerEditor.Validators
{
    /// <summary>
    /// Ensures the value is a double
    /// </summary>
    public class DoubleRule : ValidationRule
    {
        /// <summary>
        /// Validates that a value is a double
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cultureInfo">Culture info at runtime</param>
        /// <returns>Result of the validation</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // Try to parse
            double dummyValue;
            bool result = double.TryParse(value.ToString(), out dummyValue);

            return new ValidationResult(result, null);
        }
    }
}
