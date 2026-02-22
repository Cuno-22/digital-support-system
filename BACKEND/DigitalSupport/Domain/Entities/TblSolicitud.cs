using System;
using System.Collections.Generic;

namespace WebApiDigitalSupport.Domain.Entities;

public partial class TblSolicitud
{
    public int NIdSolicitud { get; set; }

    public int NIdUsuarioCliente { get; set; }

    public int NIdAplicativo { get; set; }

    public int NIdTipoSolicitud { get; set; }

    public string SMotivo { get; set; } = null!;

    public DateTime DFechaCreacion { get; set; }

    public DateTime? DFechaFinalizacion { get; set; }

    public string? SEstado { get; set; }

    public bool? BEstado { get; set; }

    public virtual TblAplicativo NIdAplicativoNavigation { get; set; } = null!;

    public virtual TblTipoSolicitud NIdTipoSolicitudNavigation { get; set; } = null!;

    public virtual TblUsuarioCliente NIdUsuarioClienteNavigation { get; set; } = null!;

    public virtual ICollection<TblAsignacionSolicitud> TblAsignacionSolicituds { get; set; } = new List<TblAsignacionSolicitud>();

    public virtual ICollection<TblNotificacion> TblNotificacions { get; set; } = new List<TblNotificacion>();

    public virtual ICollection<TblRegistroTrabajo> TblRegistroTrabajos { get; set; } = new List<TblRegistroTrabajo>();
}
