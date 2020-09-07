import { Component, Input } from '@angular/core';

@Component({
    selector: 'progress-cmp',
    templateUrl: './progress.component.html'
})
export class ProgressComponent {
    @Input() progressText: string;
    @Input() isVisible: boolean;

    show(status: string = null) {
        this.progressText = status || '';
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }
}


