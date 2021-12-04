import { HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { tap } from "rxjs";
import { AuthService } from "../services/auth.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private router: Router) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    const authToken = this.authService.getToken();

    const authReq = request.clone({
      headers: request.headers.set('Authorization', 'Bearer ' + authToken)
    });
    return next.handle(authReq).pipe(tap({
        next: v => {
        },
        error: e => { 
            if (e && e.status === 401) {
                this.authService.logOut();
                this.router.navigateByUrl('login');
            }
        }
    }));
  }
}
