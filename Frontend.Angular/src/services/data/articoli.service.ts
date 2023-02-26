import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { IArticoli, ICategory, IIva } from 'src/app/models/articoli';

@Injectable({
  providedIn: 'root'
})
export class ArticoliService {

  // articoli: IArticoli[]  = [
  //   {codart : '014600301', descrizione : 'BARILLA FARINA 1 KG', um : 'PZ', pzcart : 24, peso : 1, prezzo : 1.09, active : true, data : new Date(), imageUrl: 'assets/images/prodotti/014600301.jpg'},
  //   {codart : "013500121", descrizione : "BARILLA PASTA GR.500 N.70 1/2 PENNE", um : "PZ", pzcart : 30, peso : 0.5, prezzo : 1.3, active : true, data : new Date(), imageUrl: 'assets/images/prodotti/013500121.jpg'},
  //   {codart : "014649001", descrizione : "BARILLA PANNE RIGATE 500 GR", um : "PZ", pzcart : 12, peso : 0.5, prezzo : 0.89, active : true, data : new Date(), imageUrl: 'assets/images/prodotti/014649001.jpg'},
  //   {codart : "007686402", descrizione : "FINDUS FIOR DI NASELLO 300 GR", um : "PZ", pzcart : 8, peso : 0.3, prezzo : 6.46, active : true, data : new Date(), imageUrl: 'assets/images/prodotti/007686402.jpg'},
  //   {codart : "057549001", descrizione : "FINDUS CROCCOLE 400 GR", um : "PZ", pzcart : 12, peso : 0.4, prezzo : 5.97, active : true, data : new Date(), imageUrl: 'assets/images/prodotti/057549001.jpg'},
  // ]

  public server : string = "localhost";
  public port : string = "5051";

  constructor(private httpClient : HttpClient) { }

  // public get getArticoli () : IArticoli[]{
  //   return this.articoli;
  // }

  getArticoliByDesc = (descrizione : string) => {
    return this.httpClient.get<IArticoli[]>(`http://${this.server}:${this.port}/api/articoli/cerca/descrizione/${descrizione}`) //ALT + 0096 | ALT GR + '
    .pipe(
      map(response => {
        response.forEach(item => item.descrizioneStatoArticolo = this.getDesStatoArt(item.idStatoArticolo))
        return response;
      })
    );
  }

  getArticoliByCode = (codart: string) => {
    return this.httpClient.get<IArticoli>(`http://${this.server}:${this.port}/api/articoli/cerca/codice/${codart}`)
    .pipe(
      map(response => {
        response.descrizioneStatoArticolo = this.getDesStatoArt(response.idStatoArticolo)
        return response;
      })
    );
  }

  getArticoliByEan = (barcode: string) => {
    return this.httpClient.get<IArticoli>(`http://${this.server}:${this.port}/api/articoli/cerca/barcode/${barcode}`)
    .pipe(
      map(response => {
        response.descrizioneStatoArticolo = this.getDesStatoArt(response.idStatoArticolo)
        return response;
      })
    );
  }

  delArticoloByCodArt = (codArt: string) =>  {
    return this.httpClient.delete(`http://${this.server}:${this.port}/api/articoli/elimina/${codArt}`);
  }

  private getDesStatoArt(idStato : string){
    switch(idStato.trim()){
      case '1':
        return 'Attivo';
      case '2' :
        return 'Sospeso';
      default:
        return 'Eliminato'
    }
  }

  getIva = () => this.httpClient.get<IIva[]>(`http://${this.server}:${this.port}/api/iva`);

  getCategory = () => this.httpClient.get<ICategory[]>(`http://${this.server}:${this.port}/api/category`);

}
