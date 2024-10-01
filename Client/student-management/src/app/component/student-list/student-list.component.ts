import { Component, OnInit } from '@angular/core';
import {StudentService} from "../../services/student.service";
import { Student} from "../../models/student.model";

@Component({
  selector: 'app-student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.scss']
})
export class StudentListComponent {
students: Student[] = [];
constructor(private  studentService: StudentService) {

}
ngOnInit(): void
{
  this.loadStudents();
}
  loadStudents(){
    this.studentService.getStudents().subscribe(
      (data) => this.students = data, (error) => console.error('error while retriveing the student list', error)
    );
  }

}
