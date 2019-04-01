import { BehaviorSubject } from "rxjs";
import { OptionItem, MultiLevelOptionItem, CaseOptions } from "src/app/modules/shared-module/models";

export class CaseDataStore {

  public workingGroupsStore$: BehaviorSubject<OptionItem[]>;
  public customerRegistrationSourcesStore$: BehaviorSubject<OptionItem[]>;
  public systemsStore$: BehaviorSubject<OptionItem[]>;
  public urgenciesStore$: BehaviorSubject<OptionItem[]>;
  public impactsStore$: BehaviorSubject<OptionItem[]>;
  public suppliersStore$: BehaviorSubject<OptionItem[]>;
  public countriesStore$: BehaviorSubject<OptionItem[]>;
  public currenciesStore$: BehaviorSubject<OptionItem[]>;
  public responsibleUsersStore$: BehaviorSubject<OptionItem[]>;
  public performersStore$: BehaviorSubject<OptionItem[]>;
  public prioritiesStore$: BehaviorSubject<OptionItem[]>;
  public statusesStore$: BehaviorSubject<OptionItem[]>;
  public stateSecondariesStore$: BehaviorSubject<OptionItem[]>;
  public projectsStore$: BehaviorSubject<OptionItem[]>;
  public problemsStore$: BehaviorSubject<OptionItem[]>;
  public changesStore$: BehaviorSubject<OptionItem[]>;
  public solutionsRatesStore$: BehaviorSubject<OptionItem[]>;
  public causingPartsStore$: BehaviorSubject<OptionItem[]>;
  public regionsStore$: BehaviorSubject<OptionItem[]>;
  public departmentsStore$: BehaviorSubject<OptionItem[]>;
  public oUsStore$: BehaviorSubject<OptionItem[]>;
  public isAboutDepartmentsStore$: BehaviorSubject<OptionItem[]>;
  public isAboutOUsStore$: BehaviorSubject<OptionItem[]>;
  public caseTypesStore$: BehaviorSubject<MultiLevelOptionItem[]>;
  public productAreasStore$: BehaviorSubject<MultiLevelOptionItem[]>;
  public categoriesStore$: BehaviorSubject<MultiLevelOptionItem[]>;
  public closingReasonsStore$: BehaviorSubject<MultiLevelOptionItem[]>;

  constructor(options: CaseOptions) {
    this.workingGroupsStore$ = new BehaviorSubject(options.workingGroups);
    this.customerRegistrationSourcesStore$ = new BehaviorSubject(options.customerRegistrationSources);
    this.systemsStore$ = new BehaviorSubject(options.systems);
    this.urgenciesStore$ = new BehaviorSubject(options.urgencies);
    this.impactsStore$ = new BehaviorSubject(options.impacts);
    this.suppliersStore$ = new BehaviorSubject(options.suppliers);
    this.countriesStore$ = new BehaviorSubject(options.countries);
    this.currenciesStore$ = new BehaviorSubject(options.currencies);
    this.responsibleUsersStore$ = new BehaviorSubject(options.responsibleUsers);
    this.performersStore$ = new BehaviorSubject(options.performers);
    this.prioritiesStore$ = new BehaviorSubject(options.priorities);
    this.statusesStore$ = new BehaviorSubject(options.statuses);
    this.stateSecondariesStore$ = new BehaviorSubject(options.stateSecondaries);
    this.projectsStore$ = new BehaviorSubject(options.projects);
    this.problemsStore$ = new BehaviorSubject(options.problems);
    this.changesStore$ = new BehaviorSubject(options.changes);
    this.solutionsRatesStore$ = new BehaviorSubject(options.solutionsRates);
    this.causingPartsStore$ = new BehaviorSubject(options.causingParts);
    this.regionsStore$ = new BehaviorSubject(options.regions);
    this.departmentsStore$ = new BehaviorSubject(options.departments);
    this.oUsStore$ = new BehaviorSubject(options.oUs);
    this.isAboutDepartmentsStore$ = new BehaviorSubject(options.isAboutDepartments);
    this.isAboutOUsStore$ = new BehaviorSubject(options.isAboutOUs);
    this.caseTypesStore$ = new BehaviorSubject(options.caseTypes);
    this.productAreasStore$ = new BehaviorSubject(options.productAreas);
    this.categoriesStore$ = new BehaviorSubject(options.categories);
    this.closingReasonsStore$ = new BehaviorSubject(options.closingReasons);
  }
}