import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { AsignacionSolicitud, RegistroTrabajo, RegistroTrabajoDATA, RegistroTrabajoDLT, AsignacionSolicitudDLT} from './IRegistroTrabajo';
import { IAsignacionSolicitudEspecificaResponse, IHorasTrabajadasResponse, IRegistroTrabajoResponse, IRetornoDigitalSupportV2 } from './IRegistroTrabajoResponse';

@Injectable({
  providedIn: 'root'
})
export class RegistroService {
    private promedioHorasColaboradorUnico$: BehaviorSubject<number> = new BehaviorSubject<number>(0);

    constructor(private http: HttpClient) {}

    getPromedioHorasColaboradorUnico(nIdColaborador: number): Observable<IHorasTrabajadasResponse> {
        return this.http.get<IHorasTrabajadasResponse>(
            `https://localhost:44334/api/1.0/DigitalSupport/getPromedioHorasTrabajadasColaboradorUnico?nIdColaborador=${nIdColaborador}`
        ).pipe(
            tap((res) => {
                if (res.success && res.response?.data?.length) {
                    const promedio = res.response.data[0].nPromedioHorasResolucion;
                    this.promedioHorasColaboradorUnico$.next(promedio);
                } else {
                    this.promedioHorasColaboradorUnico$.next(0); // Si no hay datos disponibles
                }
            }),
            catchError(this.handleError)
        );
    }

    getPromedioHorasColaboradores(): Observable<IHorasTrabajadasResponse> {
        return this.http.get<IHorasTrabajadasResponse>(
            'https://localhost:44334/api/1.0/DigitalSupport/getPromedioHorasTrabajadasColaborador'
        ).pipe(
            catchError(this.handleError)
        );
    }

    getListaRegistroTrabajo(nIdColaborador: number): Observable<IRegistroTrabajoResponse> {
        return this.http.get<IRegistroTrabajoResponse>(
            `https://localhost:44334/api/1.0/DigitalSupport/getListaRegistroTrabajo?nIdColaborador=${nIdColaborador}`
        ).pipe(
            catchError(this.handleError)
        );
    }

    getAsignacionSolicitudEspecifica(nIdSolicitud: number, nIdColaborador: number): Observable<IAsignacionSolicitudEspecificaResponse> {
        return this.http.get<IAsignacionSolicitudEspecificaResponse>(
            `https://localhost:44334/api/1.0/DigitalSupport/getAsignacionSolicitudEspecifica?nIdSolicitud=${nIdSolicitud}&nIdColaborador=${nIdColaborador}`
        ).pipe(
            catchError(this.handleError)
        );
    }

    postInsertarAsignacionSolicitud(data: AsignacionSolicitud): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
          'https://localhost:44334/api/1.0/DigitalSupport/postInsertarAsignacionSolicitud', data
        ).pipe(
          catchError(this.handleError)
        );
    }

    postInsertarRegistroTrabajo(data: RegistroTrabajoDATA): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
          'https://localhost:44334/api/1.0/DigitalSupport/postInsertarRegistroTrabajo', data
        ).pipe(
          catchError(this.handleError)
        );
    }

    postActualizarAsignacionSolicitud(data: AsignacionSolicitud): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
          'https://localhost:44334/api/1.0/DigitalSupport/postActualizarAsignacionSolicitud', data
        ).pipe(
          catchError(this.handleError)
        );
    }

    postActualizarRegistroTrabajo(data: RegistroTrabajoDATA): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
          'https://localhost:44334/api/1.0/DigitalSupport/postActualizarRegistroTrabajo', data
        ).pipe(
          catchError(this.handleError)
        );
    }

    postEliminarRegistroTrabajo(data: RegistroTrabajoDLT): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
            'https://localhost:44334/api/1.0/DigitalSupport/postEliminarRegistroTrabajo', data
        ).pipe(
            catchError(this.handleError)
        );
    }
    
    postEliminarAsignacionSolicitud(data: AsignacionSolicitudDLT): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
          'https://localhost:44334/api/1.0/DigitalSupport/postEliminarAsignacionSolicitud', data
        ).pipe(
          catchError(this.handleError)
        );
    }

    get promedioHorasColaboradorUnico(): Observable<number> {
        return this.promedioHorasColaboradorUnico$.asObservable();
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