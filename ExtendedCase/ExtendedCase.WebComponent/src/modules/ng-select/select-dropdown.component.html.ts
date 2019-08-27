export const TEMPLATE = `<div
    [ngStyle]="{'top.px': top, 'left.px': left, 'width.px': width}">

    <div class="filter"
        *ngIf="!multiple && filterEnabled">
        <input
            #filterInput
            autocomplete="off"
            [placeholder]="placeholder"
            (click)="onSingleFilterClick()"
            (input)="onSingleFilterInput($event)"
            (keydown)="onSingleFilterKeydown($event)"
            (focus)="onSingleFilterFocus()">
    </div>

    <div class="options"
        (click)="onOptionsListClick()"
        #optionsList>
        <ul
            (wheel)="onOptionsWheel($event)">
            <li *ngFor="let option of optionList.filtered; trackBy: trackById"
                [ngClass]="{'highlighted': option.highlighted, 'selected': option.selected, 'disabled': option.disabled}"               
                (click)="onOptionClick(option)"
                (mouseover)="onOptionMouseover(option)">
                <span >{{option.label}}</span>
            </li>
            <li
                *ngIf="!optionList.hasShown"
                class="message">
                {{notFoundMsg}}
            </li>
        </ul>
    </div>
</div>`;