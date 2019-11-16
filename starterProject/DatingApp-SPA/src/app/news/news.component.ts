import { Component, OnInit } from '@angular/core';
import { NewsService } from '../_services/news.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {

  constructor(private newService: NewsService) { }

  news = [];

  ngOnInit() {
    this.getNews();
  }

  getNews() {
    this.newService.GetNews().subscribe(
      (data) => {

        this.news = data['articles'];
        console.log(this.news);
      }
    )
  }
}
