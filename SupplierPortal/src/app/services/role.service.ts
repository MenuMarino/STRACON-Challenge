import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Role } from '@models/role.model';
import { environment } from 'environment/environment';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  private apiUrl = `${environment.apiBaseUrl}/Role`;

  constructor(private http: HttpClient) {}

  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(this.apiUrl);
  }
}
