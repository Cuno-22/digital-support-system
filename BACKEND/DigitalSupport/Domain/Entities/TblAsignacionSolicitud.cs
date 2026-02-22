using System;
using System.Collections.Generic;
using WebApiDigitalSupport.Domain.Entities;

namespace WebApiDigitalSupport.Domain;

public partial class TblAsignacionSolicitud
{
    public int NIdAsignacionSolicitud { get; set; }

    public int NIdSolicitud { get; set; }

    public int NIdColaborador { get; set; }

    public bool? BEsCoordinador { get; set; }

    public bool? BEstado { get; set; }

    public virtual TblColaborador NIdColaboradorNavigation { get; set; } = null!;

    public virtual TblSolicitud NIdSolicitudNavigation { get; set; } = null!;
}
