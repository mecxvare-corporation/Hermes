import { HeaderComponent } from '../layout/header/header.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { Component } from '@angular/core';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [UserEditComponent, HeaderComponent],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent {

}
