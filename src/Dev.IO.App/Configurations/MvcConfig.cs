using Microsoft.AspNetCore.Mvc;

namespace DevIO.App.Configurations
{
    public static class MvcConfig
    {
        public static IServiceCollection AddMvcConfiguration(this IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews(o =>
            {
                o.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "The value entered is invalid for this field.");
                o.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "This field is required.");
                o.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "This field is required.");
                o.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "The request body cannot be empty.");
                o.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => "The value entered is invalid for this field.");
                o.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "The value entered is invalid for this field.");
                o.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "The field must be numeric.");
                o.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => "The value entered is invalid for this field.");
                o.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => "The value entered is invalid for this field.");
                o.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "The field must be numeric.");
                o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "This field is required.");

                o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddRazorPages();

            return services;
        }
    }
}
