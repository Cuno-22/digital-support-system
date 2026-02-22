using System;
using System.Collections.Generic;
using WebApiDigitalSupport.Domain.Entities;

namespace WebApiDigitalSupport.Domain;

public partial class TblColaborador
{
    public int NIdColaborador { get; set; }

    public string SNombre { get; set; } = null!;

    public string SApellido { get; set; } = null!;

    public string SEmail { get; set; } = null!;

    public string SContrasena { get; set; } = null!;

    public int NIdRolColaborador { get; set; }

    public int NIdRolUsuario { get; set; }

    public bool? BEstado { get; set; }

    public virtual TblRolColaborador NIdRolColaboradorNavigation { get; set; } = null!;

    public virtual TblRolUsuario NIdRolUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<TblAsignacionSolicitud> TblAsignacionSolicituds { get; set; } = new List<TblAsignacionSolicitud>();

    public virtual ICollection<TblRegistroTrabajo> TblRegistroTrabajos { get; set; } = new List<TblRegistroTrabajo>();
}
