import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-jumbotron',
  templateUrl: './jumbotron.component.html',
  styleUrls: ['./jumbotron.component.scss'],
})
export class JumbotronComponent implements OnInit {
  @Input() titolo: string = '';
  @Input() sottoTitolo: string = '';
  @Input() show: boolean = true;

  constructor() {}

  ngOnInit(): void {}
}
