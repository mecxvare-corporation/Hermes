import { UserMinimalInfoModel } from '../../../models/user-minimal-info';
import { Component, Input, OnInit, inject } from '@angular/core';
import { PostsService } from '../../../services/posts.service';
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon'
import { Router, RouterLink } from '@angular/router';
import { PostModel } from '../../../models/post';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-post',
  standalone: true,
  imports: [RouterLink, MatCardModule, DatePipe, MatIconModule],
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent implements OnInit {

  @Input() post: PostModel | undefined;
  postUserInfo: UserMinimalInfoModel | undefined;

  private readonly _postsService = inject(PostsService);
  private readonly _userService: UserService = inject(UserService);
  private readonly _authService: AuthService = inject(AuthService);
  private readonly _router = inject(Router);

  ngOnInit(): void {
    if(this.post){
      const postUserId = this.post.userId;
      this._userService.getCurrentUsersMinimalInfo(postUserId).subscribe((response)=>{
        this.postUserInfo = response;
      });
    }
  }

  onEdit(){
  }

  onDelete(){
    if(this.post !== undefined){
      this._postsService.deletePost(this.post.id).subscribe();
    }
  }
}
