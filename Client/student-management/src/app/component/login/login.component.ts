import { Component } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  userName: string = '';
  password: string = '';
  errorMessage: any;
constructor(private authService: AuthService, private router: Router) {
}
    login() {
      this.authService.login(this.userName, this.password).subscribe((res:any) =>{
        localStorage.setItem('jwt_token', res.token);
        this.router.navigate(['']);
      }, (error) => {
        console.error('Login failed due to invalid token', error);
      });
  }
}
