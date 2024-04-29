import { Component, EventEmitter, OnInit, Output, inject } from '@angular/core';
import { UserMinimalInfoModel } from '../../../models/user-minimal-info';
import { PostModalComponent } from './post-modal/post-modal.component';
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-create-post',
  standalone: true,
  imports: [MatCardModule, MatIconModule, PostModalComponent, RouterModule],
  providers: [MatDialog],
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent implements OnInit {

  private readonly _userService = inject(UserService);
  private readonly _authService: AuthService = inject(AuthService);

  private readonly _dialog = inject(MatDialog);

  private readonly _router = inject(Router);

  private _isDialogOpen: boolean = false;
  postUserInfo: UserMinimalInfoModel | undefined;

  @Output() postSubmitted: EventEmitter<void> = new EventEmitter<void>(); 

  constructor(){
  }

  ngOnInit(): void {
    const currentUserId = this._authService.claims['sub'];
    if(currentUserId !== null){
      this._userService.getCurrentUsersMinimalInfo(currentUserId).subscribe((response)=>{
        this.postUserInfo = response;
      });
    }
  }

  openPopup(){
    if(!this._isDialogOpen){
      const dialogRef = this._dialog.open(PostModalComponent);
      this._isDialogOpen = true;
      dialogRef.afterClosed().subscribe(result => {
        this._isDialogOpen = false;

        this.postSubmitted.emit();
      });
    }
  }

}
