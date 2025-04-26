import { CommonModule } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { FormValidateDirective } from 'form-validate-angular';
import { UserPipe } from '../../pipe/user.pipe';
import { UserModel } from '../../models/user.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { HttpErrorResponse } from '@angular/common/http';
import { RoleModel } from '../../models/role.model';

@Component({
  selector: 'app-user',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    FormValidateDirective,
    UserPipe,
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent {
  search: string = ''; // Arama için bir değişken tanımla
  trackByFn(index: number, item: any): any {
    return item; // Burada öğenin benzersiz bir özelliğini döndürebilirsiniz (örneğin ID)
  }
  users: UserModel[] = [];
  roles: RoleModel[] = [];

  @ViewChild('addModalCloseBtn') addModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;
  @ViewChild('updateModalCloseBtn') updateModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;

  createModel: UserModel = new UserModel();
  updateModel: UserModel = new UserModel();

  trackDepartment(index: number, department: any): number {
    return department.id;
  }

  constructor(private http: HttpService, private swal: SwalService) {}

  ngOnInit(): void {
    this.getAll();
    this.getAllRoles();
    // this.swal.callToast('deneme', 'success');
    // this.swal.callSwal('title', 'text', () => {
    //   alert('Delete is successful');
    // });
  }

  getAll() {
    this.http.post<UserModel[]>(
      'Users/GetAll',
      {},
      (res) => {
        console.log('Response:', res);
        this.users = res.data;
      },
      (err: HttpErrorResponse) => {
        console.error(err);
      }
    );
  }

  getAllRoles() {
    this.http.post<RoleModel[]>(
      'Users/GetAllRoles',
      {},
      (res) => {
        console.log(res); // Her kullanıcının roleNames dizisini kontrol et

        this.roles = res.data;
      },
      (err: HttpErrorResponse) => {
        console.error(err);
      }
    );
  }

  add(form: NgForm) {
    if (form.valid) {
      // fullName dinamik olarak firstName ve lastName'e göre oluşturuluyor
      this.createModel.fullName = `${this.createModel.firstName} ${this.createModel.lastName}`;
      this.http.post<string>(
        'Users/Create',
        this.createModel,
        (res) => {
          console.log(res);
          this.swal.callToast(res.data, 'success');
          this.getAll();
          this.addModalCloseBtn?.nativeElement.click();
          this.createModel = new UserModel();
        },
        (err: HttpErrorResponse) => {
          console.error(err);
        }
      );
    }
  }

  delete(id: string, fullName: string) {
    this.swal.callSwal(
      'Delete user?',
      `You want to delete ${fullName}?`,
      () => {
        this.http.post<string>(
          'Users/DeleteById',
          { id: id },
          (res) => {
            this.swal.callToast(res.data, 'info');
            this.getAll();
          },
          (err) => {
            console.error('Error deleting user:', err);
            this.swal.callToast('Error deleting user', 'error');
          }
        );
      }
    );
  }

  get(data: UserModel) {
    this.updateModel = { ...data };
    console.log(this.updateModel);
  }

  update(updateform: NgForm) {
    if (updateform.valid) {
      this.http.post<string>(
        'Users/Update',
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
