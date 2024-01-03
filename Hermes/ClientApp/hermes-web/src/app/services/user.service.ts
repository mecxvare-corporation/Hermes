import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { UserInfoModel } from '../models/user-info';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUri: string = "https://localhost:7080/api/Users";
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getUserById(id: string): Observable<UserInfoModel>{
    return this._httpClient.get<UserInfoModel>(`${this.baseUri}/${id}`);
  }
}
