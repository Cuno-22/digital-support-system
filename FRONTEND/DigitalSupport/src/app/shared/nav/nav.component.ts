import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginService } from '../../services/auth/login.service';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  userLoginOn: boolean = false;
  mostrarNavbar: boolean = true;

  constructor(private loginService:LoginService, private router: Router) { }

  ngOnInit(): void {
    this.loginService.userLoginOn.subscribe(
    {
      next:(userLoginOn) => {
        this.userLoginOn=userLoginOn;
      }
    });

    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        const rutasSinNavbar = ['/iniciar-sesion', '/dashboard/admin', '/dashboard/cliente', '/dashboard/colaborador'];
        this.mostrarNavbar = !rutasSinNavbar.includes(event.urlAfterRedirects);
      }
    });
  }

  logout(): void {
    this.loginService.logout();
    this.router.navigate(['/iniciar-sesion']);
  }
}
