using System;
using System.Collections.Generic;

namespace WebApiDigitalSupport.Domain.Entities;

public partial class TblNotificacion
{
    public int NIdNotificacion { get; set; }

    public int NIdSolicitud { get; set; }

    public string? SDescripcion { get; set; }

    public DateTime DFechaEnvio { get; set; }

    public bool? BLeido { get; set; }

    public bool? BEstado { get; set; }

    public virtual TblSolicitud NIdSolicitudNavigation { get; set; } = null!;
}
