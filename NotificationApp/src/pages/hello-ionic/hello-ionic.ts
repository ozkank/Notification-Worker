import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiService } from '../../app.service';

@Component({
  selector: 'page-hello-ionic',
  templateUrl: 'hello-ionic.html'
})

export class HelloIonicPage {
 
 
  constructor(private apiService : ApiService) {}

  ngOnInit(){}

  send() {
    this.apiService.sendMessage('message').subscribe((response) => {
      console.log(response);
    });
  }

}
