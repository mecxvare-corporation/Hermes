import { Injectable, inject } from '@angular/core';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';

const authConfig: AuthConfig = {
  issuer: 'https://localhost:5001', 
  clientId: 'SPA',
  responseType: 'id_token token',
  redirectUri: window.location.origin + '/signin-callback',
  postLogoutRedirectUri: window.location.origin + '/signout-callback',
  scope: 'openid profile',
  strictDiscoveryDocumentValidation: false,
  showDebugInformation: true,

  // set to true, to receive also an id_token via OpenId Connect (OIDC) in addition to the
  // OAuth2-based access_token
  oidc: true, // ID_Token
  userinfoEndpoint: 'https://localhost:5001/connect/userinfo'
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly _oauthService: OAuthService = inject(OAuthService);

  constructor() { }

  configure() {
    this._oauthService.configure(authConfig);
    this._oauthService.tokenValidationHandler = new JwksValidationHandler();
    this._oauthService.loadDiscoveryDocumentAndTryLogin();
  }

  loadUser(){
    return this._oauthService.loadUserProfile();
  }

  register(){
    //need implementation
  }

  isLoggedIn(): boolean{
    return this._oauthService.hasValidIdToken();
  }

  login(): void {
    this._oauthService.initImplicitFlow();
  }

  logOut(): void {
    localStorage.clear();
    this._oauthService.logOut();
  }

  get claims(){
    let claims:any = this._oauthService.getIdentityClaims();
    return claims ? claims : null;
  }

  get id_token() {
    return this._oauthService.getIdToken();
  }

  get access_token() {
    return this._oauthService.getAccessToken();
  }

  get id_token_expiration() {
    return this._oauthService.getIdTokenExpiration();
  }

  get access_token_expiration() {
    return this._oauthService.getAccessTokenExpiration();
  }

    // getUserProfile(): Observable<any> {
  //   return from(this._oauthService.loadUserProfile()).pipe(
  //     catchError((error) => {
  //       console.error('Error loading user profile:', error);
  //       throw new Error('Failed to load user profile');
  //     })
  //   );
  // }

    //    handleAuthenticationCallback() {
  //   this._oauthService.tryLogin({
  //     onTokenReceived: context => {
  //       // Handle successful token retrieval, e.g., store the token
  //       console.log('Token received', context);
  //     },
  //     onLoginError: error => {
  //       // Handle login errors, e.g., redirect to login page
  //       console.error('Login error', error);
  //     },
  //   });
  // }
}
