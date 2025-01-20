import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { Role } from '@models/role.model';
import { RoleService } from '@services/role.service';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  imports: [
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatButtonModule,
    MatCardModule,
    CommonModule,
    RouterModule,
  ],
})
export class RegisterComponent {
  username = '';
  password = '';
  role = '';
  roles: Role[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private roleService: RoleService
  ) {}

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.roleService.getRoles().subscribe({
      next: (data) => {
        this.roles = data;
      },
      error: (error) => {
        console.error('Error loading roles', error);
      },
    });
  }

  onSubmit(): void {
    const registerData = {
      username: this.username,
      password: this.password,
      roleId: this.role,
    };
    this.authService.register(registerData).subscribe({
      next: ({ message }) => {
        Swal.fire('Exito', message, 'success');
        this.router.navigate(['/login']);
      },
      error: ({ error }) => {
        Swal.fire('Error', `Error al registrar: ${error.message}`, 'error');
      },
    });
  }
}
