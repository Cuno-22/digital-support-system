using System;
using System.Collections.Generic;

namespace WebApiDigitalSupport.Domain.Entities;

public partial class TblRegistroTrabajo
{
    public int NIdRegistroTrabajo { get; set; }

    public int NIdSolicitud { get; set; }

    public int NIdColaborador { get; set; }

    public string SDescripcion { get; set; } = null!;

    public DateOnly DFechaRegistro { get; set; }

    public decimal NHorasTrabajadas { get; set; }

    public string SObservacion { get; set; } = null!;

    public bool? BEstado { get; set; }

    public virtual TblColaborador NIdColaboradorNavigation { get; set; } = null!;

    public virtual TblSolicitud NIdSolicitudNavigation { get; set; } = null!;
}
