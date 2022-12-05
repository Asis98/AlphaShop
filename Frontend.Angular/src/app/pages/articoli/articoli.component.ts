import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { map, Observable, of } from 'rxjs';
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

  public pagina: number = 1;
  public righe: number = 10;

  public filter$: Observable<string | null> = of('');
  public filter: string | null = '';

  public filterType: number = 0;
  public codArt: string = '';

  constructor(
    private articoliService: ArticoliService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.filter$ = this.route.queryParamMap.pipe(
      map((params: ParamMap) => params.get('filter'))
    );

    this.filter$.subscribe((param) => (this.filter = param));

    if (this.filter) {
      this.getArticoli(this.filter);
    }
  }

  public getArticoli = (filter: string) => {
    this.articoli$ = [];

    if (this.filterType === 0) {
      this.articoliService.getArticoliByCode(filter).subscribe({
        next: this.handleResponse.bind(this),
        error: this.handleError.bind(this),
      });
    } else if (this.filterType === 1) {
      this.articoliService.getArticoliByDesc(filter).subscribe({
        next: this.handleResponse.bind(this),
        error: this.handleError.bind(this),
      });
    } else if (this.filterType === 2) {
      this.articoliService.getArticoliByEan(filter).subscribe({
        next: this.handleResponse.bind(this),
        error: this.handleError.bind(this),
      });
    }
  };

  public refresh() {
    if (this.filter) {
      this.getArticoli(this.filter);
    }
  }

  public handleResponse(response: any) {
    if (this.filterType === 0 || this.filterType === 2) {
      let newArray: IArticoli[] = [...this.articoli$, response];
      this.articoli$ = newArray;
    } else {
      this.articoli$ = response;
    }

    this.filterType = 0;
  }

  public handleError(error: any) {
    if (this.filter && this.filterType === 0) {
      this.filterType = 1;
      this.getArticoli(this.filter);
    } else if (this.filter && this.filterType === 1) {
      this.filterType = 2;
      this.getArticoli(this.filter);
    } else {
      this.errore = error.error.message;
      this.filterType = 0;
    }
  }

  public getColoreStatoArt(idStato: string) {
    switch (idStato) {
      case '1':
        return 'badge rounded-pill alert alert-success';
      case '2':
        return 'badge rounded-pill alert alert-warning';
      default:
        return 'badge rounded-pill alert alert-danger';
    }
  }

  public elimina(codArt: string): void {
    this.codArt = codArt;

    this.articoliService.delArticoloByCodArt(codArt).subscribe({
      next: this.handleOkDelete.bind(this),
      error: this.handleErrorDelete.bind(this),
    });
  }
  public handleOkDelete(response: any) {
    this.articoli$ = this.articoli$.filter(
      (item) => item.codArt !== this.codArt
    );
    this.codArt = '';
  }
  public handleErrorDelete(error: any) {
    this.errore = error.error.message;
  }

  public modifica(codArt: string): void {
    this.router.navigate(['gestioneArticolo', codArt]);
  }
}
