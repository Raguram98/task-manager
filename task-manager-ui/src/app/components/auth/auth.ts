import { Component, computed, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-auth',
  imports: [ReactiveFormsModule],
  templateUrl: './auth.html',
  styleUrl: './auth.css',
})
export class Auth {
  private fb  = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isRegister = this.route.snapshot.data['mode'] === 'register';

  form = this.fb.group({
    name: ['', this.isRegister ? [Validators.required, Validators.minLength(3)] : []],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [
      Validators.required, 
      Validators.minLength(8), 
      Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])/)]],
  });


  get nameCtrl() { return this.form.get('name')!; }
  get emailCtrl() { return this.form.get('email')!; }
  get passwordCtrl() { return this.form.get('password')!; }


  buttonText = computed(() => this.isRegister ? 'Register' : 'Login');
  modeText = computed(() => this.isRegister ? 'Sign up' : 'Sign in');
  toggleText = computed(() => this.isRegister ? 'Already have an account? Sign in' : 'Don\'t have an account? Sign up');

  submit() {
    if(this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }

    if(this.isRegister){
      this.authService.register(this.form.value as any);
    } else {
      this.authService.login(this.form.value as any);
    }

    

  }  

  toggleMode() {
    this.router.navigate([this.isRegister ? '/login' : '/register']);
  }
}
