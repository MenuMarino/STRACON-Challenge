import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { RequestService } from '@services/request.service';
import { SupplierService } from '@services/supplier.service';
import { Request, Supplier } from '@models/request.model';
import { RequestDetailsDialogComponent } from '@components/request-details-dialog/request-details-dialog.component';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import Swal from 'sweetalert2';
import { UrlValidator } from 'app/validators/url.validator';
import { MESSAGES } from '@constants/messages.constants';

@Component({
  selector: 'app-colocador-landing',
  standalone: true,
  templateUrl: './colocador-landing.component.html',
  styleUrls: ['./colocador-landing.component.scss'],
  imports: [
    CommonModule,
    MatTableModule,
    MatDialogModule,
    MatFormFieldModule,
    MatOptionModule,
    MatSelectModule,
    FormsModule,
    MatInputModule,
    RouterModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    MatButtonModule,
  ],
})
export class ColocadorLandingComponent implements OnInit {
  suppliers: Supplier[] = [];
  requestForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private supplierService: SupplierService,
    private requestService: RequestService
  ) {
    this.requestForm = this.fb.group({
      supplierId: [null, Validators.required],
      products: this.fb.array(
        [this.createProductFormGroup()],
        [this.minLengthValidator(1)]
      ),
    });
  }

  ngOnInit(): void {
    this.loadSuppliers();
  }

  private minLengthValidator(min: number) {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const formArray = control as FormArray;
      return formArray.length >= min
        ? null
        : {
            minLength: { requiredLength: min, actualLength: formArray.length },
          };
    };
  }

  loadSuppliers(): void {
    this.supplierService.getSuppliers().subscribe({
      next: (data) => {
        this.suppliers = data;
      },
      error: () => {
        Swal.fire('Error', MESSAGES.ERROR.FETCHING_SUPPLIERS, 'error');
      },
    });
  }

  createSupplier(supplierForm: any): void {
    if (supplierForm.valid) {
      this.supplierService.createSupplier(supplierForm.value).subscribe({
        next: () => {
          Swal.fire('Exito', MESSAGES.SUCCESS.CREATING_SUPPLIER, 'success');
          supplierForm.resetForm();
          this.loadSuppliers();
        },
        error: () => {
          Swal.fire('Error', MESSAGES.ERROR.CREATING_SUPPLIER, 'error');
        },
      });
    }
  }

  createPurchaseRequest(): void {
    if (this.requestForm.valid) {
      this.requestService.createRequest(this.requestForm.value).subscribe({
        next: () => {
          Swal.fire('Exito', MESSAGES.SUCCESS.CREATING_REQUEST, 'success');
          this.requestForm.reset();
          this.requestForm.setControl(
            'products',
            this.fb.array([this.createProductFormGroup()])
          );
        },
        error: ({ error }) => {
          console.log({ error });
          Swal.fire('Error', error.message, 'error');
        },
      });
    }
  }

  addProduct(): void {
    this.products.push(this.createProductFormGroup());
  }

  removeProduct(index: number): void {
    this.products.removeAt(index);
  }

  get products(): FormArray {
    return this.requestForm.get('products') as FormArray;
  }

  private createProductFormGroup(): FormGroup {
    return this.fb.group({
      name: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(0.01)]],
      productUrl: ['', [Validators.required, UrlValidator.validate]],
    });
  }
}
