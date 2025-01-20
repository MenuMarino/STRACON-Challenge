import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { RequestService } from '@services/request.service';
import { Request } from '@models/request.model';
import { RequestDetailsDialogComponent } from '../request-details-dialog/request-details-dialog.component';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import Swal from 'sweetalert2';
import { MESSAGES } from '@constants/messages.constants';

@Component({
  selector: 'app-view-requests',
  standalone: true,
  templateUrl: './view-requests.component.html',
  imports: [
    CommonModule,
    MatTableModule,
    FormsModule,
    MatFormFieldModule,
    MatOptionModule,
    MatPaginatorModule,
    MatOptionModule,
    MatSelectModule,
    MatButtonModule,
    RouterModule,
  ],
})
export class ViewRequestsComponent implements OnInit {
  requests: Request[] = [];
  filteredRequests: Request[] = [];
  paginatedRequests: Request[] = [];
  statuses: string[] = ['Aprobada', 'Pendiente', 'Rechazada'];
  selectedStatus: string | null = null;
  pageSize = 10;
  currentPage = 0;
  totalRequests = 0;

  constructor(
    private requestService: RequestService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests(): void {
    this.requestService.getMyRequests().subscribe({
      next: (data) => {
        this.requests = data;
        this.filteredRequests = data;
        this.totalRequests = data.length;
        this.updatePaginatedRequests();
      },
      error: () => {
        Swal.fire('Error', MESSAGES.ERROR.FETCHING_REQUESTS, 'error');
      },
    });
  }

  filterByStatus(): void {
    if (this.selectedStatus) {
      this.filteredRequests = this.requests.filter(
        (request) => request.status === this.selectedStatus
      );
    } else {
      this.filteredRequests = this.requests;
    }
    this.totalRequests = this.filteredRequests.length;
    this.currentPage = 0;
    this.updatePaginatedRequests();
  }

  handlePageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex;
    this.pageSize = event.pageSize;
    this.updatePaginatedRequests();
  }

  updatePaginatedRequests(): void {
    const start = this.currentPage * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedRequests = this.filteredRequests.slice(start, end);
  }

  viewDetails(request: Request): void {
    const dialogRef = this.dialog.open(RequestDetailsDialogComponent, {
      width: '800px',
      data: { request, role: 'Colocador' },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'refresh') {
        this.loadRequests();
      }
    });
  }
}
