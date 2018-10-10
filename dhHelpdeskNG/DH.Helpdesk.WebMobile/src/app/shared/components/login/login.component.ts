import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AuthenticationService } from '../../../services/authentication';
import { UserSettingsService } from '../../../services/user'
import { throwError } from 'rxjs';

@Component({
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.scss']
})
export class LoginComponent implements OnInit {
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
    }

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private userSettingsService: UserSettingsService) {}

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

        this.isLoading = true;
        this.authenticationService.login(this.f.username.value, this.f.password.value)
            .pipe(first())
            .subscribe(
                isSuccess => {
                    if(!isSuccess) throwError('Something wrong.');//TODO: make better reaction
                    this.userSettingsService.loadUserSettings()
                        .subscribe(x => {
                            this.router.navigateByUrl(this.returnUrl);                            
                        });
                },
                error => {
                    this.error = error;
                    this.isLoading = false;
                });
    }    
}