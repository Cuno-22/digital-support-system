using System;
using System.Collections.Generic;

namespace WebApiDigitalSupport.Domain.Entities;

public partial class TblTipoCliente
{
    public int NIdTipoCliente { get; set; }

    public string SDescripcion { get; set; } = null!;

    public bool? BEstado { get; set; }

    public virtual ICollection<TblCliente> TblClientes { get; set; } = new List<TblCliente>();
}
