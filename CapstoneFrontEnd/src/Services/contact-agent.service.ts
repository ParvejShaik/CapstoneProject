import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface ContactAgent {
  id: number;
  fullName: string;
  email: string;
  phoneNumber: string;
  propertyName: string;
  locality: string;
  agent: AgentDetails;
}

interface AgentDetails {
  name: string;
  email: string;
  contact: string;
}

@Injectable({
  providedIn: 'root'
})
export class ContactAgentService {

  private baseUrl = 'http://localhost:5219/api/ContactAgent';

  constructor(private http: HttpClient) {}

  getContactAgents(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl);
  }

  contactAgent(contactData: any): Observable<any> {
    return this.http.post<any>(this.baseUrl, contactData);
  }

  getCustomersByLocality(locality: string): Observable<ContactAgent[]> {
    return this.http.get<ContactAgent[]>(`${this.baseUrl}/locality/${locality}`);
  }


  
  getContactAgentsByAgentEmail(Email: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetByAgentEmail?email=${Email}`);
  }
}
