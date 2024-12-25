import {Component, OnInit, AfterViewInit, ViewChild} from '@angular/core';
import {Student} from "../../../models/student.model";
import {MatTableDataSource} from "@angular/material/table";
import {MatPaginator, MatPaginatorModule} from "@angular/material/paginator";
import {error} from "@angular/compiler-cli/src/transformers/util";
import {Event, Router} from "@angular/router";
import {filter} from "rxjs";
import {StudentService} from "../../../services/student.service";
import {AuthService} from "../../../services/auth.service";

@Component({
  selector: 'app-student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.scss']
})
export class StudentListComponent implements OnInit, AfterViewInit{
students: Student[] = [];
dataSource = new MatTableDataSource<Student>([]);

@ViewChild(MatPaginator) paginator!: MatPaginator;
constructor(private  studentService: StudentService, private authService:  AuthService, private route: Router) {
this.dataSource = new MatTableDataSource(this.students);
}
ngOnInit(): void
{
  this.loadStudents();
}
  loadStudents(){
    this.studentService.getStudents().subscribe(
      (data) => {
        this.students = data;
        this.dataSource.data = this.students;
        },
      (error) => console.error('error while retrieving the student list', error)
    );
  }

  searchFilter(event: KeyboardEvent) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase(); // Set filter string

    // Custom filter predicate to filter by multiple fields (e.g., first name, last name)
    this.dataSource.filterPredicate = (data: Student, filter: string) => {
      const dataStr = `${data.id} ${data.firstName} ${data.lastName} ${data.city} ${data.address.streetNumber}`.toLowerCase();
      return dataStr.includes(filter);
    };
  }

  ngAfterViewInit(): void {
   this.dataSource.paginator = this.paginator;
  }

  isAuthenticated() {
    return this.authService.isAuthenticated();
  }

  login() {
    this.route.navigate(['login']);
  }

  logout() {
  this.authService.logout();
  this.route.navigate(['login']);
  }
}
