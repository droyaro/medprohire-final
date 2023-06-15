

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedProHireAPI.Models.ValidationAttributes
{
    public class RequiredIfAttribute : ValidationAttribute, IClientModelValidator
    {

        private RequiredAttribute _innerAttribute = new RequiredAttribute();

        public string[] Property { get; set; }

        public string[] Value { get; set; }

        public RequiredIfAttribute(String propertyName, String desiredvalue)
        {
            Property = propertyName.Split(',');
            Value = desiredvalue.Split(',');

        }
        public override bool IsValid(object value)
        {
            return _innerAttribute.IsValid(value);
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            ValidationResult result = ValidationResult.Success;
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();

            for (int i = 0; i < Property.Length; i++)
            {
              
                    Object proprtyvalue = type.GetProperty(Property[i]).GetValue(instance, null);
                    if (proprtyvalue != null)
                    {
                        if (proprtyvalue.ToString().ToLower() != Value[i].ToString().ToLower())
                        {
                            result = ValidationResult.Success;
                            return result;
                        }

                    }

                
            }
            return result = base.IsValid(value, context);

        }

        public void AddValidation(ClientModelValidationContext context)
        {

            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-requiredif", errorMessage);
            MergeAttribute(context.Attributes, "data-val-requiredif-property", String.Join(',', Property));
            MergeAttribute(context.Attributes, "data-val-requiredif-value", String.Join(',', Value));
        }

        private bool MergeAttribute(
            IDictionary<string, string> attributes,
            string key,
            string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }

    }
}

