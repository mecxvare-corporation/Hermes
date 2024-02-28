import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/layout/header/header.component';
import { UserComponent } from './components/user/user.component';
import { OAuthModule} from 'angular-oauth2-oidc';
import { AuthService } from './services/auth.service';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { PostAuthCallbackComponent } from './components/post-auth-callback/post-auth-callback.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, HeaderComponent, UserComponent, OAuthModule, 
    AuthCallbackComponent, PostAuthCallbackComponent],
  providers: [AuthService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  private readonly _oauthService: AuthService = inject(AuthService);

  constructor() {
    this._oauthService.configure();
  }

  ngOnInit(){
    this.isLoggedIn();
    this._oauthService.loadUser()
  }

  isLoggedIn(){
    return this._oauthService.isLoggedIn();
  }

  login(){
    this._oauthService.login();
  }

  create(){
    this._oauthService.register();
  }
}
