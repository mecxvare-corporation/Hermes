import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  imports: [],
  providers: [],
  templateUrl: './auth-callback.component.html',
  styleUrl: './auth-callback.component.scss'
})
export class AuthCallbackComponent {
  error: boolean = false;
  private readonly _router = inject(Router);
  private readonly _route = inject(ActivatedRoute);

  constructor() {}

  async ngOnInit() {
    // check for error
    const fragment = this._route.snapshot.fragment;

    if (fragment && fragment.indexOf('error') !== -1) {
      this.error = true;
      return;
    }

    await this._router.navigate(['/'], {replaceUrl: true});    
  }
}
