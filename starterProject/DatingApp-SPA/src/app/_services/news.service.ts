import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NewsService {

  url = 'https://newsapi.org/v2/top-headlines?' +
        'country=us&' +
        'apiKey=a78c6bd425f4409a98388ff4705aee49';

  constructor(private http: HttpClient) { }

  GetNews(): Observable<object> {
    return this.http.get<object>(this.url);
  }
}
