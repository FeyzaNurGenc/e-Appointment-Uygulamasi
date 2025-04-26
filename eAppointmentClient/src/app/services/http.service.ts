import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ResultModel } from '../models/result.model';
import { api } from '../constant';
import { ErrorService } from './error.service';
import { Observable } from 'rxjs/internal/Observable';
import { AppointmentModel } from '../models/appointment.model';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  token: string = '';
  constructor(private http: HttpClient, private error: ErrorService) {
    if (localStorage.getItem('token')) {
      this.token = localStorage.getItem('token') ?? '';
    }
  }

  // API'den randevuları almak için get fonksiyonu
  getAppointments(): Observable<{ data: AppointmentModel[] }> {
    return this.http.get<{ data: AppointmentModel[] }>('/api/appointments'); // Yanıtı { data: AppointmentModel[] } şeklinde belirtiyoruz
  }

  post<T>(
    apiUrl: string,
    body: any,
    callBack: (res: ResultModel<T>) => void,
    errCallBack: (res: HttpErrorResponse) => void
  ) {
    // const url = `${api}/${apiUrl}`;
    // const headers = new HttpHeaders({
    //   'Content-Type': 'application/json',
    //   Authorization: ' Bearer  ' + this.token,
    // });

    this.http
      .post<ResultModel<T>>(`${api}/${apiUrl}`, body, {
        headers: { Authorization: ' Bearer  ' + this.token },
      })
      .subscribe({
        next: (res) => {
          console.log('API Yanıtı:', res); // API yanıtını logla
          if (res.data) {
            callBack(res); // Veriyi işleyip geri gönder
          } else {
            console.error('Veri bulunamadı:', res);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.error.errorHandler(err);
          if (errCallBack !== undefined) {
            errCallBack(err);
          }
        },
      });
  }
}
