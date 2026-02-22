import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { Aplicativo, AplicativoDATA } from './IAplicativo';
import { IAplicativoResponse, IAplicativoDataResponse, IRetornoDigitalSupportV2 } from './IAplicativoResponse';

@Injectable({
  providedIn: 'root'
})
export class AplicativoService {

  private aplicativos$: BehaviorSubject<Aplicativo[]> = new BehaviorSubject<Aplicativo[]>([]);
  
  constructor(private http: HttpClient) {}

  getListaAplicativo(nIdUsuarioCliente: number): Observable<IAplicativoResponse> {
    return this.http.get<IAplicativoResponse>(
      `https://localhost:44334/api/1.0/DigitalSupport/getListaAplicativo?nIdUsuarioCliente=${nIdUsuarioCliente}`
    ).pipe(
      catchError(this.handleError)
    );
  }

  postInsertarAplicativo(data: AplicativoDATA): Observable<IRetornoDigitalSupportV2> {
    return this.http.post<IRetornoDigitalSupportV2>(
      'https://localhost:44334/api/1.0/DigitalSupport/postInsertarAplicativo', data
    ).pipe(
      catchError(this.handleError)
    );
  }

  postActualizarAplicativo(data: AplicativoDATA): Observable<IRetornoDigitalSupportV2> {
    return this.http.post<IRetornoDigitalSupportV2>(
      'https://localhost:44334/api/1.0/DigitalSupport/postActualizarAplicativo', data
    ).pipe(
      catchError(this.handleError)
    );
  }

  get listaAplicativos(): Observable<Aplicativo[]> {
    return this.aplicativos$.asObservable();
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
