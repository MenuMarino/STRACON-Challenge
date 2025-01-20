import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import Swal from 'sweetalert2';
import { MESSAGES } from '@constants/messages.constants';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [
    FormsModule,
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    RouterModule,
  ],
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    Swal.fire({
      title: 'Iniciando sesión',
      text: MESSAGES.INFO.LOGGING_IN,
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
      },
    });

    this.authService.login(this.username, this.password).subscribe({
      next: ({ role }) => {
        Swal.close();

        this.authService.setRole(role);
        switch (role) {
          case 'Colocador':
            this.router.navigate(['/colocador']);
            break;
          case 'Aprobador':
            this.router.navigate(['/aprobador']);
            break;
          default:
            Swal.fire('Error', MESSAGES.ERROR.ROLE_UNAVAILABLE, 'error');
        }
      },
      error: () => {
        Swal.close();
        Swal.fire('Error', MESSAGES.ERROR.LOGIN, 'error');
      },
    });
  }
}
