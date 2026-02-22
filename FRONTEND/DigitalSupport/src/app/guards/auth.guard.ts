import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { LoginService } from '../services/auth/login.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private loginService: LoginService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
    const userData = this.loginService.getUserData();

    // Si no hay usuario logueado
    if (!userData) {
      return this.router.parseUrl('/iniciar-sesion');
    }
    
    const perfil = userData.sPerfil;
    const allowedRoles = route.data['roles'] as string[];

    // Si la ruta tiene restricción de rol y el usuario no pertenece a ese rol
    if (allowedRoles && !allowedRoles.includes(perfil)) {
      switch (perfil) {
        case 'ADMIN':
          return this.router.parseUrl('/dashboard/admin');
        case 'COLABORADOR':
          return this.router.parseUrl('/dashboard/colaborador');
        case 'CLIENTE':
          return this.router.parseUrl('/dashboard/cliente');
        default:
          return this.router.parseUrl('/iniciar-sesion');
      }
    }
    // Si pasa la validación
    return true;
  }
}