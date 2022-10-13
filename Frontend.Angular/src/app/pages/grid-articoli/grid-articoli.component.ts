import { Component, OnInit } from '@angular/core';
import { IArticoli } from 'src/app/models/articoli';
import { ArticoliService } from 'src/services/articoli.service';

@Component({
  selector: 'app-grid-articoli',
  templateUrl: './grid-articoli.component.html',
  styleUrls: ['./grid-articoli.component.scss']
})
export class GridArticoliComponent implements OnInit {

  articoli$ : IArticoli[] = [];

  constructor(private articoliService : ArticoliService) { }

  ngOnInit(): void {
    this.articoli$ = this.articoliService.getArticoli;
  }

  public handleEdit(codart: string){

  }

  public handleDelete(codart: string){
    this.articoli$.splice(this.articoli$.findIndex(x=> x.codart === codart),1);
  }
}
