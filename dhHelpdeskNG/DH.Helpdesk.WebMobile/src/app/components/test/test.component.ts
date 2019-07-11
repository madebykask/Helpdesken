import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MbscSelectOptions, MbscSwitch } from '@mobiscroll/angular';
import { LocalStorageService } from 'src/app/services/local-storage';
import { AuthenticationService } from 'src/app/services/authentication';
import { take, catchError } from 'rxjs/operators';
import { AlertsService } from 'src/app/services/alerts/alerts.service';
import { AlertType } from 'src/app/modules/shared-module/alerts';
import { of, throwError } from 'rxjs';
import { AuthenticationApiService } from 'src/app/services/api';
import { UserSettingsApiService } from 'src/app/services/api/user/user-settings-api.service';
import { TestHttpApiService } from 'src/app/modules/shared-module/services/api/testHttpApiService';
import { HttpHeaders } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {

  @ViewChild('sendEmailsCheck') sendEmailsCheck: MbscSwitch;

  get userData() {
    const currentUser = this.localStorage.getCurrentUser();
    if (currentUser){
      return JSON.stringify(currentUser);
    }
    return 'empty';
  }

  get isSendEmailsChecked() {
    const val = this.sendEmailsCheck.value;
    return val;
  }

  options: MbscSelectOptions = {
    showInput: false,
    showOnTap: true,
    showLabel: false,
    height: 20,
    minWidth: 100,
    theme: 'mobiscroll',
    input: '#notifierInput',
    filter: true,
    display: 'bottom',
    headerText: 'Search users'
  };

  options2: MbscSelectOptions = {
    showInput: true,
    showLabel: false,
    showOnTap: true,
    filter: true,
    display: 'center',
    headerText: 'Search users2'
  };

  options3: MbscSelectOptions = {
    showInput: true,
    showLabel: false,
    input: 'notifierInput2',
    showOnTap: true,
    filter: true,
    display: 'center',
    headerText: 'Search users2'
  };

  items = [{
    value: 1,
    text: 'Option 1'
  }, {
      value: 2,
      text: 'Option 2'
  }, {
      value: 3,
      text: 'Option 4'
  }];

  loginForm: FormGroup;

  constructor(private router: Router,
              private localStorage: LocalStorageService,
              private authenticationService: AuthenticationService,
              private authenticationApiService: AuthenticationApiService,
              private userSettingsApiService: UserSettingsApiService,
              private alertService: AlertsService,
              private fb: FormBuilder,
              private testHttpService: TestHttpApiService) {
  }

  ngOnInit() {
  }

  onLogout() {
    this.router.navigateByUrl('/login');
  }

  refreshToken() {
    this.authenticationService.refreshToken().pipe(
      take(1),
      catchError(err => {
        this.alertService.showMessage(`Failed to refresh a token: ${err.toString()}`, AlertType.Error);
        return of(false);
      })
    ).subscribe((res: Boolean) => {
      if (res === true) {
        this.alertService.showMessage(`Token has been refreshed!`, AlertType.Success);
      } else {
        this.alertService.showMessage(`Token has not been refreshed`, AlertType.Warning);
      }
    });
  }

  testOptionsRequest() {
    return this.testHttpService.testLoginOptions()
    .pipe(
        catchError((error: any) => {
          return throwError(error);
    })).subscribe(res => {
      this.alertService.showMessage(`Success!`, AlertType.Success);
      if (res && res.headers instanceof HttpHeaders) {
        let msg = '';
        const headers = <HttpHeaders>res.headers;
        for (const key of headers.keys()) {
          const headerValue = headers.get(key);
          if (headerValue.indexOf('Allow-Control') !== -1) {
            msg = msg + `${key}: ${headerValue}` + '\r\n';
          }
        }
        if (msg.length) {
          this.alertService.showMessage(msg, AlertType.Success);
        }
      }
    },
    err => {
      this.alertService.showMessage(`Error: ${err.toString()}`, AlertType.Error);
    });
  }

  showNotifierSearch(selectCtrl) {
    selectCtrl.instance.setVal('test', true, false, true);
    selectCtrl.instance.show();
  }

  clearStorage() {
    this.localStorage.clearAll();
    this.authenticationService.logout();
    window.location.reload(true);
  }
}
