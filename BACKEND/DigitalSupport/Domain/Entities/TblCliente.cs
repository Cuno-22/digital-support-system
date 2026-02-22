using System;
using System.Collections.Generic;

namespace WebApiDigitalSupport.Domain.Entities;

public partial class TblCliente
{
    public int NIdCliente { get; set; }

    public string SNombre { get; set; } = null!;

    public string? SApellido { get; set; }

    public int? NEdad { get; set; }

    public DateOnly? DFechaNacimiento { get; set; }

    public string SEmail { get; set; } = null!;

    public string SContrasena { get; set; } = null!;

    public int NIdTipoCliente { get; set; }

    public bool? BEstado { get; set; }

    public virtual TblTipoCliente NIdTipoClienteNavigation { get; set; } = null!;

    public virtual ICollection<TblAplicativo> TblAplicativos { get; set; } = new List<TblAplicativo>();

    public virtual ICollection<TblUsuarioCliente> TblUsuarioClientes { get; set; } = new List<TblUsuarioCliente>();
}
