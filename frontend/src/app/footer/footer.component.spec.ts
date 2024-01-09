import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterComponent } from './footer.component';
import { DebugElement } from '@angular/core';

describe('FooterComponent', () => {
  let component: FooterComponent;
  let fixture: ComponentFixture<FooterComponent>;
  let de: DebugElement;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FooterComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FooterComponent);
    component = fixture.componentInstance;
    de = fixture.debugElement;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show the footer', () => {
    const compiled = de.nativeElement;
    expect(compiled.innerHTML).toContain('footer');
  });

  it('should show `copyright by PPLC5` text', () => {
    const compiled = de.nativeElement;
    expect(compiled.querySelector('.footer-copyright').textContent).toContain('© 2019 Copyright: PPL-C5');
  });
});
