import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {StudentListComponent} from "./component/student-list/student-list.component";
import {StudentCreateComponent} from "./component/student-create/student-create.component";
import {StudentEditComponent} from "./component/student-edit/student-edit.component";
import {StudentDeleteComponent} from "./component/student-delete/student-delete.component";
import {authGuard} from "./guards/auth.guard";
import {LoginComponent} from "./component/login/login.component";


const routes: Routes = [
  {path: '', component: StudentListComponent, canActivate: [authGuard]},
  {path: 'login', component: LoginComponent},
  {path:'create', component: StudentCreateComponent},
  {path:'edit/:id', component:StudentEditComponent},
  {path:'delete/:id', component: StudentDeleteComponent},
  { path: '**', redirectTo: '/students', pathMatch: 'full' },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
