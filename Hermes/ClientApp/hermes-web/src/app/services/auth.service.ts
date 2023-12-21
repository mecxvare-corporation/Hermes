import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserMinimalInfoModel } from '../models/user-minimal-info';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUri: string = "https://localhost:7080/api/auth";

  constructor(private readonly _httpClient: HttpClient) { }

  getAuthorizedUser(): Observable<UserMinimalInfoModel> {
    return this._httpClient.get<UserMinimalInfoModel>(this.baseUri);
  }
}
