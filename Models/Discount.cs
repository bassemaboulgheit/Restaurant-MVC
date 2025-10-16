//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Models
//{
//    public class Discount : ValidationAttribute :ICientValidator
//    {
//        public decimal MinAmount { get; set; } = 2000;
//        public string CustomerType { get; set; } = "";
//        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
//        {
//            //return base.IsValid(value, validationContext);
//            if(value == null)
//            {
//                //return new ValidationResult(ErrorMessage);
//                return new ValidationResult($"{validationContext.DisplayName} is required.");
//            }
//            if(!decimal.TryParse(value.ToString(),out decimal amount))
//                return new ValidationResult($"{validationContext.DisplayName} must be a valid number.");

//            if(amount >= MinAmount)
//                return ValidationResult.Success;
//        return new ValidationResult($"Total amount must be at least {MinAmount} to be eligible for discount.");
//        }
//    }
//}
