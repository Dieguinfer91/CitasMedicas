using System.ComponentModel.DataAnnotations;

public class CedulaAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string cedula = value as string;

        if (string.IsNullOrWhiteSpace(cedula) || cedula.Length != 10 || !long.TryParse(cedula, out _))
        {
            return new ValidationResult("La cédula debe tener exactamente 10 dígitos numéricos.");
        }

        if (!EsCedulaValida(cedula))
        {
            return new ValidationResult("La cédula no es válida.");
        }

        return ValidationResult.Success;
    }

    private bool EsCedulaValida(string cedula)
    {
        int suma = 0;
        int digitoVerificador = int.Parse(cedula.Substring(9, 1));

        for (int i = 0; i < 9; i++)
        {
            int digito = int.Parse(cedula.Substring(i, 1));
            if (i % 2 != 0) // Posiciones pares (de izquierda a derecha)
            {
                suma += digito;
            }
            else // Posiciones impares
            {
                int producto = digito * 2;
                suma += producto > 9 ? producto - 9 : producto;
            }
        }

        int modulo = suma % 10;
        int resultado = (modulo == 0) ? 0 : 10 - modulo;

        return resultado == digitoVerificador;
    }
}