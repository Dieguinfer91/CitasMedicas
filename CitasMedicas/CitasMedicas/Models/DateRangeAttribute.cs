using System;
using System.ComponentModel.DataAnnotations;

public class DateRangeAttribute : ValidationAttribute
{
    private readonly DateTime _minDate;
    private readonly DateTime _maxDate;

    public DateRangeAttribute(int daysFromNowMin, int daysFromNowMax)
    {
        _minDate = DateTime.Now.AddDays(daysFromNowMin);
        _maxDate = DateTime.Now.AddDays(daysFromNowMax);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dateValue = (DateTime)value;

        if (dateValue < _minDate || dateValue > _maxDate)
        {
            return new ValidationResult(ErrorMessage ?? $"La Fecha debe estar entre {_minDate.ToShortDateString()} y {_maxDate.ToShortDateString()}.");
        }

        return ValidationResult.Success;
    }
}
