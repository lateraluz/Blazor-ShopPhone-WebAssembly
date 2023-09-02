using System.Text;

namespace ShopPhone.Server.Extensions;
 

public static class ValidatorExtension
{

    public static string ToListErrorsString(this FluentValidation.Results.ValidationResult validationResult)
    {
        StringBuilder? str = new StringBuilder();
        foreach (var item in validationResult.Errors)
        {
            str.AppendFormat("{0},", item.ErrorMessage.Trim());
        }

        return str.ToString().Substring(0, str.Length - 1);
    }
}