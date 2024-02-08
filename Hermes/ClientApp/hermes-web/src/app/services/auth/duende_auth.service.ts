// import { Injectable, inject } from '@angular/core';
// import { OAuthService } from 'angular-oauth2-oidc';
// import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
// import { Observable, from, catchError } from 'rxjs';

// @Injectable({
//   providedIn: 'root'
// })
// export class DuendeAuthService {
//   private readonly _oauthService: OAuthService = inject(OAuthService);

//   constructor() {
//    }

//    handleAuthenticationCallback() {
//     this._oauthService.tryLogin({
//       onTokenReceived: context => {
//         // Handle successful token retrieval, e.g., store the token
//         console.log('Token received', context);
//       },
//       onLoginError: error => {
//         // Handle login errors, e.g., redirect to login page
//         console.error('Login error', error);
//       },
//     });
//   }

//   isLoggedIn(): Boolean{
//     return this._oauthService.hasValidAccessToken();
//   }

//   login(): void {
//     this._oauthService.initImplicitFlow();
//   }

//   logOut(): void {
//     this._oauthService.logOut();
//   }

//   getUserProfile(): Observable<any> {
//     return from(this._oauthService.loadUserProfile()).pipe(
//       catchError((error) => {
//         console.error('Error loading user profile:', error);
//         throw new Error('Failed to load user profile');
//       })
//     );
//   }

//   get claims(){
//     let claims:any = this._oauthService.getIdentityClaims();
//     return claims ? claims : null;
//   }

//   get id_token() {
//     return this._oauthService.getIdToken();
//   }

//   get access_token() {
//     return this._oauthService.getAccessToken();
//   }

//   get id_token_expiration() {
//     return this._oauthService.getIdTokenExpiration();
//   }

//   get access_token_expiration() {
//     return this._oauthService.getAccessTokenExpiration();
//   }

// }
