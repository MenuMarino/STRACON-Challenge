import { AbstractControl, ValidationErrors } from '@angular/forms';

export class UrlValidator {
  static validate(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value) {
      return null;
    }

    const urlPattern =
      /^(https?:\/\/)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:[0-9]{1,5})?(\/.*)?$/;
    return urlPattern.test(value) ? null : { invalidUrl: true };
  }
}
