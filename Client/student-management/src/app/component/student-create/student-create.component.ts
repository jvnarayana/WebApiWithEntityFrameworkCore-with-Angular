import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {StudentService} from "../../services/student.service";


@Component({
  selector: 'app-student-create',
  templateUrl: './student-create.component.html',
  styleUrls: ['./student-create.component.scss']
})
export class StudentCreateComponent implements OnInit{
  studentForm!: FormGroup;
  id!: number;
  constructor(private fb: FormBuilder, private studentService: StudentService, private router: Router) {
  }
  ngOnInit(): void {
    this.studentForm = this.fb.group({
      firstName : ['', [Validators.required]],
      lastName : ['', [Validators.required]],
      city: ['', [Validators.required]],
      address: ['', [Validators.required]]
    })
  }

  onSubmit(): void {
    if(this.studentForm?.valid){
      this.studentService.createStudent(this.id, this.studentForm.value).subscribe(() =>
      {
        this.router.navigate(['/students']);
      });
    }
  }


}
