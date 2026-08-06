import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleSolicitudModal } from './detalle-solicitud-modal';

describe('DetalleSolicitudModal', () => {
  let component: DetalleSolicitudModal;
  let fixture: ComponentFixture<DetalleSolicitudModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetalleSolicitudModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetalleSolicitudModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
