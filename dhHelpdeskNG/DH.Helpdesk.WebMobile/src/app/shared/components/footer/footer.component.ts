import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MbscPopup, MbscPopupOptions, MbscSelect, MbscSelectOptions, MbscNavOptions, MbscListviewOptions, MbscListview } from '@mobiscroll/angular';
import { take, finalize, map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { LanguagesApiService } from 'src/app/services/api/language/languages-api.service';
import { CasesSearchType } from 'src/app/modules/shared-module/constants';
import { UserSettingsApiService } from "src/app/services/api/user/user-settings-api.service";
import { CaseTemplateModel, CaseTemplateNode, CaseTemplateCategoryNode } from 'src/app/modules/case-edit-module/models/case/case-template.model';
import { CaseTemplateService } from 'src/app/modules/case-edit-module/services/case/case-template.service';
import { Observable, BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit  {
  searchType = CasesSearchType;
  languageId: number = 0;
  isLoadingLanguage: boolean = true;
  isVisible = true;
  templateNodes = [];
  canCreateCases$ = new BehaviorSubject<boolean>(false);
  
  @ViewChild('newCasePopup') newCasePopup: MbscPopup;
  @ViewChild('caseSearchPopup') caseSearchPopup: MbscPopup;
  @ViewChild('languages') languagesCtrl: MbscSelect;
  @ViewChild('templatesListView') templatesList: MbscListview;  
  
  languagesSettings: MbscSelectOptions = {
    cssClass: 'languages-list',
    showOnTap: false,
    display: 'bottom',
    data: [],
    onSet: (event, inst) => this.onLanguageChange(event, inst)
  };

  bottomMenuSettings: MbscNavOptions = {
    type: 'bottom',
    moreText: null,
    moreIcon: 'fa-ellipsis-h',
    menuIcon: null,
    menuText: null,
  };

  newCasePopupSettings: MbscPopupOptions = {
    buttons: [],
    closeOnOverlayTap: true,
    display: 'bottom',
    cssClass: 'mbsc-no-padding',
    onClose: (event, inst) => {
      //make templates menu go to the root level
      const el = this.templatesList.elem.nativeElement;
      if (el && el.children.lengh)
        this.templatesList.instance.navigate(el.children[0]);
    }
  }
  
  popupMenuSettings: MbscPopupOptions = {
    buttons: [],
    closeOnOverlayTap: true,
    display: 'bottom',
    cssClass: 'mbsc-no-padding',
  };
 
  menuListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: false
  };

  templatesListSettings: MbscListviewOptions = {
    enhance: true,
    swipe: false,
    onNavStart: (ev, inst) => this.newCasePopup.instance.position(),
    onNavEnd: (ev, inst) => this.newCasePopup.instance.position(),
    onItemTap: (ev, inst) => this.newCasePopup.instance.position()
  };

  constructor(private router: Router,
              private userSettingsService : UserSettingsApiService,
              private languagesService: LanguagesApiService,
              private caseTemplateService: CaseTemplateService,
              private ngxTranslateService: TranslateService) {
  }

  ngOnInit() {
    this.loadLanguages();
    if (this.userSettingsService.getUserData().createCasePermission) {
      this.loadTemplates().subscribe((templateNodes) => {
        this.templateNodes = templateNodes;
        this.canCreateCases$.next(this.templateNodes.length > 0);
      })
    };

    // apply translations
    this.languagesCtrl.setText = this.ngxTranslateService.instant("Välj");
    this.languagesCtrl.cancelText  = this.ngxTranslateService.instant("Avbryt");
  }

  openLanguages() {
    if (!this.isLoadingLanguage) {
        this.languagesCtrl.instance.show();
    }
  }

  isCategory(node) {
    return node && node instanceof CaseTemplateCategoryNode;
  }

  openTemplate(templateId:number) {
    this.hidePopups();
    this.router.navigate(['/case/template', templateId]);
  }

  setLanguage(languageId: number) {
    if (languageId) {
      this.userSettingsService.setCurrentLanguage(languageId);

      // reload will reopen the app
      window.location.reload(true);
    }
  }

  logout() {
    this.goTo('/login');
  }

  goToCases(searchType: CasesSearchType) {
    this.hidePopups();
    this.router.navigate(['/casesoverview', CasesSearchType[searchType]]);
  }

  goToOverview() {
    this.caseSearchPopup.instance.show();
  }

  goTo(url: string = null) {
    this.hidePopups();
    this.router.navigateByUrl(url);
  }

  createCase() {
    if (this.canCreateCases$.value) {
      this.newCasePopup.instance.show()
    }
  }

  private onLanguageChange(event, inst) {
    let val = inst.getVal();
    this.setLanguage(val ? +val : null);
  }

  private loadLanguages() {
    this.languageId = this.userSettingsService.getCurrentLanguage() || 0;
    if (this.languageId === 0)
       return;
    
    this.isLoadingLanguage = true;
    this.languagesService.getLanguages().pipe(
        take(1),
        finalize(() => this.isLoadingLanguage = false)
    ).subscribe((data) => {
        this.languagesCtrl.refreshData(data);
    });
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

  private hidePopups() {
    this.caseSearchPopup.instance.hide();
    this.newCasePopup.instance.hide();
  }

}