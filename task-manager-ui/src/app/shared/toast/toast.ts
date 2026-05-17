import { Component, inject } from '@angular/core';
import { ToastNotification, ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-toast',
  imports: [],
  templateUrl: './toast.html',
  styleUrl: './toast.css',
})
export class Toast {
  toast = inject(ToastService);

  styles(type: string) {
    const map: Record<string, string> = {
      success: 'border-1-4 border-emerald-500 bg-slate-800',
      error: 'border-1-4 border-red-500 bg-slate-800',
      info: 'border-1-4 border-violet-500 bg-slate-800',
      warning: 'border-1-4 border-amber-500 bg-slate-800'
    }
    return map[type] ?? map['info'];
  }

  icon(type: string) {
    const map: Record<string, string> = {
      success: '✓', error: '✕', info: 'i', warning: '!'
    }
    return map[type] ?? map['info'];
  }

  iconStyle(type: string) {
    const map: Record<string, string> = {
      success: 'bg-emerald-500/20 text-emerald-400',
      error:   'bg-red-500/20 text-red-400',
      info:    'bg-violet-500/20 text-violet-400',
      warning: 'bg-amber-500/20 text-amber-400',
    };
    return map[type] ?? map['info'];
  }

  trackById(_: number, t: ToastNotification) { return t.id; }
}
