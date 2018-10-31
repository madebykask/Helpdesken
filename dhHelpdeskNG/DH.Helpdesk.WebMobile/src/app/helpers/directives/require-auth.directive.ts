import { Directive, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthenticationService } from '../../services/authentication';
import { Subscription, Subject } from 'rxjs';
import { LoggerService } from '../../services/logging';
import { takeUntil } from 'rxjs/operators';

@Directive({ selector: '[requireAuth]' })
export class RequireAuthDirective implements OnInit {
  
  private _destroy$ = new Subject();

  constructor(
    private templateRef: TemplateRef<any>,    
    private viewContainer: ViewContainerRef,
    private authenticationService: AuthenticationService,
    private _logger: LoggerService
  ) {    
  }
  
  ngOnInit() {
    this.updateState();
    this.authenticationService.authenticationChanged$.pipe(
          takeUntil(this._destroy$)
    ).subscribe((e:any) => this.updateState());
  }

  ngOnDestroy(): void {
    this._destroy$.next();
  }

  private updateState(){
    let isAuthenticated = this.authenticationService.isAuthenticated();
    //this._logger.log(">>>authentication changed! IsAuthenticated: " +  isAuthenticated);    
    if (isAuthenticated && this.viewContainer.length === 0) {
        this.viewContainer.createEmbeddedView(this.templateRef);
    }
    else if (!isAuthenticated && this.viewContainer.length > 0){
      this.viewContainer.clear();
    }
  }
}