import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { Colaborador, RolColaborador, ColaboradorDATA, RolColaboradorDATA } from './IColaborador';
import { IColaboradorResponse, IColaboradorDataResponse, IRolColaboradorResponse, IRolColaboradorDataResponse, IRetornoDigitalSupportV2} from './IColaboradorResponse';

@Injectable({
  providedIn: 'root'
})
export class ColaboradorService {

    private colaboradoresActivos$: BehaviorSubject<Colaborador[]> = new BehaviorSubject<Colaborador[]>([]);
    private rolColaboradores$: BehaviorSubject<RolColaborador[]> = new BehaviorSubject<RolColaborador[]>([]);

    constructor(private http: HttpClient) {}

    getListaColaborador(nIdRolColaborador: number): Observable<IColaboradorResponse> {
        return this.http.get<IColaboradorResponse>(
           `https://localhost:44334/api/1.0/DigitalSupport/getListaColaborador?nIdRolColaborador=${nIdRolColaborador}`
        ).pipe(
        catchError(this.handleError)
        ); 
    }

    getListaColaboradoresActivos(): Observable<IColaboradorResponse> {
        return this.http.get<IColaboradorResponse>(
            'https://localhost:44334/api/1.0/DigitalSupport/getListaColaboradoresActivos'
        ).pipe(
            tap((res) => {
            if (res.success && res.response?.data) {
                this.colaboradoresActivos$.next(res.response.data);
            }
            }),
            catchError(this.handleError)
        );
    }

    getListaRolColaborador(): Observable<IRolColaboradorResponse> {
        return this.http.get<IRolColaboradorResponse>(
          'https://localhost:44334/api/1.0/DigitalSupport/getListaRolColaborador'
        ).pipe(
          catchError(this.handleError)
        );
    }

    postInsertarColaborador(data: ColaboradorDATA): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
            'https://localhost:44334/api/1.0/DigitalSupport/postInsertarColaborador', data
        ).pipe(
             catchError(this.handleError)
        );
    }

    postInsertarRolColaborador(data: RolColaboradorDATA): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
            'https://localhost:44334/api/1.0/DigitalSupport/postInsertarRolColaborador', data
        ).pipe(
             catchError(this.handleError)
        );
    }

    postActualizarColaborador(data: ColaboradorDATA): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
            'https://localhost:44334/api/1.0/DigitalSupport/postActualizarColaborador', data
        ).pipe(
             catchError(this.handleError)
        );
    }

    postActualizarRolColaborador(data: RolColaboradorDATA): Observable<IRetornoDigitalSupportV2> {
        return this.http.post<IRetornoDigitalSupportV2>(
            'https://localhost:44334/api/1.0/DigitalSupport/postActualizarRolColaborador', data
        ).pipe(
             catchError(this.handleError)
        );
    }

    get colaboradoresActivos(): Observable<Colaborador[]> {
        return this.colaboradoresActivos$.asObservable();
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