import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormArray,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { AuthService } from '@services/auth.service';
import { RequestService } from '@services/request.service';
import { MatButtonModule } from '@angular/material/button';
import { Request } from '@models/request.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import Swal from 'sweetalert2';
import { MatInputModule } from '@angular/material/input';
import { UrlValidator } from 'app/validators/url.validator';
import { MESSAGES } from '@constants/messages.constants';

@Component({
  selector: 'app-request-details-dialog',
  templateUrl: './request-details-dialog.component.html',
  styleUrls: ['./request-details-dialog.component.scss'],
  imports: [
    MatDialogModule,
    CommonModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
  ],
})
export class RequestDetailsDialogComponent implements OnInit {
  role: string | null = null;
  requestForm: FormGroup;
  errorMessage: string = '';

  constructor(
    public dialogRef: MatDialogRef<RequestDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { request: Request },
    private requestService: RequestService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.requestForm = this.fb.group({
      supplierId: [''],
      products: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.authService.role$.subscribe((role) => {
      this.role = role;
      if (this.role === 'Colocador') {
        this.requestForm = this.fb.group({
          supplierId: [this.data.request.supplierId, Validators.required],
          products: this.fb.array(
            this.buildProductForms(this.data.request.products)
          ),
        });
      }
    });
  }

  buildProductForms(products: any[]): FormGroup[] {
    return products.map((product) =>
      this.fb.group({
        productId: [product.productId],
        name: [product.name, Validators.required],
        quantity: [product.quantity, [Validators.required, Validators.min(1)]],
        unitPrice: [
          product.unitPrice,
          [Validators.required, Validators.min(0)],
        ],
        productUrl: [product.productUrl, Validators.required],
      })
    );
  }

  get products(): FormArray {
    return this.requestForm?.get('products') as FormArray;
  }

  addProduct(): void {
    this.products.push(
      this.fb.group({
        productId: [null],
        name: ['', Validators.required],
        quantity: [1, [Validators.required, Validators.min(1)]],
        unitPrice: [0, [Validators.required, Validators.min(0)]],
        productUrl: ['', [Validators.required, UrlValidator.validate]],
      })
    );
  }

  removeProduct(index: number): void {
    this.products.removeAt(index);
  }

  saveRequest(): void {
    if (this.requestForm.valid) {
      const updatedRequest = this.requestForm.value;
      this.requestService
        .updateRequest(this.data.request.requestId, updatedRequest)
        .subscribe({
          next: () => {
            this.dialogRef.close('refresh');
          },
          error: () => {
            Swal.fire('Error', MESSAGES.ERROR.UPDATING_REQUEST, 'error');
          },
        });
    }
  }

  approveRequest(): void {
    this.requestService.approveRequest(this.data.request.requestId).subscribe({
      next: () => {
        this.dialogRef.close('refresh');
      },
      error: () => {
        Swal.fire('Error', MESSAGES.ERROR.APPROVING_REQUEST, 'error');
      },
    });
  }

  rejectRequest(): void {
    this.requestService.rejectRequest(this.data.request.requestId).subscribe({
      next: () => {
        this.dialogRef.close('refresh');
      },
      error: () => {
        Swal.fire('Error', MESSAGES.ERROR.REJECTING_REQUEST, 'error');
      },
    });
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
