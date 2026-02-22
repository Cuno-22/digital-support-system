import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { Cliente, UsuarioCliente, UsuarioClienteDATA, ClienteEmpresa,ClienteUC, ClientePersonaNatural, ClienteTipo } from './ICliente';
import { IClienteResponse, IUsuarioClienteResponse, IClienteUCResponse, IUsuarioClienteDataResponse, ITipoClienteResponse, IRetornoDigitalSupportV2, IClienteUCResponseV2} from './IClienteResponse';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {

  private clientes$ : BehaviorSubject<ClienteTipo []> = new BehaviorSubject<ClienteTipo []>([]);
  private usuariosClientes$: BehaviorSubject<UsuarioCliente[]> = new BehaviorSubject<UsuarioCliente[]>([]);

  constructor(private http: HttpClient) {}

  getListaCliente(nIdTipoCliente: number): Observable<IClienteResponse> {
    return this.http.get<IClienteResponse>(
      `https://localhost:44334/api/1.0/DigitalSupport/getListaCliente?nIdTipoCliente=${nIdTipoCliente}`
    ).pipe(
      tap((res) => {
        if (res.success && res.response?.data) {
          const clientes = res.response.data[0].data ?? [];
          if (nIdTipoCliente === 1) {
            // Cliente Empresa
            this.clientes$.next(clientes as ClienteEmpresa[]);
          } else if (nIdTipoCliente === 2) {
            // Cliente Persona Natural
            this.clientes$.next(clientes as ClientePersonaNatural[]);
          } else {
            const mensaje = res.errors?.[0]?.message;
            if (mensaje) console.warn('Error:', mensaje);
            this.clientes$.next([]);
          }
        }
      }),
      catchError(this.handleError)
    );
  }

  getListaUsuarioCliente(): Observable<IUsuarioClienteResponse> {
    return this.http.get<IUsuarioClienteResponse>(
        'https://localhost:44334/api/1.0/DigitalSupport/getListaUsuarioCliente'
    ).pipe(
        tap((res) => {
        if (res.success && res.response?.data) {
            this.usuariosClientes$.next(res.response.data);
        }
        }),
        catchError(this.handleError)
    );
  }

  getListaTipoCliente(): Observable<ITipoClienteResponse> {
    return this.http.get<ITipoClienteResponse>(
      'https://localhost:44334/api/1.0/DigitalSupport/getListaTipoCliente'
    ).pipe(
      catchError(this.handleError)
    );
  }

  getListaClienteporUC(nIdUsuarioCliente: number): Observable<IClienteUCResponseV2>{
    return this.http.get<IClienteUCResponseV2>(
      `https://localhost:44334/api/1.0/DigitalSupport/getListaClienteporUC?nIdUsuarioCliente=${nIdUsuarioCliente}`
    ).pipe(
      catchError(this.handleError)
    );
  }

  postInsertarCliente(data: Cliente): Observable<IRetornoDigitalSupportV2> {
    return this.http.post<IRetornoDigitalSupportV2>(
      'https://localhost:44334/api/1.0/DigitalSupport/postInsertarCliente', data
    ).pipe(
      catchError(this.handleError)
    );
  }

  postInsertarUsuarioCliente(data: UsuarioClienteDATA): Observable<IRetornoDigitalSupportV2> {
    return this.http.post<IRetornoDigitalSupportV2>(
      'https://localhost:44334/api/1.0/DigitalSupport/postInsertarUsuarioCliente', data
    ).pipe(
      catchError(this.handleError)
    );
  }

  postActualizarCliente(data: Cliente): Observable<IRetornoDigitalSupportV2> {
    return this.http.post<IRetornoDigitalSupportV2>(
      'https://localhost:44334/api/1.0/DigitalSupport/postActualizarCliente', data
    ).pipe(
      catchError(this.handleError)
    );
  }

  postActualizarUsuarioCliente(data: UsuarioClienteDATA): Observable<IRetornoDigitalSupportV2> {
    return this.http.post<IRetornoDigitalSupportV2>(
      'https://localhost:44334/api/1.0/DigitalSupport/postActualizarUsuarioCliente', data
    ).pipe(
      catchError(this.handleError)
    );
  }

  get clientes(): Observable<ClienteTipo[]> {
    return this.clientes$.asObservable();
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