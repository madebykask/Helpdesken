import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { switchMap, finalize, take, map, catchError } from 'rxjs/operators';
import { AuthenticationService } from '../../../services/authentication';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { throwError, Subject } from 'rxjs';
import { ErrorHandlingService } from '../../../services/logging/error-handling.service';
import { config } from '@env/environment';
import { CommunicationService, Channels } from 'src/app/services/communication';


@Component({
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
    private destroy$ = new Subject();
    version = config.version;
    hasMicrosoftLogin = config.microsoftShowLogin;
    loginForm: FormGroup;
    isLoading = false;
    submitted = false;
    returnUrl: string;
    error = '';
    pageSettings = {};
    loginDisplay = false;

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
        private communicationService: CommunicationService,
        private authenticationService: AuthenticationService,
        private userSettingsService: UserSettingsApiService,
        private errorHandlingService: ErrorHandlingService,
        ) {}

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

    microsoftLogin() {
      this.showLoginError = false;
      this.isLoading = true;
      
      this.authenticationService.microsoftLogin().pipe(
        take(1),
        switchMap(currentUser => {
          this.communicationService.publish(Channels.UserLoggedIn, currentUser);
          return this.userSettingsService.applyUserSettings();
        }),
        finalize(() => this.isLoading = false)
      ).subscribe(res => {
            this.showLoginError = false;
            this.router.navigateByUrl(this.returnUrl);
        },
        error => {
          if ((error.name && error.name === "BrowserAuthError") || (error.status && error.status === 400)) {  
            this.showLoginError = true;
          } else {
              this.errorHandlingService.handleError(error);
          }
        });
    }
  
    // logout() {
    //   this.msalService.logout();
    // }
    // setLoginDisplay() {
    //   // this.loginDisplay = this.authServiceMsal.instance.getAllAccounts().length > 0;
    // }

    ngOnDestroy(): void {
        this.destroy$.next();
    }

    // convenience getter for easy access to form fields
    get f() { return this.loginForm.controls; }

    getError(field: string): string {
        const ctrl = this.loginForm.get(field);
        let message = '';
        if (ctrl.errors) {
            for (const err in ctrl.errors) {
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

        this.authenticationService.login(this.f.username.value, this.f.password.value).pipe(
          take(1),
          switchMap(currentUser => {
            this.communicationService.publish(Channels.UserLoggedIn, currentUser);
            return this.userSettingsService.applyUserSettings();
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
