import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PatientModel } from '../../models/patient.model';
import { DoctorModel } from '../../models/doctor.model';
import { HttpService } from '../../services/http.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SwalService } from '../../services/swal.service';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormValidateDirective } from 'form-validate-angular';
import { PatientPipe } from '../../pipe/patient.pipe';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-patient',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    FormValidateDirective,
    PatientPipe,
  ],
  templateUrl: './patients.component.html',
  styleUrl: './patients.component.css',
})
export class PatientsComponent implements OnInit {
  search: string = ''; // Arama için bir değişken tanımla

  patients: PatientModel[] = [];

  @ViewChild('addModalCloseBtn') addModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;
  @ViewChild('updateModalCloseBtn') updateModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;

  createModel: PatientModel = new PatientModel();
  updateModel: PatientModel = new PatientModel();

  trackDepartment(index: number, department: any): number {
    return department.id;
  }

  constructor(private http: HttpService, private swal: SwalService) {}

  ngOnInit(): void {
    this.getAll();
    // this.swal.callToast('deneme', 'success');
    // this.swal.callSwal('title', 'text', () => {
    //   alert('Delete is successful');
    // });
  }

  getAll() {
    this.http.post<DoctorModel[]>(
      'Patients/GetAll',
      {},
      (res) => {
        console.log('Response:', res);
        this.patients = res.data;
      },
      (err: HttpErrorResponse) => {
        console.error(err);
      }
    );
  }

  add(form: NgForm) {
    if (form.valid) {
      this.http.post<string>(
        'Patients/Create',
        this.createModel,
        (res) => {
          console.log(res);
          this.swal.callToast(res.data, 'success');
          this.getAll();
          this.addModalCloseBtn?.nativeElement.click();
          this.createModel = new PatientModel();
        },
        (err: HttpErrorResponse) => {
          console.error(err);
        }
      );
    }
  }

  delete(id: string, fullName: string) {
    this.swal.callSwal(
      'Delete patient?',
      `You want to delete ${fullName}?`,
      () => {
        this.http.post<string>(
          'Patients/DeleteById',
          { id: id },
          (res) => {
            this.swal.callToast(res.data, 'info');
            this.getAll();
          },
          (err) => {
            console.error('Error deleting doctor:', err);
            this.swal.callToast('Error deleting doctor', 'error');
          }
        );
      }
    );
  }

  get(data: PatientModel) {
    this.updateModel = { ...data };
  }

  update(updateform: NgForm) {
    if (updateform.valid) {
      this.http.post<string>(
        'Patients/Update',
        this.updateModel,
        (res) => {
          console.log(res);
          this.swal.callToast(res.data, 'success');
          this.getAll();
          this.updateModalCloseBtn?.nativeElement.click();
        },
        (err: HttpErrorResponse) => {
          console.error(err);
        }
      );
    }
  }
}
