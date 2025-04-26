import { PatientModel } from './patient.model';

export class AppointmentModel {
  id: string = '';
  startDate: string = '';
  endDate: string = '';
  title: string = '';
  text: string = ''; //burada title text kullanmamızın sebebi fullName için title kullandık ve fullName takvimde görünmedi
  //bunun yerine biz de text kullandık
  patient: PatientModel = new PatientModel();
}
