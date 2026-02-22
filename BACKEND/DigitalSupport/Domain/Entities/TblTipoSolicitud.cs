using System;
using System.Collections.Generic;

namespace WebApiDigitalSupport.Domain.Entities;

public partial class TblTipoSolicitud
{
    public int NIdTipoSolicitud { get; set; }

    public string SDescripcion { get; set; } = null!;

    public bool? BEstado { get; set; }

    public virtual ICollection<TblSolicitud> TblSolicituds { get; set; } = new List<TblSolicitud>();
}
