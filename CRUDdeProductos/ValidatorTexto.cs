using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDdeProductos
{
    internal class ValidatorTexto : Ivalidador
    {
        public string MensajeError { get; private set; }
        public bool EsValido(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                MensajeError = "El campo no puede estar vacío.";
                return false;
            }
            return true;
        }
    }
}
