import { Component, OnInit, inject } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { Observable } from 'rxjs';
import { UserMinimalInfoModel } from '../../../models/user-minimal-info';
import { CommonModule, NgOptimizedImage } from '@angular/common';

@Component({
  selector: 'hermes-header',
  standalone: true,
  imports: [CommonModule, NgOptimizedImage],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  private readonly _authService = inject(AuthService);

  userMinimalInfo$: Observable<UserMinimalInfoModel> | undefined;

  ngOnInit() {
    this.userMinimalInfo$ = this._authService.getAuthorizedUser();
  }
}
