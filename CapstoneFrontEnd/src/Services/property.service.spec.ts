import { TestBed } from '@angular/core/testing';

import { PropertyService } from '../Services/property.service';

describe('PropertyService', () => {
  let service: PropertyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PropertyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
