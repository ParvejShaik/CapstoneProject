import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AgentService {

  private baseUrl = 'http://localhost:5119/api/Agent';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/login`, { mail: email, password });
  }
  getAllAgents(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}`);
  }

  getAgentByEmail(email: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/email/${email}`);
  }
  getAgentsByLocality(locality: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/locality/${locality}`);
  }
  deleteAgent(Name: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/${Name}`);
  }
  
  createAgent(agent: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}`, agent);
  }
}
