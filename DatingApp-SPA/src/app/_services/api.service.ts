import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
<<<<<<< HEAD
=======
import { Country } from '../_models/country.enum';
import { Category } from '../_models/category.enum';
>>>>>>> dev

@Injectable({
  providedIn: 'root'
})
export class ApiService {

    API_KEY = '83361ab188d840e3b5b51f3d4863498b';
    url: string = 'https://newsapi.org/v2/top-headlines';

  constructor(private httpClient: HttpClient) { }
  public getNews(userObj: {country: string , category: Category, sources: string , keywords: string, pagesize: string, page :string}){
  let apiUrl = this.url + '?country=' + userObj.country + '&category=' + userObj.category;

  if (userObj.sources === ' ') {
      apiUrl += '&sources=' + userObj.sources;
      if (userObj.keywords === ' ') {
        apiUrl += '&q=' + userObj.keywords;
      }
    } else if (userObj.keywords === ' ') {
      apiUrl += '&q=' + userObj.keywords;
      if (userObj.sources === ' ') {
        apiUrl += '&sources=' + userObj.sources;
      }
    }
  apiUrl += '&pageSize=' + userObj.pagesize + '&page=' + userObj.page + '&apiKey=' + this.API_KEY;
  return this.httpClient.get(apiUrl);
 }
}
