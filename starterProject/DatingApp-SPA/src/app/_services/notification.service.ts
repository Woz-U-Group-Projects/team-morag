import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  notifications = 0;
  messageRecieved;

  GetNotification() {
    return this.notifications;
  }

  AddNotification(amount) {
    this.notifications += amount;
  }

  SetNotifications(amount) {
    this.notifications = amount;
  }

  getMessageRecieved() {
    return this.messageRecieved;
  }

  SetMessageReceved(bool) {
    this.messageRecieved = bool;
  }

  constructor() { }
}
