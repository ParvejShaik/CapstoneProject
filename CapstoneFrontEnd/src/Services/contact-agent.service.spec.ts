import { TestBed } from '@angular/core/testing';

import { ContactAgentService } from './contact-agent.service';

describe('ContactAgentService', () => {
  let service: ContactAgentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContactAgentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
