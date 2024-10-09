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
  ) {
    this.studentForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      city: ['', [Validators.required]],
      address: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.studentId = +this.route.snapshot.paramMap.get('id')!;
    this.loadStudentData();
  }
  loadStudentData(): void{
    this.studentService.getStudentByID(this.studentId).subscribe((student) => {
      const fullAddress = `${student.address.streetNumber || ''} ${student.address.city || ''} ${student.address.state || ''} ${student.address.zipcode || ''}`;

      this.studentForm.patchValue({
        firstName: student.firstName,
        lastName: student.lastName,
        city: student.city,
        address: fullAddress.trim()
      });
    });
  }

  onSubmit(): void {
    if (this.studentForm.valid) {
      this.studentService.updateStudent(this.studentId, this.studentForm.value).subscribe(() => {
        this.router.navigate(['']);
      });
    }
  }

  cancel() {
    this.router.navigate(['']);
  }
}
