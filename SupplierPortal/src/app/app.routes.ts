import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { AprobadorLandingComponent } from './pages/aprobador-landing/aprobador-landing.component';
import { ColocadorLandingComponent } from './pages/colocador-landing/colocador-landing.component';
import { ViewRequestsComponent } from '@components/view-requests/view-requests.component';

export const routes: Routes = [
  {
    path: 'colocador',
    component: ColocadorLandingComponent,
    canActivate: [AuthGuard],
    data: { role: 'Colocador' },
  },
  {
    path: 'aprobador',
    component: AprobadorLandingComponent,
    canActivate: [AuthGuard],
    data: { role: 'Aprobador' },
  },
  {
    path: 'view-requests',
    component: ViewRequestsComponent,
    canActivate: [AuthGuard],
    data: { role: 'Colocador' },
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '**', redirectTo: '/login', pathMatch: 'full' },
];
