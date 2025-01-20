import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RequestService } from '@services/request.service';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { RequestDetailsDialogComponent } from '@components/request-details-dialog/request-details-dialog.component';
import Swal from 'sweetalert2';
import { Request } from '@models/request.model';
import { MESSAGES } from '@constants/messages.constants';

@Component({
  selector: 'app-aprobador-landing',
  standalone: true,
  templateUrl: './aprobador-landing.component.html',
  styleUrls: ['./aprobador-landing.component.scss'],
  imports: [CommonModule, MatTableModule, MatDialogModule],
})
export class AprobadorLandingComponent implements OnInit {
  pendingRequests: Request[] = [];

  constructor(
    private requestService: RequestService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadPendingRequests();
  }

  loadPendingRequests(): void {
    this.requestService.getPendingRequests().subscribe({
      next: (data) => {
        this.pendingRequests = data;
      },
      error: () => {
        Swal.fire('Error', MESSAGES.ERROR.FETCHING_PENDING, 'error');
      },
    });
  }

  viewRequestDetails(request: any): void {
    const dialogRef = this.dialog.open(RequestDetailsDialogComponent, {
      width: '600px',
      data: { request },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'refresh') {
        this.loadPendingRequests();
      }
    });
  }
}
