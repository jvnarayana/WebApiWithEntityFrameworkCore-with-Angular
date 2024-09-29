import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student.service";


@Component({
  selector: 'app-student-delete',
  templateUrl: './student-delete.component.html',
  styleUrls: ['./student-delete.component.scss']
})
export class StudentDeleteComponent implements  OnInit{
  studentId!: number;
  constructor( private studentService: StudentService, private router: Router, private route: ActivatedRoute) {
  }
ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id){
      this.studentId = +id;
    }
}
  deleteStudent(): void {
    this.studentService.deleteStudent(this.studentId).subscribe(() => {
      this.router.navigate(['']);
    });
  }

  cancel(): void {
    this.router.navigate(['']);
  }
}
