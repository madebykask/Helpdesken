import { Component, Input } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { ValidatorError } from '../../shared/validation-types';

@Component({
    selector: 'validation-error',
    templateUrl: './validation-errors.component.html'
})

export class ValidationErrorComponent {
    @Input() control: AbstractControl;

    getErrors(): Array<string> {
        const errorsArr = Object.keys(this.control.errors)
            .filter((errName: string) => {
                return (this.control.errors[errName] as ValidatorError).message;
            })
            .map((errName: string) => {
                return (this.control.errors[errName] as ValidatorError).message;
        });

        return errorsArr;
    };
}