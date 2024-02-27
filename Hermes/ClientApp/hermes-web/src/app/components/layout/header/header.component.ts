import { Component, OnInit, inject } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { Observable } from 'rxjs';
import { UserMinimalInfoModel } from '../../../models/user-minimal-info';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'hermes-header',
  standalone: true,
  imports: [CommonModule, NgOptimizedImage],
  providers: [AuthService],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  private readonly _authService: AuthService = inject(AuthService);
  private readonly _httpClient: HttpClient = inject(HttpClient);

  private baseUri: string = "https://localhost:7080/api/auth";

  userMinimalInfo$: Observable<UserMinimalInfoModel> | undefined;


  ngOnInit() {
    
  }

  getAuthorizedUser() {
    const token = this._authService.access_token;
    debugger
    console.log(token)
    this.userMinimalInfo$ = this._httpClient.get<UserMinimalInfoModel>(this.baseUri);
  }

  isLoggedIn(){
    return this._authService.isLoggedIn();
  }

  logOut(){
    this._authService.logOut();
  }
}
