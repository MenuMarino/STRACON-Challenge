import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environment/environment';
import { Request } from '@models/request.model';

@Injectable({
  providedIn: 'root',
})
export class RequestService {
  private apiUrl = `${environment.apiBaseUrl}/PurchaseRequest`;

  constructor(private http: HttpClient) {}

  getPendingRequests(): Observable<Request[]> {
    return this.http.get<Request[]>(`${this.apiUrl}/pending`, {
      withCredentials: true,
    });
  }

  approveRequest(requestId: number): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(
      `${this.apiUrl}/${requestId}/approve`,
      null,
      {
        withCredentials: true,
      }
    );
  }

  rejectRequest(requestId: number): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(
      `${this.apiUrl}/${requestId}/reject`,
      null,
      {
        withCredentials: true,
      }
    );
  }

  getMyRequests(): Observable<Request[]> {
    return this.http.get<Request[]>(`${this.apiUrl}/myrequests`, {
      withCredentials: true,
    });
  }

  createRequest(newRequest: any): Observable<Request> {
    return this.http.post<Request>(this.apiUrl, newRequest, {
      withCredentials: true,
    });
  }

  updateRequest(requestId: number, updatedRequest: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${requestId}`, updatedRequest, {
      withCredentials: true,
    });
  }
}
