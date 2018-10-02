import { DepartmentsService } from "./departments-service";
import { RegionsService } from "./regions-service";
import { Injectable } from "@angular/core";
import { OUsService } from "./ous-service";
import { CaseTypesService } from "./caseTypes-service";
import { ProductAreasService } from "./productAreas-service";
import { CategoriesService } from "./categories-service";

@Injectable({ providedIn: 'root' })
export class CaseOrganizationService {
    protected constructor (private _departmentService: DepartmentsService,
        private _regionService: RegionsService,
        private _OUsService: OUsService,
        private _caseTypesService: CaseTypesService,
        private _productAreasService: ProductAreasService,
        private _categoriesService: CategoriesService) {
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

}