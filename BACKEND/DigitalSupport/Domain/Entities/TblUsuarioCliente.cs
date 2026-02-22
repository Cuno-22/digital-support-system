using System;
using System.Collections.Generic;

namespace WebApiDigitalSupport.Domain.Entities;

public partial class TblUsuarioCliente
{
    public int NIdUsuarioCliente { get; set; }

    public string SNombre { get; set; } = null!;

    public string SApellido { get; set; } = null!;

    public string SEmail { get; set; } = null!;

    public string SContrasena { get; set; } = null!;

    public int NIdCliente { get; set; }

    public int NIdRolUsuario { get; set; }

    public bool? BEstado { get; set; }

    public virtual TblCliente NIdClienteNavigation { get; set; } = null!;

    public virtual TblRolUsuario NIdRolUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<TblSolicitud> TblSolicituds { get; set; } = new List<TblSolicitud>();
}
