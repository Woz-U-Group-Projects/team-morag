import { Component, OnInit, ViewChild, ElementRef, Renderer2, AfterViewChecked } from '@angular/core';
import { ApiService } from '../_services/api.service';
import { NewsItem } from '../_models/news-item';
import { faSpinner } from '@fortawesome/free-solid-svg-icons'
import { Country } from '../_models/country.enum';
import { Category } from '../_models/category.enum';
@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit, AfterViewChecked {
  @ViewChild('selectElement', {static: true}) selectElm: ElementRef;
  articles: NewsItem[] = [];
  countries = Country;
  isLoading = false;
  faSpinner = faSpinner;
  userObj: {country: string , category: Category, sources: string , keywords: string, pagesize: string, page: string}  =
      {country: Country.us, category: Category.science, sources: '', keywords: '', pagesize: '20', page: '1'};


  constructor(private apiService: ApiService, private r2: Renderer2) { }
  ngOnInit() {
    this.getNews();
  }

  ngAfterViewChecked() {
      console.log(this.selectElm);
  }

  getNews() {
    this.isLoading = true;
    const country = Object.keys(Country).filter((x) =>
    {
      if (this.countries[x] === this.userObj.country) {
        return x;
      }
    });
    this.userObj.country = country.toString();
    console.log(this.selectElm);
    this.apiService.getNews(this.userObj).subscribe((data) => {
      this.isLoading = false;
      console.log(data);

      this.articles = data['articles'];
    });
  }
}
