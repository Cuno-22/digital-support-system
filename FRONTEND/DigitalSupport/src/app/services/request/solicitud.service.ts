import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { Solicitud, SolicitudDATA, SolicitudUC, TipoSolicitud } from './ISolicitud';
import { ISolicitudResponse, ISolicitudDataResponse, ISolicitudFiltradaResponse, ISolicitudUCResponse, ITipoSolicitudResponse, IRetornoDigitalSupport, IRetornoDigitalSupportV2 } from './ISolicitudResponse';

@Injectable({
  providedIn: 'root'
})
export class SolicitudService {

  private solicitudesPendientes$: BehaviorSubject<Solicitud[]> = new BehaviorSubject<Solicitud[]>([]);
  private solicitudesFinalizadas$: BehaviorSubject<Solicitud[]> = new BehaviorSubject<Solicitud[]>([]);
  private solicitudesTotales$: BehaviorSubject<Solicitud[]> = new BehaviorSubject<Solicitud[]>([]);
  
  constructor(private http: HttpClient) {}

  getListaSolicitud(nIdTipoSolicitud: number): Observable<ISolicitudFiltradaResponse> {
    return this.http.get<ISolicitudFiltradaResponse>(
      `https://localhost:44334/api/1.0/DigitalSupport/getListaSolicitud?nIdTipoSolicitud=${nIdTipoSolicitud}`
    ).pipe(
      catchError(this.handleError)
    );
  }

  getListaTipoSolicitud(): Observable<ITipoSolicitudResponse> {
    return this.http.get<ITipoSolicitudResponse>(
      'https://localhost:44334/api/1.0/DigitalSupport/getListaTipoSolicitud'
    ).pipe(
      catchError(this.handleError)
    );
  }

  getListaTotalSolicitudes(): Observable<ISolicitudResponse> {
    return this.http.get<ISolicitudResponse>(
      'https://localhost:44334/api/1.0/DigitalSupport/getListaTotalSolicitudes'
    ).pipe(
      tap((res) => {
        if (res.success && res.response?.data) {
          this.solicitudesTotales$.next(res.response.data);
        }
      }),
      catchError(this.handleError)
    );
  }

  getListaSolicitudEnProcesoPendiente(): Observable<ISolicitudResponse> {
    return this.http.get<ISolicitudResponse>(
      'https://localhost:44334/api/1.0/DigitalSupport/getListaSolicitudEnProcesoPendiente'
    ).pipe(
      tap((res) => {
        if (res.success && res.response?.data) {
          this.solicitudesPendientes$.next(res.response.data);
        }
      }),
      catchError(this.handleError)
    );
  }

  getListaSolicitudFinalizada(): Observable<ISolicitudResponse> {
    return this.http.get<ISolicitudResponse>(
      'https://localhost:44334/api/1.0/DigitalSupport/getListaSolicitudFinalizada'
    ).pipe(
      tap((res) => {
        if (res.success && res.response?.data) {
          this.solicitudesFinalizadas$.next(res.response.data);
        }
      }),
      catchError(this.handleError)
    );
  }

  getSolicitudEspecificoUC(nIdUsuarioCliente: number): Observable<ISolicitudUCResponse> {
    return this.http.get<ISolicitudUCResponse>(
      `https://localhost:44334/api/1.0/DigitalSupport/getSolicitudEspecificoUC?nIdUsuarioCliente=${nIdUsuarioCliente}`
    ).pipe(
      catchError(this.handleError)
    );
  }

  postInsertarSolicitud(data: SolicitudDATA): Observable<IRetornoDigitalSupportV2> {
    return this.http.post<IRetornoDigitalSupportV2>(
      'https://localhost:44334/api/1.0/DigitalSupport/postInsertarSolicitud', data
    ).pipe(
      catchError(this.handleError)
    );
  }

  postActualizarSolicitud(data: SolicitudDATA): Observable<IRetornoDigitalSupportV2> {
    return this.http.post<IRetornoDigitalSupportV2>(
      'https://localhost:44334/api/1.0/DigitalSupport/postActualizarSolicitud', data
    ).pipe(
      catchError(this.handleError)
    );
  }

  get solicitudesPendientes(): Observable<Solicitud[]> {
    return this.solicitudesPendientes$.asObservable();
  }

  get solicitudesFinalizadas(): Observable<Solicitud[]> {
    return this.solicitudesFinalizadas$.asObservable();
  }

  get solicitudesTotales(): Observable<Solicitud[]> {
    return this.solicitudesTotales$.asObservable();
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
