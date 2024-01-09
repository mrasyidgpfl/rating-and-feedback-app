import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class ViewReportCustomerService {

  ROOT_URL = 'https://ecomratingfeedback.azurewebsites.net/api/';
  tableValuesFunctionName = 'GetCustomerDataExceptLabel';
  badgesLabelsFunctionName = 'GetBadgeFeedbackCustomer';

  constructor(private http: HttpClient) { }

  getTableValues(idCustomer: string) {
    return this.http.get<string[]>(this.ROOT_URL + this.tableValuesFunctionName + '?IdCust=' + idCustomer);
  }

  getBadgesAndLabels(idCustomer: string) {
    return this.http.get<any>(this.ROOT_URL + this.badgesLabelsFunctionName + '?IdCust=' + idCustomer);
  }

}
