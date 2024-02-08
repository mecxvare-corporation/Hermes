import { Routes } from '@angular/router';
import { UserComponent } from './components/user/user.component';
import { HomeComponent } from './components/home/home.component';

export const routes: Routes = [
    {path: 'users/view/:id', component: UserComponent },
    {path: 'home', component: HomeComponent},
    {path: '**', component: HomeComponent}
];
