import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HttpService } from '../../services/http.service';
import { DoctorModel } from '../../models/doctor.model';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { departments } from '../../constant';
import { FormValidateDirective } from 'form-validate-angular';
import { FormsModule, NgForm } from '@angular/forms';
import { SwalService } from '../../services/swal.service';
import { pipe } from 'rxjs';
import { DoctorPipe } from '../../pipe/doctor.pipe';

@Component({
  selector: 'app-doctors',
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    FormValidateDirective,
    DoctorPipe,
  ],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.css',
})
export class DoctorsComponent implements OnInit {
  search: string = ''; // Arama için bir değişken tanımla

  doctors: DoctorModel[] = [];
  departments = departments;

  @ViewChild('addModalCloseBtn') addModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;
  @ViewChild('updateModalCloseBtn') updateModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;

  createModel: DoctorModel = new DoctorModel();
  updateModel: DoctorModel = new DoctorModel();

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
      'Doctors/GetAll',
      {},
      (res) => {
        console.log('Response:', res);
        this.doctors = res.data;
      },
      (err: HttpErrorResponse) => {
        console.error(err);
      }
    );
  }

  add(form: NgForm) {
    if (form.valid) {
      this.http.post<string>(
        'Doctors/Create',
        this.createModel,
        (res) => {
          console.log(res);
          this.swal.callToast(res.data, 'success');
          this.getAll();
          this.addModalCloseBtn?.nativeElement.click();
          this.createModel = new DoctorModel();
        },
        (err: HttpErrorResponse) => {
          console.error(err);
        }
      );
    }
  }

  delete(id: string, fullName: string) {
    this.swal.callSwal(
      'Delete doctor',
      `You want to delete ${fullName}?`,
      () => {
        this.http.post<string>(
          'Doctors/DeleteById',
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

  get(data: DoctorModel) {
    this.updateModel = { ...data };
    this.updateModel.departmentValue = data.department.value;
  }

  update(updateform: NgForm) {
    if (updateform.valid) {
      this.http.post<string>(
        'Doctors/Update',
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
