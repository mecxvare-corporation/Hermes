import { PostAuthCallbackComponent } from './components/post-auth-callback/post-auth-callback.component';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { UserEditComponent } from './components/user/user-edit/user-edit.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { UserComponent } from './components/user/user.component';
import { Routes } from '@angular/router';

export const routes: Routes = [
    {path: '', redirectTo: '/login', pathMatch: 'full'},
    {path: 'login', component:LoginComponent},
    {path: 'signin-callback', component: AuthCallbackComponent},
    {path: 'home', component: HomeComponent},
    {path: 'user/:userId', component: UserComponent, children: [{
        path: 'edit', component: UserEditComponent}]},
    {path: 'signout-callback', component: PostAuthCallbackComponent},
];
