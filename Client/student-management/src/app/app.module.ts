import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StudentListComponent } from './component/student/student-list/student-list.component';
import { StudentCreateComponent } from './component/student/student-create/student-create.component';
import { StudentEditComponent } from './component/student/student-edit/student-edit.component';
import { StudentDeleteComponent } from './component/student/student-delete/student-delete.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule, HTTP_INTERCEPTORS} from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { ConfirmDeleteDialogComponent } from './component/student/student-delete/confirm-delete-dialog/confirm-delete-dialog.component';
import {MatDialogModule} from "@angular/material/dialog";
import {MatPaginatorModule} from "@angular/material/paginator";
import {JwtInterceptor} from "./interceptors/jwt.interceptor";
import {JwtModule} from "@auth0/angular-jwt";
import {AuthService} from "./services/auth.service";
import {LoginComponent} from "./component/login/login.component";
import {MatSelectModule} from "@angular/material/select";
import { AddressEditComponent } from './component/address/address-edit/address-edit.component';
import { AddressDeleteComponent } from './component/address/address-delete/address-delete.component';
import {AddressListComponent} from "./component/address/address-list/address-list.component";
import {AddressCreateComponent} from "./component/address/address-create/address-create.component";
import { RegisterComponent } from './component/register/register.component';
import { HomeComponent } from './component/home/home.component';


export function tokenGetter(){
  return localStorage.getItem('jwt_token');
}


@NgModule({
  declarations: [
    AppComponent,
    StudentListComponent,
    StudentCreateComponent,
    StudentEditComponent,
    StudentDeleteComponent,
    ConfirmDeleteDialogComponent,
    LoginComponent,
    AddressListComponent,
    AddressCreateComponent,
    AddressEditComponent,
    AddressDeleteComponent,
    RegisterComponent,
    HomeComponent
  ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        BrowserAnimationsModule,
        MatButtonModule,
        MatInputModule,
        MatTableModule,
        MatFormFieldModule,
        MatIconModule,
        MatCardModule,
        MatDialogModule,
        MatPaginatorModule,
        JwtModule.forRoot({
            config: {
                tokenGetter: tokenGetter,
                allowedDomains: ['localhost:5081'],
                disallowedRoutes: []
            }
        }),
        MatSelectModule
    ],
  providers: [
    AuthService,{
     provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
