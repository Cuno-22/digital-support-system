import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from '../../../shared/nav/nav.component';
import { Router, RouterModule, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { BaseChartDirective } from 'ng2-charts';
import { ChartData, ChartOptions } from 'chart.js';
import { LoginService } from '../../../services/auth/login.service';
import { RegistroService } from '../../../services/register/registro.service';
import { SolicitudService } from '../../../services/request/solicitud.service';
import { ClienteService } from '../../../services/clients/cliente.service';
import { ColaboradorService } from './../../../services/collab/colaborador.service';
import { EstadisticaService } from '../../../services/charts/estadistica.service';
import { UsuarioCliente } from './../../../services/clients/ICliente';
import { IRetorno_DigitalSupportV3 } from '../../../services/auth/IRetornoResponse';

@Component({
  selector: 'app-dashboard-admin',
  standalone: true,
  imports: [CommonModule, NavComponent, RouterModule, MatIconModule, BaseChartDirective],
  templateUrl: './dashboard.admin.component.html',
  styleUrls: ['./dashboard.admin.component.css']
})
export class DashboardAdminComponent implements OnInit, OnDestroy {
  userData?: IRetorno_DigitalSupportV3;
  solicitudesPendientes: number = 0;
  solicitudesAtendidas: number = 0;
  solicitudesTotales: number = 0;
  horasTrabajadas: number = 0;
  colaboradoresActivos: number = 0;

  mostrarUserInfo= false;
  mostrarNotificaciones = false;
  mostrarPanelAdmin = false;

  listaUsuariosCliente: UsuarioCliente[] = [];

  private subscriptions: Subscription[] = [];

  // GRAFICO 1 (barras horizontales)
  @ViewChild('chart1') chart1?: BaseChartDirective;
  public chart1Data: ChartData<'bar', number[], string | number> = {
    labels: [],
    datasets: [{ data: [], label: 'Solicitudes Finalizadas' }]
  };
  public chart1Options: ChartOptions<'bar'> = {
    responsive: true,
    indexAxis: 'y',
    plugins: { legend: { display: false } }
  };

  // GRAFICO 2 (top 5 - barras verticales)
  @ViewChild('chart2') chart2?: BaseChartDirective;
  public chart2Data: ChartData<'bar', number[], string | number> = {
    labels: [],
    datasets: [{ data: [], label: 'Top 5 Colaboradores' }]
  };
  public chart2Options: ChartOptions<'bar'> = {
    responsive: true,
    plugins: { legend: { display: false } }
  };

  //GRAFICO 3 (porcentaje de atencion - pastel)
  @ViewChild('chart3') chart3?: BaseChartDirective;
  public chart3Data: ChartData<'pie', number[], string | number> = {
    labels: [],
    datasets: [{ data: [], label: 'Porcentaje de Atención' }]
  };
  public chart3Options: ChartOptions<'pie'> = {
    responsive: true,
    plugins: { legend: { position: 'right' } }
  };

  // GRAFICO 4 (Radar - Solicitudes Usuario Cliente)
  @ViewChild('chart4') chart4?: BaseChartDirective;
  public chart4Data: ChartData<'radar', number[], string | number> = {
    labels: [],
    datasets: [{ data: [], label: 'Solicitudes por Cliente' }]
  };
  public chart4Options: ChartOptions<'radar'> = {
    responsive: true,
    plugins: { legend: { position: 'top' } },
    scales: {
      r: { beginAtZero: true }
    }
  };

  // GRAFICO 5 (barras lineales - solicitudes por meses)
  @ViewChild('chart5') chart5?: BaseChartDirective;
  public mesesSeleccionados = 6;
  public chart5Data: ChartData<'line', number[], string | number> = {
    labels: [],
    datasets: [{ data: [], label: 'Solicitudes por mes' }]
  };
  public chart5Options: ChartOptions<'line'> = {
    responsive: true,
    plugins: { legend: { display: true } }
  };

  //GRAFICO 6 (Avance de solicitudes de un cliente - línea con área)
  @ViewChild('chart6') chart6?: BaseChartDirective;
  public chart6Data: ChartData<'line', number[], string | number> = {
    labels: [],
    datasets: [{ data: [], label: 'Avance de Solicitudes por UC' }]
  };

  public chart6Options: ChartOptions<'line'> = {
    responsive: true,
    plugins: { legend: { display: true } }
  };

  constructor(private loginService: LoginService, private solicitudService: SolicitudService, private registroService: RegistroService, private clienteService: ClienteService, private colaboradorService: ColaboradorService, private estadisticaService: EstadisticaService, private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    const sub1 = this.loginService.userData.subscribe(data => {
      this.userData = data;
    });

    const sub2 = this.solicitudService.getListaSolicitudEnProcesoPendiente().subscribe();
    const sub3 = this.solicitudService.getListaSolicitudFinalizada().subscribe();
    const sub4 = this.solicitudService.getListaTotalSolicitudes().subscribe();

    const sub5 = this.solicitudService.solicitudesPendientes.subscribe(pendientes => {
      this.solicitudesPendientes = pendientes.length;
    });

    const sub6 = this.solicitudService.solicitudesFinalizadas.subscribe(finalizadas => {
      this.solicitudesAtendidas = finalizadas.length;
    });

    const sub7 = this.solicitudService.solicitudesTotales.subscribe(totales => {
      this.solicitudesTotales = totales.length;
    });

    const sub8 = this.colaboradorService.getListaColaboradoresActivos().subscribe();

    const sub9 = this.colaboradorService.colaboradoresActivos.subscribe(activos => {
      this.colaboradoresActivos = activos.length;
    });

    const sub10 = this.registroService.getPromedioHorasColaboradores().subscribe(res => {
    const colaboradores = res.response?.data ?? [];

    const colaboradoresValidos = colaboradores.filter(c => c.nPromedioHorasResolucion > 0);
    const totalHoras = colaboradoresValidos.reduce((sum, col) => sum + col.nPromedioHorasResolucion, 0);

    this.horasTrabajadas = colaboradoresValidos.length > 0
      ? +(totalHoras / colaboradoresValidos.length).toFixed(2)
      : 0;
  });

  const sub11 = this.router.events.subscribe(event => {
    if (event instanceof NavigationEnd) {
      this.mostrarPanelAdmin = event.urlAfterRedirects === '/dashboard/admin';
    }
  });

  const subUC = this.clienteService.getListaUsuarioCliente().subscribe(res => {
    if (res.success && res.response?.data) {
      this.listaUsuariosCliente = res.response.data;
    }
  });
    this.cargarSolicitudesFinalizadas();
    this.cargarTop5Colaboradores();
    this.cargarAtencionColaboradores();
    this.cargarSolicitudesUsuarioCliente();
    this.cargarSolicitudesPorTipoEnMes(this.mesesSeleccionados);

    this.mostrarPanelAdmin = this.router.url === '/dashboard/admin';
    
    this.subscriptions.push(sub1, sub2, sub3, sub4, sub5, sub6, sub7, sub8, sub9, sub10, sub11);
    this.subscriptions.push(subUC);
  }

  toggleUserMenu() {
    this.mostrarUserInfo = !this.mostrarUserInfo;
    this.mostrarNotificaciones = false;
  }

  toggleNotificaciones() {
    this.mostrarNotificaciones = !this.mostrarNotificaciones;
    this.mostrarUserInfo = false;
  }

  // Gráfico 1: Solicitudes finalizadas por colaborador
  private cargarSolicitudesFinalizadas() {
    const sub = this.estadisticaService.getSolicitudFinalizadaPorColaborador().subscribe(res => {
      const data = res.response?.data ?? [];
      const labels = data.map(c => c.sNombreColaborador);
      const valores = data.map(c => c.nSolicitudesResueltas);

      this.chart1Data = {
        labels,
        datasets: [{ data: valores, label: 'Solicitudes Finalizadas', backgroundColor: '#4CAF50' }]
      };
      setTimeout(() => this.chart1?.chart?.update(), 50);
    });
    this.subscriptions.push(sub);
  }

  // Gráfico 2: Top 5 colaboradores
  private cargarTop5Colaboradores() {
    const sub = this.estadisticaService.getTop5ColaboradorSolicitud().subscribe(res => {
      const data = res.response?.data ?? [];
      const labels = data.map(c => c.sNombreColaborador);
      const valores = data.map(c => c.nSolicitudesResueltas);
      const colors = ['#FF5722', '#FFC107', '#2196F3', '#9C27B0', '#009688'];

      this.chart2Data = {
        labels,
        datasets: [{ data: valores, label: 'Top 5 Colaboradores', backgroundColor: colors.slice(0, labels.length) }]
      };
      setTimeout(() => this.chart2?.chart?.update(), 50);
    });
    this.subscriptions.push(sub);
  }

  //Grafico 3: Porcentaje de Atencion
  private cargarAtencionColaboradores(): void {
    const sub = this.estadisticaService.getListaPorcentajeColaboradorAtencion().subscribe(res => {
      const data = res.response?.data ?? [];
      const labels = data.map(c => c.sNombreColaborador);
      const valores = data.map(c => c.nPorcentajeAtencion);

      const colors = ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF'];

      this.chart3Data = {
        labels,
        datasets: [{
          data: valores,
          label: 'Porcentaje de Atención',
          backgroundColor: colors.slice(0, labels.length)
        }]
      };

      setTimeout(() => this.chart3?.chart?.update(), 50);
    });
    this.subscriptions.push(sub);
  }

  //Grafico 4:
  private cargarSolicitudesUsuarioCliente(): void {
    const sub = this.estadisticaService.getSolicitudUsuarioCliente().subscribe(res => {
      const data = res.response?.data ?? [];
      const labels = data.map(c => c.sNombreUsuarioCliente);
      const valores = data.map(c => c.nSolicitudesEstablecidas);

      this.chart4Data = {
        labels,
        datasets: [{
          data: valores,
          label: 'Solicitudes por Usuario Cliente',
          borderColor: '#FF5722',
          backgroundColor: 'rgba(255,87,34,0.3)',
          pointBackgroundColor: '#FF5722'
        }]
      };

      setTimeout(() => this.chart4?.chart?.update(), 50);
    });
    this.subscriptions.push(sub);
  }

  // Gráfico 5: Solicitudes por meses
  private cargarSolicitudesPorTipoEnMes(nMeses: number) {
    this.mesesSeleccionados = nMeses;

    const sub = this.estadisticaService.getSolicitudPorMeses(nMeses).subscribe(res => {
      const registros = res.response?.data?.[0]?.data ?? [];

      // Filtrar los registros por tipo de solicitud
      const tiposDeSolicitud = ['Error de Software', 'Requerimiento de Software', 'Capacitación sobre uso del software']; // Puedes ajustar estos valores según los tipos de solicitud reales.
      const counts = tiposDeSolicitud.map(() => 0);

      // Contar las solicitudes por tipo
      registros.forEach(r => {
        const tipoSolicitud = r.sTipoSolicitud;
        if (tiposDeSolicitud.includes(tipoSolicitud)) {
          const idx = tiposDeSolicitud.indexOf(tipoSolicitud);
          if (idx >= 0) counts[idx] += 1;
        }
      });

      // Asignar los datos al gráfico
      this.chart5Data = {
        labels: tiposDeSolicitud,
        datasets: [{
          data: counts,
          label: 'Tipos de Solicitud más Comunes',
          borderColor: '#2196F3',
          backgroundColor: 'rgba(33,150,243,0.3)',
          fill: true
        }]
      };

      setTimeout(() => this.chart5?.chart?.update(), 50);
    });

    this.subscriptions.push(sub);
  }

  //Grafico 6: Avance del Historial de Solicitudes por UC
  private cargarHistorialUC(nIdUsuarioCliente: number) {
    const sub = this.estadisticaService.getHistorialSolicitudUsuarioCliente(nIdUsuarioCliente).subscribe(res => {
      if (res.success && res.response?.data) {
        const solicitudes = res.response.data;

        // Ordenar por fecha
        solicitudes.sort((a: any, b: any) =>
          new Date(a.dFechaCreacion).getTime() - new Date(b.dFechaCreacion).getTime()
        );

        // Labels = fechas de creación
        const labels = solicitudes.map((s: any) =>
          new Date(s.dFechaCreacion).toLocaleDateString()
        );

        // Data = acumulado de solicitudes
        let acumulado = 0;
        const data = solicitudes.map(() => ++acumulado);

        // 🚀 Reemplazamos el objeto entero
        this.chart6Data = {
          labels,
          datasets: [{
            data,
            label: 'Avance de Solicitudes por UC',
            borderColor: '#2196F3',
            backgroundColor: 'rgba(33,150,243,0.3)',
            fill: true,
            tension: 0.3,
            pointBackgroundColor: '#2196F3'
          }]
        };

        setTimeout(() => this.chart6?.chart?.update(), 50);
      }
    });
    this.subscriptions.push(sub);
  }

  // Handler del dropdown
  onMonthsChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    const n = Number(value) || 6;
    this.cargarSolicitudesPorTipoEnMes(n);
  }

  onClienteChange(event: Event) {
    const select = event.target as HTMLSelectElement;
    const id = Number(select.value) || 0;
    this.cargarHistorialUC(id);
  }

  cerrarSesion(): void {
    this.loginService.logout();
    this.router.navigateByUrl('/iniciar-sesion');
  }

  gestionarNotificaciones() {
    this.router.navigate(['/dashboard/notificacion/gestionar-visualizar']);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
