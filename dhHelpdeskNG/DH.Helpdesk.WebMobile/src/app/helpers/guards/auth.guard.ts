import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationStateService, AuthenticationService } from '../../services/authentication';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

    constructor(private _router: Router,
      private _authService: AuthenticationService,
      private _authStateService: AuthenticationStateService) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
      // console.log('>>>>>> AuthGuard canActivate');
        if (this._authStateService.hasToken()) {
            if (this._authStateService.isTokenExpired()) {
                 return this._authService.refreshToken()
                    .pipe(
                        switchMap((r: boolean) => {
                            if (!r) {
                              this._router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
                            };
                            return of(r);
                        }))
            }
            return of(true);
        }

        // not logged in so redirect to login page with the return url
        this._router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
        return of(false);
    }
}
