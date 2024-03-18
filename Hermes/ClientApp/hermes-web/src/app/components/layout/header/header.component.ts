import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
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
  @Input() showLogOutOption: boolean | undefined;
  @Input() userMinimalInfo: UserMinimalInfoModel | undefined;

  @Output() logOutClicked: EventEmitter<void> = new EventEmitter<void>();

  ngOnInit() {
  }
  
  logOut(){
    this.logOutClicked.emit();
  }
}
