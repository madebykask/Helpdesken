import { Directive, HostListener, forwardRef} from '@angular/core';
import { DefaultValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import * as cm from '../utils/common-methods';

const TRIM_VALUE_ACCESSOR: any = {
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => TrimValueAccessor),
    multi: true
};

/**
 * The trim accessor for writing trimmed value and listening to changes that is
 * used by the {@link NgModel}, {@link FormControlDirective}, and
 * {@link FormControlName} directives.
 */
@Directive({
    selector:'input:not([type=checkbox]):not([type=radio]):not([type=password])[formControlName],input:not([type=checkbox]):not([type=radio]):not([type=password])[formControl],input:not([type=checkbox]):not([type=radio]):not([type=password])[ngModel],textarea[formControlName],textarea[formControl],textarea[ngModel],[ngDefaultControl]',
    providers: [TRIM_VALUE_ACCESSOR]
})
export class TrimValueAccessor extends DefaultValueAccessor {

    @HostListener('input', ['$event.target.value'])
    handleInputChange(val: string) {
        if (val) {
            val = this.trimValue(val);
        }
        
        this.onChange(val);
    }

    //remove spaces on leave
    @HostListener('blur', ['$event.target.value'])
    handleLeave(value:string) {
        let newVal = this.trimValue(value);
        if (newVal !== value) {
            this.writeValue(newVal);
        }
        this.onTouched();
    }

    //writes value on UI
    writeValue(value: any): void {
        if (typeof value === 'string') {
            value = this.trimValue(value);
        }

        super.writeValue(value);
    }

    private trimValue(val: string):string {
        if (val && val.length) {
            val = cm.removeMultipleSpaces(val, true);
        }
        return val;
    }
}