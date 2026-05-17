import { Injectable, signal } from '@angular/core';


export type ToastType = 'success' | 'error' | 'info' | 'warning';

export interface ToastNotification{
  id: number;
  message: string;
  type: ToastType;
}

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  toasts = signal<ToastNotification[]>([]);

  private counter = 0;

  show(message: string, type: ToastType = 'info', duration = 3500){
    const id = ++this.counter;

    this.toasts.update(current => [...current, {id, message, type}]);

    setTimeout(() => this.dismiss(id), duration);
  }

  dismiss(id: number){
    this.toasts.update(current => current.filter(t => t.id !== id));
  }

  success(message: string) { this.show(message, 'success'); }
  error(message: string) { this.show(message, 'error'); }
  info(message: string) { this.show(message, 'info'); }
  warning(message: string) { this.show(message, 'warning'); }
}
