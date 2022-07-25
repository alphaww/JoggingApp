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
    if (dateTo) {
      queryParams = queryParams.append("to",dateTo.toISOString());
    } 
    if (dateFrom) {
      queryParams = queryParams.append("from",dateFrom.toISOString());
    }

    return this.http.get<Jog[]>(url,{params:queryParams});
  }

  get(jogId: any) {
    return this.http.get<Jog>(this.baseUrl + '/jog/' + jogId);
  }

  insert(jog: any) {
    return this.http.post(this.baseUrl + '/jog/insert', jog)
  }

  update(jog: any) {
    return this.http.put(this.baseUrl + '/jog/' + jog.id + '/update', jog)
  }

  delete(jogId: any) {
    return this.http.delete(this.baseUrl + '/jog/' + jogId + '/delete')
  }
}
