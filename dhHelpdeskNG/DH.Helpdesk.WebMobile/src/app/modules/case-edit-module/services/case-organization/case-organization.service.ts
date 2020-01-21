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

    getRegions(customerId?: number) {
        return this.regionService.getRegions(customerId);
    }

    getDepartments(regionId?: number, customerId?: number) {
        return this.departmentService.getDepartmentsByRegion(regionId, customerId);
    }

    getOUs(departmentId?: number, customerId?: number) {
        return this.oUsService.getOUsByDepartment(departmentId, customerId);
    }

    getCaseTypes(customerId?: number) {
        return this.caseTypesService.getCaseTypes(customerId);
    }

    getProductAreas(caseTypeId?: number, includeId?: number, customerId?: number) {
        return this.productAreasService.getProductAreas(caseTypeId, includeId, customerId);
    }

    getCategories(customerId?: number) {
        return this.categoriesService.getCategories(customerId);
    }

    getClosingReasons(customerId?: number) {
        return this.closingReasonsService.getClosingReasons(customerId);
    }

    getPerformers(performerUserId?: number, workingGroupId?: number, customerId?: number) {
      return this.perfomersService.getPerformers(performerUserId, workingGroupId, customerId);
    }

    getWorkingGroups(customerId?: number) {
      return this.workingGroupsService.getWorkingGroups(customerId);
    }

    getStateSecondaries(customerId?: number) {
      return this.stateSecondariesService.getStateSecondaries(customerId);
    }
}
