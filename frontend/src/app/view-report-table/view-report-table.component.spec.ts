import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewReportTableComponent } from './view-report-table.component';

xdescribe('ViewReportTableComponent', () => {
  let component: ViewReportTableComponent;
  let fixture: ComponentFixture<ViewReportTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ViewReportTableComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewReportTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  xit('should create', () => {
    expect(component).toBeTruthy();
  });

});
