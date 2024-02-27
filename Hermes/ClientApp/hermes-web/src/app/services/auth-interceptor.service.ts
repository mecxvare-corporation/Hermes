import { Injectable, inject } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService {
  private readonly _authService: AuthService = inject(AuthService);

  constructor() { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this._authService.access_token;
debugger
    // This will add headers to all requests, this is not a good idea,
    // we can add maybe if statment to check if URL starts with our api.
    if (token) {
      const headers = req.headers.set('Authorization', `Bearer ${token}`);
      const authReq = req.clone({ headers });
      console.log('token in the request');
      return next.handle(authReq);
    }
    console.log('No token in the request');
    return next.handle(req);
  }
}
