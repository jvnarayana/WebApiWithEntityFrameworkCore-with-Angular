import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddressDeleteComponent } from './address-delete.component';

describe('AddressDeleteComponent', () => {
  let component: AddressDeleteComponent;
  let fixture: ComponentFixture<AddressDeleteComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddressDeleteComponent]
    });
    fixture = TestBed.createComponent(AddressDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
