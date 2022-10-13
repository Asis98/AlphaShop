import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthappService {
  constructor() {}

  public autentica = (userId: string, password: string): boolean => {
    let retVal = userId === 'Asia' && password === 'stella';
    if (retVal) {
      sessionStorage.setItem('Utente', userId);
    }
    return retVal;
  };

  public loggedUser = (): string | null =>
    sessionStorage.getItem('Utente') ? sessionStorage.getItem('Utente') : '';

  public isLogged = (): boolean => !!sessionStorage.getItem('Utente');

  public clearUser = () : void => sessionStorage.removeItem("Utente");

  public clearAll = () : void => sessionStorage.clear();
}
