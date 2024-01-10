import { Routes } from '@angular/router';
import { UserComponent } from './components/user/user.component';

export const routes: Routes = [
    {path: 'users/view/:id', component: UserComponent }
];
