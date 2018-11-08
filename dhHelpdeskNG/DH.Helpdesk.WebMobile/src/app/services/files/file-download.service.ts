import { Injectable } from "@angular/core";
import { HttpApiServiceBase } from "../api";
import { HttpClient } from "@angular/common/http";
import { LocalStorageService } from "../local-storage";
import { saveAs } from "file-saver"
import { throwError } from "rxjs";
import { take } from "rxjs/operators";
import { WindowWrapper } from "src/app/helpers";

@Injectable({ providedIn: 'root' })
export class FileDownloadService extends HttpApiServiceBase {
    constructor(httpClient:HttpClient, localStorageService: LocalStorageService, private windowWrapper: WindowWrapper){
        super(httpClient, localStorageService);
    }

    downloadCaseFile(caseId:number, fileId:number,  fileName:string) {
        let window = this.windowWrapper.nativeWindow;        
        let url = this.buildResourseUrl(`/api/case/${caseId}/file/${fileId}`, { inline: true }, true, false);
        this.getFileBody(url, null).pipe(
            take(1),
        ).subscribe(data => {
            //saveAs(data, fileName); // uses file-saver.js
            window.location.href = window.URL.createObjectURL(data);
        });
    }
}
