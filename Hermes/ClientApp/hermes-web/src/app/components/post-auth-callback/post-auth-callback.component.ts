import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-post-auth-callback',
  standalone: true,
  imports: [],
  templateUrl: './post-auth-callback.component.html',
  styleUrl: './post-auth-callback.component.scss'
})
export class PostAuthCallbackComponent {
  private readonly _router = inject(Router);

  async ngOnInit() {
    await this._router.navigate(['/'], {replaceUrl: true});    
  }
}
