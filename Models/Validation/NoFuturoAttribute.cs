using System;
using System.ComponentModel.DataAnnotations;

namespace cromosmundial_proyecto_final.Models.Validation
{
    public class NoFuturoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue > DateTime.Now)
                {
                    return new ValidationResult("La fecha no puede ser en el futuro.");
                }
            }

            return ValidationResult.Success;
        }
    }

    public class JugadorExisteAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext context)
        {
            if (value is int jugadorId)
            {
                if (jugadorId <= 0)
                {
                    return new ValidationResult("Debe seleccionar un jugador.");
                }
            }

            return ValidationResult.Success;
        }
    }
}