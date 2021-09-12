using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Cancun.App.Configurations
{
    public static class MvcConfig
    {
        public static IServiceCollection AddMvcConfiguration(this IServiceCollection services)
        {
            services.AddControllersWithViews(o =>
            {
                o.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "The filled value is invalid for this field.");
                o.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "This field needs to be filled.");
                o.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "This field needs to be filled.");
                o.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "It is necessary that the body in the request is not empty.");
                o.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => "The filled value is invalid for this field.");
                o.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "The filled value is invalid for this field.");
                o.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "Field must be numeric");
                o.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => "The filled value is invalid for this field.");
                o.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => "The filled value is invalid for this field.");
                o.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "Field must be numeric.");
                o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "This field needs to be filled.");

                o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddRazorPages();

            return services;
        }
    }
}