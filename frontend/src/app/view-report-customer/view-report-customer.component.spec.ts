import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewReportCustomerComponent } from './view-report-customer.component';

xdescribe('ViewReportCustomerComponent', () => {
  let component: ViewReportCustomerComponent;
  let fixture: ComponentFixture<ViewReportCustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewReportCustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewReportCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should something something something', () => {
    expect(component).toBeTruthy();
  });

});
