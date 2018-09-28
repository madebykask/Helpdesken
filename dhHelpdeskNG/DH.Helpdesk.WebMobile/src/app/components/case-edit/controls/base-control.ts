import { FormGroup } from "@angular/forms";
import { Input } from "@angular/core";

export class BaseControl {
    @Input() form: FormGroup;

    constructor() {

    }

    protected getFormControl(name: string) {
        if(this.form == null) return null;

        return this.form.get(name);
    }
}