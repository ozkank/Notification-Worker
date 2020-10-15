import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

@Injectable()
export class ApiService {

    constructor(private httpClient: HttpClient) {
    }

    get apiUrl() {
        return "https://app-boyner-notification-prod.azurewebsites.net/";
      }

      sendMessage(message: any) {
        let serviceUrl = this.apiUrl + "Notification/send?message=" + message;
        return this.httpClient.post(serviceUrl, null);
      }

}