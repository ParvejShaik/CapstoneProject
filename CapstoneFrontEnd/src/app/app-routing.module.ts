import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';

import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { UserDashboardComponent } from './user-dashboard/user-dashboard.component';
import { AgentDashboardComponent } from './agent-dashboard/agent-dashboard.component';
import { AgentloginComponent } from './agentlogin/agentlogin.component';


const routes: Routes = [
  { path: '', component : HomeComponent },
  { path: 'login', component : LoginComponent },
  { path: 'admin-dashboard', component : AdminDashboardComponent },
  { path: 'register', component : RegisterComponent },
  { path: 'user-dashboard', component: UserDashboardComponent },
  { path: 'agent-dashboard', component: AgentDashboardComponent },
  {path : 'agent-login',component : AgentloginComponent}
 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
