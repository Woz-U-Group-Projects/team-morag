import { Component, OnInit } from '@angular/core';
import { Message } from '../models/message';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {
  model: Message = new Message();

  constructor() { }

  ngOnInit() {
  }
  onSubmit() {
    console.log('Submit Successful: ', this.model);
}

}
