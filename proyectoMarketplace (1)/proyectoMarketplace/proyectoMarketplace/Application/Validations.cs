using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application

{
    public class Validations
    {
        public static bool EsTelefonoValido(string telefono)
        {
            return telefono.Length == 10 && telefono.All(char.IsDigit);
        }

        public static bool EsCorreo(string input)
        {
            var emailPattern = "^[a-zA-Z0-9._-]+@[a-zA-Z.]+\\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(input, emailPattern);
        }

        public static bool EsUsername(string input)
        {
            var usernamePattern = "^[a-zA-Z0-9._-]{3,}$";
            return Regex.IsMatch(input, usernamePattern);
        }

        public static bool NombreValido(string nombre, string apellido)
        {
            var namePattern = @"^[a-zA-Z\s\u00C0-\u024F]+$";
            return Regex.IsMatch(nombre, namePattern) && Regex.IsMatch(apellido, namePattern);
        }

        public static bool EsDireccionValida(string direccion)
        {
            var pattern = @"^(Calle|Carrera|Avenida|Diagonal|Transversal|Circular)\s*\d+[A-Za-z]?\s*#\s*\d+[A-Za-z]?\s*-\s*\d+$";
            return Regex.IsMatch(direccion.Trim(), pattern, RegexOptions.IgnoreCase);
        }

        public static bool EsDepartamentoValido(string departamento)
        {
            var departmentPattern = @"^[a-zA-Z\s\u00C0-\u024F]+$";
            return Regex.IsMatch(departamento, departmentPattern);
        }

        public static bool EsCiudadValida(string ciudad)
        {
            var cityPattern = @"^[a-zA-Z\s\u00C0-\u024F]+$";
            return Regex.IsMatch(ciudad, cityPattern);
        }

        public static bool EsBarrioValido(string barrio)
        {
            var neighborhoodPattern = @"^[a-zA-Z0-9\s\u00C0-\u024F]+$";
            return Regex.IsMatch(barrio, neighborhoodPattern);
        }

        public static bool EsComplementoValido(string complemento)
        {
            var complementPattern = @"^[a-zA-Z0-9-\s\u00C0-\u024F]+$";
            return Regex.IsMatch(complemento, complementPattern);
        }

        public static bool IsValidBusinessName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            if (name.Length < 2 || name.Length > 100)
                return false;

            // Solo letras, números, espacios y caracteres especiales comunes en nombres de empresa
            return Regex.IsMatch(name, @"^[\p{L}0-9\s\.\,\-\&\'\(\)]+$");
        }

    }
}
