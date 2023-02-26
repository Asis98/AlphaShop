
export interface IArticoli{
  codArt: string,
  descrizione: string,
  um: string,
  codStat : string,
  pzCart: number,
  pesoNetto: number,
  prezzo: number,
  idStatoArticolo : string,
  descrizioneStatoArticolo: string,
  dataCreazione: Date,
  imageUrl: string
}

export interface IIva {
  idIva: number;
  descrizione: string;
  aliquota: number;
}

export interface ICategory{
  id: number;
  descrizione : string;
}
