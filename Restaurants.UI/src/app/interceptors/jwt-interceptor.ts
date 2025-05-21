import { inject, Injectable } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    private readonly authService: AuthService = inject(AuthService);

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const isLoggedIn = this.authService.isLoggedIn();

        if(isLoggedIn){
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${localStorage.getItem('accessToken')}`
                }
            });
        }
        return next.handle(request);
    }      
}