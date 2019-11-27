import { Component, OnInit } from '@angular/core';
import { ApiService } from '../_services/api.service';
import { NewsItem } from '../_models/news-item';
@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {
  articles: NewsItem[] = [];


  constructor(private apiService: ApiService) { }
  ngOnInit() {
    this.apiService.getNews().subscribe((data) => {
      console.log(data);

      this.articles = data['articles'];
    });
  }
}