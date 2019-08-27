import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MetaDataService } from '../services/data/meta-data.service';
import { FormListItem, FormMetaDataResponse } from '../models/form-data.model';
import { FormGroup, FormControl } from '@angular/forms';
import { ItemModel } from '../models/form.model';
import { WindowWrapper } from '../shared/window-wrapper';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
    selector: 'forms-list',
    templateUrl: './forms-list.component.html',
    providers: [ MetaDataService
    ]
})
export class ExtendedCaseFormsListComponent implements OnInit {

    isLoaded = false;
    metaData: any;
    metaDataJs: any;
    selectedFormId: number;

    modalRef: BsModalRef;
    @ViewChild('code_template', { static: false }) codeTemplate: TemplateRef<any>;

    private items: Array<ItemModel>;
    private form: FormGroup;
    private codeSource = '';

    constructor(private readonly metaDataService: MetaDataService, private readonly windowWrapper: WindowWrapper,
        private readonly modalService: BsModalService) {
        this.form = new FormGroup({
            formsList: new FormControl('formsList')
        });
    }

    ngOnInit(): void {
        this.metaDataService.getFormsList().subscribe((data: Array<FormListItem>) => {
            this.items = data.map((value: FormListItem, index: number) => {
                return new ItemModel(value.Id.toString(), `${value.Name || ''} (${value.Id.toString()})`);
            });
            this.isLoaded = true;
        });

        this.form.controls.formsList.valueChanges.subscribe((formId: number) => {
            this.selectedFormId = formId;
            this.metaDataService.getMetaDataById(formId).subscribe((response: FormMetaDataResponse) => {
                this.metaDataJs = response.MetaData;
                const correctedMetadata = response.MetaData.replace(/[\\]+/ig, '\\\\');// eval removes single backslash(\) symbol, duplicate to avoid it
                this.metaData = eval(`(${correctedMetadata})`);

            });
        });
    };

    getTemplateProperties(): PropertiesItem[] {
        let res = new Array<PropertiesItem>();
        Object.keys(this.metaData).forEach((key: string) => {
            if (key === 'tabs') return;
            let prop = new PropertiesItem();
            prop.name = key;
            prop.value = new Array<string>();
            Object.keys(this.metaData[key]).forEach((key2: string) => {
                //if (this.metaData[key][key2] instanceof Array) {
                //    this.metaData[key][key2].map
                //} else {
                    prop.value.push(key2);
                //}
            });

            res.push(prop);
        });
        return res;
    }

    isTemplateLoaded(): boolean {
        return this.metaData != null;
    }

    openPreview(params: any): void {
        //'/ExtendedCase/?formId=15&caseStatus=20&userRole=2&customerId=43&userGuid=61e94543-f1d2-4916-ad62-5b8d48de65a6'
        let baseUrl = `/?formId=${this.selectedFormId}&autoLoad=1&caseStatus=${params.caseStatus || ''}&userRole=${params.userRole || ''}&customerId=${params.customerId || ''}&userGuid=${params.userGuid || ''}`;
        this.windowWrapper.nativeWindow.open(baseUrl, '_blank');
    }

    viewCode(obj: any) {
        if (obj == null) {
            alert('No data found in object.');
            return;
        }
        if (obj.push != null || typeof obj === 'object') {
            this.openModal(JSON.stringify(obj));
            return;
        }

        this.openModal(obj.toString());
    }

    viewCodeWithGf(obj: any) {
        if (obj == null) {
            alert('No data found in object.');
            return;
        }

        if ((typeof obj === 'string' || obj instanceof String) && this.metaData.globalFunctions != null) {
            let func = this.metaData.globalFunctions[obj.toString()];
            if (func != null) obj = func;
        }

        this.viewCode(obj);
    }

    isGlobalFunction(obj: any) {
        if (obj == null) {
            alert('No data found in object.');
            return false;
        }

        return typeof obj === 'string';
    }


    openModal(content: string) {
        this.codeSource = content;
        this.modalRef = this.modalService.show(this.codeTemplate);
    }

}

class PropertiesItem {
    name: string;
    value: string[];
}