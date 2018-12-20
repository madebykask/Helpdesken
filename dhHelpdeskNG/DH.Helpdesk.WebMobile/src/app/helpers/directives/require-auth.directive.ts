import { Directive, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CommunicationService, Channels } from 'src/app/services/communication';
import { AuthenticationStateService } from 'src/app/services/authentication';

@Directive({ selector: '[requireAuth]' })
export class RequireAuthDirective implements OnInit {
  
  private destroy$ = new Subject();

  constructor(
    private templateRef: TemplateRef<any>,    
    private viewContainer: ViewContainerRef,
    private _commService: CommunicationService,
    private _authService: AuthenticationStateService
  ) {    
  }
  
  ngOnInit() {
    this.updateState();
    this._commService.listen(Channels.AuthenticationChange).pipe(
          takeUntil(this.destroy$)
    ).subscribe((e:any) => this.updateState());
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private updateState(){
    let isAuthenticated = this._authService.isAuthenticated();
    if (isAuthenticated && this.viewContainer.length === 0) {
        this.viewContainer.createEmbeddedView(this.templateRef);
    }
    else if (!isAuthenticated && this.viewContainer.length > 0){
      this.viewContainer.clear();
    }
  }
}