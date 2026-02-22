using System;
using System.Collections.Generic;
using WebApiDigitalSupport.Domain.Entities;

namespace WebApiDigitalSupport.Domain;

public partial class TblAplicativo
{
    public int NIdAplicativo { get; set; }

    public string SNombreAplicativo { get; set; } = null!;

    public string SDescripcion { get; set; } = null!;

    public DateOnly DFechaLanzamiento { get; set; }

    public DateOnly DFechaModificacion { get; set; }

    public string SVersion { get; set; } = null!;

    public int NIdCliente { get; set; }

    public bool? BEstado { get; set; }

    public virtual TblCliente NIdClienteNavigation { get; set; } = null!;

    public virtual ICollection<TblSolicitud> TblSolicituds { get; set; } = new List<TblSolicitud>();
}
