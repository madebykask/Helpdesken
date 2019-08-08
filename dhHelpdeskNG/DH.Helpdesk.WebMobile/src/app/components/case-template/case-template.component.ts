import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { MbscListviewOptions, MbscListview } from '@mobiscroll/angular';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { CaseTemplateModel, CaseTemplateNode, CaseTemplateCategoryNode } from 'src/app/models/caseTemplate/case-template.model';
import { BehaviorSubject, Subject } from 'rxjs';
import { takeUntil, distinctUntilChanged, filter } from 'rxjs/operators';
import { AppStore, AppStoreKeys } from 'src/app/store/app-store';

@Component({
  selector: 'case-template',
  templateUrl: './case-template.component.html',
  styleUrls: ['./case-template.component.scss']
})
export class CaseTemplateComponent implements OnInit, OnDestroy {

  constructor(private userSettingsService: UserSettingsApiService,
    private router: Router,
    private appStore: AppStore) {
  }

  @ViewChild('templatesListView', { static: false }) templatesList: MbscListview;

  templatesListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: false
  };

  canCreateCases$ = new BehaviorSubject<boolean>(false);
  templateNodes = [];

  private destroy$ = new Subject<any>();

  isCategory(node) {
    return node && node instanceof CaseTemplateCategoryNode;
  }

  ngOnInit() {
   if (this.userSettingsService.getUserData().createCasePermission) {
      //load templates from app store - templates are loaded in the footer component first
      this.appStore.select<CaseTemplateModel[]>(AppStoreKeys.Templates).pipe(
        takeUntil(this.destroy$),
        distinctUntilChanged(),
        filter(Boolean) // aka new Boolean(val) to filter null values
      ).subscribe((items: CaseTemplateModel[]) => {
        //console.log('>>>got templates from store: ' + items.length);
        this.templateNodes = this.processTemplates(items);
        this.canCreateCases$.next(this.templateNodes.length > 0);
      });
    }
  }

  private processTemplates(templates: CaseTemplateModel[]): Array<CaseTemplateNode | CaseTemplateCategoryNode> {
    const templateNodes: CaseTemplateNode[] = [];
    const categoryNodes: CaseTemplateCategoryNode[] = [];
    if (templates && templates.length) {
      templates.forEach(t => {
        const categoryId = t.categoryId || 0;
        const templateNode = new CaseTemplateNode(t.id, t.name);
        if (categoryId === 0) {
          templateNodes.push(templateNode);
        } else {
          let group = categoryNodes.find(x => x.id === categoryId);
          if (group === null || group === undefined) {
            group = new CaseTemplateCategoryNode(categoryId, t.categoryName || '');
            categoryNodes.push(group);
          }
          group.items.push(templateNode);
          group.items = this.sortByName(group.items);
        }
      });
    }
    return [...this.sortByName(templateNodes), ...this.sortByName(categoryNodes)];
  }

  private sortByName(items: (CaseTemplateNode | CaseTemplateCategoryNode)[]) {
    if (items === null || items === undefined) { return; }
    return items.sort((a, b) => a.name.localeCompare(b.name));
  }

  openTemplate(templateId: number) {
    this.router.navigate(['/case/template', templateId]);
  }

  trackByFn(index, item) {
    return item.id;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
