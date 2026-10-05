using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDdeProductos
{
    internal class ValidadorDecimal : Ivalidador
    {
        public string MensajeError { get; private set; } = string.Empty;

        public bool EsValido(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor) || !decimal.TryParse(valor, out decimal resultado) || resultado < 0)
            {
                MensajeError = "El valor debe ser un número decimal positivo.";
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