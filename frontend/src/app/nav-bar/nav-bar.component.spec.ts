import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavBarComponent } from './nav-bar.component';
import { DebugElement } from '@angular/core';
import { NavbarModule } from 'angular-bootstrap-md';

describe('NavBarComponent', () => {
  let component: NavBarComponent;
  let fixture: ComponentFixture<NavBarComponent>;
  let de: DebugElement;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [NavbarModule],
      declarations: [NavBarComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavBarComponent);
    component = fixture.componentInstance;
    de = fixture.debugElement;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  xit('should have a navbar', () => {
    const compiled = de.nativeElement;
    expect(compiled.innerHTML).toContain('mdb-navbar');
  });

  it('should show `Peer Review System` brand on the navbar', () => {
    const compiled = de.nativeElement;
    expect(compiled.querySelector('mdb-navbar-brand').textContent).toContain('Peer Review System');
  });

  it('should have a `home` button on the navbar', () => {
    const compiled = de.nativeElement;
    expect(compiled.innerHTML).toContain('Home');
  });

  it('should have a `profile` icon on the right side of the navbar', () => {
    const compiled = de.nativeElement;
    expect(compiled.innerHTML).toContain('Profile');
  });
});
