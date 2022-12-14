import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { WelcomeComponent } from './pages/welcome/welcome.component';
import { LoginComponent } from './pages/login/login.component';
import { FormsModule } from '@angular/forms';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { ArticoliComponent } from './pages/articoli/articoli.component';
import { CoreModule } from './core/core.module';
import { LogoutComponent } from './pages/logout/logout.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GridArticoliComponent } from './pages/grid-articoli/grid-articoli.component';
import { ArticoliCardComponent } from './components/articoli-card/articoli-card.component';
import { HttpClientModule } from '@angular/common/http';
import { RegistrazioneComponent } from './pages/registrazione/registrazione.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { GestioneArticoloComponent } from './pages/gestione-articolo/gestione-articolo.component';

@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent,
    LoginComponent,
    NotFoundComponent,
    ArticoliComponent,
    LogoutComponent,
    GridArticoliComponent,
    ArticoliCardComponent,
    RegistrazioneComponent,
    GestioneArticoloComponent
  ],
  imports: [BrowserModule, AppRoutingModule, FormsModule, CoreModule, BrowserAnimationsModule, HttpClientModule, NgxPaginationModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
