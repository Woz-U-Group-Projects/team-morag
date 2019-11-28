import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

    API_KEY = '83361ab188d840e3b5b51f3d4863498b';
    url: string = 'https://newsapi.org/v2/top-headlines';

  constructor(private httpClient: HttpClient) { }
  public getNews(country = 'us', category = 'science', sources?, keywords?, pagesize = 20, page = 1){
  let apiUrl = this.url + '?country=' + country + '&category=' + category;

  if (sources) {
      apiUrl += '&sources=' + sources;
      if (keywords) {
        apiUrl += '&q=' + keywords;
      }
    } else if (keywords) {
      apiUrl += '&q=' + keywords;
      if (sources) {
        apiUrl += '&sources=' + sources;
      }
    }
  apiUrl += '&pageSize=' + pagesize + '&page=' + page + '&apiKey=' + this.API_KEY;
  return this.httpClient.get(apiUrl);
 }
}
