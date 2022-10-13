import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IArticoli } from 'src/app/models/articoli';

@Component({
  selector: 'app-articoli-card',
  templateUrl: './articoli-card.component.html',
  styleUrls: ['./articoli-card.component.scss'],
})
export class ArticoliCardComponent implements OnInit {
  @Input() articolo!: IArticoli;
  @Output() delete = new EventEmitter();
  @Output() edit = new EventEmitter();
  constructor() {}

  ngOnInit(): void {}

  public editArt(){
    this.edit.emit(this.articolo.codart);
  }

  public deleteArt(){
    this.delete.emit(this.articolo.codart);
  }
}
