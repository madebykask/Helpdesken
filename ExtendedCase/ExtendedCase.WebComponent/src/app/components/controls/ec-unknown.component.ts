import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { ComponentCommService } from '../../services/component-comm.service';
import { BaseControlTemplateModel } from '../../models/template.model';

@Component({
    selector: 'ec-unknown',
    templateUrl: './ec-unknown.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})

export class ExtendedUnknowControlComponent {
    @Input() fieldTemplate: BaseControlTemplateModel;
}
