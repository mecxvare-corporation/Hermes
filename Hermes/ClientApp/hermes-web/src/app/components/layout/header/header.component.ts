import { UserMinimalInfoModel } from '../../../models/user-minimal-info';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';
import { Component, OnInit, inject } from '@angular/core';
import { MatBadgeModule } from '@angular/material/badge';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'hermes-header',
  standalone: true,
  imports: [CommonModule, NgOptimizedImage, MatIconModule, MatBadgeModule, MatMenuModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  private readonly _userService: UserService = inject(UserService);
  private readonly _authService: AuthService = inject(AuthService);

  userMinimalInfo: UserMinimalInfoModel | undefined;

  ngOnInit() {
    const currentUserId = this._authService.claims['sub'];
    if(currentUserId !== null){
      this._userService.getCurrentUsersMinimalInfo(currentUserId).subscribe((response)=>{
        this.userMinimalInfo = response;
      });
    }
  }

  logOut(){
    this._authService.logOut();
  }
}
