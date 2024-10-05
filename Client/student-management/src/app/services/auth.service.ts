import { Injectable } from '@angular/core';
import {HttpClient, HttpParams, HttpResponse} from "@angular/common/http";
import {Router} from "@angular/router";
import {JwtHelperService} from "@auth0/angular-jwt";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
private API_BASE_URL = 'http://localhost:5081/api/Auth';
  constructor(private httpClient: HttpClient,private route: Router, private jwtHelper: JwtHelperService) {

  }
  login(userName: string, password: string){
    return this.httpClient.get(`${this.API_BASE_URL}/Login`, {
      params: { userName, password },
      responseType: 'json'  // Expect a plain text response (JWT token)
    })
  }
  logout()
  {
    localStorage.removeItem('jwt_token');
    this.route.navigate(['login']);
  }
  isAuthenticated(): boolean{
    const token = localStorage.getItem('jwt_token');
    return <boolean>(token && !this.jwtHelper.isTokenExpired(token));
  }
  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }
}
