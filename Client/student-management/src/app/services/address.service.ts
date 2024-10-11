import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Address, Student} from "../models/student.model";

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  private  API_BASE_URL = 'http://localhost:5081/api/address'
  constructor(private  httpClient: HttpClient) {

  }
  getAddresses(): Observable<Address[]>{
    return this.httpClient.get<Address[]>(this.API_BASE_URL)
  }
  getAddressByID(id:number): Observable<Address>{
    return this.httpClient.get<Address>(`${this.API_BASE_URL}/${id}`)
  }
}
