using System.ComponentModel.DataAnnotations;

namespace University.Core.Validations

{
    public class FormValidator
    {
        public static FormValidatorResults Validate(object form)
        {
            var context = new ValidationContext(form, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(form, context, results, true);
          
            return new FormValidatorResults(isValid, results);
        }
    }
}
