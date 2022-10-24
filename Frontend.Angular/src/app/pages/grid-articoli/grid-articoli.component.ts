import { Component, OnInit } from '@angular/core';
import { IArticoli } from 'src/app/models/articoli';
import { ArticoliService } from 'src/services/data/articoli.service';

@Component({
  selector: 'app-grid-articoli',
  templateUrl: './grid-articoli.component.html',
  styleUrls: ['./grid-articoli.component.scss'],
})
export class GridArticoliComponent implements OnInit {
  public articoli$: IArticoli[] = [];
  public errore: string = '';

  constructor(private articoliService: ArticoliService) {}

  ngOnInit(): void {
    this.articoliService.getArticoliByDesc('Barilla').subscribe({
      next: this.handleResponse.bind(this),
      error: this.handleError.bind(this),
    });
  }

  public handleEdit(codart: string) {}

  public handleDelete(codart: string) {
    this.articoli$.splice(
      this.articoli$.findIndex((x) => x.codArt === codart),
      1
    );
  }

  public handleResponse(response: IArticoli[]) {
    this.articoli$ = response;
  }

  public handleError(error: Object) {
    this.errore = error.toString();
  }
}
