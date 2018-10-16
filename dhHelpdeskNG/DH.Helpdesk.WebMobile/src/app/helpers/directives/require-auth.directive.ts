import {Directive, OnInit, TemplateRef, ViewContainerRef} from '@angular/core';
import { AuthenticationService } from '../../services/authentication';
import { Subscription } from 'rxjs';
import { LoggerService } from '../../services/logging';

@Directive({ selector: '[requireAuth]' })
export class RequireAuthDirective implements OnInit {
  
  private authEventSubscription : Subscription;

  constructor(
    private templateRef: TemplateRef<any>,    
    private viewContainer: ViewContainerRef,
    private authenticationService: AuthenticationService,
    private _logger: LoggerService
  ) {    
  }
  
  ngOnInit() {           
    this.authEventSubscription = this.authenticationService.authenticationChanged$.subscribe((e:any) => this.updateState());    
    this.updateState();
  }

  ngOnDestroy(){
    if (this.authEventSubscription)
      this.authEventSubscription.unsubscribe();
  }

  private updateState(){
    let isAuthenticated = this.authenticationService.isAuthenticated();
    this._logger.log(">>>authentication changed! IsAuthenticated: " +  isAuthenticated);
    
    if (isAuthenticated && this.viewContainer.length === 0) {
        this.viewContainer.createEmbeddedView(this.templateRef);
    }
    else if (!isAuthenticated && this.viewContainer.length > 0){
      this.viewContainer.clear();
    }
  }
}