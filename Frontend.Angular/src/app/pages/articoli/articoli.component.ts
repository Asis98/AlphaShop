import { Component, OnInit } from '@angular/core';
import { ArticoliService } from 'src/services/data/articoli.service';
import { IArticoli } from '../../models/articoli';

@Component({
  selector: 'app-articoli',
  templateUrl: './articoli.component.html',
  styleUrls: ['./articoli.component.scss'],
})
export class ArticoliComponent implements OnInit {
  public articoli$: IArticoli[] = [];
  public errore: string = '';

  public pagina : number = 1;
  public righe : number = 10;

  constructor(private articoliService: ArticoliService) {}

  ngOnInit(): void {
    this.articoliService.getArticoliByDesc('Barilla').subscribe({
      next: this.handleResponse.bind(this),
      error: this.handleError.bind(this),
    });
  }

  public handleResponse(response: IArticoli[]) {
    this.articoli$ = response;
  }

  public handleError(error: Object) {
    this.errore = error.toString();
  }

  public getColoreStatoArt(idStato : string){
    switch(idStato){
      case 'Attivo':
        return 'badge rounded-pill alert alert-success';
      case 'Sospeso' :
        return 'badge rounded-pill alert alert-warning';
      default:
        return 'badge rounded-pill alert alert-danger'
    }
  }
}
