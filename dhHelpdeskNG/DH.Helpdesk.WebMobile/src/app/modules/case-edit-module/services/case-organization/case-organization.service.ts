import { DepartmentsService } from '../../../../services/case-organization/departments-service';
import { RegionsService } from '../../../../services/case-organization/regions-service';
import { Injectable } from '@angular/core';
import { OUsService } from '../../../../services/case-organization/ous-service';
import { CaseTypesService } from '../../../../services/case-organization/caseTypes-service';
import { ProductAreasService } from '../../../../services/case-organization/productAreas-service';
import { CategoriesService } from '../../../../services/case-organization/categories-service';
import { ClosingReasonsService } from '../../../../services/case-organization/closingReasons-service';
import { PerfomersService } from '../../../../services/case-organization/perfomers-service';
import { WorkingGroupsService } from '../../../../services/case-organization/workingGroups-service';
import { StateSecondariesService } from '../../../../services/case-organization/stateSecondaries-service';

@Injectable({ providedIn: 'root' })
export class CaseOrganizationService {
    protected constructor (private departmentService: DepartmentsService,
        private regionService: RegionsService,
        private oUsService: OUsService,
        private caseTypesService: CaseTypesService,
        private productAreasService: ProductAreasService,
        private categoriesService: CategoriesService,
        private closingReasonsService: ClosingReasonsService,
        private perfomersService: PerfomersService,
        private workingGroupsService: WorkingGroupsService,
        private stateSecondariesService: StateSecondariesService) {
    }

    getRegions() {
        return this.regionService.getRegions();
    }

    getDepartments(regionId?: number) {
        return this.departmentService.getDepartmentsByRegion(regionId);
    }

    getOUs(departmentId?: number) {
        return this.oUsService.getOUsByDepartment(departmentId);
    }

    getCaseTypes() {
        return this.caseTypesService.getCaseTypes();
    }

    getProductAreas(caseTypeId?: number, includeId?: number) {
        return this.productAreasService.getProductAreas(caseTypeId, includeId);
    }

    getCategories() {
        return this.categoriesService.getCategories();
    }

    getClosingReasons() {
        return this.closingReasonsService.getClosingReasons();
    }

    getPerformers(performerUserId?: number, workingGroupId?: number) {
      return this.perfomersService.getPerformers(performerUserId, workingGroupId);
    }

    getWorkingGroups() {
      return this.workingGroupsService.getWorkingGroups();
    }

    getStateSecondaries() {
      return this.stateSecondariesService.getStateSecondaries();
    }
}
