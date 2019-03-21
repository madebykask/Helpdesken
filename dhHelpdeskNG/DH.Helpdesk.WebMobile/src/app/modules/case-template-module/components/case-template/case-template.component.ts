import { MbscListviewOptions, MbscListview } from '@mobiscroll/angular';
import { Component, OnInit, ViewChild } from '@angular/core';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { Observable, BehaviorSubject } from 'rxjs';
import { CaseTemplateService } from 'src/app/modules/case-edit-module/services/case/case-template.service';
import { CaseTemplateModel, CaseTemplateNode, CaseTemplateCategoryNode } from 'src/app/modules/case-edit-module/models/case/case-template.model';
import { take, finalize, map } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-case-template',
  templateUrl: './case-template.component.html',
  styleUrls: ['./case-template.component.scss']
})
export class CaseTemplateComponent implements OnInit {

  templateNodes = [];
  
  constructor(private userSettingsService: UserSettingsApiService,
    private caseTemplateService: CaseTemplateService,
    private router: Router) {
  }
 @ViewChild('templatesListView') templatesList: MbscListview;

  templatesListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: false
  };


  canCreateCases$ = new BehaviorSubject<boolean>(false);

  ngOnInit() {
   if (this.userSettingsService.getUserData().createCasePermission) {
      this.loadTemplates().subscribe((templateNodes) => {
        this.templateNodes = templateNodes;
        this.canCreateCases$.next(this.templateNodes.length > 0);
      });
    }
 }
  
  private loadTemplates() {
    return this.caseTemplateService.loadTemplates().pipe(
      take(1),
      map((items: CaseTemplateModel[]) => {
        return this.processTemplates(items);
    }));
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

}
