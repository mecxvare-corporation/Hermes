import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { UserModel } from '../models/user-info';
import { UserMinimalInfoModel } from '../models/user-minimal-info';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private _baseUsersUri: string = "https://localhost:7080/api/Users";
  private readonly _httpClient: HttpClient = inject(HttpClient);

  constructor(){
  }

  getAllUsers(): Observable<UserModel[]>{
    return this._httpClient.get<UserModel[]>(`${this._baseUsersUri}`);
  }

  getUserById(userId: string): Observable<UserModel>{
    return this._httpClient.get<UserModel>(`${this._baseUsersUri}/${userId}`);
  }

  getCurrentUsersMinimalInfo(userId: string): Observable<UserMinimalInfoModel>{
    return this._httpClient.get<UserModel>(`${this._baseUsersUri}/${userId}`).pipe(
      map(user => ({...user, fullname: `${user.firstName}${user.lastName}`})));
  }
}
