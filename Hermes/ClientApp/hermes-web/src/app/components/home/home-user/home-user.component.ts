import { CommonModule, NgOptimizedImage } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { Component, OnInit, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { UserModel } from '../../../models/user-info';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home-user',
  standalone: true,
  imports: [CommonModule,  NgOptimizedImage, MatIconModule],
  templateUrl: './home-user.component.html',
  styleUrl: './home-user.component.scss'
})
export class HomeUserComponent implements OnInit {

  private readonly _userService = inject(UserService);
  private readonly _authService = inject(AuthService);
  private readonly _route = inject(ActivatedRoute);

  currentUser: UserModel | undefined;

  ngOnInit(): void {
    const currentUserId = this._authService.claims['sub'];
    
    if(currentUserId !== null){
      this._userService.getUserById(currentUserId).subscribe(
        user => this.currentUser = user);
    }
  }
}
