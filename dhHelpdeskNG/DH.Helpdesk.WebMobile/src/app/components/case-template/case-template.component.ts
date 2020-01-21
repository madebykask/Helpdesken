import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { MbscListviewOptions, MbscListview } from '@mobiscroll/angular';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { CaseTemplateModel, CaseTemplateNode, CaseTemplateCategoryNode, CustomerCaseTemplateModel } from 'src/app/models/caseTemplate/case-template.model';
import { BehaviorSubject, Subject, from } from 'rxjs';
import { takeUntil, distinctUntilChanged, filter, switchMap } from 'rxjs/operators';
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
      this.appStore.select<CustomerCaseTemplateModel[]>(AppStoreKeys.Templates).pipe(
        takeUntil(this.destroy$),
        distinctUntilChanged(),
        filter(Boolean), // aka new Boolean(val) to filter null values
      ).subscribe((ct: CustomerCaseTemplateModel[]) => {
        //console.log('>>>got templates from store: ' + items.length);
        this.templateNodes = this.processTemplates(ct);
        this.canCreateCases$.next(this.templateNodes.length > 0);
      });
    }
  }

  openTemplate(template: CaseTemplateNode | CaseTemplateCategoryNode ) {
    if (template.disabled) {
      return;
    }
    this.router.navigate(['/case/template', template.customerId, template.id]);
  }

  trackByFn(index, item) {
    return item.id;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private processTemplates(customerTemplates: CustomerCaseTemplateModel[]): Array<CaseTemplateNode | CaseTemplateCategoryNode> {
    let resultNodes = new Array<CaseTemplateNode | CaseTemplateCategoryNode>();
    if (customerTemplates && customerTemplates.length) {
      customerTemplates.forEach(ct => {
        const templateNodes: CaseTemplateNode[] = [];
        const categoryNodes: CaseTemplateCategoryNode[] = [];
        if (customerTemplates.length > 1) {
          // add customer header
          resultNodes.push(new CaseTemplateNode(0, ct.customerName, ct.customerId, true));
        }
        if (ct.items && ct.items.length) {
          ct.items.forEach(t => {
            const categoryId = t.categoryId || 0;
            const templateNode = new CaseTemplateNode(t.id, t.name, ct.customerId);
            if (categoryId === 0) {
              templateNodes.push(templateNode);
            } else {
              let group = categoryNodes.find(x => x.id === categoryId);
              if (group === null || group === undefined) {
                group = new CaseTemplateCategoryNode(categoryId, t.categoryName || '', ct.customerId);
                categoryNodes.push(group);
              }
              group.items.push(templateNode);
              group.items = this.sortByName(group.items);
            }
          });
        }
        resultNodes = resultNodes.concat(this.sortByName(templateNodes));
        resultNodes = resultNodes.concat(this.sortByName(categoryNodes));
      });
    }
    return resultNodes;
  }

  private sortByName(items: (CaseTemplateNode | CaseTemplateCategoryNode)[]) {
    if (!items) { return; }
    return items.sort((a, b) => a.name.localeCompare(b.name));
  }
}
