import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IArticoli, ICategory, IIva } from 'src/app/models/articoli';
import { ArticoliService } from 'src/services/data/articoli.service';

@Component({
  selector: 'app-gestione-articolo',
  templateUrl: './gestione-articolo.component.html',
  styleUrls: ['./gestione-articolo.component.scss'],
})
export class GestioneArticoloComponent implements OnInit {
  title: string = 'Modifica Articoli';

  codArt: string = '';
  articolo!: IArticoli;

  ivaList : IIva[] = [];
  categoryList : ICategory[] = [];

  constructor(
    private route: ActivatedRoute,
    private articoliService: ArticoliService
  ) {}

  ngOnInit(): void {
    this.codArt = this.route.snapshot.params['codArt'];
    this.articoliService.getArticoliByCode(this.codArt).subscribe({
      next: this.handleResponse.bind(this),
      error: this.handleError.bind(this),
    });

    this.articoliService.getIva().subscribe(
      response => this.ivaList = response
    );

    this.articoliService.getCategory().subscribe(response => this.categoryList = response);
  }
  handleResponse(response: any) {
    this.articolo = response;

    console.log(this.articolo);
  }

  handleError(error: any) {
    console.log(error);
  }
}
