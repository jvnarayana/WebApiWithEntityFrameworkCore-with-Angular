import { Component, OnInit } from '@angular/core';
import {FormGroup, FormBuilder, Validators} from "@angular/forms";
import {StudentService} from "../../services/student.service";
import {ActivatedRoute, Router} from "@angular/router";


@Component({
  selector: 'app-student-edit',
  templateUrl: './student-edit.component.html',
  styleUrls: ['./student-edit.component.scss']
})
export class StudentEditComponent implements OnInit {
  studentForm!: FormGroup;
  studentId!: number;

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.studentId = +this.route.snapshot.paramMap.get('id')!;
    this.studentService.getStudentByID(this.studentId).subscribe((student) => {
      this.studentForm = this.fb.group({
        firstName: [student.firstName, [Validators.required]],
        lastName: [student.lastName, [Validators.required]],
        city: [student.city, [Validators.required]],
        address: [student.address, [Validators.required]]
      });
    });
  }

  onSubmit(): void {
    if (this.studentForm.valid) {
      this.studentService.updateStudent(this.studentId, this.studentForm.value).subscribe(() => {
        this.router.navigate(['/students']);
      });
    }
  }

}
