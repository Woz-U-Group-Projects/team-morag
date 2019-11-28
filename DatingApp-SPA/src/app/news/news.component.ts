import { Component, OnInit } from '@angular/core';
import { ApiService } from '../_services/api.service';
import { NewsItem } from '../_models/news-item';
import { faSpinner } from '@fortawesome/free-solid-svg-icons'
@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {
  articles: NewsItem[] = [];
  isLoading = false;
  faSpinner = faSpinner;


  constructor(private apiService: ApiService) { }
  ngOnInit() {
    this.getNews();
  }

  getNews() {
    this.isLoading = true;
    this.apiService.getNews().subscribe((data) => {
      this.isLoading = false;
      console.log(data);

      this.articles = data['articles'];
    });
  }
}