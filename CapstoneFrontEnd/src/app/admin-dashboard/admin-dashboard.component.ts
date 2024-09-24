import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AgentService } from 'src/Services/agent.service';
import { ContactAgent, ContactAgentService } from 'src/Services/contact-agent.service';
import { PropertyService } from 'src/Services/property.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  contactAgents: ContactAgent[] = [];
  agents: any[] = [];
  properties: any[] = [];
  customers: ContactAgent[] = [];
  localityFilter: string = '';
  propertyLocationFilter: string = '';

  agentForm: any = { 
    name: '', 
    mail: '', 
    phoneNumber: '', 
    locality: '', 
    password: '' 
  };

  propertyForm: any = { 
    title: '', 
    price: 0, 
    location: '', 
    size: 0, 
    numberOfBedrooms: 0, 
    status: '', 
    description: '', 
    image: '', // This will hold the Base64 string
    agent: {
      name: '',
      contact: '',
      email: ''
    }
  };

  constructor(
    private agentService: AgentService, 
    private propertyService: PropertyService, 
    private contactAgentService: ContactAgentService, 
    private router: Router 
  ) { }

  ngOnInit(): void {
    this.loadAgents();
    this.loadProperties();
    this.loadCustomers();
    this.getContactAgents();
  }
  
  logout() {
    
    localStorage.removeItem('authToken'); 
    sessionStorage.removeItem('userData'); 


    this.router.navigate(['/login'],{ replaceUrl: true }); 
  }

  isAddAgentModalOpen: boolean = false;
  
  isAddPropertyModalOpen: boolean = false;

  loadAgents() {
    this.agentService.getAllAgents().subscribe(data => this.agents = data);
  }

  filterAgents() {
    if (this.localityFilter) {
      this.agentService.getAgentsByLocality(this.localityFilter).subscribe(data => this.agents = data);
    } else {
      this.loadAgents();
    }
  }

  loadProperties() {
    this.propertyService.getAllProperties().subscribe(data => this.properties = data);
  }

  filterProperties() {
    if (this.propertyLocationFilter) {
      this.propertyService.getPropertiesByLocation(this.propertyLocationFilter).subscribe(data => this.properties = data);
    }  else {
      this.loadProperties();
    }
  }

  loadCustomers() {
    this.contactAgentService.getContactAgents().subscribe(data => {
      this.customers = data;
      console.log(this.customers); 
    }, error => {
      console.error('Error fetching customers:', error);
    });
  }

  filterCustomers() {
    if (this.localityFilter) {
      this.contactAgentService.getCustomersByLocality(this.localityFilter).subscribe(data => this.customers = data);
    } else {
      this.loadCustomers();
    }
  }

  deleteAgent(agent: any) {
    if (agent && agent.name) {
      const name = agent.name;
      if (confirm('Are you sure you want to delete this agent?')) {
        this.agentService.deleteAgent(name).subscribe(
          (response) => {
            console.log('Agent deleted successfully:', response);
            this.loadAgents();
          },
          (error) => {
            console.error('Error deleting agent:', error);
          }
        );
      }
    } else {
      console.error('Invalid agent');
    }
  }

  deleteProperty(property: any) {
    if (confirm('Are you sure you want to delete this property?')) {
      const title = property.title; 
      this.propertyService.deleteProperty(title).subscribe(() => {
        this.loadProperties();
      }, (error) => {
        console.error('Error deleting property:', error);
      });
    }
  }


  openAddPropertyModal() {
    this.isAddPropertyModalOpen = true; 
  }
  
  
  closeAddPropertyModal() {
    this.isAddPropertyModalOpen = false; 
    this.resetPropertyForm(); 
  }
  

  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.propertyForm.image = reader.result; 
      };
      reader.readAsDataURL(file);
    }
  }
  

  addProperty() {
    const formData = new FormData();
    

    if (this.propertyForm.image) {
        const imageFile = this.dataURLtoFile(this.propertyForm.image, 'property-image.jpg'); 
        formData.append('file', imageFile); 
    }
    
    // First, upload the image
    this.propertyService.uploadImage(formData).subscribe({
        next: (uploadResponse) => {
            // After successfully uploading, create the property
            const propertyData = {
                title: this.propertyForm.title,
                price: this.propertyForm.price,
                location: this.propertyForm.location,
                size: this.propertyForm.size,
                numberOfBedrooms: this.propertyForm.numberOfBedrooms,
                status: this.propertyForm.status,
                description: this.propertyForm.description,
                image: uploadResponse.fileName, // Assuming the backend returns the image file name
                agent: {
                    name: this.propertyForm.agent.name,
                    contact: this.propertyForm.agent.contact,
                    email: this.propertyForm.agent.email
                }
            };
    
            this.propertyService.createProperty(propertyData).subscribe({
                next: (response) => {
                    console.log('Property added successfully:', response);
                    this.loadProperties(); // Reload properties
                    this.closeAddPropertyModal(); // Close the modal
                },
                error: (error) => {
                    console.error('Error adding property:', error);
                }
            });
        },
        error: (error) => {
            console.error('Error uploading image:', error);
        }
    });
  }
  
  // Reset the property form fields
  resetPropertyForm() {
    this.propertyForm = {
        title: '',
        price: null,
        location: '',
        size: null,
        numberOfBedrooms: null,
        status: '',
        description: '',
        image: '',
        agent: {
            name: '',
            contact: '',
            email: ''
        }
    };
  }
  


dataURLtoFile(dataUrl: string, fileName: string): File {
  const arr = dataUrl.split(',');
  const mimeMatch = arr[0].match(/:(.*?);/);
  
  if (!mimeMatch) {
      throw new Error('Invalid data URL');
  }

  const mime = mimeMatch[1];
  const bstr = atob(arr[1]);
  let n = bstr.length;
  const u8arr = new Uint8Array(n);

  while (n--) {
      u8arr[n] = bstr.charCodeAt(n);
  }

  return new File([u8arr], fileName, { type: mime });
}


openAddAgentModal() {
  this.isAddAgentModalOpen = true; 
}


closeAddAgentModal() {
  this.isAddAgentModalOpen = false; 
  this.resetAgentForm(); 
}


addAgent(): void {
  this.agentService.createAgent(this.agentForm).subscribe({
    next: (response: any) => {  
      console.log('Agent Added:', response);
      this.loadAgents(); 
      this.closeAddAgentModal(); 
      this.resetAgentForm(); 
    },
    error: (error: any) => {  
      console.error('Error adding agent:', error);

    }
  });
}



resetAgentForm() {
  this.agentForm = {
    name: '',
    mail: '',
    phoneNumber: '',
    locality: '',
    password: ''
  };
}
 
getContactAgents(): void {
  this.contactAgentService.getContactAgents().subscribe(contactAgents => {
    this.contactAgents = contactAgents;
  });
}
}
