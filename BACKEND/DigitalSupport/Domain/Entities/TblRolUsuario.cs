using System;
using System.Collections.Generic;

namespace WebApiDigitalSupport.Domain.Entities;

public partial class TblRolUsuario
{
    public int NIdRolUsuario { get; set; }

    public string SNombreRol { get; set; } = null!;

    public virtual ICollection<TblColaborador> TblColaboradors { get; set; } = new List<TblColaborador>();

    public virtual ICollection<TblUsuarioCliente> TblUsuarioClientes { get; set; } = new List<TblUsuarioCliente>();
}
