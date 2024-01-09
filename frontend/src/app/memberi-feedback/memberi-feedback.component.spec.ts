import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberiFeedbackComponent } from './memberi-feedback.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

xdescribe('MemberiFeedbackComponent', () => {
  let component: MemberiFeedbackComponent;
  let fixture: ComponentFixture<MemberiFeedbackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,        // tambahan
        BrowserAnimationsModule, // tambahan
        RouterTestingModule,
      ],
      declarations: [
        MemberiFeedbackComponent,
      ],
      schemas: [NO_ERRORS_SCHEMA,],
      providers: [],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MemberiFeedbackComponent);
    component = fixture.componentInstance;
    component.kembalian = {
      XD: ['123,123,123', '123,123,123'],
      EE: ['123,123,123', '123,123,123']
    };
    component.kembalianKey = Object.keys(component.kembalian);
    component.id = 'P1';
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have getAssignmentName that changes name and ID', () => {
    component.getAssignment();
  });

  it('should have initiatePeriodIndex that changes periodIndexes', () => {
    component.initiatePeriodIndex('1');
    expect(component.initialPeriodIndex).toEqual(1);
    expect(component.currentPeriodIndex).toEqual(1);
  });

  it('should have getRatingID that changes ratingID', () => {
    component.getRatingID();
  });

  xit('should reset values when resetValues is called', () => {
    component.resetValues();
    expect(component.rating).toEqual(0);
    expect(component.feedback).toEqual(null);
    expect(component.badgepicked).toEqual(null);
    expect(component.comment).toEqual(null);
  });

  xit('should increment the period index when incrementPersonIndex' +
    ' and person index is more than person count', () => {
      component.currentPersonIndex = 1;
      component.currentPeriodIndex = 0;
      component.incrementPersonIndex();
      expect(component.currentPeriodIndex).toEqual(1);
      expect(component.currentPersonIndex).toEqual(0);
    });

  xit('should increment the person index when incrementPersonIndex is called', () => {
    component.currentPersonIndex = 0;
    component.incrementPersonIndex();
    expect(component.currentPersonIndex).toEqual(1);
  });
});
