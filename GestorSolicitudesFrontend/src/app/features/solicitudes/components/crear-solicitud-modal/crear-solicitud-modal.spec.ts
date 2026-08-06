import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrearSolicitudModal } from './crear-solicitud-modal';

describe('CrearSolicitudModal', () => {
  let component: CrearSolicitudModal;
  let fixture: ComponentFixture<CrearSolicitudModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CrearSolicitudModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CrearSolicitudModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
