import { Component, OnInit, inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Observable } from 'rxjs';
import { UserInfoModel } from '../../models/user-info';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule,  NgOptimizedImage],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent implements OnInit {
  private readonly _userService = inject(UserService);
  private readonly _route = inject(ActivatedRoute);
  private _id: string = "";

  userInfo$: Observable<UserInfoModel> | undefined;

  ngOnInit(): void {
    this._id =this._route.snapshot.paramMap.get('id') || "";
    this.getUserInfo(this._id)
  }
  
  getUserInfo(id:string){
    this.userInfo$ = this._userService.getUserById(id);
  }
}
