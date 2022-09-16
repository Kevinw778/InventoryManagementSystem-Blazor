using IMS.CoreBusiness;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels.Validations
{
    public class Sell_EnsureEnoughProductQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var sellViewModel = validationContext.ObjectInstance as SellViewModel;
            if (sellViewModel != null)
            {
                if (sellViewModel.Product != null)
                {
                    if (sellViewModel.Product != null)
                    {
                        if (sellViewModel.Product.Quantity < sellViewModel.QuantityToSell)
                        {
                            return new ValidationResult($"There isn't enough product ({sellViewModel.Product.Quantity}) in the warehouse",
                                new[] { validationContext.MemberName });
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
