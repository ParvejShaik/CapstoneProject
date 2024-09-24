import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AgentService } from 'src/Services/agent.service';
import { ContactAgent, ContactAgentService } from 'src/Services/contact-agent.service';

@Component({
  selector: 'app-agent-dashboard',
  templateUrl: './agent-dashboard.component.html',
  styleUrls: ['./agent-dashboard.component.css']
})
export class AgentDashboardComponent implements OnInit {
  agentDetails: any;
  customers: any [] = [];
  email: string = '';

  constructor(private agentService: AgentService, private contactAgentService: ContactAgentService, private router: Router) {}

  ngOnInit(): void {
    this.loadAgentDetails();
  }
  logout() {
    
    localStorage.removeItem('authToken'); 
    sessionStorage.removeItem('userData'); 

  
    this.router.navigate(['/login'],{ replaceUrl: true }); 
  }
  loadAgentDetails() {
    const email = localStorage.getItem('email'); 
    if (email) { 
      this.agentService.getAgentByEmail(email).subscribe(
        data => {
          this.agentDetails = data;
          this.email = this.agentDetails.email; 
          this.loadCustomersByEmail(); 
        },
        error => {
          console.error('Error fetching agent details', error);
        }
      );
    } else {
      console.error('No email found in local storage');
    }
  }

  loadCustomersByEmail() { 
    if (this.email) {
      this.contactAgentService.getContactAgentsByAgentEmail(this.email).subscribe(
        data => {
          console.log("data",data);
          
          this.customers = data;
          console.log("customers",this.customers)
        },
        error => {
          console.error('Error fetching customers for email', error);
        }
      );
    } else {
      console.error('Email is required to fetch customers');
    }
  }
}
