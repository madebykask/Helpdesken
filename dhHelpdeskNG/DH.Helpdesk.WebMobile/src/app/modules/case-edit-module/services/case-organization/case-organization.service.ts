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
    protected constructor (private _departmentService: DepartmentsService,
        private _regionService: RegionsService,
        private _OUsService: OUsService,
        private _caseTypesService: CaseTypesService,
        private _productAreasService: ProductAreasService,
        private _categoriesService: CategoriesService,
        private _closingReasonsService: ClosingReasonsService,
        private _perfomersService: PerfomersService,
        private _workingGroupsService: WorkingGroupsService,
        private _stateSecondariesService: StateSecondariesService) {
    }

    getRegions() {
        return this._regionService.getRegions();
    }

    getDepartments(regionId?: number) {
        return this._departmentService.getDepartmentsByRegion(regionId);
    }

    getOUs(departmentId?: number) {
        return this._OUsService.getOUsByDepartment(departmentId);
    }

    getCaseTypes() {
        return this._caseTypesService.getCaseTypes();
    }

    getProductAreas(caseTypeId?: number, includeId?: number) {
        return this._productAreasService.getProductAreas(caseTypeId, includeId);
    }

    getCategories() {
        return this._categoriesService.getCategories();
    }

    getClosingReasons() {
        return this._closingReasonsService.getClosingReasons();
    }

    getPerformers(performerUserId?: number, workingGroupId?: number) {
      return this._perfomersService.getPerformers(performerUserId, workingGroupId);
    }

    getWorkingGroups() {
      return this._workingGroupsService.getWorkingGroups();
    }

    getStateSecondaries() {
      return this._stateSecondariesService.getStateSecondaries();
    }
}
