using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDdeProductos
{
    internal class ValidadorEntero : Ivalidador
    {
        public string MensajeError { get; private set; } = string.Empty;

        public bool EsValido(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor) || !int.TryParse(valor, out int resultado) || resultado < 0)
            {
                MensajeError = "El valor debe ser un número entero positivo.";
                return false;

            }
            else
            {
                MensajeError = string.Empty;
                return true;
            }
        }
    }
}
