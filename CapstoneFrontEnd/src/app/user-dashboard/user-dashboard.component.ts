import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ContactAgentService } from 'src/Services/contact-agent.service';
import { PropertyService } from 'src/Services/property.service';

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.css']
})
export class UserDashboardComponent implements OnInit {
  properties: any[] = [];
  propertyLocationFilter: string = '';
  contactData = {
    fullName: '',
    email: '',
    phoneNumber: '',
    propertyName: '',
    locality: '',
    agent: {
      name: '',
      email: '',
      contact: ''
    }
  };
  isModalOpen = false; 
  errorMessage: string | null = null;

  constructor(private propertyService: PropertyService, private contactAgentService: ContactAgentService,private router : Router) {}
  logout() {
    
    localStorage.removeItem('authToken'); 
   
    this.router.navigate(['/login'],{ replaceUrl: true }); 
  }
  ngOnInit() {
    this.loadProperties();
  }

  loadProperties() {
    this.propertyService.getAllProperties().subscribe(
      (response: any) => {
        this.properties = response;
      },
      (error: any) => {
        console.error('Error fetching properties', error);
      }
    );
  }
  filterProperties() {
    if (this.propertyLocationFilter) {
      this.propertyService.getPropertiesByLocation(this.propertyLocationFilter).subscribe(data => this.properties = data);
    }  else {
      this.loadProperties();
    }
  }
  buyProperty(property: any) {
    this.contactData.propertyName = property.title;
    this.contactData.locality = property.location;
    this.contactData.agent.name = property.agent.name;
    this.contactData.agent.email = property.agent.email;
    this.contactData.agent.contact = property.agent.contact;

    this.isModalOpen = true; 
  }

  closeModal() {
    this.isModalOpen = false; 
  }

  submitContactForm() {
    this.contactAgentService.contactAgent(this.contactData).subscribe(
      (response: any) => {
        alert('Our agent will contact you soon');
        this.closeModal();
        this.resetContactForm();
      },
      (error: any) => {
        console.error('Error contacting agent', error);
      }
    );
  }

  resetContactForm() {
    this.contactData = {
      fullName: '',
      email: '',
      phoneNumber: '',
      propertyName: '',
      locality: '',
      agent: {
        name: '',
        email: '',
        contact: ''
      }
    };
  }
}
