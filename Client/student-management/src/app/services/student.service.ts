import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import { Student} from "../models/student.model";

@Injectable({
  providedIn: 'root'
})
export class StudentService {
private  API_BASE_URL = 'http://localhost:5081/api/student'
  constructor(private httpClient: HttpClient) {

  }

  getStudents(): Observable<Student[]>{
  return this.httpClient.get<Student[]>(this.API_BASE_URL)
  }
  getStudentByID(id:number): Observable<Student>{
  return this.httpClient.get<Student>(`${this.API_BASE_URL}/${id}`)
  }
  createStudent(id:number, student:Student): Observable<Student>{
  return this.httpClient.post<Student>(this.API_BASE_URL, student);
  }

  updateStudent(id: number, student: Student): Observable<Student> {
    return this.httpClient.put<Student>(`${this.API_BASE_URL}/${id}`, student);
  }

  deleteStudent(id:number):Observable<void>{
  return this.httpClient.delete<void>(`${this.API_BASE_URL}/${id}`)
  }
}
