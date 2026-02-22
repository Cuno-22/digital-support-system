import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { DashboardAdminComponent } from './pages/dashboard/admin/dashboard.admin.component';
import { DashboardClienteComponent } from './pages/dashboard/client/dashboard.cliente.component';
import { DashboardColaboradorComponent } from './pages/dashboard/collaborator/dashboard.colaborador.component';
//import { FooterComponent } from './shared/footer/footer.component';
import { HeaderComponent } from './shared/header/header.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoginComponent, DashboardAdminComponent, DashboardClienteComponent, DashboardColaboradorComponent, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  protected title = 'DigitalSupport';
}
