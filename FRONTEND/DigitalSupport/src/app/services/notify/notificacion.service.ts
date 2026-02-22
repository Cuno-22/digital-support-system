import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { Notificacion, NotificacionDATA, NotificacionDLT, MensajeUC } from './INotificacion';
import { INotificacionResponse, IMensajeUCResponse, IRetornoDigitalSupportV2 } from './INotificacionResponse';

@Injectable({
  providedIn: 'root'
})
export class NotificacionService {

    private notificaciones$: BehaviorSubject<Notificacion[]> = new BehaviorSubject<Notificacion[]>([]);

    constructor(private http: HttpClient) {}

    getListaNotificacion(): Observable<INotificacionResponse> {
        return this.http.get<INotificacionResponse>(
            'https://localhost:44334/api/1.0/DigitalSupport/getListaNotificacion'
        ).pipe(
        tap((res) => {
            if (res.success && res.response?.data) {
            this.notificaciones$.next(res.response.data);
            }
        }),
        catchError(this.handleError)
        );
    }

    getListaNotificacionesUC(nIdUsuarioCliente: number): Observable<INotificacionResponse> {
        return this.http.get<INotificacionResponse>(
            `https://localhost:44334/api/1.0/DigitalSupport/getListaNotificacionesUC?nIdUsuarioCliente=${nIdUsuarioCliente}`
        ).pipe(
        tap((res) => {
            if (res.success && res.response?.data) {
            this.notificaciones$.next(res.response.data);
            }
        }),
        catchError(this.handleError)
        );
    }

    getMensajeNotificacionUC(nIdNotificacion: number): Observable<IMensajeUCResponse> {
        return this.http.get<IMensajeUCResponse>(
            `https://localhost:44334/api/1.0/DigitalSupport/getMensajeNotificacionUC?nIdNotificacion=${nIdNotificacion}`
        ).pipe(
            catchError(this.handleError)
        );
    }

    postInsertarNotificacion(data: NotificacionDATA): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
            'https://localhost:44334/api/1.0/DigitalSupport/postInsertarNotificacion', data
        ).pipe(
            catchError(this.handleError)
        );
    }

    postActualizarNotificacion(data: NotificacionDATA): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
            'https://localhost:44334/api/1.0/DigitalSupport/postActualizarNotificacion', data
        ).pipe(
            catchError(this.handleError)
        );
    }

    postEliminarNotificacion(data: NotificacionDLT): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
            'https://localhost:44334/api/1.0/DigitalSupport/postEliminarNotificacion', data
        ).pipe(
            catchError(this.handleError)
        );
    }

    get notificaciones(): Observable<Notificacion[]> {
        return this.notificaciones$.asObservable();
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