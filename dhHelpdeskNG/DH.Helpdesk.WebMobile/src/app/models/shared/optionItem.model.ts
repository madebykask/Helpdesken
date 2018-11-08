export class OptionItem {
    constructor(value: any, text: string, group:string = null, html: string = null, disabled: boolean = null) {
        this.value = value;
        this.text = text;
        this.group = group;
        this.html = html;
        this.disabled = disabled;
    }
    public value: any;
    public text: string; 
    public group?: string;
    public html?: string;
    public disabled?: boolean;
}