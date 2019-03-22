
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MbscListviewOptions, MbscListview } from '@mobiscroll/angular';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { CaseTemplateService } from 'src/app/services/case-organization/case-template.service';
import { CaseTemplateModel, CaseTemplateNode, CaseTemplateCategoryNode } from 'src/app/models/caseTemplate/case-template.model';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';

@Component({
  selector: 'case-template',
  templateUrl: './case-template.component.html',
  styleUrls: ['./case-template.component.scss']
})
export class CaseTemplateComponent implements OnInit {

  @ViewChild('templatesListView') templatesList: MbscListview;

  templatesListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: false
  };

  canCreateCases$ = new BehaviorSubject<boolean>(false);
  templateNodes = [];
  
  constructor(private userSettingsService: UserSettingsApiService,
    private caseTemplateService: CaseTemplateService,
    private router: Router) {
  }  

  ngOnInit() {
   if (this.userSettingsService.getUserData().createCasePermission) {
      this.caseTemplateService.loadTemplates().pipe(
        take(1)
      ).subscribe((items: CaseTemplateModel[]) => {
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
        }
        else {
          let group = categoryNodes.find(x => x.id === categoryId);
          if (group === null || group === undefined) {
            group = new CaseTemplateCategoryNode(categoryId, t.categoryName || '');
            categoryNodes.push(group);
          }
          group.items.push(templateNode)
          group.items = this.sortByName(group.items);
        }
      });
    }
    return [...this.sortByName(templateNodes), ...this.sortByName(categoryNodes)];
  }

  private sortByName(items: (CaseTemplateNode | CaseTemplateCategoryNode)[]) {
    if (items === null || items === undefined) return;
    return items.sort((a, b) => a.name.localeCompare(b.name));
  }

  isCategory(node) {
    return node && node instanceof CaseTemplateCategoryNode;
  }

  openTemplate(templateId:number) {
    this.router.navigate(['/case/template', templateId]);
  }

  trackByFn(index, item) {
    return item.id;
  }
}
