import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Supplier } from '@models/request.model'; // Assuming a Supplier model exists
import { environment } from 'environment/environment'; // Ensure you have the environment set up

@Injectable({
  providedIn: 'root',
})
export class SupplierService {
  private apiUrl = `${environment.apiBaseUrl}/Supplier`;

  constructor(private http: HttpClient) {}

  getSuppliers(): Observable<Supplier[]> {
    return this.http.get<Supplier[]>(`${this.apiUrl}`, {
      withCredentials: true,
    });
  }

  createSupplier(newSupplier: Supplier): Observable<Supplier> {
    return this.http.post<Supplier>(this.apiUrl, newSupplier, {
      withCredentials: true,
    });
  }
}
