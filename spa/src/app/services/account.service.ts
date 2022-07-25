import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import { User } from '../models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  currentUser: User

  constructor(private http: HttpClient) { 
    var user = localStorage.getItem('user')
    this.currentUser = JSON.parse(user)
  }

  login(model: any) {
    return this.http.post(this.baseUrl + '/user/log-in', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post(this.baseUrl + '/user/register', model).pipe(
      map((user: User) => {
        if (user) {
         this.setCurrentUser(user);
        }
      })
    )
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser = user;
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser = null;
  }
}
