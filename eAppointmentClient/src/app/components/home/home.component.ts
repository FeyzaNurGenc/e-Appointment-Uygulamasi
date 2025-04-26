import {
  ChangeDetectorRef,
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { DepartmentModel, DoctorModel } from '../../models/doctor.model';
import { departments } from '../../constant';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule, DatePipe, NgIf } from '@angular/common';
import { DxSchedulerModule } from 'devextreme-angular';
import { HttpService } from '../../services/http.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Appointment } from 'devextreme/ui/scheduler';
import { AppointmentModel } from '../../models/appointment.model';
import { CreateAppointmentModel } from '../../models/create-appointment.model';
import { FormValidateDirective } from 'form-validate-angular';
import { PatientModel } from '../../models/patient.model';
import { SwalService } from '../../services/swal.service';
import e from 'cors';

declare const $: any; // jQuery kullanıyorsanız bunu tanımlayın

@Component({
  selector: 'app-home',
  imports: [
    FormsModule,
    CommonModule,
    DxSchedulerModule,
    FormValidateDirective,
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [DatePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class HomeComponent implements OnInit {
  departments = departments;
  doctors: DoctorModel[] = [];

  selectedDate: Date = new Date(); // Varsayılan olarak bugünün tarihi

  selectedDepartmentValue: number = 0;
  selectedDoctorId: string = '';

  @ViewChild('addModalCloseBtn') addModalCloseBtn:
    | ElementRef<HTMLButtonElement>
    | undefined;

  // @ViewChild('addModal') addModal!: ElementRef;

  // appointments: any = [
  //   {
  //     startDate: new Date('2024-04-11 09:00'),
  //     endDate: new Date('2024-04-11 09:30'),
  //     Title: 'Feyza Genç',
  //   },
  //   {
  //     startDate: new Date('2024-04-11 09:30'),
  //     endDate: new Date('2024-04-11 10:00'),
  //     Title: 'Esra Genç',
  //   },
  //   {
  //     startDate: new Date('2024-04-11 11:30'),
  //     endDate: new Date('2024-04-11 12:00'),
  //     Title: 'Zehra Genç',
  //   },
  // ];

  appointments: AppointmentModel[] = [
    // {
    //   text: 'Feyza Genç', // Title yerine text kullanılmalı
    //   startDate: new Date(2025, 1, 17, 9, 0), // Aylar 0'dan başlar! Şubat = 1
    //   endDate: new Date(2025, 1, 17, 9, 30),
    // },
    // {
    //   text: 'Esra Genç',
    //   startDate: new Date(2025, 1, 17, 9, 30),
    //   endDate: new Date(2025, 1, 17, 10, 0),
    // },
    // {
    //   text: 'Zehra Genç',
    //   startDate: new Date(2025, 1, 17, 11, 30),
    //   endDate: new Date(2025, 1, 17, 12, 0),
    // },
  ];

  createModel: CreateAppointmentModel = new CreateAppointmentModel();

  constructor(
    private http: HttpService,
    private date: DatePipe,
    private swal: SwalService,
    private cdr: ChangeDetectorRef
  ) {}
  ngOnInit() {
    this.http.getAppointments().subscribe({
      next: (response: { data: AppointmentModel[] }) => {
        this.appointments = response.data;
        console.log('Randevular:', this.appointments); // Burada randevuların doğru geldiğini kontrol edin
        this.cdr.detectChanges();
      },
      error: (err: HttpErrorResponse) => {
        console.error('Hata oluştu:', err);
      },
    });
  }

  getAllDoctor() {
    this.selectedDoctorId = '';
    if (this.selectedDepartmentValue > 0) {
      this.http.post<DoctorModel[]>(
        'appointments/GetAllDoctorByDepartment',
        {
          DepartmentValue: +this.selectedDepartmentValue,
        },
        (res) => {
          this.doctors = res.data;
        },
        (err: HttpErrorResponse) => {
          console.error(err);
        }
      );
    }
  }

  getAllAppointments() {
    if (this.selectedDoctorId) {
      this.http.post<AppointmentModel[]>( //AppointmentModelde text kullandıktan sonra aşağıda yorum satırı eklediğim kod yazıldı
        'appointments/GetAllByDoctorId',
        {
          doctorId: this.selectedDoctorId,
        },
        (res) => {
          console.log(res.data);
          this.appointments = res.data;
          //aşağıdaki kod fullName'nin text olması için yazıldı
          this.appointments = res.data.map((item: any) => ({
            ...item,
            text: item.title,
          }));
        },
        (err: HttpErrorResponse) => {
          console.error(err);
        }
      );
    }
  }

  onAppointmentFormOpening(event: any) {
    event.cancel = true; // Randevu pencere açılmasını engelle
    //  event.preventDefault(); // Ekstra önleme
    // Eğer startDate ve endDate string ise, önce Date objesine çevir
    const startDate = new Date(event.appointmentData.startDate);
    const endDate = new Date(event.appointmentData.endDate);

    this.createModel.startDate =
      this.date.transform(
        event.appointmentData.startDate,
        'dd.MM.yyyy HH:mm'
      ) ?? '';

    this.createModel.endDate =
      this.date.transform(event.appointmentData.endDate, 'dd.MM.yyyy HH:mm') ??
      '';
    this.createModel.doctorId = this.selectedDoctorId;

    this.createModel.fullName = event.appointmentData.title;

    $('#addModal').modal('show');
  }

  getPatient() {
    this.http.post<PatientModel>(
      'Appointments/GetPatientByIdentityNumber',
      { identityNumber: this.createModel.identityNumber },
      (res) => {
        if (res.data == null) {
          this.createModel.patientId = null;
          this.createModel.firstName = '';
          this.createModel.lastName = '';
          this.createModel.city = '';
          this.createModel.town = '';
          this.createModel.fullAddress = '';
          return;
        }
        this.createModel.patientId = res.data.id;
        this.createModel.firstName = res.data.firstName;
        this.createModel.lastName = res.data.lastName;
        this.createModel.city = res.data.city;
        this.createModel.town = res.data.town;
        this.createModel.fullAddress = res.data.fullAddress;
      },
      (err: HttpErrorResponse) => {
        console.error(err);
      }
    );
  }

  create(form: NgForm) {
    if (form.valid) {
      this.http.post<string>(
        'Appointments/Create',
        this.createModel,
        (res) => {
          this.swal.callToast(res.data);
          this.addModalCloseBtn?.nativeElement.click();
          this.createModel = new CreateAppointmentModel();
          this.getAllAppointments();
        },
        (err: HttpErrorResponse) => {
          console.error(err);
        }
      );
    }
  }

  //Silmeden önce
  onAppointmentDeleted(event: any) {
    event.cancel = true;
  }
  //Silmeden sonra
  onAppointmentDeleting(event: any) {
    event.cancel = true;
    console.log(event);
    this.swal.callSwal(
      'Delete appointment?',
      ` You want to delete ${event.appointmentData.patient.fullName}  appointment?`,
      () => {
        this.http.post<string>(
          'Appointments/DeleteById',
          { id: event.appointmentData.id },
          (res) => {
            this.swal.callToast(res.data, 'info');
            this.getAllAppointments();
          },
          (err: HttpErrorResponse) => {
            console.error(err);
          }
        );
      }
    );
  }

  onAppointmentUpdating(event: any) {
    event.cancel = true;
    const data = {
      id: event.oldData.id,
      startDate: new Date(event.newData.startDate).toISOString(), // ISO formatına çevir
      endDate: new Date(event.newData.endDate).toISOString(),
    };
    this.http.post<string>(
      'Appointments/Update',
      data,
      (res) => {
        this.swal.callToast(res.data);
        this.getAllAppointments();
      },
      (err: HttpErrorResponse) => {
        console.error(err);
      }
    );
  }
}
