import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignarUsuarioModal } from './asignar-usuario-modal';

describe('AsignarUsuarioModal', () => {
  let component: AsignarUsuarioModal;
  let fixture: ComponentFixture<AsignarUsuarioModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AsignarUsuarioModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AsignarUsuarioModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
