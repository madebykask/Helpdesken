import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { switchMap, finalize, take } from 'rxjs/operators';

import { AuthenticationService } from '../../../services/authentication';
import { UserSettingsService } from '../../../services/user'
import { throwError, Subject } from 'rxjs';
import { ErrorHandlingService } from '../../../services/errorhandling/error-handling.service';
import { UserData } from 'src/app/models';
import { config } from '@env/environment';

@Component({
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
    private _destroy$ = new Subject();
    version = config.version;
    loginForm: FormGroup;
    isLoading = false;
    submitted = false;
    returnUrl: string;
    error = '';
    errorMessages = {
        username: {
            required: 'Username required'
        },
        password: {
            required: 'Password required'
        }
    };   

    showLoginError = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private userSettingsService: UserSettingsService,
        private errorHandlingService: ErrorHandlingService) {}

    ngOnInit() {        
        this.loginForm = this.formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });

        // reset login status
        this.authenticationService.logout();

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    ngOnDestroy(): void {
        this._destroy$.next();
    }

    // convenience getter for easy access to form fields
    get f() { return this.loginForm.controls; }

    getError(field: string): string {
        var ctrl = this.loginForm.get(field);
        var message = '';
        if (ctrl.errors) {
            for(var err in ctrl.errors) {
                if (ctrl.errors[err]) {
                    message = this.errorMessages[field][err];
                }
            }
        }
        return message;
    }

    onSubmit() {
        
        this.submitted = true;

        // stop here if form is invalid
        if (this.loginForm.invalid) {
            return;
        }
        
        this.showLoginError = false;
        this.isLoading = true;

        this.authenticationService.login(this.f.username.value, this.f.password.value)
            .pipe(
                take(1),                
                switchMap(isSuccess => {                    
                    if (!isSuccess) throwError('Something wrong.');
                    return this.userSettingsService.loadUserSettings().pipe(
                        take(1),
                        switchMap((userData:UserData) =>{
                            return this.userSettingsService.applyUserSettings();
                        })
                    );
                }),                
                finalize(() => this.isLoading = false) 
            ).subscribe(res => {                
                    this.showLoginError = false;
                    this.router.navigateByUrl(this.returnUrl);                                                    
                },
                error => {                    
                    if (error.status && error.status === 400) {                           
                        this.showLoginError = true;
                    } else {
                        this.errorHandlingService.handleError(error);
                    }
                });
    }    
}