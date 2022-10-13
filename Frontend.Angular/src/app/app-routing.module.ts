import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RouteGuardService } from 'src/services/route-guard.service';
import { ArticoliComponent } from './pages/articoli/articoli.component';
import { GridArticoliComponent } from './pages/grid-articoli/grid-articoli.component';
import { LoginComponent } from './pages/login/login.component';
import { LogoutComponent } from './pages/logout/logout.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { WelcomeComponent } from './pages/welcome/welcome.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'index', component: LoginComponent },
  { path: 'login', component: LoginComponent },
  {
    path: 'welcome/:userId',
    component: WelcomeComponent,
    canActivate: [RouteGuardService],
  },
  {
    path: 'articoli',
    component: ArticoliComponent,
    canActivate: [RouteGuardService],
  },
  {
    path: 'articoli/grid',
    component: GridArticoliComponent,
    canActivate: [RouteGuardService],
  },
  { path: 'logout', component: LogoutComponent },
  { path: '**', component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
