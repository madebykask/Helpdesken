import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

// This module is created as a hack. Site is deployed under iis and if route is other than / -> 404 error is return. IIS must be configured to return index.html in case of 404, but not able to reconfig prod iis.
@Component({
    selector: 'ec-router',
    templateUrl: './router.component.html'
})
export class ExtendedCaseRouter {
    showFormsList = false;

    constructor(private activatedRoute: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.showFormsList = this.activatedRoute.snapshot.queryParams.hasOwnProperty('showFormsList');
    }

}