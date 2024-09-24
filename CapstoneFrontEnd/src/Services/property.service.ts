import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class PropertyService {

  private baseUrl = 'http://localhost:5261/api/Properties';
  private imageBaseUrl = 'http://localhost:5261/images/'; 

  constructor(private http: HttpClient) { }

  getAllProperties(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}`).pipe(map(properties => 
      properties.map(property => ({
        ...property,
        image: property.image ? `${this.imageBaseUrl}${property.image}` : null // Prepend image URL
      }))
    ));
  }

  getPropertiesByLocation(location: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/by-location/${location}`);
  }


  getPropertyByTitle(title: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${title}`);
  }

  createProperty(property: any): Observable<any> {
    return this.http.post<any>(this.baseUrl, property, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  deleteProperty(title: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/${title}`);
  }
  uploadImage(formData: FormData): Observable<any> {
    return this.http.post<any>('http://localhost:5261/api/Properties/upload', formData);
}


}
