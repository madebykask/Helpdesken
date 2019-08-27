import { ElementRef, Input, Directive, OnInit, Inject } from '@angular/core';
import { NgControl } from '@angular/forms';
import { AppConfig } from '../../shared/app-config/app-config';
import * as cm from '../../utils/common-methods';
import { IAppConfig } from 'src/app/shared/app-config/app-config.interface';

@Directive({
    selector: '[formControlName][mask]',
    host: {
        '(input)': 'onInputChange($event)',
        '(keydown.backspace)': 'onDelete($event, false)',
        '(keydown.delete)': 'onDelete($event, true)'
    }
})
export class Mask implements OnInit {
    @Input('mask') textMaskConfig: {
        type: MaskType,
        regex: any,
        custom: CustomMask;
    } = {
        type: null,
        regex: null,
        custom: null
    };
    separator: string;
    inputElement: HTMLInputElement;
    private useComponentValue = false;
    private value = '';

    constructor(public model: NgControl, inputElement: ElementRef,
        @Inject(AppConfig) config: IAppConfig) {
        this.inputElement = inputElement.nativeElement;
        this.separator = config.decimalSeparator;
    }

    ngOnInit(): void {

    }

    onDelete(isDelete: boolean) {
        this.useComponentValue = true;
        this.value = this.inputElement.value;
        let caretPosition = this.inputElement.selectionStart;

        if (caretPosition > 0) {
            let removePosition = isDelete ? caretPosition : caretPosition - 1;
            if (this.textMaskConfig.type === MaskType.Decimal ||
                this.textMaskConfig.type === MaskType.Percentage) {
                if (this.value[removePosition] === this.separator) { // dont remove separator from value
                    removePosition = isDelete ? removePosition + 1 : removePosition - 1;
                }
            }

            this.value = cm.removeAt(this.value, removePosition, 1);


        } else {
            this.useComponentValue = false;
            //if (!isDelete) this.isPreviousDelete = false;//if backspace - dont mark as useComponentValue
            //this.value = '';
        }

        // if backspace OR delete on first symbol and all values are 0 - delete all
        if (this.textMaskConfig.type === MaskType.Decimal ||
            this.textMaskConfig.type === MaskType.Percentage) {
            if ((caretPosition === 1 && !isDelete) ||
                (caretPosition === 0 && isDelete)) {
                const match = this.value.replace(/[\D]+/g, '').match(/^[0]+$/g);
                if (match) {
                    this.value = '';
                    this.useComponentValue = true;
                }
            }
        }
        //console.log(`Mask backspaceValue <- ${this.value}`);
    }

    onInputChange() {
        let initialValue = this.useComponentValue ? this.value.trim() : this.inputElement.value.trim();
        let caretPosition = this.inputElement.selectionStart;
        this.useComponentValue = false;
        //console.log(`Mask initial ${initialValue}`);

        let mask: CustomMask;

        let regexMaskFactory = (regex: any): CustomMask => {
            let c: CustomMask = (config: CustomMaskConfig) => {
                let replace = (s: string) => s.replace(new RegExp(regex, 'g'), '');
                let textBeforeCaret = initialValue.slice(0, config.currentCaretPosition);
                let maskedValue = replace(config.initialValue);
                let newCaretPosition = replace(textBeforeCaret).length;

                let result: CustomMaskResult = {
                    maskedValue: maskedValue,
                    newCaretPosition: newCaretPosition
                };

                return result;
            };

            return c;
        };
        let result: CustomMaskResult;

        if (initialValue.length === 0) {
            result = {
                maskedValue: initialValue,
                newCaretPosition: caretPosition
            };
        } else {
            let count = 0;
            switch (this.textMaskConfig.type) {
                case MaskType.LettersOnly:
                    mask = regexMaskFactory('[^a-zA-Z\\s]');
                    break;
                case MaskType.NumbersOnly:
                    initialValue = initialValue.replace(new RegExp(`[${this.separator}]`, 'g'),
                        match => (++count === 1) ? match : '');
                    mask = regexMaskFactory(`[^0-9]`);
                    break;
                case MaskType.Alphanumeric:
                    mask = regexMaskFactory(`[^a-zA-Z0-9${this.separator}\\-]`);
                    break;
                case MaskType.Regex:
                    if (!this.textMaskConfig.regex) {
                        mask = null;
                        break;
                    }
                    mask = regexMaskFactory(this.textMaskConfig.regex);
                    break;
                case MaskType.Custom:
                    if (!this.textMaskConfig.custom) {
                        mask = null;
                        break;
                    }
                    mask = this.textMaskConfig.custom;
                    break;
                case MaskType.Decimal:
                    {
                        initialValue = initialValue.replace(new RegExp(`[^\\d${this.separator}]+`, 'g'), '');
                        initialValue = initialValue.replace(new RegExp(`[${this.separator}]`, 'g'),
                            match => (++count === 1) ? match : '');
                        const regex = `^(\\d*)([${this.separator}]?)(\\d{0,2})?\\d*$`;
                        let matches = new RegExp(regex, 'g').exec(initialValue);
                        let maskedValue = initialValue;
                        if (matches) {
                            const fractionalPart = ((matches[3] || '')[0] || '0') + ((matches[3] || '')[1] || '0');
                            maskedValue = (matches[1] || '0') + this.separator + fractionalPart;
                        }
                        result = {
                            maskedValue: maskedValue,
                            newCaretPosition: caretPosition
                        };
                        break;
                    }
                case MaskType.Percentage: {
                    initialValue = initialValue.replace(new RegExp(`[^\\d${this.separator}]+`, 'g'), '');
                    initialValue = initialValue.replace(new RegExp(`[${this.separator}]`, 'g'),
                        match => (++count === 1) ? match : '');
                    const regex = `^(\\d{0,3})\\d*([${this.separator}]?)(\\d{0,2})?\\d*$`;
                    let matches = new RegExp(regex, 'g').exec(initialValue);
                    let maskedValue = initialValue;
                    if (matches) {
                        const fractionalPart = ((matches[3] || '')[0] || '0') + ((matches[3] || '')[1] || '0');
                        maskedValue = (matches[1] || '0') + this.separator + fractionalPart;
                    }
                    result = {
                        maskedValue: maskedValue,
                        newCaretPosition: caretPosition
                    };
                    break;
                }
                default:
                    mask = null;
            }
        }
        if (mask) {
            result = mask({ initialValue: initialValue, currentCaretPosition: caretPosition });
        }
        if (!result) return;

        //// set the new value
        if (this.inputElement.value !== result.maskedValue) {
            //console.log(`Mask masked ${result.maskedValue}`);
            this.model.control.setValue(result.maskedValue);
        }

        //input type number does not support setSelectionRange
        if (this.isInt(result.newCaretPosition) && this.inputElement.type !== 'number') this.inputElement.setSelectionRange(result.newCaretPosition, result.newCaretPosition);
    }

    isInt(value: any) {
        return !isNaN(value) && parseInt(Number(value) as any) == value && !isNaN(parseInt(value, 10));
    }
}

export type CustomMaskConfig = { initialValue: string; currentCaretPosition: number };
export type CustomMaskResult = { maskedValue: string; newCaretPosition?: number };
export type CustomMask = (config: CustomMaskConfig) => CustomMaskResult;

export enum MaskType {
    LettersOnly,
    NumbersOnly,
    Decimal,
    Alphanumeric,
    Percentage,
    Regex,
    Custom
}
