import { Component, OnInit } from '@angular/core';
import { NewsService } from '../_services/news.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {

  constructor(private newService: NewsService) { }

  ngOnInit() {
    this.getNews();
  }

  getNews() {
    this.newService.GetNews().subscribe(
      (data) => {
        console.log(data);
      }
    )
  }
}
