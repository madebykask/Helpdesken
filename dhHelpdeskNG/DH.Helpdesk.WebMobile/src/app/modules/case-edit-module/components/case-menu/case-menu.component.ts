import { Component, Input, EventEmitter, Output } from '@angular/core';
import { MbscNavOptions } from 'src/lib/mobiscroll/src/js/navigation.angular';
import { BehaviorSubject, Subject } from 'rxjs';
import { OptionItem } from 'src/app/modules/shared-module/models';
import { map, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'case-menu',
  templateUrl: './case-menu.component.html',
  styleUrls: ['./case-menu.component.scss']
})

export class CaseMenuComponent {
  @Input() dataSource: BehaviorSubject<OptionItem[]>;
  @Output() clickWorkflow = new EventEmitter<number>();
  private destroy$ = new Subject();

  bottomMenu: MbscNavOptions = {
    theme: 'ios',
    menuIcon: 'foundation-foot',
    display: 'bottom',
    type: 'hamburger'
  };
  localDataSource: OptionItem[];

  constructor() {
  }

  ngOnInit(): void {
    this.initDataSource();
    this.initEvents();
  }

  onDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  trackByFn(index, item) {
    return item.value;
  }

  applyWorkflow(id: number) {
    this.clickWorkflow.emit(id);
  }

  private initDataSource() {
    if (!this.dataSource) {
      this.localDataSource = [];
    }
  }

  private initEvents() {
    this.dataSource.pipe(
      map(((options) => {
        options = options || [];
        return options;
      })),
        takeUntil(this.destroy$)
      ).subscribe((options) => {
        this.localDataSource = options;
      });
  }
}
