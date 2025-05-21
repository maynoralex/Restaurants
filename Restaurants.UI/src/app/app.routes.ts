import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: "", redirectTo: "login"  , pathMatch: "full" },
    { path: "login", loadComponent: () => import('./login/login.component').then(m => m.LoginComponent) },
    { path: "restaurants", loadComponent: () => import('./restaurant-list/restaurant-list.component').then(m => m.RestaurantListComponent) },
];
