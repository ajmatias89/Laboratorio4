using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDdeProductos
{
    internal interface Ivalidador
    {
        bool EsValido(string valor);
        string MensajeError { get; }
    }
}