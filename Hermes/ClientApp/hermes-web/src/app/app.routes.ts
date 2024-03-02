import { Routes } from '@angular/router';
import { UserComponent } from './components/user/user.component';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { PostAuthCallbackComponent } from './components/post-auth-callback/post-auth-callback.component';
import { LoginComponent } from './components/login/login.component';

export const routes: Routes = [
    {path: 'users/view/:id', component: UserComponent },
    {path: 'signin-callback', component: AuthCallbackComponent},
    {path: 'signout-callback', component: PostAuthCallbackComponent},
    {path: 'login', component:LoginComponent},
];
