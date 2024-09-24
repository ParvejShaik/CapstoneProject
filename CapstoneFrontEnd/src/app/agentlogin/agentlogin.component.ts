import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AgentService } from 'src/Services/agent.service';


@Component({
  selector: 'app-agentlogin',
  templateUrl: './agentlogin.component.html',
  styleUrls: ['./agentlogin.component.css']
})
export class AgentloginComponent implements OnInit {

  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private agentService: AgentService, private router: Router) {}
  
 
  login() {
    this.agentService.login(this.email, this.password).subscribe(
      response => {
        localStorage.setItem('email', this.email); 
        this.router.navigate(['/agent-dashboard']);
      },
      error => {
        this.errorMessage = 'Invalid email or password.';
        console.error('Login error', error);
      }
    );
  }
  ngOnInit(): void {
    
  }
}
