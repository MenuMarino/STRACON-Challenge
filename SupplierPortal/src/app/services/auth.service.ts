import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'environment/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private roleSubject = new BehaviorSubject<string | null>(
    this.getStoredRole()
  );
  public role$ = this.roleSubject.asObservable();

  private apiUrl = `${environment.apiBaseUrl}/Auth`;

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/Login`,
      { username, password },
      { withCredentials: true }
    );
  }

  register(registerData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Register`, registerData);
  }

  private getStoredRole(): string | null {
    return localStorage.getItem('userRole');
  }

  validateToken(): Observable<any> {
    return this.http.get(`${this.apiUrl}/ValidateToken`, {
      withCredentials: true,
    });
  }

  setRole(role: string): void {
    localStorage.setItem('userRole', role);
    this.roleSubject.next(role);
  }

  clearRole(): void {
    localStorage.removeItem('userRole');
    this.roleSubject.next(null);
  }
}
