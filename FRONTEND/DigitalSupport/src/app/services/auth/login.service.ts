import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from './loginRequest';
import { Router } from '@angular/router';
import { IRetorno_DigitalSupportV3, IRetornoResponse } from './IRetornoResponse';
import { Observable, throwError, BehaviorSubject, catchError, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private currentUserLoginOn = new BehaviorSubject<boolean>(false);
  private currentUserData = new BehaviorSubject<IRetorno_DigitalSupportV3>({
    nIdUsuario: 0,
    sRetorno: '',
    sNombreUsuario: '',
    sPerfil: ''
  });

  constructor(private http: HttpClient, private router: Router) {
    const storedUser = localStorage.getItem('usuario');

    try {
      if (storedUser) {
        const parsedUser: IRetorno_DigitalSupportV3 = JSON.parse(storedUser);
        if (parsedUser && parsedUser.nIdUsuario > 0) {
          this.currentUserData.next(parsedUser);
          this.currentUserLoginOn.next(true);
        }
      }
    } catch (error) {
      console.error('Error al leer usuario desde localStorage:', error);
      localStorage.removeItem('usuario');
    }
  }

  login(credentials: LoginRequest): Observable<IRetornoResponse> {
    return this.http.post<IRetornoResponse>(
      'https://localhost:44334/api/1.0/DigitalSupport/postAutenticacionUsuario',
      credentials
    ).pipe(
      tap((response: IRetornoResponse) => {
        if (
          response.success &&
          response.response &&
          Array.isArray(response.response.data) &&
          response.response.data.length > 0
        ) {
          const data = response.response.data[0];
          const user: IRetorno_DigitalSupportV3 = {
            nIdUsuario: data.nIdUsuario,
            sRetorno: data.sRetorno,
            sNombreUsuario: data.sNombreUsuario,
            sPerfil: data.sPerfil
          };

          this.setUserData(user);
        } else {
          this.currentUserLoginOn.next(false);
        }
      }),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      console.error('Error de red o cliente:', error.error);
    } else {
      console.error(`Error del servidor (status ${error.status}):`, error.error);
    }
    return throwError(() => new Error('Ocurrió un error. Por favor intenta nuevamente.'));
  }

  get userData(): Observable<IRetorno_DigitalSupportV3> {
    return this.currentUserData.asObservable();
  }

  get userLoginOn(): Observable<boolean> {
    return this.currentUserLoginOn.asObservable();
  }

  getUserData(): IRetorno_DigitalSupportV3 | null {
    const user = this.currentUserData.getValue();
    return user && user.nIdUsuario > 0 ? user : null;
  }
  
  setUserData(data: IRetorno_DigitalSupportV3): void {
    this.currentUserData.next(data);
    localStorage.setItem('usuario', JSON.stringify(data));
    this.currentUserLoginOn.next(true);
  }

  logout(): void {
    localStorage.removeItem('usuario');
    this.currentUserLoginOn.next(false);
    this.currentUserData.next({
      nIdUsuario: 0,
      sRetorno: '',
      sNombreUsuario: '',
      sPerfil: ''
    });

    // Redirigir sin recargar completamente (más elegante)
    this.router.navigate(['/iniciar-sesion']);
  }
}


