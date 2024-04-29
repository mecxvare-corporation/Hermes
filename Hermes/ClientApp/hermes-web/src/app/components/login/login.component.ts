import { AuthService } from '../../services/auth.service';
import { MatCardModule } from '@angular/material/card'
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatCardModule],
  providers: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly _oauthService: AuthService = inject(AuthService);

  login(){
    this._oauthService.logIn();
  }
}
