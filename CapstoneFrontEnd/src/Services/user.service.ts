import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5178/api/Auth'; // Update with your actual API URL
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  constructor(private http: HttpClient) {}

  login(credentials: any): Observable<any> {
    alert("login");
    console.log(credentials)
    return this.http.post(`${this.apiUrl}/Login`, credentials);
  }

  register(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData).pipe(
      catchError((error) => {
        console.error('Registration error:', error);
        return throwError(error);
      })
    );
}
getToken() {
  return localStorage.getItem('token');
}

getRole() {
  return localStorage.getItem('role');
}

isLoggedIn() {
  const token = this.getToken();
  return token !== null;
}
}