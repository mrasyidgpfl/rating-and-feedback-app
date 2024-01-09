import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberiRatingComponent } from './giv-rat.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { AppRoutingModule } from '../app-routing.module';

describe('MemberiRatingComponent', () => {
  let component: MemberiRatingComponent;
  let fixture: ComponentFixture<MemberiRatingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MemberiRatingComponent ],
      schemas: [ NO_ERRORS_SCHEMA, ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MemberiRatingComponent);
    component = fixture.componentInstance;
    component.id = "P1";
    fixture.detectChanges();
  });

  // Untuk keperluan testing detailpage ini dikomen
  // TODO : Hapus komen ini dan di bawah ini
  /*it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have getAssignmentName that changes name and ID', () => {
    component.getAssignmentName();
    expect(component).toThrow();
  });

  it('should have getRatingID that changes ratingID', () => {
    component.getRatingID();
    expect(component).toThrow();
  });*/

});
