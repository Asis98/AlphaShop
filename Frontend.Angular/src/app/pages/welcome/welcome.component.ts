import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SalutiDataService } from 'src/services/data/saluti-data.service';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss']
})
export class WelcomeComponent implements OnInit {

  public utente : string = 'Asia';

  titolo: string = 'Benvenuti in Alphashop';
  sottotitolo : string = 'Visualizza le offerte del giorno';

  public saluto  : string = '';

  constructor(private route: ActivatedRoute, private salutiSvc : SalutiDataService) { }

  ngOnInit(): void {
    this.utente = this.route.snapshot.params['userId'];
  }

  public saluti() : void {
    this.salutiSvc.getSaluti(this.utente).subscribe(sal=>this.saluto=sal.toString());
  }

}
