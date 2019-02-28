import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MbscListviewOptions, MbscListview } from '@mobiscroll/angular';
import { CaseTemplateService } from 'src/app/modules/case-edit-module/services/case/case-template.service';
import { CaseTemplateCategoryNode, CaseTemplateNode, CaseTemplateModel } from 'src/app/modules/case-edit-module/models/case/case-template.model';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {
  templateNodes = [];

  templatesListSettings: MbscListviewOptions = {
    enhance: false,
    swipe: false
  };

  @ViewChild('templatesListView') templatesList: MbscListview;

  constructor(private router: Router,
              private caseTemplateService: CaseTemplateService) {
  }  

  ngOnInit() {
    this.loadTemplates();
  }

  onLogout() {
    this.router.navigateByUrl('/login');
  }
  
  private loadTemplates() {
    this.caseTemplateService.loadTemplates().pipe(
      take(1)
    ).subscribe((items:CaseTemplateModel[]) => {
        this.templateNodes = this.processTemplates(items);
    });
  } 

  private processTemplates(templates:CaseTemplateModel[]): Array<CaseTemplateNode | CaseTemplateCategoryNode> {
    const templateNodes:CaseTemplateNode[] = [];
    const categoryNodes:CaseTemplateCategoryNode[] = [];
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
