import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/layout/header/header.component';
import { UserComponent } from './components/user/user.component';
import { AuthConfig, OAuthModule, OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks'
import { HomeComponent } from './components/home/home.component';

const authConfig: AuthConfig = {
  issuer: 'https://localhost:5001', // Write IdentityProvider here
  clientId: 'angular-client',
  dummyClientSecret: 'not-required',
  responseType: 'id-token token',
  logoutUrl: window.location.origin + '/home',
  redirectUri: window.location.origin + '/',
  scope: 'openid profile',
  strictDiscoveryDocumentValidation: false,
  showDebugInformation: true,

  // set to true, to receive also an id_token via OpenId Connect (OIDC) in addition to the
  // OAuth2-based access_token
  oidc: true, // ID_Token
};

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, HeaderComponent, UserComponent, OAuthModule, HomeComponent],
  providers: [OAuthService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {

  constructor(private oauthService: OAuthService) {
    this.configure();
  }

  private configure() {
    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }
}
