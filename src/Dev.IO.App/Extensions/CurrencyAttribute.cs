﻿using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DevIO.App.Extensions
{
    public class CurrencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var currency = Convert.ToDecimal(value, new CultureInfo("en-US"));
            }
            catch (Exception)
            {
                return new ValidationResult($"The value '{value}' is not a valid currency format.");
            }

            return ValidationResult.Success;
        }
    }

    public class CurrencyValidationAdapter : AttributeAdapterBase<CurrencyAttribute>
    {
        public CurrencyValidationAdapter(CurrencyAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-currency", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return $"The field must be a valid currency format.";
        }
    }

    public class CurrencyValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is CurrencyAttribute currencyAttribute)
            {
                return new CurrencyValidationAdapter(currencyAttribute, stringLocalizer);
            }
            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }

}
