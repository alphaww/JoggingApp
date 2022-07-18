import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Jog } from '../models/jog';

@Injectable({
  providedIn: 'root'
})
export class JogService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  search(dateFrom: Date, dateTo: Date) {
    const url = this.baseUrl + '/jog/search';
 
    let queryParams = new HttpParams();
    queryParams = queryParams.append("dateFrom",dateFrom.toISOString());
    queryParams = queryParams.append("dateTo",dateTo.toISOString());
 
    return this.http.get<Jog[]>(url,{params:queryParams});
  }

  insert(jog: Jog) {
    return this.http.post(this.baseUrl + '/jog/insert', jog)
  }
}
