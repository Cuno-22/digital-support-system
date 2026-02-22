using System;
using System.Collections.Generic;

namespace WebApiDigitalSupport.Domain.Entities;

public partial class TblRolColaborador
{
    public int NIdRolColaborador { get; set; }

    public string SDescripcion { get; set; } = null!;

    public bool? BEstado { get; set; }

    public virtual ICollection<TblColaborador> TblColaboradors { get; set; } = new List<TblColaborador>();
}
