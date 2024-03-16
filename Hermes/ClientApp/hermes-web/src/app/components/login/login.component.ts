import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [],
  providers: [AuthService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly _oauthService: AuthService = inject(AuthService);

  login(){
    this._oauthService.login();
  }

  create(){
    this._oauthService.register();
  }

}
