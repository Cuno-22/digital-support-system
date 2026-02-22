import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { Administrador, RolUsuario} from './IAdmin';
import { IAdministradorResponse, IRolUsuarioResponse } from './IAdminResponse';


@Injectable({
  providedIn: 'root'
})
export class AdminService {
    private admins$: BehaviorSubject<Administrador[]> = new BehaviorSubject<Administrador[]>([]);

    constructor(private http: HttpClient) {}

    getListaAdministrador(): Observable<IAdministradorResponse> {
        return this.http.get<IAdministradorResponse>(
            'https://localhost:44334/api/1.0/DigitalSupport/getListaAdministrador'
        ).pipe(
            tap((res) => {
            if (res.success && res.response?.data) {
                this.admins$.next(res.response.data);
            }
            }),
            catchError(this.handleError)
        );
    }


    getListaRolUsuario(): Observable<IRolUsuarioResponse> {
        return this.http.get<IRolUsuarioResponse>(
            'https://localhost:44334/api/1.0/DigitalSupport/getListaRolUsuario'
        ).pipe(
            catchError(this.handleError)
        );
    }


    get administradores(): Observable<Administrador[]> {
        return this.admins$.asObservable();
    }

    private handleError(error: HttpErrorResponse) {
        if (error.status === 0) {
        console.error('Error de red o cliente:', error.error);
        } else {
        console.error('Error del backend:', error.status, error.error);
        }
        return throwError(() => new Error('Sucedió un error al obtener las solicitudes.'));
    }
}