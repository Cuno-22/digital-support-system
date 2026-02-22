import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FooterComponent } from '../../shared/footer/footer.component';
import { ReactiveFormsModule , FormBuilder, Validators} from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';
import { LoginService } from '../../services/auth/login.service';
import { LoginRequest } from '../../services/auth/loginRequest';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FooterComponent, ReactiveFormsModule, MatProgressSpinnerModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: any;
  loginError: string = '';
  loginMessage: string = '';

  hidePassword: boolean = true;
  visualizarLoadingScreen: boolean = false;
  mensajeLoadingScreen: string = '';

  constructor(private formBuilder: FormBuilder, private router: Router, private loginService: LoginService) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      sEmail: ['', [Validators.required, Validators.email]],
      sContrasena: ['', Validators.required],
    });
    // Limpia cualquier valor almacenado previamente
     this.loginForm.reset();

    // Listener para detectar cambios en localStorage (sincronización entre pestañas)
    window.addEventListener('storage', (event) => {
      if (event.key === 'nIdAdmin' || event.key === 'nIdColaborador' || event.key === 'nIdUsuarioCliente') {
        // Si los datos de sesión cambian, redirigir al login
        this.router.navigateByUrl('/iniciar-sesion');
      }
    });
  }

  get email() {
    return this.loginForm.controls.sEmail;
  }

  get password() {
    return this.loginForm.controls.sContrasena;
  }

  togglePasswordVisibility() {
    this.hidePassword = !this.hidePassword;
  }
  
  login() {
    this.loginError = '';
    this.loginMessage = '';

    if (this.loginForm.valid) {
      const loginData: LoginRequest = {
        sEmail: this.loginForm.value.sEmail,
        sContrasena: this.loginForm.value.sContrasena
      };

      this.loginService.login(loginData).subscribe({
        next: (response) => {
          const perfil = response.response?.data?.[0]?.sPerfil || '';
          const mensaje = response.response?.data?.[0]?.sRetorno;
          const idUsuario = response.response?.data?.[0]?.nIdUsuario;
          const nombre = response.response?.data?.[0]?.sNombreUsuario;

          if (
            mensaje === 'Correo Electronico o Contraseña incorrectos' ||
            (mensaje?.trim().toLowerCase().includes('incorrecto') ?? false)
          ) {
            this.loginError = mensaje ?? 'Credenciales incorrectas.';
            return;
          }

          if (response.success && perfil) {
            this.visualizarLoadingScreen = true;
            this.mensajeLoadingScreen = mensaje ?? 'Cargando...';

            setTimeout(() => {
              this.loginService.setUserData({
                sPerfil: perfil ?? '',
                nIdUsuario: idUsuario ?? 0,
                sNombreUsuario: nombre ?? '',
                sRetorno: mensaje ?? ''
              });

              if (perfil === 'ADMIN') {
                localStorage.setItem('nIdAdmin', idUsuario?.toString() ?? '0');
                localStorage.setItem('sNombreAdmin', nombre?.toString() ?? '');
                localStorage.removeItem('nIdColaborador');
                localStorage.removeItem('nIdUsuarioCliente');
                this.router.navigateByUrl('/dashboard/admin');
              } else if (perfil === 'COLABORADOR') {
                localStorage.setItem('nIdColaborador', idUsuario?.toString() ?? '0');
                localStorage.setItem('sNombreColaborador', nombre?.toString() ?? '');
                localStorage.removeItem('nIdAdmin');
                localStorage.removeItem('nIdUsuarioCliente');
                this.router.navigateByUrl('/dashboard/colaborador');
              } else if (perfil === 'CLIENTE') {
                localStorage.setItem('nIdUsuarioCliente', idUsuario?.toString() ?? '0');
                localStorage.setItem('sNombreUsuarioCliente', nombre?.toString() ?? '');
                localStorage.removeItem('nIdAdmin');
                localStorage.removeItem('nIdColaborador');
                this.router.navigateByUrl('/dashboard/cliente');
              } else {
                this.loginError = "Tipo de usuario no reconocido.";
              }

              this.visualizarLoadingScreen = false;
            }, 3000);

            this.loginForm.reset();
          } else {
            this.loginError = response.errors?.[0]?.message || 'Credenciales incorrectas.';
          }
        },
        error: () => {
          this.loginError = "Error del servidor. Intenta más tarde.";
        }
      });
    } else {
      this.loginForm.markAllAsTouched();
      this.loginError = "Por favor completa correctamente todos los campos.";
    }
  }
}