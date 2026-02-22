import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../../shared/nav/nav.component';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { ColaboradorService } from '../../../../services/collab/colaborador.service';
import { RolColaborador, RolColaboradorDATA } from '../../../../services/collab/IColaborador';


@Component({
  selector: 'rol-colaborador',
  standalone: true,
  imports: [CommonModule, MatIconModule, FormsModule],
  templateUrl: './rol-colaborador.component.html',
  styleUrls: ['./rol-colaborador.component.css']
})
export class RolColaboradorComponent implements OnInit, OnDestroy {
    
    listaRolesColaborador: RolColaborador[] = [];

    nuevoRol: RolColaboradorDATA = {
        sDescripcion: '',
        bEstado: true
    };

    mensajeError: string = '';
    mensajeExito: string = '';
    mostrarMensaje: boolean = false;

    modoEdicion: boolean = false;
    rolEditando: RolColaborador | null = null;

    private subscriptions: Subscription[] = [];
    
    constructor(private colaboradorService: ColaboradorService, private router: Router) {}
    
    ngOnInit(): void {
        this.cargarRolesColaborador();
    }

    cargarRolesColaborador(): void {
        const sub = this.colaboradorService.getListaRolColaborador().subscribe(res => {
            if (res.success && res.response?.data) {
                this.listaRolesColaborador = res.response.data;
            }
        });
        this.subscriptions.push(sub);
    }

    crearRolColaborador(): void {
        const descripcion = this.nuevoRol.sDescripcion.trim();

        const rolExistente = this.listaRolesColaborador.some(
            rol => rol.sFuncionColaborador.toLowerCase() === descripcion.toLowerCase()
        );

        if (rolExistente) {
            this.mostrarError('El rol de colaborador ya existe. Ingrese uno diferente.');
            return;
        }
        
        if (!descripcion) {
            this.mostrarError('La descripción del rol es obligatoria.');
            return;
        }

        const sub = this.colaboradorService.postInsertarRolColaborador(this.nuevoRol).subscribe(res => {
        if (res?.nRetorno === 0 && res?.sRetorno) {
            this.mostrarError(res.sRetorno);
            return;
        }

        this.mostrarMensajeExito(res?.sRetorno ?? 'Rol de colaborador registrado correctamente');
        this.resetFormulario();
        this.cargarRolesColaborador();
        });

        this.subscriptions.push(sub);
    }

    iniciarEdicion(rol: RolColaborador): void {
        this.modoEdicion = true;
        this.rolEditando = rol;

        this.nuevoRol = {
            nIdRolColaborador: rol.nIdRolColaborador,
            sDescripcion: rol.sFuncionColaborador,
            bEstado: rol.bEstado
        };
    }

    cancelarEdicion(): void {
        this.modoEdicion = false;
        this.rolEditando = null;
        this.resetFormulario();
    }

    actualizarRolColaborador(): void {
        const descripcion = this.nuevoRol.sDescripcion.trim();

        if (!descripcion) {
            this.mostrarError('La descripción del rol es obligatoria.');
            return;
        }

        // Validación: Estado no definido (solo si fuera posible que venga null)
        if (this.nuevoRol.bEstado === null || this.nuevoRol.bEstado === undefined) {
            this.mostrarError('El estado del rol debe ser seleccionado.');
            return;
        }

        // Validación: Descripción duplicada
        const descripcionDuplicada = this.listaRolesColaborador.some(
            rol =>
                rol.sFuncionColaborador.trim().toLowerCase() === descripcion.toLowerCase() &&
                rol.nIdRolColaborador !== this.nuevoRol.nIdRolColaborador
        );

        if (descripcionDuplicada) {
            this.mostrarError('Ya existe un rol con esa descripción. Ingrese una diferente.');
            return;
        }

        // Validación: ID necesario
        if (!this.nuevoRol.nIdRolColaborador) {
            this.mostrarError('No se pudo identificar el rol a actualizar.');
            return;
        }

        const sub = this.colaboradorService.postActualizarRolColaborador(this.nuevoRol).subscribe(res => {
            if (res?.nRetorno === 0 && res?.sRetorno) {
                this.mostrarError(res.sRetorno);
                return;
            }

            this.mostrarMensajeExito(res?.sRetorno ?? 'Rol actualizado correctamente');
            this.cancelarEdicion();
            this.cargarRolesColaborador();
        });

        this.subscriptions.push(sub);
    }

    mostrarError(msg: string): void {
        this.mensajeError = msg;
        setTimeout(() => this.mensajeError = '', 3500);
    }

    mostrarMensajeExito(msg: string): void {
        this.mensajeExito = msg;
        this.mostrarMensaje = true;
        setTimeout(() => {
        this.mostrarMensaje = false;
        this.mensajeExito = '';
        }, 3500);
    }

    resetFormulario(): void {
        this.nuevoRol = {
        sDescripcion: '',
        bEstado: true
        };
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(sub => sub.unsubscribe());
    }
}