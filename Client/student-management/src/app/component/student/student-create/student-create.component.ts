import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {StudentService} from "../../../services/student.service";
import {AddressService} from "../../../services/address.service";
import {Address} from "../../../models/student.model";


@Component({
  selector: 'app-student-create',
  templateUrl: './student-create.component.html',
  styleUrls: ['./student-create.component.scss']
})
export class StudentCreateComponent implements OnInit{
  studentForm!: FormGroup;
  id!: number;
  addresses: Address[] = [];
  constructor(private fb: FormBuilder, private studentService: StudentService, private addressService: AddressService, private router: Router) {
  }
  ngOnInit(): void {
    this.studentForm = this.fb.group({
      firstName : ['', [Validators.required]],
      lastName : ['', [Validators.required]],
      city: ['', [Validators.required]],
      addressId:['',[Validators.required]],
      address: ['']
    })
    this.loadAddresses();
  }

  loadAddresses(): void{
    this.addressService.getAddresses().subscribe(data =>{
      this.addresses = data;
    });
  }
  onSubmit(): void {
    if(this.studentForm?.valid){
      const studentData = {
        ...this.studentForm.value,
        addressId: this.studentForm.get('addressId')?.value
      };
      this.studentService.createStudent(this.id, studentData).subscribe(() =>
      {
        this.router.navigate(['']);
      });
    }
  }


  cancel() {
    this.router.navigate(['']);
  }

}
