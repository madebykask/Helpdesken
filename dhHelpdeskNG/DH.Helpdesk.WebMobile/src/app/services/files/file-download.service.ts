import { Injectable } from "@angular/core";
import { HttpApiServiceBase } from "../api";
import { HttpClient } from "@angular/common/http";
import { LocalStorageService } from "../local-storage";
import { saveAs } from "file-saver"

@Injectable({ providedIn: 'root' })
export class FileDownloadService extends HttpApiServiceBase {
    constructor(httpClient:HttpClient, localStorageService: LocalStorageService){
        super(httpClient, localStorageService);
    }

    downloadCaseFile(caseId:number, fileId:number,  fileName:string) {
        let url = this.buildResourseUrl(`/api/case/${caseId}/file/${fileId}`, null, true, false);
        this.getFileBody(url, null).subscribe(
            data => {
                // uses file-saver.js
                saveAs(data, fileName);
            },
            err => {
                //todo: add error handler
                alert("Unknown error while downloading the file.");
                console.error(err);
            });
    }

    //alternative approach  
    saveFile(blob){
        var url = window.URL.createObjectURL(blob);
        var pwa = window.open(url);
        if (!pwa || pwa.closed || typeof pwa.closed == 'undefined') {
            alert( 'Please disable your Pop-up blocker and try again.');
        }
    }  
}
