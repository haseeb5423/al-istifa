import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout/layout.component';
import { HomeComponent } from './features/home/home.component';
import { NotFoundComponent } from './layout/not-found/not-found.component';
import { canActivateGuard } from '@core/guards/can-activate.guard';

export const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: '',
                component: HomeComponent
            }, 
            {
                path: 'profile',
                canActivate: [canActivateGuard],
                loadComponent: () => import('./features/profile/pages/profile-page/profile-page.component').then(m => m.ProfilePageComponent)
            }
        ]
    },
    {
        path: 'login',
        loadComponent: () => import('./features/authentication/pages/sign-in/sign-in.component').then(m => m.SignInComponent)
    },
    {
        path: 'register/step1',
        loadComponent: () => import('./features/authentication/pages/sign-up/sign-up.component').then(m => m.SignUpComponent)
    },
    {
        path: 'register/step2',
        loadComponent: () => import('./features/authentication/components/sign-up-steps/step2/step2.component').then(m => m.Step2Component)
    },
    { path: '**', component: NotFoundComponent }
];

export const routingProviders = [canActivateGuard];