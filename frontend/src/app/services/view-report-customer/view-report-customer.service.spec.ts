import { TestBed, getTestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { ViewReportCustomerService } from './view-report-customer.service';

describe('ViewReportCustomerService', () => {
  let injector: TestBed;
  let service: ViewReportCustomerService;
  let httpMock: HttpTestingController;
  const idCustomer = 'C1';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ViewReportCustomerService]
    });
    injector = getTestBed();
    service = injector.get(ViewReportCustomerService);
    httpMock = injector.get(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getTableValues should return an observable', () => {

    const functionName = 'GetCustomerDataExceptLabel';
    const dummyValues = [
      'xd',
      'yd',
    ];

    service.getTableValues(idCustomer).subscribe(res => {
      expect(res).toEqual(dummyValues);
    });

    const mockReq = httpMock.expectOne(service.ROOT_URL + functionName + '?IdCust=' + idCustomer);
    expect(mockReq.request.method).toBe('GET');
    mockReq.flush(dummyValues);
  });

  it('getBadgesAndLabels should return an observable', () => {

    const functionName2 = 'GetBadgeFeedbackCustomer';
    const dummyValues = {'period 3': 'aserm'};

    service.getBadgesAndLabels(idCustomer).subscribe(res => {
      expect(res).toBe(dummyValues);
    });

    const mockReq = httpMock.expectOne(service.ROOT_URL + functionName2 + '?IdCust=' + idCustomer);
    expect(mockReq.request.method).toBe('GET');
    mockReq.flush(dummyValues);

  });

});
