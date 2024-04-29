import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptorInterceptor } from './services/auth-interceptor.interceptor';
import { provideOAuthClient } from 'angular-oauth2-oidc';


export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideAnimations(),
    provideHttpClient(withInterceptors([authInterceptorInterceptor])),
    provideOAuthClient(), provideAnimations()]
};
