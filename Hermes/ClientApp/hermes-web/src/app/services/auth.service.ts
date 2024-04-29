import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';
import { Injectable, inject } from '@angular/core';

const authConfig: AuthConfig = {
  issuer: 'https://localhost:5001', 
  clientId: 'SPA',
  responseType: 'id_token token',
  redirectUri: window.location.origin + '/signin-callback',
  postLogoutRedirectUri: window.location.origin + '/signout-callback',
  scope: 'openid profile',
  strictDiscoveryDocumentValidation: false,
  showDebugInformation: true,
  oidc: true, 
  userinfoEndpoint: 'https://localhost:5001/connect/userinfo'
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly _oauthService: OAuthService = inject(OAuthService);

  constructor() {
    this.configureOauth();
   }

   configureOauth() {
    this._oauthService.configure(authConfig);
    this._oauthService.loadDiscoveryDocument().then(()=>{
      this._oauthService.tokenValidationHandler = new JwksValidationHandler();
    });
  }

  loadUser(){
    return this._oauthService.loadUserProfile();
  }

  isLoggedIn(): boolean{
    return this._oauthService.hasValidIdToken();
  }

  logIn(): void {
    this._oauthService.initImplicitFlow();
  }

  saveRecivedData(): Promise<boolean> {
    return this._oauthService.tryLogin();
  }

  logOut(): void {
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
}
