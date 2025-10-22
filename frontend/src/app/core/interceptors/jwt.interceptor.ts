import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { TokenService } from '../services/token.service';

@Injectable({
  providedIn: 'root'
})

export class JwtInterceptor implements HttpInterceptor {
  private isRefreshing = false;

  constructor(private tokenService: TokenService, private authService: AuthService, private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.tokenService.getAccessToken();

    let request = req;
    if (token) {
      request = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    }

    return next.handle(request).pipe(
      catchError(err => {

        if (err instanceof HttpErrorResponse && err.status === 401 && !this.isRefreshing) {
          this.isRefreshing = true;

          return this.tokenService.refreshToken().pipe(
            switchMap(res => {
              this.isRefreshing = false;

              if (res?.accessToken) {
                this.tokenService.setAccessToken(res.accessToken);
              }

              const newReq = req.clone({
                setHeaders: { Authorization: `Bearer ${res.accessToken}` }
              });

              return next.handle(newReq);
            }),
            catchError(() => {
              this.isRefreshing = false;
              this.authService.logout(true);
              this.router.navigate(['/login']);
              return throwError(() => err);
            })
          );
        }

        return throwError(() => err);
      })
    );
  }
}
