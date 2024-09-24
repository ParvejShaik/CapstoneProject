import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/Services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent  {

  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private userService: UserService, private router: Router) {
    this.registerForm = this.fb.group({
      UserName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      Email: ['', [Validators.required, Validators.email]],
      Password: ['', [Validators.required, Validators.minLength(8)]],
      PhoneNumber: [''],
      Role: ['', [Validators.required, this.validateRole]]
    });
  }

  validateRole(control: any) {
    if (control.value !== 'User') {
      return { invalidRole: true };
    }
    return null;
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const { UserName, Email, Password, PhoneNumber, Role } = this.registerForm.value;
      this.userService.register({ UserName, Email, Password, PhoneNumber, Role }).subscribe(() => {
        alert('Registered successfully');
        this.router.navigate(['/login']);
      }, error => {
        alert('Registration failed. Please try again.');
      });
    }
  }
  }

