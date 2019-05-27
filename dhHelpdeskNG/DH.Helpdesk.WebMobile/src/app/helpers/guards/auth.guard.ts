import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationStateService, AuthenticationService } from '../../services/authentication';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(private router: Router,
    private authService: AuthenticationService,
    private authStateService: AuthenticationStateService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.authStateService.hasToken()) {
      if (this.authStateService.isTokenExpired()) {
        return this.authService.refreshToken().pipe(
          switchMap((r: boolean) => {
            if (!r) {
              this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
            }
            return of(r);
          }));
      }
      return of(true);
    }

    // not logged in so redirect to login page with the return url
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return of(false);
  }
}
