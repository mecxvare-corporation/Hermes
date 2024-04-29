import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Component, OnInit, inject } from '@angular/core';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './auth-callback.component.html',
  styleUrl: './auth-callback.component.scss'
})
export class AuthCallbackComponent implements OnInit {
  error: boolean = false;
  private readonly _router = inject(Router);
  private readonly _route = inject(ActivatedRoute);
  private readonly _authService = inject(AuthService);


  constructor() {}

  ngOnInit() {
    const fragment = this._route.snapshot.fragment;

    if (fragment && fragment.indexOf('error') !== -1) {
      this.error = true;
    }else {
      this._authService.saveRecivedData().then(()=>{
        localStorage.setItem('acctoken', this._authService.access_token);
        this._router.navigate([`/home`], {replaceUrl: true}); 
      });
    }
  }
}