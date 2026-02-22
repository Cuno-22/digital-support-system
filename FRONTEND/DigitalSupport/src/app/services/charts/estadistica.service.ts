import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { SolicitudColaborador, TopColaborador, AtencionColaborador, RegistroSolicitudMes, SolicitudUsuarioCliente, SolicitudHistorialUC } from './IEstadistica';
import { ISolicitudColaboradorResponse, ITopColaboradorResponse, IAtencionColaboradorResponse, IRegistroSolicitudMesResponse, ISolicitudUsuarioClienteResponse, ISolicitudHistorialUCResponse, IRetornoDigitalSupport, IRetornoDigitalSupportV2 } from './IEstadisticaResponse';

@Injectable({
  providedIn: 'root'
})
export class EstadisticaService {

    constructor(private http: HttpClient) {}


    getSolicitudFinalizadaPorColaborador(): Observable<ISolicitudColaboradorResponse> {
        return this.http.get<ISolicitudColaboradorResponse>(
            'https://localhost:44334/api/1.0/DigitalSupport/getSolicitudFinalizadaPorColaborador'
        ).pipe(         
            catchError(this.handleError)
        );
    }

    getTop5ColaboradorSolicitud(): Observable<ITopColaboradorResponse> {
        return this.http.get<ITopColaboradorResponse>(
            'https://localhost:44334/api/1.0/DigitalSupport/getTop5ColaboradorSolicitud'
        ).pipe(         
            catchError(this.handleError)
        );
    }

    getListaPorcentajeColaboradorAtencion(): Observable<IAtencionColaboradorResponse> {
        return this.http.get<IAtencionColaboradorResponse>(
            'https://localhost:44334/api/1.0/DigitalSupport/getListaPorcentajeColaboradorAtencion'
        ).pipe(         
            catchError(this.handleError)
        );
    }

    getSolicitudUsuarioCliente(): Observable<ISolicitudUsuarioClienteResponse> {
        return this.http.get<ISolicitudUsuarioClienteResponse>(
            'https://localhost:44334/api/1.0/DigitalSupport/getSolicitudUsuarioCliente'
        ).pipe(         
            catchError(this.handleError)
        );
    }

    getSolicitudPorMeses(nMesesAntes: number): Observable<IRegistroSolicitudMesResponse> {
        return this.http.get<IRegistroSolicitudMesResponse>(
            `https://localhost:44334/api/1.0/DigitalSupport/getSolicitudPorMeses?nMesesAntes=${nMesesAntes}`
        ).pipe(
          catchError(this.handleError)
        );
    }

    getHistorialSolicitudUsuarioCliente(nIdUsuarioCliente: number): Observable<ISolicitudHistorialUCResponse> {
        return this.http.get<ISolicitudHistorialUCResponse>(
            `https://localhost:44334/api/1.0/DigitalSupport/getHistorialSolicitudUsuarioCliente?nIdUsuarioCliente=${nIdUsuarioCliente}`
        ).pipe(
          catchError(this.handleError)
        );
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