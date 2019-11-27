import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

    API_KEY = '83361ab188d840e3b5b51f3d4863498b';

  constructor(private httpClient: HttpClient) { }
  public getNews(){
    return this.httpClient.get(`https://newsapi.org/v2/top-headlines?sources=the-verge&apiKey=${this.API_KEY}`);
  }
}
