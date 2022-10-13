import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthappService } from 'src/services/authapp.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  userId : string = 'Asia';
  password : string = '';

  autenticato : boolean = true;

  okMsg : string = 'Accesso completato';
  errMsg : string = 'Spiacente, la userId e/o la password sono errati!';

  titolo: string = 'Accesso e Autenticazione';
  sottotitolo : string = 'Inserisci la userId e la password';

  constructor(private route: Router, private basicAuth  : AuthappService) { }

  ngOnInit(): void {
  }

  public gestioneAutenticazione(){
    if(this.basicAuth.autentica(this.userId, this.password)){
      this.route.navigate(['welcome', this.userId]);
      this.autenticato = true;
    }
    else{
      this.autenticato=false;
    }
  }

}
