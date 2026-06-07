import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastService } from './toast.service';

export interface AuthResponse {
  token: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;  
  private tokenKey = 'authToken';
  private toastService = inject(ToastService);

  isLoggedIn = signal(this.hasToken());

  userId = computed(() => {
    const token = this.getToken();
    if(!token) return null;
    const payload = this.parseToken(token);
    return payload?.nameid;
  })

  constructor(private http: HttpClient, private router: Router) {
    this.loadTokenFromStorage();
  }

  register(req: RegisterRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`,req).subscribe({
      next: (res) => {
        this.setToken(res.token);
        this.isLoggedIn.set(true);
        this.router.navigate(['/tasks']);
        this.toastService.success('Registration successful!');
      },
      error: (err) => {
        this.toastService.error('Registration failed.');
      }
    });
  }

  login(req: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`,req).subscribe({
      next: (res) => {
        this.setToken(res.token);
        this.isLoggedIn.set(true);
        this.router.navigate(['/tasks']);
        this.toastService.success('Login successful!')
        '; ';
      },
      error: (err) => {
        console.error('Login failed:', err);
        this.toastService.error('Login failed.');
      }
    });
  }

  logout() {
    this.clearToken();
    this.isLoggedIn.set(false);
    this.router.navigate(['/']);
    this.toastService.info('You have been logged out.');
  }

  private setToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private clearToken(){
    localStorage.removeItem(this.tokenKey);
  }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }

  private loadTokenFromStorage(): void {
    if(this.hasToken()){
      this.isLoggedIn.set(true);
    }
  }

  public parseToken(token: string) {
    try{
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload));
    } catch (error) {
      console.error('Error parsing token:', error);
      return null;
    }
  }
}
