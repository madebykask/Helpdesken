import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from '../../services/authentication'
import { takeUntil, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private authenticationService: AuthenticationService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this.authenticationService.hasToken()) {
            if (this.authenticationService.isTokenExpired()) { 
                 return this.authenticationService.refreshToken()
                    .pipe(
                        switchMap((r: boolean) => {                            
                            if(!r) this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});                                                
                            return of(r);
                        }))
            }
            return of(true);
        }

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
        return of(false);
    }
}