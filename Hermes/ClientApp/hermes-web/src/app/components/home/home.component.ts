import { Component, inject } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  private readonly _oauthService: OAuthService = inject(OAuthService);

  public login() {
    this._oauthService.initImplicitFlow();
  } 

  public logOut() {
      this._oauthService.logOut();
  }

  public isLoggedIn(): Boolean{
    return this._oauthService.hasValidAccessToken();
  }

  public get name() {
      let claims = this._oauthService.getIdentityClaims();
      if (!claims) return null;
      return claims['given_name'];
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
