import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  imports: [],
  providers: [AuthService],
  templateUrl: './auth-callback.component.html',
  styleUrl: './auth-callback.component.scss'
})
export class AuthCallbackComponent {
  error: boolean = false;
  private readonly _router = inject(Router);
  private readonly _route = inject(ActivatedRoute);
  private readonly _authService = inject(AuthService)

  constructor() {}

  ngOnInit() {
    // check for error
    const fragment = this._route.snapshot.fragment;

    if (fragment && fragment.indexOf('error') !== -1) {
      this.error = true;
      return;
    }

    localStorage.setItem('acctoken', this._authService.access_token);
    this._router.navigate(['/'], {replaceUrl: true});    
  }
}
